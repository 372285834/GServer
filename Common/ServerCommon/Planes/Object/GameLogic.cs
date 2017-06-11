using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 游戏逻辑基础类，所有逻辑类必须实现该接口
    /// </summary>
    public interface GameLogicInterface<logicT> where logicT : new()
    {
        
    }

    public class GameLogic
    {
        /// <summary>
        /// 
        /// </summary>
        public GameLogic()
        {

        }

        /// <summary>
        /// 逻辑分类
	    /// </summary>
        eGameLogicType mType;
        public eGameLogicType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        /// <summary>
        /// 逻辑名称
        /// </summary>
        short mName;
        public short Name
        {
            get { return mName; }
            set { mName = value; }
        }

        public GameLogic(eGameLogicType type, short name)
        {
            mType = type;
            mName = name;
            GameLogicManager.Instance.AddGameLogic(this);
        }
    }

    public class BaseGameLogic<logicT> : GameLogic
    {
        public BaseGameLogic(eGameLogicType type, short name, logicT logic) : base(type, name)
        {
            mLogic = logic;
        }

        logicT mLogic = default(logicT);
        public logicT Logic
        {
            get { return mLogic; }
            set { mLogic = value; }
        }
    }
}
