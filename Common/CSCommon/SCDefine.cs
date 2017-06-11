using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public static class SCDefine
    {
        public const float cCommonCD = 1f;
        public const float PlayerDefaultSpeed = 5;

        public const int NpcNormalAtkId = 100;


    }


    public class CSLog
    {
        public delegate void Delegate_LogFun(string format, params object[] args);
        public static Delegate_LogFun LogFun;
    }
}
