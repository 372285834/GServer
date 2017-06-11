using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    public class LogConfig
    {
        public static bool IsUseLog = true;
    }

    public static class Log
    {
        public static Logger Server = new Logger("Server");
        public static Logger Net = new Logger("Net");
        public static Logger Common = new Logger("Common");
        public static Logger Table = new Logger("Table");
        public static Logger Login = new Logger("Login");
        public static Logger Social = new Logger("Social");
        public static Logger Guild = new Logger("Guild");
        public static Logger Fight = new Logger("Fight");
        public static Logger Item = new Logger("Item");
        public static Logger Mail = new Logger("Mail");

    }
}


