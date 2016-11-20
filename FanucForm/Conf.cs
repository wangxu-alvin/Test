using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FanucSampling
{
    namespace Configuration
    {
        class Conf
        {
            public static SDKConf sdk;
            public static CNCConf[] cncs;
            public static FreqConf freq;

            private static string getProperty(string prop)
            {
                return ConfigurationManager.AppSettings[prop];
            }

            public static void load()
            {
                ConfigurationManager.RefreshSection("appSettings");
                // load sdk config
                sdk = new SDKConf();
                sdk.appid = getProperty("appid");
                sdk.appkey = getProperty("appkey");
                sdk.protocol = getProperty("protocol");
                sdk.server = getProperty("server");
                sdk.api_url = getProperty("api_url");

                freq = new FreqConf();
                freq.refreshView = double.Parse(getProperty("refreshView"));
                freq.sampling = int.Parse(getProperty("sampling"));
                freq.report = double.Parse(getProperty("report"));

                // load cnc config
                int total = int.Parse(getProperty("total"));
                CNCConf.timeout = int.Parse(getProperty("timeout"));
                CNCConf.abnormal_timeout = int.Parse(getProperty("abnormal_timeout"));

                cncs = new CNCConf[total];
                for (int i = 0; i < total; i++)
                {
                    string sbInfo = getProperty(i + "");
                    string[] temp = sbInfo.Split(new char[1]{':'});
                    CNCConf conf = new CNCConf(i);
                    conf.sbNo = temp[0];
                    conf.ip = temp[1];
                    conf.port = ushort.Parse(temp[2]);
                    conf.run = bool.Parse(temp[3]);
                    cncs[i] = conf;
                }
            }
        }

        public class SDKConf
        {
            public string appid;
            public string appkey;
            public string protocol;
            public string server;
            public string api_url;
        }

        public class CNCConf
        {
            public CNCConf(int i)
            {
                index = i;
            }
            public int index;
            public string sbNo;
            public static int timeout;
            public static int abnormal_timeout;
            public bool run;
            public string ip;
            public ushort port;
        }

        public class FreqConf
        {
            public double refreshView;
            public int sampling;
            public double report;
        }
    }
}
