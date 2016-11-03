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
                sdk.api_5004 = getProperty("api_5004");

                // load cnc config
                int total = int.Parse(getProperty("total"));
                CNCConf.timeout = int.Parse(getProperty("timeout"));
                CNCConf.skips = getProperty("skips");

                cncs = new CNCConf[total];
                for (int i = 0; i < total; i++)
                {
                    string ipPort = getProperty(i + "");
                    string[] temp = ipPort.Split(new char[1]{':'});
                    CNCConf conf = new CNCConf(i);
                    conf.ip = temp[0];
                    conf.port = ushort.Parse(temp[1]);
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
            public string api_5004;
        }

        public class CNCConf
        {
            public CNCConf(int i)
            {
                index = i;
            }
            public int index;
            public static int timeout;
            public static string skips;
            public string ip;
            public ushort port;
        }
    }
}
