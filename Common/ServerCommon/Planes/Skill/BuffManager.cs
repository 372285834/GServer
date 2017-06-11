using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSCommon.Data;
using CSCommon;
using System.Collections;
using ServerFrame;

namespace ServerCommon.Planes
{
    public class BuffManager
    {
        RoleActor mOwner; //buff所有者
        Dictionary<UInt64, Buff> mBuffers = new Dictionary<UInt64, Buff>(); //所有buff
        Dictionary<eBuffEffectType, List<Buff>> mBufferEffects = new Dictionary<eBuffEffectType, List<Buff>>(); //所有buff生效器
        List<CSTable.BuffData> mAddList = new List<CSTable.BuffData>();
        List<Buff> mDelList = new List<Buff>();

        public void SetOwner(RoleActor owner)
        {
            mOwner = owner;
        }

        public void Tick(long elapsedMillisecond)
        {
            mDelList.Clear();
            mAddList.Clear();
            foreach (var val in mBuffers.Values)
            {
                if (!val.Tick(elapsedMillisecond))
                {
                    if (null == mOwner || mOwner.IsDie)
                        return;

                    mDelList.Add(val);
                }
            }

            foreach (var buff in mDelList)
            {
                CSTable.BuffData tempData = buff.Data;
                DeleteBuffer(buff.Id);
                if (tempData.assId > 0)
                {
                    //触发后续buff
                    mAddList.Add(tempData);
                }
            }
            foreach (var val in mAddList)
            {
                CreateBuff(val.assId, val.assLevel);
            }
        }

        /// <summary>
        /// 创建buff
        /// </summary>
        public bool CreateBuff(int buffId, int lv)
        {
            var buffTpl = CSTable.StaticDataManager.Buff[buffId];
            var buffLvTpl = CSTable.StaticDataManager.BuffLevel[buffId, lv];
            if (null == buffTpl || null == buffLvTpl)
                return false;

            //免疫debuff
            if (mOwner.IsImmunityDebuff())
                return false;

            if (!CheckReplace(buffLvTpl))
                return false;

            Buff buff = new Buff();
            if (!buff.Init(buffLvTpl))
                return false;

            AddBuff(buff);

            return true;
        }

        /// <summary>
        /// 通过技能创建buff
        /// </summary>
        public bool CreateBuffBySkill(int buffId, int lv, SkillActive skill)
        {
            var buffTpl = CSTable.StaticDataManager.Buff[buffId];
            var buffLvTpl = CSTable.StaticDataManager.BuffLevel[buffId, lv];
            if (null == buffTpl || null == buffLvTpl)
                return false;

            //免疫debuff
            if (mOwner.IsImmunityDebuff())
                return false;

            if (!CheckReplace(buffLvTpl))
                return false;

            Buff buff = new Buff();
            if (!buff.Init(buffLvTpl))
                return false;

            AddBuffBySkill(buff, skill);

            return true;
        }

        private bool AddBuff(Buff buff)
        {
            if (null == buff)
                return false;

            buff.SetOwner(mOwner);
            Buff oldBuff = GetBuff(buff.BuffId);
            if (null == oldBuff)
            {
                mBuffers[buff.Id] = buff;
                buff.RegisteBuffEffectType();
                mOwner.PlayerStatus.BuffAddStatusType(buff.Data, mOwner);
                buff.SendAddBuff();
                return true;
            }
            else
            {
                if (buff.Data.replaceMode == (int)eBuffReplaceMode.StackNum)
                {
                    mBuffers[buff.Id] = buff;
                    buff.RegisteBuffEffectType();
                }
                else
                {
                    oldBuff.Init(buff.LevelData);
                    oldBuff.SendUpdateBuff();
                    oldBuff.ReComputeAffectValue();
                }
                
            }

            return false;
        }


        private bool AddBuffBySkill(Buff buff, SkillActive skill)
        {
            if (null == buff)
                return false;

            buff.SetOwner(mOwner);
            buff.SetCaster(skill.Owner);
            Buff oldBuff = GetBuff(buff.BuffId);
            if (null == oldBuff)
            {
                mBuffers[buff.Id] = buff;
                buff.RegisteBuffEffectType();
                mOwner.PlayerStatus.BuffAddStatusType(buff.Data, mOwner);
                buff.SendAddBuff();
                return true;
            }
            else
            {
                if (buff.Data.replaceMode == (int)eBuffReplaceMode.StackNum)
                {
                    mBuffers[buff.Id] = buff;
                    buff.RegisteBuffEffectType();
                }
                else
                {
                    oldBuff.Init(buff.LevelData);
                    oldBuff.SendUpdateBuff();
                    oldBuff.ReComputeAffectValue();
                }

            }

            return false;
        }

