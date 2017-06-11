using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public enum eNPCState
    {
        None,

        Idle,                  //发呆
        Patrol,                //巡逻
        FollowTarget,          //追击
        Transport,             //运输
        WaitJumpMap,           //等待跳转地图
        CastSpell,             //释放技能
        WaitCoolDown,          //等待cd
        ReturnSpawnPoint,      //回出生点
        Dead,                  //怪物躺尸时间
        FixedBody,             //怪物定身
        Pause,                 //怪物行为暂停

        Max,
    };
}
