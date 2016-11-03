using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.isesol.ismes.platform.api.sdk;
using FanucSampling.Configuration;
using FanucSampling.DataStructure;
using java.util;

namespace FanucSampling
{
    public class WisSDK
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApiSDK sdk;

        public WisSDK()
        {
            sdk = new ApiSDK(Conf.sdk.server, Conf.sdk.appid, Conf.sdk.appkey);
        }
        
        private string call(string api, Dictionary<string, string> data)
        {
            HashMap map = new HashMap();
            foreach (var item in data)
            {
                map.put(item.Key, item.Value);
            }
            string json = "";
            try
            {
                json = sdk.call(api, map, Conf.sdk.protocol);
            }
            catch (SDKException e)
            {
                log.Error(api + ":" + e.getMessage());
                throw e;
            }
            return json;
        }

        #region 数据上报
        private void reportRealtime(CNC cnc)
        {
            // 实时数据 9001
            Acts acts = cnc.getActs();
            Coordinate coor = cnc.getCoordinate();
            Load load = cnc.getLoad();
            if (acts.accept && coor.accept && load.accept)
            {
                // 数据都已经读取过
                if (!acts.reported || !coor.reported || !load.reported)
                {
                    // 数据只要有一项没上报过
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    try
                    {
                        data.Add("sbbh", cnc._conf.sbNo); // 设备编号
                        data.Add("type", "9001");
                        StringBuilder content = new StringBuilder();
                        content.Append("{\"zzzsz_s\":\"").Append(acts.value).Append("\"");
                        content.Append(",\"xaxis\":\"").Append(coor.x).Append("\"");
                        content.Append(",\"yaxis\":\"").Append(coor.y).Append("\"");
                        content.Append(",\"zaxis\":\"").Append(coor.z).Append("\"");
                        content.Append(",\"zzfzz\":\"").Append(load.value).Append("\"");
                        content.Append(",\"sjsj\":\"").Append(acts.time).Append("\"");
                        content.Append("}");
                        data.Add("content", content.ToString());
                        this.call(Conf.sdk.api_url, data);
                        acts.reported = true;
                        coor.reported = true;
                        load.reported = true;
                    }
                    catch (SDKException e)
                    {
                        // do nothing
                    }
                }
            }
        }

        private void reportState(CNC cnc)
        {
            // 状态数据 9002
            StateInfo si = cnc.getStateInfo();
            if (si.accept && si.getReportInfo() != "")
            {
                // 只有状态发生变化才上报
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("sbbh", cnc._conf.sbNo); // 设备编号
                data.Add("type", "9002");
                StringBuilder content = new StringBuilder();
                content.Append("{\"sbztdm\":\"").Append(si.getReportInfo()).Append("\"}");
                data.Add("content", content.ToString()); // 设备状态代码
                try
                {
                    this.call(Conf.sdk.api_url, data);
                }
                catch (SDKException e)
                {
                    // do nothing
                }
            }
        }

        private void reportCount(CNC cnc)
        {
            // 报工
            Prog prog = cnc.getProgram();
            Count count = cnc.getCount();
            Count lastCount = cnc.getLastCount();
            if (prog.accept && count.accept && count.value != lastCount.value)
            {
                // 数据都已经读取过并且本次读到的加工个数与上次不相同
                if (!prog.reported || !count.reported)
                {
                    // 数据只要有一项没上报过
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    try
                    {
                        data.Add("sbbh", cnc._conf.sbNo); // 设备编号
                        data.Add("type", "9004");
                        StringBuilder content = new StringBuilder();
                        content.Append("{\"cxmc\":\"").Append(prog.name).Append("\"");
                        content.Append(",\"cxbz\":\"").Append(prog.comment).Append("\"");
                        content.Append(",\"ljjggs\":\"").Append(count.getReportInfo()).Append("\""); // 零件加工个数
                        content.Append(",\"sjsj\":\"").Append(count.time).Append("\"");
                        content.Append("}");
                        data.Add("content", content.ToString());
                        this.call(Conf.sdk.api_url, data);
                        prog.reported = true;
                        count.reported = true;
                        cnc.setLastCount(count);
                    }
                    catch (SDKException e)
                    {
                        // do nothing
                    }
                }
            }
        }

        public void reportDisconnect(object sender, EventArgs args)
        {
            CNC cnc = (CNC)sender;
            // 数据只要有一项没上报过
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("sbbh", cnc._conf.sbNo); // 设备编号
            data.Add("type", "9002");
            StringBuilder content = new StringBuilder();
            content.Append("{\"sbztdm\":\"").Append("40").Append("\"}");
            data.Add("content", content.ToString()); // 设备状态代码
            try
            {
                this.call(Conf.sdk.api_url, data);
            }
            catch (SDKException e)
            {
                // do nothing
            }
        }

        public void reportDummy()
        {
            // 数据只要有一项没上报过
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("sbbh", "sb01"); // 设备编号
            data.Add("type", "9001");
            StringBuilder content = new StringBuilder();
            content.Append("{\"zzzsz_s\":\"").Append("123").Append("\"");
            content.Append(",\"xaxis\":\"").Append("123.123").Append("\"");
            content.Append(",\"yaxis\":\"").Append("234.234").Append("\"");
            content.Append(",\"zaxis\":\"").Append("345.345").Append("\"");
            content.Append(",\"zzfzz\":\"").Append("1000").Append("\"");
            content.Append(",\"sjsj\":\"").Append(DateTime.Now.ToString()).Append("\"");
            content.Append("}");
            data.Add("content", content.ToString());
            this.call(Conf.sdk.api_url, data);
            try
            {
                this.call(Conf.sdk.api_url, data);
            }
            catch (SDKException e)
            {
                // do nothing
            }
        }

        public void report(CNC cnc)
        {
            // 报工信息
            reportCount(cnc);
            // 实时数据
            reportRealtime(cnc);
            // 状态信息
            reportState(cnc);
        }
        #endregion
    }
}
