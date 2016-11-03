using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanucSampling
{
    namespace DataStructure
    {
        public class Coordinate
        {
            public string x;
            public string y;
            public string z;
            public string time;
        }

        public class StateInfo
        {
            public short emergency;
            public short run;
            public short alarm;
            public string time;
        }

        public class Acts
        {
            public string value;
            public string time;
        }

        public class Load
        {
            public string value;
            public string time;
        }

        public class Prog
        {
            public string name;
            public string nTime;
            public string comment;
            public string cTime;
        }
    }
}
