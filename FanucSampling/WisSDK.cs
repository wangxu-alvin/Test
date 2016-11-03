using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.isesol.ismes.platform.api.sdk;
using FanucSampling.Configuration;
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

        public string call(string api, Dictionary<string, string> data)
        {
            HashMap map = new HashMap();
            foreach (var item in data)
            {
               // map.put(item.Key, item.Value);
                map.put("a", "1");
                map.put("b", "2");
            }
            string json = "";
            try
            {
                //json = sdk.call(api, map, Conf.sdk.protocol);
                json = sdk.call("", map, "http");
            }
            catch (SDKException e)
            {
                log.Error(e.getMessage());
            }
            return json;
        }
    }
}
