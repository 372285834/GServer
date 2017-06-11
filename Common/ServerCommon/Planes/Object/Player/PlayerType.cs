using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public enum ePlayerState
    {
        None,

        Idle,                  //发呆
        Quest,                 //进行任务
        FollowTarget,          //追击
        CastSpell,             //释放技能
        WaitCoolDown,          //等待cd
        Dead,                  //玩家躺尸时间
        FixedBody,             //玩家定身
        Pause,                 //玩家行为暂停

        Max,
    };
}
