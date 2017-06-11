using CSCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon
{
    public static class ServerDefine
    {
        
        public static string ExePath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        //假设无穷远点
        public static SlimDX.Vector3 RemotePos = new SlimDX.Vector3(255, 0, 0);

        public const float FloatZeroValue = 0.000001f;

        //战斗随机最大值
        public const int FightRandomMax = 10000;

    }
}
