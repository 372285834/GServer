using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon;
using CSCommon.Data;

namespace ServerCommon.Planes
{
    public struct EffectParam
    {
        public int type;
        public float value;
        public EffectParam(int t, float v)
        {
            type = t;
            value = v;
        }
    }

    public class BuffData
    {
        protected WeakReference mOwnerRef;
        public RoleActor Owner
        {
            get { return mOwnerRef.Target as RoleActor; }
        }

        protected WeakReference mCaster;
        public RoleActor Caster
        {
            get { return mCaster.Target as RoleActor; }
        }

        public long mRemindTime = 0; //buff持续时间
        protected float mIntervalTime; //间隔时间

        ulong mId;
        public ulong Id
        {
            get { return mId; }
        }

        CSTable.BuffData mData;
        public CSTable.BuffData Data
        {
            get { return mData; }
        }

        CSTable.BuffLevelData mLvData;
        public CSTable.BuffLevelData LevelData
        {
            get { return mLvData; }
        }

        public int BuffId
        {
            get { return mData.id; }
        }

        public int Level
        {
            get { return mLvData.level; }
        }

        eBuffLogicType mLogicType;
        public eBuffLogicType LogicType
        {
            get { return mLogicType; }
        }

        BuffManager mBuffMgr;
        public BuffManager BuffMgr
        {
            get { return mBuffMgr; }
        }

        protected List<EffectParam> mEffectValue = new List<EffectParam>();
        public List<EffectParam> EffectValue
        {
            get { return mEffectValue; }
        }

        Dictionary<eBuffEffectType, float> mAllEffectValue = new Dictionary<eBuffEffectType, float>(); //所有本buff加的属性
        public Dictionary<eBuffEffectType, float> AllEffectValue
        {
            get { return mAllEffectValue; }
        }

        public bool Init(CSTable.BuffLevelData levelData)
        {
            if (null == levelData) return false;

            if (mId == 0)
                mId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Buffer);
            mLvData = levelData;
            mData = CSTable.StaticDataManager.Buff[levelData.id];
            mRemindTime = (long)(LevelData.affectTime * 1000);
            mIntervalTime = LevelData.affectRate * 1000;
            mIntervalTime = mIntervalTime != 0 ? mIntervalTime : -1000;
            ParseEffectParam();
            if (LevelData.affectRate < 0)
                mLogicType = eBuffLogicType.Static;
            else if (LevelData.affectRate == 0)
                mLogicType = eBuffLogicType.Fixed;
            else
                mLogicType = eBuffLogicType.Continual;

            return true;
        }

        public void SetOwner(RoleActor owner)
        {
            mOwnerRef = new WeakReference(owner);
            mBuffMgr = owner.BuffMgr;
        }

        public void SetCaster(RoleActor caster)
        {
            mCaster = new WeakReference(caster);
        }

        /// <summary>
        /// 动态添加
        /// </summary>
        public void AddResultValue(eBuffEffectType type, float value)
        {
            if (null == Owner) return;

            float tempValue = value;
            switch (type)
            {
                case eBuffEffectType.影响生命:
                    Owner.ChangeHP((int)tempValue, Caster);
                    if (tempValue < 0)
                    {
                        Owner.UpdateEnmityList(Caster.Id, (int)tempValue);
                        Owner.SendFlutterInfo((int)eFlutterInfoType.BuffReduceHp, BuffId, Level, -1 * (int)tempValue, Caster);
                    }
                    else if (tempValue > 0)
                    {
                        Owner.SendFlutterInfo((int)eFlutterInfoType.BuffAddHp, BuffId, Level, -1 * (int)tempValue, Caster);
                    }
                    break;
                case eBuffEffectType.影响技能加成:
                    float attackTotal = Caster.FinalRoleValue.Atk * tempValue;
                    float defendTotal = Owner.FinalRoleValue.GetDef(Caster.ElemType);
                    tempValue = AttackImpact.ComputeAttackImpact(attackTotal, defendTotal, eHitType.Hit, Caster, Owner);
                    Owner.ChangeHP(-1 * (int)tempValue, Caster);
                    Owner.SendFlutterInfo((int)eFlutterInfoType.BuffReduceHp, BuffId, Level, (int)tempValue, Caster);
                    break;
            }
        }

        /// <summary>
        /// 设置buff实际影响值
        /// </summary>
        public void SetEffectValue(eBuffEffectType type, float value)
        {
            mAllEffectValue[type] = value;
        }

        public float GetEffectValue(eBuffEffectType type)
        {
            if (!mAllEffectValue.ContainsKey(type))
                return 0;

            return mAllEffectValue[type];
        }

        /// <summary>
        /// 解析buff参数
        /// </summary>
        public void ParseEffectParam()
        {
            mEffectValue.Add(new EffectParam(mLvData.attri1, mLvData.value1));
            mEffectValue.Add(new EffectParam(mLvData.attri2, mLvData.value2));
            mEffectValue.Add(new EffectParam(mLvData.attri3, mLvData.value3));
        }

        public void ClearUp()
        {
            mId = 0;
            mRemindTime = 0;
            mIntervalTime = 0;
            mCaster = null;
            mBuffMgr = null;
            mData = null;
            mLvData = null;
            mEffectValue.Clear();
            mAllEffectValue.Clear();
        }

    }
}
