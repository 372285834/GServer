using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    /// <summary>
    /// 副本目标逻辑基类
    /// </summary>
    public class BaseAchieveObjective : IAchieveObjective
    {
        public virtual CSCommon.eAchieveEventType LogicType { get { return CSCommon.eAchieveEventType.KillMonster; } }

        public static bool LoadAllLogic()
        {
            var assmbly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assmbly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseAchieveObjective)))
                    continue;

                System.Activator.CreateInstance(type);
            }

            return true;
        }

        BaseGameLogic<IAchieveObjective> mInstance;

        public BaseAchieveObjective()
        {
            mInstance = new BaseGameLogic<IAchieveObjective>(eGameLogicType.AchieveLogic, (short)this.LogicType, this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual bool OnInit(Achieve data)
        {
            return true;
        }

        /// <summary>
        /// 目标完成
        /// </summary>
        public virtual bool OnFinish(Achieve data)
        {
            return true;
        }

        public static void _Common_Listener(EventDispatcher listener, object data)
        {
            var achieve = listener as Achieve;
            achieve.AddTargetNum(1);
        }

        public static List<int> GetListIdParseString(string str)
        {
            List<int> list = new List<int>();
            if (string.IsNullOrEmpty(str))
            {
                return list;
            }
            var ids = str.Split('|');
            foreach (var i in ids)
            {
                int id = Convert.ToInt32(i);
                list.Add(id);
            }
            return list;
        }
    }
}