﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanucSampling.Configuration;
using FanucSampling.DataStructure;
using log4net;
using System.Diagnostics;

namespace FanucSampling
{
    public class CNC
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler disconnectionHandler;
        Stopwatch sw = new Stopwatch();

        public CNCConf _conf;

        private Connection _connection = Connection.NOT_CONNECTED;
        private Coordinate _coordinate = new Coordinate();
        private StateInfo _stateInfo = new StateInfo();
        private Acts _acts = new Acts();
        private Load _load = new Load();
        private Prog _prog = new Prog();
        private Count _count = new Count();
        private Count _lastCount = new Count();

        ushort Flibhndl = 0;

        private string _lastMethod;
        private string _lastResult;
        private string _lastTime;

        public CNC(CNCConf conf)
        {
            _conf = conf;
        }

        public Connection getConnection()
        {
            return _connection;
        }

        public Coordinate getCoordinate()
        {
            return _coordinate;
        }

        public StateInfo getStateInfo()
        {
            return _stateInfo;
        }

        public Acts getActs()
        {
            return _acts;
        }

        public Load getLoad()
        {
            return _load;
        }

        public Prog getProgram()
        {
            return _prog;
        }

        public Count getCount()
        {
            return _count;
        }

        public Count getLastCount()
        {
            return _lastCount;
        }

        public void setLastCount(Count count)
        {
            _lastCount = count;
        }

