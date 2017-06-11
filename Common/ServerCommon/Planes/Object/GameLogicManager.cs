using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using ServerFrame;

namespace ServerCommon.Planes
{
    class GameLogicManager : Singleton<GameLogicManager>
    {
        /// <summary>
        /// 游戏逻辑
	    /// </summary>
	    Dictionary<eGameLogicType, Dictionary<short, GameLogic>> mLogics = new Dictionary<eGameLogicType, Dictionary<short, GameLogic>>();

        public bool Init()
        {
            return LoadAllGameLogic();
        }

	    /// <summary>
        /// 添加游戏逻辑
	    /// </summary>
	    public bool AddGameLogic(GameLogic logic)
        {
            if (logic == null || logic.Name == 0)
                return false;

            eGameLogicType type = logic.Type;
            if (type < 0 || type >= eGameLogicType.MAX)
                return false;

            if (!mLogics.ContainsKey(type))
                mLogics[type] = new Dictionary<short, GameLogic>();

            if (mLogics[type].ContainsKey(logic.Name))
                return false;

            mLogics[type][logic.Name] = logic;

            return true;
        }

	    /// <summary>
        /// 删除游戏逻辑
	    /// </summary>
	    public bool RemoveGameLogic(GameLogic logic)
        {
            if (logic == null || logic.Name == 0)
                return false;

            eGameLogicType type = logic.Type;
            if (type < 0 || type >= eGameLogicType.MAX)
                return false;

            if (!mLogics.ContainsKey(type))
                return false;

            if (!mLogics[type].ContainsKey(logic.Name))
                return false;

            return mLogics[type].Remove(logic.Name);
        }

	    /// <summary>
        /// 取得游戏逻辑
	    /// </summary>
	    public GameLogic GetGameLogic(eGameLogicType type, short name)
        {
            if (type < 0 || type >= eGameLogicType.MAX || name == 0)
                return null;

            if (mLogics.ContainsKey(type) && mLogics[type].ContainsKey(name))
                return mLogics[type][name];

            return null;
        }

	    /// <summary>
        /// 载入所有逻辑数据
	    /// </summary>
	    public bool LoadAllGameLogic()
        {
            bool ret = true;
            if (!BaseNPCLogic.LoadAllLogic()) ret = false;

            if (!BasePlayerLogic.LoadAllLogic()) ret = false;

            if (!BaseSkillSelector.LoadAllLogic()) ret = false;

            if (!BaseSkillChecker.LoadAllLogic()) ret = false;

            if (!BaseSkillConsumer.LoadAllLogic()) ret = false;

            if (!BaseSkillLogic.LoadAllLogic()) ret = false;

            if (!BaseInstanceObjective.LoadAllLogic()) ret = false;

            if (!BaseTaskObjective.LoadAllLogic()) ret = false;

            if (!BaseAchieveObjective.LoadAllLogic()) ret = false;

            return ret;
        }

    }
}
