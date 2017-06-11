using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon
{
    public enum eServerType{
        Register,
        Gate,
        Planes,
        Data,
        Path,
        Com,
    }

    public class IServer
    {
        static IServer smInstance = new IServer();
        public static IServer Instance
        {
            get { return smInstance; }
        }
        public IServer()
        {
            mExePath = AppDomain.CurrentDomain.BaseDirectory;            
        }

        string mExePath;
        public string ExePath
        {
            get { return mExePath; }
        }
        public void LoadRPCModule(string dllname)
        {
            System.Reflection.Assembly assembly;
            assembly = System.Reflection.Assembly.LoadFrom(mExePath+"/ServerCommon.dll");
            RPC.RPCEntrance.BuildRPCMethordExecuter(assembly);
        }

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        public extern static uint timeGetTime();

        Int64 mLogicNowTick = 0;
        System.DateTime mLogicDateTime;
        Int64 mElapseMilliSecondTime;
        
        public void Tick()
        {
            ServerFrame.Time.Tick();
            ServerFrame.TimerManager.Tick();
            Int64 nowTick = (Int64)timeGetTime();
            if (mLogicNowTick < 0)
            {
                mLogicNowTick = nowTick;
            }
            mElapseMilliSecondTime = nowTick - mLogicNowTick ;

            mLogicNowTick = nowTick;
            mLogicDateTime = System.DateTime.Now;            
        }
        
        public System.DateTime GetNowDateTime()
        {
            return mLogicDateTime;
        }
        public Int64 GetElapseMilliSecondTime()
        {
            return mElapseMilliSecondTime;
        }
        static System.DateTime mBeginTime = new System.DateTime(1970, 1, 1, 8, 0, 0);
        public static Double TotalSecond1970Time
        {
            get { return (System.DateTime.Now - mBeginTime).TotalSeconds; }
        }
        
        public static DateTime GetDateTimeFromTotalSecond(Double seconds)
        {
            DateTime date = new DateTime(1970, 1, 1, 8, 0, 0);

            return date.AddSeconds(seconds);
        }

        public static void LoadAllTemplateData(string path)
        {

            string dir = System.AppDomain.CurrentDomain.BaseDirectory + "CSCommon.dll";
            System.Reflection.Assembly m_CSCommonAssembly = System.Reflection.Assembly.LoadFile(dir);
            Type[] aTypes = m_CSCommonAssembly.GetTypes();
            foreach (var type in aTypes)
            {
                var atts = type.GetCustomAttributes(typeof(ServerFrame.Editor.Template), true);
                if (atts.Length > 0)
                {
                    System.Reflection.PropertyInfo tInstance = null;
                    var tType = type;
                    while (tInstance == null)
                    {
                        tType = tType.BaseType;
                        if (tType==null)
                            break;
                        tInstance = tType.GetProperty("Instance");
                    }
                    var tMethod = type.GetMethod("LoadAllTemplate");
                    var obj = tInstance.GetValue(null, null);
                    if (tMethod != null)
                    {
                        tMethod.Invoke(obj, new object[] { path });
                    }
                    tMethod = type.GetMethod("LoadCommonTemplate");
                    if (tMethod != null)
                    {
                        tMethod.Invoke(obj, new object[] { path });
                    }
                }
            }
        }
    }
}
