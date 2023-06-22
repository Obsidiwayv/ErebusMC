using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErebusLauncher
{
    class SystemConfig
    {
        public static String VERSION = "1.0";

        public static Boolean IS_DEV = false;

        public static String COMBINE_VERSION = $"{VERSION}{(IS_DEV ? "-Development" : "")}";

        public static String APPID = "1114934288106926140";

        public static String DEFAULT_NAME = "ErebusLauncher";
    }
}
