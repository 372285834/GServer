using CSCommon;
using CSCommon.Data;
using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public class SkillCheats : ASkillObject
    {
        public override eSkillType BaseType
        {
            get { return eSkillType.Cheats; }
        }

        protected CSTable.SkillPassiveData mData;
        protected CSTable.CheatsData mLvData;

        public override int ID
        {
            get { return mData.id; }
        }

        public override CSTable.ISkillTable Data
        {
            get { return mData; }
        }
        public CSTable.CheatsData LevelData
        {
            get { return mLvData; }
        }
        
        public override int Level
        {
            get { return LevelData.level; }
        }

        RoleActor mOwner = null;
        public RoleActor Owner //技能所有者
        {
            get { return mOwner; }
        }

        public override bool Init(int tid, byte lv)
        {
            mData = CSTable.StaticDataManager.SkillPassive[tid];
            mLvData = CSTable.StaticDataManager.Cheats[tid, lv];

            if (mLvData==null)
            {
                Log.Log.Server.Warning("not find Cheats lv tid={0},lv={1}", tid, lv);
            }

            return true;
        }

        public override void SetOwner(RoleActor owner)
        {
            mOwner = owner;
        }

    }

    public class SkillBodyChannel : SkillCheats
    {
        public override eSkillType BaseType
        {
            get { return eSkillType.BodyChannel; }
        }
    }
}
