using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    /// <summary>
    /// 若要修改属性，请对应修改客户端的EditSupport.MapNpcEditItem类，否则影响地图编辑器
    /// </summary>
    [Serializable]
    public class MapInfo_Npc
    {
        //索引;		
        public int id;

        //角色模版id;		
        public int tid;

        //客户端显示名字;		
        public string name;

        //x;		
        public float posX;

        //z;		
        public float posZ;

        //方向;		
        public float dir;

        //所属阵营;		
        public int camp;

        //缩放比例;		
        public float scale;

        //重生时间;		
        public float rebornTime;

        //怪物等级;		
        public int level;

        //警戒范围范围;		
        public float guardRange;

        //追击范围;		
        public float followRange;

        //巡逻范围
        public float patrolRange;

        //最小巡逻间隔时间
        public float patrolInternalMin;

        //最大巡逻间隔时间
        public float patrolInternalMax;

        //功能枚举id
        public byte actionId;
    }

    /// <summary>
    /// 若要修改属性，请对应修改客户端的EditSupport.MapNpcEditItem类，否则影响地图编辑器
    /// </summary>
    [Serializable]
    public class MapInfo_Portal
    {
        //索引;		
        public int id;

        //客户端显示名字;		
        public string name;

        //x;		
        public float posX;

        //z;		
        public float posZ;

        public int mapId;

        public float targetX;

        public float targetZ;

        //触发器范围
        public float triggerRange;
    }

    [Serializable]
    public class MapInfo_PatrolNode
    {
        //索引;		
        public int id;

        //x;		
        public float posX;

        //z;		
        public float posZ;

        //关联节点id
        public List<int> NextNodeIds = new List<int>();

        //节点类型
        public byte Type;

        //传送id
        public int PortalId;
    }


    [Serializable]
    public class MapInfo_Path
    {
        public int id;
        public List<MapInfo_PatrolNode> PatrolNodeList = new List<MapInfo_PatrolNode>();
    }

    [Serializable]
    public class MapInfo
    {
        public List<MapInfo_Npc> NpcList = new List<MapInfo_Npc>();
        public List<MapInfo_Portal> PortalList = new List<MapInfo_Portal>();
        public List<MapInfo_Path> PathList = new List<MapInfo_Path>();
    }
}
