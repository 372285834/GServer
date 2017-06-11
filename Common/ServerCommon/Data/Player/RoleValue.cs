using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ServerCommon.Planes;
using CSCommon;

namespace CSCommon.Data
{
    public enum eValueType
    {
        Base,
        Equip,
        Gem,
        Buff,
        Skill,
        Achieve,
        Image,
        MAX,
    }

    /// <summary>
    /// 人物一级属性
    /// </summary>
    //public struct RolePoint
    //{
    //    public ushort Power;
    //    public ushort Body;
    //    public ushort Dex;
    //    // overload operator +
    //    public static RolePoint operator +(RolePoint a, RolePoint b)
    //    {
    //        return new RolePoint()
    //        {
    //            Power = (ushort)(a.Power + b.Power),
    //            Body = (ushort)(a.Body + b.Body),
    //            Dex = (ushort)(a.Dex + b.Dex),
    //        };
    //    }

    //    public void Reset()
    //    {
    //        Power = 0;
    //        Body = 0;
    //        Dex = 0;
    //    }

    //}

    public class AttrStruct : RPC.IAutoSaveAndLoad
    {
        [RPC.AutoSaveLoad(true)]
        public byte attrIdx { get; set; }

        [RPC.AutoSaveLoad(true)]
        public string attrValue { get; set; }
    }
     
    //[StructLayout(LayoutKind.Explicit)]
    //struct AttriValue
    //{
    //    [FieldOffset(0)]
    //    int IntVal;
    //    [FieldOffset(0)]
    //    float FloatVal;
    //    [FieldOffset(0)]
    //    Int64 LongVal;
    //}


    /// <summary>
    /// 人物二级属性
    /// </summary>
    public class RoleValue
    {
        public Dictionary<eValueType, GameAttribute[]> mAttrs = new Dictionary<eValueType, GameAttribute[]>(); //一级属性
        public Dictionary<eValueType, GameAttribute[]> mSecondAttrs = new Dictionary<eValueType, GameAttribute[]>(); //二级属性

        public RoleValue()
        {
            for (var i = eValueType.Base; i < eValueType.MAX; i++)
            {
                mAttrs[i] = new GameAttribute[(int)eSkillAttrIndex.MAX];
                mSecondAttrs[i] = new GameAttribute[(int)eSkillAttrIndex.MAX];
                for (var j = 0; j < (int)eSkillAttrIndex.MAX; j++)
                {
                    mAttrs[i][j] = new GameAttribute();
                    mSecondAttrs[i][j] = new GameAttribute();
                }
            }
        }

