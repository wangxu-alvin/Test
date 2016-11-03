using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using java.util;
using log4net;
using FanucSampling.Configuration;

namespace FanucSampling
{
    class Program
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ArrayList cncs = new ArrayList();

        public static void abc(string[] args)
        {
            Conf.load();
            WisSDK sdk = new WisSDK();
            foreach (CNCConf conf in Conf.cncs)
            {
                CNC cnc = new CNC(conf);
                cncs.add(cnc);
            }
            while(true)
            {
                foreach(CNC cnc in cncs)
                {
                    if (!cnc.isConnected())
                    {
                        cnc.connectCNC();
                    }
                    if (cnc.isConnected())
                    {
                        cnc.rdMechanicalCoordinate();
                    }
                    if (cnc.isConnected())
                    {
                        cnc.rdStateInfo();
                    }
                    if (cnc.isConnected())
                    {
                        cnc.rdActs();
                    }
                    if (cnc.isConnected())
                    {
                        cnc.rdLoad();
                    }
                    if (cnc.isConnected())
                    {
                        cnc.rdProgramName();
                    }
                }
            }
        }
    }

}