        public bool DeleteBuffer(UInt64 id)
        {
            Buff tempBuff = GetBuff(id);
            if (null == tempBuff)
                return false;

            mBuffers.Remove(id);
            tempBuff.OnBuffEnd();
            mOwner.PlayerStatus.DelStatusType(tempBuff.Data.type);
            tempBuff.ReComputeAffectValue();
            tempBuff.SendRemoveBuff();
            tempBuff.ClearUp();

            return true;
        }

        /// <summary>
        /// 添加buff效果
        /// </summary>
        public void AddBufferEffect(eBuffEffectType type, Buff buff)
        {
            if (!mBufferEffects.ContainsKey(type))
                mBufferEffects[type] = new List<Buff>();
            mBufferEffects[type].Add(buff);
        }

        /// <summary>
        /// 移除指定类型的所有buff效果
        /// </summary>
        public bool RemoveBufferEffect(eBuffEffectType type)
        {
            if (!mBufferEffects.ContainsKey(type))
                return false;

            List<Buff> buffers = mBufferEffects[type];
            for (var i = 0; i < buffers.Count; i++)
            {
                DeleteBuffer(buffers[i].Id);
            }
            buffers.Clear();
            return true;
        }

        /// <summary>
        /// 移除指定类型的所有buff状态
        /// </summary>
        public bool RemoveBufferStatus(eBuffStatusType type)
        {
            List<Buff> buffers = mBuffers.Values.ToList();
            for (var i = 0; i < buffers.Count; i++)
            {
                if (buffers[i].Data.type == (int)type)
                    DeleteBuffer(buffers[i].Id);
            }
            buffers.Clear();
            return true;
        }

        /// <summary>
        /// 更新buff影响属性
        /// </summary>
        /// <param name="type"></param>
        public void UpdateBufferEffect(eBuffEffectType type)
        {
            if (!mBufferEffects.ContainsKey(type))
                return;

            foreach (var buff in mBufferEffects[type])
            {
                buff.ReComputeAffectValue();
            }
        }

        /// <summary>
        /// 刷新buff影响属性
        /// </summary>
        /// <param name="type"></param>
        public void RefreshAffectValue(eBuffEffectType type)
        {
            float tempValue = 0;
            foreach (var buff in mBuffers.Values)
            {
                if (buff.LogicType == eBuffLogicType.Fixed)
                    tempValue += buff.GetEffectValue(type);
            }
            switch (type)
            {
                case eBuffEffectType.影响生命百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.HPRate, tempValue);
                    break;
                case eBuffEffectType.影响生命上限百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.MaxHPRate, tempValue);
                    break;
                case eBuffEffectType.影响移动速度百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.SpeedRate, tempValue);
                    break;
                case eBuffEffectType.影响攻击力百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.AtkRate, tempValue);
                    break;
                case eBuffEffectType.影响闪避百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.DodgeRate, tempValue);
                    break;
                case eBuffEffectType.影响命中百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.HitRate, tempValue);
                    break;
                case eBuffEffectType.影响防御百分比:
                    mOwner.SetAttrBasePer(eValueType.Buff, eSkillAttrIndex.Def_All, tempValue);
                    break;
            }
            mOwner.UpdateAttr();

        }

        public Buff GetBuff(UInt64 id)
        {
            if (mBuffers.ContainsKey(id))
                return mBuffers[id];

            return null;
        }

        public Buff GetBuff(int buffId)
        {
            foreach (var val in mBuffers)
            {
                if (val.Value.BuffId == buffId)
                    return val.Value;
            }
            return null;
        }

        public Dictionary<UInt64, Buff> Buffers
        {
            get { return mBuffers; }
        }

        private bool CheckReplace(CSTable.BuffLevelData buffData)
        {
            Buff buff = GetBuff(buffData.id);
            if (null == buff)
                return true;

            if (buff.Data.replaceMode == (int)eBuffReplaceMode.NoReplace)
                return false;

            if (buff.Data.replaceMode == (int)eBuffReplaceMode.ReplaceLevel)
            {
                if (buffData.level < buff.Level)
                    return false;
            }

            return true;
        }

        public void ClearUp()
        {
            mBuffers.Clear();
            mBufferEffects.Clear();
            mOwner.PlayerStatus.CleanUp();
        }

    }
}
