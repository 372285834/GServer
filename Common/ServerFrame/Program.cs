using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSCommon
{
    public class Program
    {
        public enum enAssemblyType
        {
            This,
            FrameSet,
            ExamplePlugins,
            UISystem,
            All,
        }

        public static AutoResetEvent mAutoEvent = new AutoResetEvent(false);

        static System.Reflection.Assembly mFrameSetAssembly = null;
        public static System.Reflection.Assembly FrameSetAssembly
        {
            get
            {
                if(mFrameSetAssembly == null)
                {
                    try
                    {
                        mFrameSetAssembly = System.Reflection.Assembly.LoadFrom("FrameSet.dll");
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }

                return mFrameSetAssembly;
            }
        }
        static System.Reflection.Assembly mExamplePluginsAssembly = null;
        public static System.Reflection.Assembly ExamplePluginsAssembly
        {
            get
            {
                if (mExamplePluginsAssembly == null)
                {
                    try
                    {
                        mExamplePluginsAssembly = System.Reflection.Assembly.LoadFrom("ExamplePlugins.dll");
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }

                return mExamplePluginsAssembly;
            }
        }
        static System.Reflection.Assembly mUISystemAssembly = null;
        public static System.Reflection.Assembly UISystemAssembly
        {
            get
            {
                if (mUISystemAssembly == null)
                {
                    try
                    {
                        mUISystemAssembly = System.Reflection.Assembly.LoadFrom("UISystem.dll");
                    }
                    catch (System.Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }

                return mUISystemAssembly;
            }
        }

        public static string GetValuedGUIDString(Guid guid)
        {
            string retString = guid.ToString();
            retString = retString.Replace("-", "_");

            return retString;
        }

        public static Type GetType(string typeStr, enAssemblyType asType)
        {
            Type retType = null;

            switch (asType)
            {
                case enAssemblyType.This:
                    return System.Reflection.Assembly.GetExecutingAssembly().GetType(typeStr);

                case enAssemblyType.FrameSet:
                    {
                        if (FrameSetAssembly != null)
                            return FrameSetAssembly.GetType(typeStr);
                    }
                    break;

                case enAssemblyType.ExamplePlugins:
                    {
                        if (ExamplePluginsAssembly != null)
                            return ExamplePluginsAssembly.GetType(typeStr);
                    }
                    break;
                case enAssemblyType.UISystem:
                    {
                        if (UISystemAssembly != null)
                            return UISystemAssembly.GetType(typeStr);
                    }
                    break;
                case enAssemblyType.All:
                    {
                        retType = System.Reflection.Assembly.GetExecutingAssembly().GetType(typeStr);

                        if (FrameSetAssembly != null && retType == null)
                            retType = FrameSetAssembly.GetType(typeStr);

                        if (ExamplePluginsAssembly != null && retType == null)
                            retType = ExamplePluginsAssembly.GetType(typeStr);

                        if (UISystemAssembly != null && retType == null)
                            retType = UISystemAssembly.GetType(typeStr);
                        
                    }
                    break;
            }

            return retType;
        }

        public static Type[] GetTypes(enAssemblyType asType)
        {
            List<Type> types = new List<Type>();

            switch (asType)
            {
                case enAssemblyType.This:
                    return System.Reflection.Assembly.GetExecutingAssembly().GetTypes();

                case enAssemblyType.FrameSet:
                    {
                        if (FrameSetAssembly != null)
                            return FrameSetAssembly.GetTypes();
                    }
                    break;

                case enAssemblyType.ExamplePlugins:
                    {
                        if (ExamplePluginsAssembly != null)
                            return ExamplePluginsAssembly.GetTypes();
                    }
                    break;

                case enAssemblyType.UISystem:
                    {
                        if (UISystemAssembly != null)
                            return UISystemAssembly.GetTypes();
                    }
                    break;
                    
                case enAssemblyType.All:
                    {
                        if (FrameSetAssembly != null)
                            types.AddRange(FrameSetAssembly.GetTypes());
                        if (ExamplePluginsAssembly != null)
                            types.AddRange(ExamplePluginsAssembly.GetTypes());
                        if (UISystemAssembly != null)
                            types.AddRange(UISystemAssembly.GetTypes());

                        types.AddRange(System.Reflection.Assembly.GetExecutingAssembly().GetTypes());
                    }
                    break;
            }

            return types.ToArray();
        }
    }
}
