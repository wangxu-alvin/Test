using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FanucSampling.Configuration;
using FanucSampling.DataStructure;
using log4net;

namespace FanucSampling
{
    class CNC
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CNCConf _conf;

        private bool _connected = false;
        private Coordinate _coordinate = new Coordinate();
        private StateInfo _stateInfo = new StateInfo();
        private Acts _acts = new Acts();
        private Load _load = new Load();
        private Prog _prog = new Prog();

        ushort Flibhndl = 0;

        private string _lastMethod;
        private string _lastResult;
        private string _lastTime;

        public CNC(CNCConf conf)
        {
            _conf = conf;
        }

        public bool isConnected()
        {
            return _connected;
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

        public void connectCNC()
        {
            _connected = false;
            // 获取库句柄 ( Ethernet ) 并进行连接
            short ret;
            ret = Focas1.cnc_allclibhndl3(_conf.ip, _conf.port, CNCConf.timeout, out Flibhndl);
            recordOperation("连接机床", ret);
            if (ret == Focas1.EW_OK)
            {
                _connected = true;
                log.Debug("连接【" + _conf.index + "】机床成功");
            }
        }

        public void rdMechanicalCoordinate()
        {
            short ret;
            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();
            for (short i = 0; i < 3; i++)
            {
                ret = Focas1.cnc_machine(Flibhndl, (short)(i + 1), 8, odbaxis);
                recordOperation("读取机械坐标" + i, ret);
                if (ret == Focas1.EW_OK)
                {
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
                    _coordinate.time = DateTime.Now.ToString();
                }
            }
        }

        // Function related to others_cnc_statinfo
        public void rdStateInfo()
        {
            short ret;
            Focas1.ODBST buf = new Focas1.ODBST();
            ret = Focas1.cnc_statinfo(Flibhndl, buf);
            recordOperation("读取状态信息", ret);
            if (ret == Focas1.EW_OK)
            {
                _stateInfo.emergency = buf.emergency;
                _stateInfo.run = buf.run;
                _stateInfo.alarm = buf.alarm;
                _stateInfo.time = DateTime.Now.ToString();
            }
        }

        // Function related to controlled axis&spindle_cnc_acts
        public void rdActs()
        {
            short ret;
            Focas1.ODBACT odbact = new Focas1.ODBACT();
            ret = Focas1.cnc_acts(Flibhndl, odbact);
            recordOperation("读取主轴转速", ret);
            if (ret == Focas1.EW_OK)
            {
                _acts.value = odbact.data.ToString();
                _acts.time = DateTime.Now.ToString();
            }
        }

        // Function related to controlled axis&spindle_cnc_rdspload
        public void rdLoad()
        {
            short ret;
            Focas1.ODBSPN odbspn = new Focas1.ODBSPN();
            ret = Focas1.cnc_rdspload(Flibhndl, Focas1.ALL_SPINDLES, odbspn);
            recordOperation("读取主轴负载", ret);
            if (ret == Focas1.EW_OK)
            {
                _load.value = odbspn.data[0].ToString();
                _load.time = DateTime.Now.ToString();
            }
        }

        // Function related to CNC program_cnexeprgname
        public void rdProgramName()
        {
            short ret;                 // 返回值
            Focas1.ODBEXEPRG exeprg = new Focas1.ODBEXEPRG();
            ret = Focas1.cnc_exeprgname(Flibhndl, exeprg);
            recordOperation("读取程序名", ret);
            if (ret == Focas1.EW_OK)
            {
                _prog.name = exeprg.name.ToString();
                _prog.nTime = DateTime.Now.ToString();
            }
        }


        private void recordOperation(string lastMethod, short ret)
        {
            _lastMethod = lastMethod;
            _lastResult = ReturnCodeDesc.getDesc(ret);
            _lastTime = DateTime.Now.ToString();
            log.Debug("[" + _conf.index + "]:" + _lastMethod + "," + _lastResult);
        }

        private void verifyConnection(short ret)
        {
            if (ret == (short)Focas1.focas_ret.EW_SOCKET)
            {
                _connected = false;
            }

        }
    }
}
