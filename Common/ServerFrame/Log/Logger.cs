using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
    }

    [Serializable]
    public class LogInfo
    {
        public bool IsFilePrinting = true;
        public string Name = "None";
    }

    public class Logger
    {
        public LogInfo Config = new LogInfo();
        public string Name { get { return Config.Name; } }
        public Logger(string name)
        {
            Config.Name = name;
        }

        public void Enable(bool isEnable)
        {
            Config.IsFilePrinting = isEnable;
        }

        public bool CanPrint()
        {
            return (Config.IsFilePrinting);
        }

        int num = 0;
        public void FilePrint(LogLevel level, string message)
        {
            if (this.CanPrint())
            {
                num++;
                string str = string.Format("[{0}][{1}][{2}] {3}", num.ToString(), this.Name, System.DateTime.Now, message);
                switch (level)
                {
                    case LogLevel.Debug:
                    case LogLevel.Info:
                    case LogLevel.Warning:
                    case LogLevel.Error:
                        FileLog.WriteLine(str);
                        break;
                }
            }
        }

        public void FilePrint(string format, params object[] args)
        {
            string message = string.Format(format, args);
            this.FilePrint(LogLevel.Debug, message);
        }

        public void Print(LogLevel level, string message)
        {
            if (!LogConfig.IsUseLog)
                return;

            this.FilePrint(level, message);
        }

        public void Print(LogLevel level, string format, params object[] args)
        {
            if (!LogConfig.IsUseLog)
                return;

            string message = string.Format(format, args);
            this.Print(level, message);
        }

        public void Print(string format, params object[] args)
        {
            if (!LogConfig.IsUseLog)
                return;
            Print(LogLevel.Debug, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            Print(LogLevel.Warning, format, args);
        }
        public void Warning(string format)
        {
            Print(LogLevel.Warning, format);
        }

        public void Info(string format, params object[] args)
        {
            Print(LogLevel.Info, format, args);
        }
        public void Info(string format)
        {
            Print(LogLevel.Info, format);
        }

        public void Error(string format, params object[] args)
        {
            Print(LogLevel.Error, format, args);
        }
        public void Error(string format)
        {
            Print(LogLevel.Error, format);
        }

        public void Debug(string format, params object[] args)
        {
            Print(LogLevel.Debug, format, args);
        }
        public void Debug(string format)
        {
            Print(LogLevel.Debug, format);
        }

    }
}
