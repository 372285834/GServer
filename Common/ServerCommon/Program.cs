using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon
{
    public class Program
    {
        static System.Reflection.Assembly mCSCommonAssem = System.Reflection.Assembly.LoadFrom("CSCommon.dll");

        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            Type retType = null;

            try
            {
                retType = System.Type.GetType(typeName);
                if (retType == null)
                    retType = mCSCommonAssem.GetType(typeName);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return retType;
        }
    }
}
