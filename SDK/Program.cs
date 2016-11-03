using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.isesol.ismes.platform.api.sdk;

namespace SDK
{
    class Program
    {
        static void abc(string[] args)
        {
           // ApiSDK openApi = new ApiSDK("localhost:8080/ismes-web", "4A8A638D63614A45AA2EA68E993DDF0D", "NDFkNTRmMzJmZjhiMjZhZjQzZWQ0OGE3Y2I3NzI1M2I=");
          //  String json = null;
          //  java.util.HashMap map = new java.util.HashMap();

           // map.put("a", "1");
            //map.put("b", "2");
           // json = openApi.call("/interf/bg_5004_service", map, "http");
            ApiSDK openApi = new ApiSDK("localhost:8080/ismes-web", "4A8A638D63614A45AA2EA68E993DDF0D", "NDFkNTRmMzJmZjhiMjZhZjQzZWQ0OGE3Y2I3NzI1M2I=");
            String json = null;
            java.util.HashMap map = new java.util.HashMap();

            map.put("a", "1");
            map.put("b", "2");
            json = openApi.call("/interf/bg_5004_service", map, "http");
            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
