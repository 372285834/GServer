using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// Actor类型
    /// </summary>
    public enum eActorGameType
    {
        Unknow          = 0,
        Common          = 1,
        Player          = 2,
        Npc             = 3,        
        Potal           = 4,
        Summon          = 5,
        GatherItem      = 6,
        PlayerImage     = 7, //玩家镜像
    }

    /// <summary>
    /// actor初始化器
    /// </summary>
    public class IActorInitBase
    {       
        private eActorGameType mGameType = eActorGameType.Unknow;
        public eActorGameType GameType
        {
            get { return mGameType; }
            set { mGameType = value; }
        }

        private string mActorName = "";
        public string ActorName
        {
            get { return mActorName; }
            set { mActorName = value; }
        }
    }

    /// <summary>
    /// Actor是在场景中的显示对象基类，在服务器有逻辑对象
    /// </summary>
    public abstract class AActorBase : EventDispatcher
    {
        /// <summary>
        /// 全局唯一id
        /// </summary>
        public virtual ulong Id
        {
            get { return 0; }
        }


        protected APlacement mPlacement;
        public APlacement Placement
        {
            get { return mPlacement; }
        }

        protected IActorInitBase mActorInit;
        public IActorInitBase ActorInit
        {
            get { return mActorInit; }
        }

        public virtual eActorGameType GameType
        {
            get
            {
                if (mActorInit == null)
                    return eActorGameType.Unknow;
                return mActorInit.GameType;
            }
        }

        public virtual bool Initialize(IActorInitBase init)
        {
            mActorInit = init;
            mPlacement = new RolePlacement(this);
            return true;
        }

        public virtual void Tick(Int64 elapsedMillisecond)
        {
            if (mPlacement != null)
                mPlacement.Tick(this, elapsedMillisecond);
        }


        protected MapInstance mHostMap = NullMapInstance.Instance;
        public MapInstance HostMap
        {
            set { mHostMap = value; }
            get { return mHostMap; }
        }

        MapCellInstance mHostMapCell;
        public MapCellInstance HostMapCell
        {
            get { return mHostMapCell; }
            set { mHostMapCell = value; }
        }

        public virtual void OnEnterMap(MapInstance map){}
        public virtual void OnLeaveMap() { }
        public virtual void OnJumpToMap() { }

    }
}
