using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanucSampling
{
    namespace DataStructure
    {
        public abstract class DataStructure
        {
            // 是否已经接收数据 仅在刚运行是时有用
            public bool accept = false;
            // 该条数据是否已经上报
            public bool reported = false;
            // 该条数据是否被上报
            public string time;

            public abstract string getReportInfo();

            public void resetData()
            {
                reported = false;
                time = DateTime.Now.ToString();
            }
        }
        public class Coordinate : DataStructure
        {
            public string x;
            public string y;
            public string z;

            public override string getReportInfo()
            {
                return x + "," + y + "," + z;
            }
        }

        public class StateInfo : DataStructure
        {
            // public short emergency = -1; 暂时不需要急停
            public short run = -1;
            public short alarm = -1;
            public string lastReport = "";

            public override string getReportInfo()
            {
                if (!accept)
                {
                    return "";
                }
                // 40停机状态通过事件机制上报， WisSDK.reportDisconnect
                string result = "";
                if (alarm == 1)
                {
                    result = "30";
                }
                else
                {
                    result = run == 0 ? "10" : "20";
                }
                if (result != lastReport)
                {
                    lastReport = result;
                    return result;
                }
                else
                {
                    return "";
                }
            }
        }

        public class Acts : DataStructure
        {
            public string value;

            public override string getReportInfo()
            {
                return value;
            }
        }

        public class Load : DataStructure
        {
            public string value;

            public override string getReportInfo()
            {
                return value;
            }
        }

        public class Prog : DataStructure
        {
            public string name;
            public string seq;
            public string comment;

            public override string getReportInfo()
            {
                return name;
            }
        }

        public class Count : DataStructure
        {
            public int value = -1;

            public override string getReportInfo()
            {
                return value + "";
            }
        }

        public enum Connection
        {
            CONNECTED, CONNECTING, NOT_CONNECTED
        }

    }
}
