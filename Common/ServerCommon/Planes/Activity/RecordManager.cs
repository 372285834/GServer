using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public struct AcceptAchieveData
    {
        public int id;             //静态表id
        public int targetNum;      //目标值
        public byte getReward;     //是否领取奖励
    }

    public class RecordManager
    {
        protected PlayerInstance mRole;
        Dictionary<int, AcceptAchieveData> AcceptAchieveDict = new Dictionary<int, AcceptAchieveData>();
        public List<int> CopyList = new List<int>();
        //public Dictionary<int, Achieve> AchieveDict = new Dictionary<int, Achieve>();
        public Dictionary<CSCommon.eAchieveType, List<Achieve>> AchieveDict = new Dictionary<CSCommon.eAchieveType, List<Achieve>>();

        public List<Achieve> GetAchieveList(CSCommon.eAchieveType atype)
        {
            List<Achieve> list = new List<Achieve>();
            AchieveDict.TryGetValue(atype, out list);
            return list;
        }

        public Achieve GetAchieve(int id)
        {
            foreach (var i in AchieveDict.Values)
            {
                foreach (var ave in i)
                {
                    if (ave.mTemplate.id == id)
                        return ave;
                }
            }
            return null;
        }

        public AcceptAchieveData GetAchieveData(int id)
        {
            AcceptAchieveData data;
            AcceptAchieveDict.TryGetValue(id, out data);
            return data;
        }

        public void Init(PlayerInstance role, CSCommon.Data.AchieveData data)
        {
            mRole = role;
            if(data.RoleId != mRole.Id)
            {
                data.RoleId = mRole.Id;
            }
            AchieveUnSerialize(data.Achievement);
            CopyUnSerialize(data.CopyInfo);

            //初始化成就监听
            InitAchieveObjiects();

        }

        public void Save(CSCommon.Data.AchieveData data)
        {
            data.Achievement = AchieveSerialize();
            data.CopyInfo = CopySerialize();
        }

        public byte[] AchieveSerialize()
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            int count = 0;
            foreach (var i in AchieveDict.Values)
            {
                foreach(var ave in i)
                {
                    if (ave.data.targetNum > 0)
                        count++;
                }
            }
            dw.Write(count);
            foreach (var i in AchieveDict.Values)
            {
                foreach (var ave in i)
                {
                    dw.Write(ave.data.id);
                    dw.Write(ave.data.targetNum);
                    dw.Write(ave.data.getReward);
                }
            }
            return dw.Trim();
        }

        public void AchieveUnSerialize(byte[] buffer)
        {
            AcceptAchieveDict.Clear();
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            int count = dr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                AcceptAchieveData data = new AcceptAchieveData();
                data.id = dr.ReadInt32();
                data.targetNum = dr.ReadInt32();
                data.getReward = dr.ReadByte();
                AcceptAchieveDict[data.id] = data;
            }
        }

        #region 副本
        public byte[] CopySerialize()
        {
            RPC.DataWriter dw = new RPC.DataWriter();
            dw.Write(CopyList.Count);
            foreach (var i in CopyList)
            {
                dw.Write(i);
            }
            return dw.Trim();
        }

        public void CopyUnSerialize(byte[] buffer)
        {
            CopyList.Clear();
            if (buffer == null || buffer.Length <= 0)
            {
                return;
            }
            RPC.DataReader dr = new RPC.DataReader(buffer, 0, buffer.Length, buffer.Length);
            int count = dr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                int id = dr.ReadInt32();
                CopyList.Add(id);
            }
        }

        #endregion

        public void InitAchieveObjiects()
        {
            List<Achieve> Achievelist = new List<Achieve>();
            var tpls = CSTable.StaticDataManager.AchieveTpl.Dict;
            foreach(var i in tpls)
            {
                Achieve ai = new Achieve(mRole, i.Value);
                AcceptAchieveData data = GetAchieveData(i.Key);
                ai.Init(data);
                Achievelist.Add(ai);
            }
            AchieveDict[CSCommon.eAchieveType.Achieve] = Achievelist;

            List<Achieve> Achievelist2 = new List<Achieve>();
            var tpls2 = CSTable.StaticDataManager.AchieveName.Dict;
            foreach (var i in tpls2)
            {
                Achieve ai = new Achieve(mRole, i.Value);
                AcceptAchieveData data = GetAchieveData(i.Key);
                ai.Init(data);
                Achievelist2.Add(ai);
            }
            AchieveDict[CSCommon.eAchieveType.AchieveName] = Achievelist2;
        }

        public void UseAchieveName(int id)
        {
            var list = GetAchieveList(CSCommon.eAchieveType.AchieveName);
            foreach (var i in list)
            {
                if (i.mTemplate.id == id)
                    i.data.getReward = (byte)CSCommon.eBoolState.True;
                else
                    i.data.getReward = (byte)CSCommon.eBoolState.False;
            }
        }

    }
}
