using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log
{
	public delegate void WriteLogDelegate(string str);

    public class FileLog
    {
        static FileLog smInstance = new FileLog();
	    public WriteLogDelegate Deleget_WriteLog;
		protected System.IO.StreamWriter mLog;
		protected System.IO.StreamWriter mDataHeader;
		
		~FileLog()
		{
			End();
		}
        public static FileLog Instance
        {
            get { return smInstance; }
        }
        public static FileLog GetInstance()
        {
            return smInstance;
        }

		public void Begin(System.String LogName,bool bCreateDataHeader)
	    {
		    System.DateTime dateTime = System.DateTime.Now;
		    System.String strTime = dateTime.Month + "_" + dateTime.Day + "_" + dateTime.Hour + "_" + dateTime.Minute + "_" + dateTime.Second;

		    System.String mExePath = System.AppDomain.CurrentDomain.BaseDirectory;
		    System.String logDir = mExePath + "/log/";
		    if(!System.IO.Directory.Exists(logDir))
			    System.IO.Directory.CreateDirectory(logDir);
		    mLog = new System.IO.StreamWriter(mExePath + "/log/" + LogName + strTime + ".log" ,true);
		    WriteLine(dateTime.ToString());
		    if (bCreateDataHeader)
		    {
		    }
	    }
	    public void End()
	    {
		    if (mLog != null)
		    {
			    try
			    {
				    mLog.Close();
			    }
			    finally
			    {
				    mLog = null;
			    }			
		    }
		    if (mDataHeader != null)
		    {
			    try
			    {
				    mDataHeader.Close();
			    }
			    finally
			    {
				    mDataHeader = null;
			    }
		    }
	    }
	    public void Flush()
	    {
		    if(mLog!=null)
		    {
			    mLog.Flush();
		    }
		    if(mDataHeader!=null)
		    {
			    mDataHeader.Flush();
		    }
	    }

	    public static void WriteLine(System.String str)
	    {
		    System.Diagnostics.Debug.WriteLine(str);
            if (smInstance.Deleget_WriteLog != null)
                smInstance.Deleget_WriteLog(str);
		    if (smInstance.mLog == null)
		    {
			    //smInstance.Begin(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".log");
			    return;
		    }
		    smInstance.mLog.WriteLine(str);
	    }

	    public static void WriteLine(System.String format, params System.Object[] args)
	    {
		    System.String str = System.String.Format(format, args);
            if (smInstance.Deleget_WriteLog != null)
                smInstance.Deleget_WriteLog(str);
            if (smInstance.mLog == null)
		    {
			    //smInstance.Begin(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".log",false);
			    return;
		    }
		    smInstance.mLog.WriteLine(str);
	    }

	    public static void WriteDataHead(System.String str)
	    {
		    if (smInstance.mDataHeader == null)
			    return;
		    smInstance.mDataHeader.WriteLine(str);
	    }
    }
}