        public bool connectCNC()
        {
            if (_connection == Connection.NOT_CONNECTED)
            {
                Flibhndl = 0;
                _connection = Connection.CONNECTING;
                // 获取库句柄 ( Ethernet ) 并进行连接
                short ret;
                sw.Reset();
                int timeout = _conf.run ? CNCConf.timeout : CNCConf.abnormal_timeout;
                log.Info("start---allclibhndl3");
                ret = Focas1.cnc_allclibhndl3(_conf.ip, _conf.port, timeout, out Flibhndl);
                log.Info("end---allclibhndl3");
                sw.Stop();
                log.Info("[" + _conf.index + "]连接机床耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("连接机床", ret);
                if (ret == Focas1.EW_OK)
                {
                    _connection = Connection.CONNECTED;
                    _conf.run = true;
                    log.Debug("连接【" + _conf.index + "】机床成功");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public void rdMechanicalCoordinate()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();
                sw.Reset();
                for (short i = 0; i < 3; i++)
                {
                    log.Info("start---machine");
                    ret = Focas1.cnc_machine(Flibhndl, (short)(i + 1), 8, odbaxis);
                    log.Info("end---machine");
                    checkConnection(ret);
                    recordOperation("读取机械坐标" + i, ret);
                    if (ret == Focas1.EW_OK)
                    {
                        if (odbaxis == null || odbaxis.data.Length == 0)
                        {
                            log.Error("读取坐标返回值错误");
                            return;
                        }
                        if (i == 0)
                        {
                            _coordinate.x = "" + odbaxis.data[0] * Math.Pow(10, -3);
                        }
                        else if (i == 1)
                        {
                            _coordinate.y = "" + odbaxis.data[0] * Math.Pow(10, -3);
                        }
                        else
                        {
                            _coordinate.z = "" + odbaxis.data[0] * Math.Pow(10, -3);
                        }
                        _coordinate.resetData();
                    }
                    sw.Stop();
                    log.Debug("[" + _conf.index + "]读取坐标耗时：" + sw.Elapsed);
                }
                _coordinate.accept = true;
            }
        }

        public void rdStateInfo()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                Focas1.ODBST buf = new Focas1.ODBST();
                sw.Reset();
                log.Info("start---statinfo");
                ret = Focas1.cnc_statinfo(Flibhndl, buf);
                log.Info("end---statinfo");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取状态耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取状态信息", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (buf == null) 
                    {
                        log.Error("读取状态数据的结果为null");
                        return;
                    }
                    //_stateInfo.emergency = buf.emergency;
                    _stateInfo.run = buf.run;
                    _stateInfo.alarm = buf.alarm;
                    _stateInfo.resetData();
                }
                _stateInfo.accept = true;
             }
        }

        public void rdActs()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                Focas1.ODBACT odbact = new Focas1.ODBACT();
                sw.Reset();
                log.Info("start---rdActs");
                ret = Focas1.cnc_acts(Flibhndl, odbact);
                log.Info("end---rdActs");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取主轴转速耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取主轴转速", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (odbact == null)
                    {
                        log.Error("读取主轴转速结果为null");
                        return;
                    }
                    _acts.value = odbact.data.ToString();
                    _acts.resetData();
                }
                _acts.accept = true;
            }
        }

        public void rdLoad()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                Focas1.ODBSPN odbspn = new Focas1.ODBSPN();
                sw.Reset();
                log.Info("start---rdLoad，句柄：" + Flibhndl);
                ret = Focas1.cnc_rdspload(Flibhndl, Focas1.ALL_SPINDLES, odbspn);
                log.Info("end---rdLoad");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取主轴负载耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取主轴负载", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (odbspn == null || odbspn.data.Length == 0)
                    {
                        log.Error("读取主轴负载结果为null");
                        return;
                    }
                    _load.value = odbspn.data[0].ToString();
                    _load.resetData();
                }
                _load.accept = true;
            }
        }

        public void rdProgramInfo()
        {
            bool result = false;
            if (_connection == Connection.CONNECTED)
            {
                short ret;                 // 返回值
                Focas1.ODBEXEPRG exeprg = new Focas1.ODBEXEPRG();
                sw.Reset();
                log.Info("start---exeprgname");
                ret = Focas1.cnc_exeprgname(Flibhndl, exeprg);
                log.Info("end---exeprgname");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取程序名耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取程序名", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (exeprg == null || exeprg.name.Length == 0)
                    {
                        log.Error("读取程序结果为null");
                        return;
                    }
                    _prog.name = exeprg.name[0].ToString();
                    string name = "";
                    for (int i = 1; i < exeprg.name.Length; i++)
                    {
                        if (exeprg.name[i].ToString() == "\0")
                        {
                            break;
                        }
                        name += exeprg.name[i];
                    }
                    _prog.name = String.Format("O{0:0000}", int.Parse(name));
                    result = true;
                }
                Focas1.ODBPRO pnum = new Focas1.ODBPRO();
                sw.Reset();
                log.Info("start---rdprgnum");
                ret = Focas1.cnc_rdprgnum(Flibhndl, pnum);
                log.Info("end---rdprgnum");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取程序号耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取程序号", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (pnum.data == null)
                    {
                        log.Error("读取程序号结果为null");
                        return;
                    }
                    _prog.seq = String.Format("O{0:0000}", pnum.data);
                    result |= result;

                    short type = 2;
                    short readNum = 1;
                    short seq = short.Parse(pnum.data.ToString());
                    Focas1.PRGDIR2 buf = new Focas1.PRGDIR2();
                    sw.Reset();
                    log.Info("start---rdprogdir2");
                    ret = Focas1.cnc_rdprogdir2(Flibhndl, type, ref seq, ref readNum, buf);
                    log.Info("end---rdprogdir2");
                    sw.Stop();
                    log.Debug("[" + _conf.index + "]读取程序备注耗时：" + sw.Elapsed);
                    checkConnection(ret);
                    recordOperation("读取程序备注", ret);
                    if (ret == Focas1.EW_OK)
                    {
                        if (buf == null || buf.dir1 == null)
                        {
                            log.Error("读取程序备注结果为null");
                            return;
                        }
                        _prog.comment = buf.dir1.comment;
                        result |= result;
                    }
                }
                if (result)
                {
                    _prog.resetData();
                }
                _prog.accept = true;
            }
        }

        public void rdCount()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                Focas1.IODBPSD_1 obd = new Focas1.IODBPSD_1();
                sw.Reset();
                log.Info("start---rdparam");
                ret = Focas1.cnc_rdparam(Flibhndl, 6711, 0, 8, obd);
                log.Info("end---rdparam");
                sw.Stop();
                log.Debug("[" + _conf.index + "]读取加工零件数耗时：" + sw.Elapsed);
                checkConnection(ret);
                recordOperation("读取加工零件个数", ret);
                if (ret == Focas1.EW_OK)
                {
                    if (obd == null)
                    {
                        log.Error("读取加工零件个数结果为null");
                        return;
                    }
                    _count.value = obd.ldata;
                    _count.resetData();
                }
               _count.accept = true;
            }
        }

        public void release()
        {
            if (_connection == Connection.CONNECTED)
            {
                short ret;
                log.Info("start---freelibhndl");
                ret = Focas1.cnc_freelibhndl(Flibhndl);
                log.Info("end---freelibhndl");
                Focas1.IODBPSD_1 obd = new Focas1.IODBPSD_1();
                checkConnection(ret);
                recordOperation("释放连接", ret);
                if (ret == Focas1.EW_OK)
                {
                    _connection = Connection.NOT_CONNECTED;
                }
            }
        }

        public void sample()
        {
            this.connectCNC();
            this.rdActs();
            this.rdLoad();
            this.rdMechanicalCoordinate();
            this.rdProgramInfo();
            this.rdCount();
            this.rdStateInfo();
           // this.release();
        }

        public string getLastMethod()
        {
            return _lastMethod;
        }

        public string getLastResult()
        {
            return _lastResult;
        }

        public string getLastTime()
        {
            return _lastTime;
        }

        private void recordOperation(string lastMethod, short ret)
        {
            _lastMethod = lastMethod;
            _lastResult = ReturnCodeDesc.getDesc(ret);
            _lastTime = DateTime.Now.ToString();
            if (ret == Focas1.EW_OK)
            {
                log.Debug("[" + _conf.index + "]:" + _lastMethod + "," + _lastResult);
            }
            else
            {
                log.Warn("[" + _conf.index + "]:" + _lastMethod + "," + _lastResult);
            }
        }

        private void checkConnection(short ret)
        {
            if (ret != (short)Focas1.focas_ret.EW_OK)
            {
                if (_connection == Connection.CONNECTED)
                {
                    // 触发下线通知
                    EventHandler handler = disconnectionHandler;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }
                }
                _connection = Connection.NOT_CONNECTED;
                _conf.run = false;
            }
        }
    }
}