        public ushort Power //内功
        {
            get { return (ushort)GetAttrValue(eSkillAttrIndex.Power); }
        }
        public ushort Body //外功
        {
            get { return (ushort)GetAttrValue(eSkillAttrIndex.Body); }
        }
        public ushort Dex //身法
        {
            get { return (ushort)GetAttrValue(eSkillAttrIndex.Dex); }
        }
        public float GetDef(eElemType elemType)
        {
            return GetAttrValue(CommonUtil.ElemTypeTranslate(elemType));
        }
        public float AllDefRate //全防御百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.Def_All); }
        }
        public int MaxHP
        {
            get { return GetAttrValue(eSkillAttrIndex.MaxHP); }
        }
        public float MaxHPRate
        {
            get { return GetAttrValue(eSkillAttrIndex.MaxHPRate); }
        }
        public int MaxMP
        {
            get { return GetAttrValue(eSkillAttrIndex.MaxMP); }
        }
        public int Atk                //攻击
        {
            get { return GetAttrValue(eSkillAttrIndex.Atk); }
        }
        public float Crit             //暴击
        {
            get { return GetAttrValue(eSkillAttrIndex.Crit); }
        }
        public float CritRate             //暴击百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.CritRate); }
        }
        public float CritDef          //暴抗
        {
            get { return GetAttrValue(eSkillAttrIndex.CritDef); }
        }
        public float CritDefRate         //暴抗百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.CritDefRate); }
        }
        public int DeadlyHit        //致命一击
        {
            get { return GetAttrValue(eSkillAttrIndex.DeadlyHit); }
        }
        public float DeadlyHitRate        //致命一击百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.DeadlyHitRate); }
        }
        public int Hit                //命中值
        {
            get { return GetAttrValue(eSkillAttrIndex.Hit); }
        }
        public int Dodge              //闪避值
        {
            get { return GetAttrValue(eSkillAttrIndex.Dodge); }
        }
        public float UpHurtRate           //伤害加深百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.UpHurt); }
        }
        public float DownHurtRate         //伤害减免百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.DownHurt); }
        }
        public float UnusualDefRate       //异常状态抗性百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.UnusualDefRate); }
        }
        public float Block             //格挡值
        {
            get { return GetAttrValue(eSkillAttrIndex.Block); }
        }
        public float BlockRate         //格挡率
        {
            get { return GetAttrValue(eSkillAttrIndex.BlockRate); }
        }
        public int HPRecover          //HP恢复 
        {
            get { return GetAttrValue(eSkillAttrIndex.HPRecover); }
        }
        public int MPRecover          //MP恢复 
        {
            get { return GetAttrValue(eSkillAttrIndex.MPRecover); }
        }
        public float UpExpRate        //提升杀怪经验百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.UpExpRate); }
        }
        public float Speed             //移动速度
        {
            get { return GetAttrValue(eSkillAttrIndex.MoveSpeed); }
        }
        public float DamageReflect      //伤害反射百分比
        {
            get { return GetAttrValue(eSkillAttrIndex.DamageReflect); }
        }

        public void Serialize(RPC.DataWriter dw)
        {
            for (var i = eElemType.Gold; i < eElemType.MAX; i++)
            {
                dw.Write(GetDef(i));
            }
            dw.Write(AllDefRate);
            dw.Write(MaxHPRate);
            dw.Write(MaxHP);
            dw.Write(MaxMP);
            dw.Write(Atk);
            dw.Write(Crit);
            dw.Write(CritRate);
            dw.Write(CritDef);
            dw.Write(CritDefRate);
            dw.Write(DeadlyHit);
            dw.Write(DeadlyHitRate);
            dw.Write(Hit);
            dw.Write(Dodge);
            dw.Write(UpHurtRate);
            dw.Write(DownHurtRate);
            dw.Write(UnusualDefRate);
            dw.Write(Block);
            dw.Write(BlockRate);
            dw.Write(HPRecover);
            dw.Write(MPRecover);
            dw.Write(UpExpRate);
            dw.Write(Speed);
            dw.Write(DamageReflect);
        }

        public void Deserizle(RPC.DataReader dr)
        {
            for (var i = eElemType.Gold; i < eElemType.MAX; i++)
            {
                SetSecondAttrBase(eValueType.Image, CommonUtil.ElemTypeTranslate(i), (int)dr.ReadSingle());
            }
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Def_All, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.MaxHPRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.MaxHP, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.MaxMP, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Atk, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Crit, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.CritRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.CritDef, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.CritDefRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.DeadlyHit, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.DeadlyHitRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Hit, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Dodge, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.UpHurt, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.DownHurt, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.UnusualDefRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.Block, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.BlockRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.HPRecover, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.MPRecover, dr.ReadInt32());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.UpExpRate, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.MoveSpeed, (int)dr.ReadSingle());
            SetSecondAttrBase(eValueType.Image, eSkillAttrIndex.DamageReflect, (int)dr.ReadSingle());
        }

        public void SetAttrBase(eValueType type, eSkillAttrIndex idx, int value)
        {
            mAttrs[type][(int)idx].Base = value;
        }

        public void SetSecondAttrBase(eValueType type, eSkillAttrIndex idx, int value)
        {
            mSecondAttrs[type][(int)idx].Base = value;
        }

        public void AddAttrBase(eValueType type, eSkillAttrIndex idx, int value)
        {
            mAttrs[type][(int)idx].AddBase(value);
        }

        public void SetAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            mAttrs[type][(int)idx].SetBasePer(value);
        }

        public void AddAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            mAttrs[type][(int)idx].AddBasePer(value);
        }

        public void AddSecondAttrBase(eValueType type, eSkillAttrIndex idx, int value)
        {
            mSecondAttrs[type][(int)idx].AddBase(value);
        }

        public void SetSecondAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            mSecondAttrs[type][(int)idx].SetBasePer(value);
        }

        public void AddSecondAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            mSecondAttrs[type][(int)idx].AddBasePer(value);
        }

        /// <summary>
        /// 获取最终属性值
        /// </summary>
        public int GetAttrValue(eSkillAttrIndex idx)
        {
            int currValue = 0;

            currValue += GetAttrValueMain(idx);
            currValue += GetAttrValueEx(idx);

            return currValue;
        }

        public int GetAttrBase(eValueType type, eSkillAttrIndex idx)
        {
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
            {
                return mAttrs[type][(int)idx].Base;
            }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
            {
                return mSecondAttrs[type][(int)idx].Base;
            }
            return 0;
        }

        public int GetAttrValueMain(eSkillAttrIndex idx)
        {
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
            {
                int tempValue = 0;
                for (var i = eValueType.Base; i < eValueType.MAX; i++)
                {
                    tempValue += mAttrs[i][(int)idx].Value;
                }
                return tempValue;
            }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
            {
                int tempValue = 0;
                for (var i = eValueType.Base; i < eValueType.MAX; i++)
                {
                    tempValue += mSecondAttrs[i][(int)idx].Value;
                }
                return tempValue;
            }
            return 0;
        }

        public int GetAttrValueEx(eSkillAttrIndex idx)
        {
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
            {

            }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
            {
                //float tempValue = 0;
                //tempValue = BuffMgr.GetEffectValue(BuffMgr.BufferTypeTranslate(idx));
                //if (tempValue < 0 && tempValue + GetAttrValueMain(idx) < 0)
                //    tempValue = -1 * GetAttrValueMain(idx);

                //return tempValue;
            }
            return 0;
        }

        public void ClearUp()
        {
            for (var idx = eSkillAttrIndex.None; idx < eSkillAttrIndex.MAX; idx++)
            {
                for (var type = eValueType.Base; type < eValueType.MAX; type++)
                {
                    mAttrs[type][(int)idx].reset();
                    mSecondAttrs[type][(int)idx].reset();
                }
            }
        }

        //public void AddAllDef(int value)
        //{
        //    for (int i = 0; i < this.Def.Length; i++)
        //    {
        //        Def[i] += value;
        //    }
        //}
        //public void ReduceAllDef(int value)
        //{
        //    for (int i = 0; i < this.Def.Length; i++)
        //    {
        //        Def[i] -= value;
        //    }
        //}

        //public RoleValue AddValue(RoleValue value)
        //{
        //    for (int i = 0; i < this.Def.Length; i++)
        //    {
        //        Def[i] += value.Def[i];
        //    }            
        //    this.MaxHP += value.MaxHP;
        //    this.MaxMP += value.MaxMP;
        //    this.Atk += value.Atk;
        //    this.Crit += value.Crit;
        //    this.CritDef += value.CritDef;
        //    this.Hit += value.Hit;
        //    this.Dodge += value.Dodge;
        //    this.DeadlyHit += value.DeadlyHit;
        //    this.UpHurtRate += value.UpHurtRate;
        //    this.DownHurtRate += value.DownHurtRate;
        //    this.UnusualDefRate += value.UnusualDefRate;
        //    this.Block += value.Block;
        //    this.BlockRate += value.BlockRate;
        //    this.HPRecover += value.HPRecover;
        //    this.MPRecover += value.MPRecover; 
        //    this.Speed += value.Speed;
        //    this.DamageReflect += value.DamageReflect;
        //    return this;
        //}

        //public void Reset()
        //{
        //    for (int i = 0; i < this.Def.Length; i++)
        //    {
        //        Def[i] = 0;
        //    }
        //    this.MaxHP = 0;
        //    this.MaxMP = 0;
        //    this.Atk = 0;
        //    this.Crit = 0;
        //    this.CritDef = 0;
        //    this.Hit = 0;
        //    this.Dodge = 0;
        //    this.DeadlyHit = 0;
        //    this.UpHurtRate = 0;
        //    this.DownHurtRate = 0;
        //    this.UnusualDefRate = 0;
        //    this.Block = 0;
        //    this.BlockRate = 0;
        //    this.HPRecover = 0;
        //    this.MPRecover = 0;
        //    this.Speed = 0;
        //    this.DamageReflect = 0;
        //}
    }
}
