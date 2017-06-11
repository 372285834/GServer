using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ServerCommon
{
    /// <summary>
    /// 默认值仅在没有文件时加载，若已有配置文件则默认读取配置文件
    /// </summary>
    [Serializable]
    public class GameSet
    {
        #region 加载
        static GameSet mInstance;
        public static GameSet Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Reload();
                }
                return mInstance;
            }
        }

        public static GameSet Load()
        {
            var path = @"Bin\srvcfg\Game.cfg";
            if (!File.Exists(path))
            {
                var cf = new GameSet();
                cf.Save(path);
                Log.Log.Common.Print(Log.LogLevel.Warning, "not find GameSet!");
                return cf;
            }
            using (System.IO.FileStream sr = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(GameSet));
                return xml.Deserialize(sr) as GameSet;
            }
        }

        public static void Reload()
        {
            mInstance = Load();
        }

        public void Save(string path)
        {
            using (System.IO.FileStream sr = new System.IO.FileStream(path, FileMode.OpenOrCreate))
            {
                System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(GameSet));
                xml.Serialize(sr, this);
            }
        }

        #endregion

        //人物创建后的出生点
        public SlimDX.Vector3 BornPoint = new SlimDX.Vector3(25, 0, 20);
        //人物创建后的默认地图
        public ushort DefaultMapId = 100;

        //位置同步容差
        public float PosSyncDistRange = 0.1f;

        //npc返回出生点的移动速度倍率
        public float NPCReturnToBornPointSpeedRate = 1.0f;

        //技能释放范围容差
        public float SkillRangeSync = 0.6f;
        //技能CD时间容差
        public float SkillCDTimeSync = 0.2f;

        //普通buff间隔时间
        public float BuffTickInterval = 2.0f;

        //副本没人时自动关闭的时间,(秒)
        public float DungeonShutDownTime = 1 * 5;


        //搜索玩家间隔时间（毫秒）
        public int ScanPlayerTime = 200;
        //最小同步位置时间（毫秒）
        public int SyncPosTime = 200;
        //竞技场等待时间（毫秒）
        public int ChallengeWaitTime = 5000;
        //躺尸时间（毫秒）
        public int DeadBodyTime = 3000;
    }
}
