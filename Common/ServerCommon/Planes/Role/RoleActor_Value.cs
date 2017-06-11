using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{

    public partial class RoleActor
    {
        //public RoleValue BuffRoleValue = new RoleValue(); //buff数据
        public virtual RoleValue FinalRoleValue { get { return null; } }    //最终结果
        public List<eSkillAttrIndex> mUpdateAttrList = new List<eSkillAttrIndex>(); //更新属性列表

        public virtual int CurHP
        {
            set {  }
            get { return 0; }
        }

        public virtual int CurMP
        {
            set { }
            get { return 0; }
        }

        #region 战斗公式
        //static bool IsRateHit(float atk,float def,float atkRate,float defRate)
        //{
        //    var rate = atk / (atk + def) + atkRate - defRate;
        //    if (rate >= 1)
        //        return true;
        //    if (rate <= 0)
        //        return false;

        //    var hit = (int)(rate * ServerDefine.FightRandomMax);
        //    var r = ServerFrame.Util.Rand.Next(ServerDefine.FightRandomMax);
        //    if (r <= hit)
        //        return true;

        //    return false;
        //}

        // 1、免伤公式
        // 免伤率=防御/（防御参数+防御）*min（0.9/0.8,防御方等级/攻击方等级）
        // 
        // 2、随机属性公式
        // 命中率=命中等级/3000
        // 闪避率=闪避等级/3000
        // 暴击率=暴击等级/3000
        // 暴抗率=暴抗等级/3000
        // 格挡率=格挡等级/3000
        // 
        // 战斗时先计算是否命中，命中概率=攻击方命中率/（攻击方命中率+防御方闪避率），
        // 如果命中进入减伤和伤害加深的计算
        // 之后再判断是否暴击，暴击概率=攻击方暴击率/（攻击方暴击率+防御方暴抗率），暴击的话乘以暴击系数，不暴击按正常伤害
        // 最后判断是否格挡，格挡的话在已有伤害的基础上乘以50%
        //public static eHitType CheckHitType(RoleActor caster, RoleActor rev)
        //{

        //    RoleValue casterVal = caster.FinalRoleValue;
        //    RoleValue revVal = rev.FinalRoleValue;
        //    if (casterVal.Hit == 0)
        //        casterVal.Hit = 1;
        //    var hit = (int)((float)casterVal.Hit / (casterVal.Hit + revVal.Dodge) * ServerDefine.FightRandomMax);
        //    var r = ServerFrame.Util.Rand.Next(ServerDefine.FightRandomMax);

        //    if (IsRateHit(casterVal.Hit, revVal.Dodge,0,0))
        //    {//命中

        //        if (IsRateHit(0, 1, casterVal.DeadlyHit / 3000 + casterVal.DeadlyHitRate, 0))
        //        {//致命
        //            return eHitType.Deadly;
        //        }

        //        if (IsRateHit(casterVal.Crit, revVal.CritDef,casterVal.CritRate,revVal.CritDefRate))
        //        {//暴击
        //            return eHitType.Crit;
        //        }

        //        if (IsRateHit(0, 1, rev.FinalRoleValue.Block / 3000 + rev.FinalRoleValue.BlockRate, 0))
        //        {//格挡
        //            return eHitType.Block;
        //        }

        //        return eHitType.Hit;
        //    }
        //    else
        //        return eHitType.Miss;
        //}


        //免伤率=防御/（防御参数+防御）*min（0.9/0.8,防御方等级/攻击方等级）							
        //其中防御参数=1400	
        //public static int CalcDamage(RoleActor caster, RoleActor rev, eHitType hType,float powerPercent,int extradmg,bool isBuff)
        //{
        //    if (hType == eHitType.Miss)
        //        return 0;
        //    if (hType != eHitType.AddHp)
        //    {
        //        var skAtk = caster.FinalRoleValue.Atk * powerPercent + extradmg;
        //        var def = rev.FinalRoleValue.Def[(int)rev.ElemType];
        //        //var dmg = skAtk * caster.RoleLevel / rev.RoleLevel * skAtk / (skAtk + def);
        //        var defRate = def / (1400 + def) * Math.Min(0.9 / 0.8, rev.RoleLevel / caster.RoleLevel);
        //        if (defRate > 1)
        //            defRate = 1;
        //        if (defRate < 0)
        //            defRate = 0;

        //        var dmg = skAtk * (1 - defRate);//skAtk * skAtk / (skAtk + def);
        //        dmg = dmg * (1 + caster.FinalRoleValue.UpHurtRate - rev.FinalRoleValue.DownHurtRate);      //伤害加深或者削弱

        //        if (hType == eHitType.Crit)
        //        {
        //            dmg = (int)(dmg * 1.5f);
        //        }
        //        else if (hType == eHitType.Deadly)
        //        {
        //            dmg = (int)(dmg * 2.5f);
        //        }
        //        else if (hType == eHitType.Block)
        //        {
        //            dmg = (int)(dmg * 0.5f);
        //        }

        //        return (int)dmg;
        //    }
        //    else
        //    {
        //        var dmg = caster.FinalRoleValue.Atk * powerPercent + extradmg;
        //        if (hType == eHitType.Crit)
        //        {
        //            dmg = (int)(dmg * 1.5f);
        //        }
        //        return (int)dmg;
        //    }          

        //}

        //public void ReceiveDamage(int damage, RoleActor caster)
        //{
        //    int remainDamage = damage;
        //    foreach (var buff in BuffMgr.Buffs())
        //    {
        //        remainDamage = buff.ReduceShildValue(remainDamage);
        //        if (remainDamage == 0)
        //            break;
        //    }

        //    if (remainDamage < damage)
        //    {
        //        var player = this as PlayerInstance;
        //        if (player != null)
        //        {
        //            var pkg = new RPC.PackageWriter();
        //            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, Id).RPC_SkillReceiveData(pkg, 0, 0, (byte)eHitType.Shield, damage - remainDamage);
        //            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        //        }
        //    }

        //    ChangeHP(-damage, caster);
        //}
        #endregion

        public virtual void ChangeHP(int deltaVal,RoleActor caster)
        {
            if (deltaVal == 0)
                return;

            int tempCurHp = CurHP;
            tempCurHp += deltaVal;
            if (tempCurHp > FinalRoleValue.MaxHP)
                tempCurHp = FinalRoleValue.MaxHP;
            CurHP = tempCurHp;
            if (CurHP <= 0)
            {
                CurHP = 0;
                OnDie();
            }
        }

        public virtual void ChangeMP(int deltaVal)
        {
            if (deltaVal == 0)
                return;

            int tempCurMp = CurMP;
            tempCurMp += deltaVal;
            if (tempCurMp > FinalRoleValue.MaxMP)
                tempCurMp = FinalRoleValue.MaxMP;
            CurMP = tempCurMp;
            if (CurMP < 0)
            {
                CurMP = 0;
            }
        }

        public void OnAttrChanged(eSkillAttrIndex attrIdx)
        {
            if (mUpdateAttrList.Find(x => x.Equals(attrIdx)) != eSkillAttrIndex.None)
                return;

            OnAttrChangedMain(attrIdx);
            OnAttrChangedEx(attrIdx);
        }

        public void OnAttrChangedMain(eSkillAttrIndex attrIdx)
        {
            if (mUpdateAttrList.Find(x => x == attrIdx) != eSkillAttrIndex.None)
                return;

            mUpdateAttrList.Add(attrIdx);
        }

        public void OnAttrChangedEx(eSkillAttrIndex attrIdx)
        {

        }

        public virtual void UpdateAttr()
        {
            if (mUpdateAttrList.Count == 0)
                return;

            if (CurHP > FinalRoleValue.MaxHP)
            {
                CurHP = FinalRoleValue.MaxHP;
                OnAttrChanged(eSkillAttrIndex.HP);
            }
            if (CurMP > FinalRoleValue.MaxMP)
            {
                CurMP = FinalRoleValue.MaxMP;
                OnAttrChanged(eSkillAttrIndex.MP);
            }
            List<AttrStruct> attrs = new List<AttrStruct>();
            foreach (var idx in mUpdateAttrList)
            {
                if (!IsBroadCastAttr(idx))
                    continue;

                AttrStruct attr = new AttrStruct();
                if (!GetAttrStruct(ref attr, idx))
                    continue;

                attrs.Add(attr);
            }
            mUpdateAttrList.Clear();
            if (attrs.Count == 0)
                return;

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            RPC.DataWriter dw = new RPC.DataWriter();
            RPC.IAutoSaveAndLoad.DaraWriteList<AttrStruct>(attrs, dw, true);
            Wuxia.H_RpcRoot.smInstance.HIndex(pkg, this.Id).RPC_UpdateRoleAttr(pkg, dw);
            HostMap.SendPkg2Clients(null, GetPosition(), pkg);
        }

        /// <summary>
        /// 是否广播该属性
        /// </summary>
        /// <param name="idx"></param>
        public bool IsBroadCastAttr(eSkillAttrIndex idx)
        {
            switch (idx)
            {
                case eSkillAttrIndex.Level:
                case eSkillAttrIndex.HP:
                case eSkillAttrIndex.MP:
                case eSkillAttrIndex.MaxHP:
                case eSkillAttrIndex.MaxHPRate:
                case eSkillAttrIndex.MaxMP:
                case eSkillAttrIndex.MoveSpeed:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        public bool GetAttrStruct(ref AttrStruct attr, eSkillAttrIndex idx)
        {
            if (idx <= eSkillAttrIndex.None || idx >= eSkillAttrIndex.MAX)
                return false;

            attr.attrIdx = (byte)idx;
            switch (idx)
            {
                case eSkillAttrIndex.Level:
                    attr.attrValue = mRoleLevel.ToString();
                    break;
                case eSkillAttrIndex.HP:
                    attr.attrValue = CurHP.ToString();
                    break;
                case eSkillAttrIndex.MaxHP:
                case eSkillAttrIndex.MaxHPRate:
                    attr.attrValue = FinalRoleValue.MaxHP.ToString();
                    break;
                case eSkillAttrIndex.MP:
                    attr.attrValue = CurMP.ToString();
                    break;
                case eSkillAttrIndex.MaxMP:
                    attr.attrValue = FinalRoleValue.MaxMP.ToString();
                    break;
                case eSkillAttrIndex.MoveSpeed:
                    attr.attrValue = FinalRoleValue.Speed.ToString();
                    break;
                default:
                    return false;
            }

            return true;
        }

        public void SetAttrBase(eValueType type, eSkillAttrIndex idx, int value)
        {
	        OnAttrChanged(idx);
	        if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
	        {
		        FinalRoleValue.SetAttrBase(type, idx, value);
		        return ;
	        }
	        if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
	        {
		        FinalRoleValue.SetSecondAttrBase(type, idx, value);
	        }
        }

        public void AddAttrBase(eValueType type, eSkillAttrIndex idx, int value) 
        {
	        OnAttrChanged(idx);
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
	        {
                FinalRoleValue.AddAttrBase(type, idx, value);
		        return;
	        }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
	        {
                FinalRoleValue.AddSecondAttrBase(type, idx, value);
	        }
        }

        public void SetAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            OnAttrChanged(idx);
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
            {
                FinalRoleValue.SetAttrBasePer(type, idx, value);
                return;
            }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
            {
                FinalRoleValue.SetSecondAttrBasePer(type, idx, value);
            }
        }

        public void AddAttrBasePer(eValueType type, eSkillAttrIndex idx, float value)
        {
            OnAttrChanged(idx);
            if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
            {
                FinalRoleValue.AddAttrBasePer(type, idx, value);
                return;
            }
            if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
            {
                FinalRoleValue.AddSecondAttrBasePer(type, idx, value);
            }
        }

        #region 设置附加属性，暂时不需要
        //public void AddAttrBaseAdd(eValueType type, eSkillAttrIndex idx, int value) 
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].AddBaseAdd(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].AddBaseAdd(value);
        //    }
        //}

        //public void ResetAttrBaseAdd(eSkillAttrIndex idx)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].ReSetBaseAdd();
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].ReSetBaseAdd();
        //    }
        //}

        //public void AddAttrBasePercent(eSkillAttrIndex idx, int value)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].AddBasePer(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].AddBasePer(value);
        //    }
        //}

        //public void SetAttrBaseDelta(eSkillAttrIndex idx, int value)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].SetBaseDelta(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].SetBaseDelta(value);
        //    }
        //}

        //public void AddAttrBaseDelta(eSkillAttrIndex idx, int value)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].AddBaseDelta(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].AddBaseDelta(value);
        //    }
        //}

        //public void AddAttrPercent(eSkillAttrIndex idx, int value)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].AddPer(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].AddPer(value);
        //    }
        //}

        //public void AddAttrDelta(eSkillAttrIndex idx, int value)
        //{
        //    OnAttrChanged(idx);
        //    if (idx >= eSkillAttrIndex.Power && idx <= eSkillAttrIndex.Dex)
        //    {
        //        FinalRoleValue.mAttrs[(int)idx].AddDelta(value);
        //        return;
        //    }
        //    if (idx >= eSkillAttrIndex.HP && idx < eSkillAttrIndex.MAX)
        //    {
        //        FinalRoleValue.mSecondAttrs[(int)idx].AddDelta(value);
        //    }
        //}
        #endregion

        #region 添加装备属性
        //public void AddEquipAttri(CSCommon.eEquipValueType type, int tplValue)
        //{
        //    switch (type)
        //    {
        //        case eEquipValueType.Power:
        //            ////outValue.Power += Convert.ToUInt16(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Power, tplValue);
        //            break;
        //        case eEquipValueType.Body:
        //            ////outValue.Body += Convert.ToUInt16(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Body, tplValue);
        //            break;
        //        case eEquipValueType.Dex:
        //            ////outValue.Dex += Convert.ToUInt16(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Dex, tplValue);
        //            break;
        //        case eEquipValueType.Hp:
        //            ////outValue.MaxHP += Convert.ToInt32(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.MaxHP, tplValue);
        //            break;
        //        case eEquipValueType.HpRate:
        //            //outValue.MaxHPRate += tplValue;
        //            AddAttrBasePercent(eSkillAttrIndex.HP, tplValue);
        //            break;
        //        case eEquipValueType.Mp:
        //            //outValue.MaxMP += Convert.ToInt32(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.MP, tplValue);
        //            break;
        //        case eEquipValueType.Atk:
        //            //outValue.Atk += Convert.ToInt32(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Atk, tplValue);
        //            break;
        //        case eEquipValueType.GoldDef:
        //            //outValue.Def[0] += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_Gold, tplValue);
        //            break;
        //        case eEquipValueType.WoodDef:
        //            //outValue.Def[1] += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_Wood, tplValue);
        //            break;
        //        case eEquipValueType.WaterDef:
        //            //outValue.Def[2] += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_Water, tplValue);
        //            break;
        //        case eEquipValueType.FireDef:
        //            //outValue.Def[3] += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_Fire, tplValue);
        //            break;
        //        case eEquipValueType.EarthDef:
        //            //outValue.Def[4] += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_Earth, tplValue);
        //            break;
        //        case eEquipValueType.AllDef:
        //            //outValue.AddAllDef(Convert.ToInt32(tplValue));
        //            AddAttrBaseDelta(eSkillAttrIndex.Def_All, tplValue);
        //            break;
        //        case eEquipValueType.AllDefRate:
        //            //outValue.AllDefRate += tplValue;
        //            AddAttrBasePercent(eSkillAttrIndex.Def_All, tplValue);
        //            break;
        //        case eEquipValueType.Crit:
        //            //outValue.Crit += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.Crit, tplValue);
        //            break;
        //        case eEquipValueType.CritRate:
        //            //outValue.CritRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.CritRate, tplValue);
        //            break;
        //        case eEquipValueType.CritDef:
        //            //outValue.CritDef += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.CritDef, tplValue);
        //            break;
        //        case eEquipValueType.CritDefRate:
        //            //outValue.CritDefRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.CritDefRate, tplValue);
        //            break;
        //        case eEquipValueType.Hit:
        //            //outValue.Hit += Convert.ToInt32(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Hit, tplValue);
        //            break;
        //        case eEquipValueType.Dodge:
        //            //outValue.Dodge += Convert.ToInt32(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.Dodge, tplValue);
        //            break;
        //        case eEquipValueType.Move:
        //            //outValue.Speed += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.MoveSpeed, tplValue);
        //            break;
        //        case eEquipValueType.UpExpRate:
        //            //outValue.UpExpRate += Convert.ToUInt16(tplValue);
        //            AddAttrBaseDelta(eSkillAttrIndex.UpExpRate, tplValue);
        //            break;
        //        case eEquipValueType.DsRate:
        //            //outValue.DeadlyHitRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.DeadlyHitRate, tplValue);
        //            break;
        //        case eEquipValueType.UpHurt:
        //            //outValue.UpHurtRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.UpHurt, tplValue);
        //            break;
        //        case eEquipValueType.DownHurt:
        //            //outValue.DownHurtRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.DownHurt, tplValue);
        //            break;
        //        case eEquipValueType.UnusualDefRate:
        //            //outValue.UnusualDefRate += tplValue;
        //            AddAttrBaseDelta(eSkillAttrIndex.UnusualDefRate, tplValue);
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region 添加被动技能属性
        //public void AddSkillValue(ePassiveEffect type, float tplValue)//, ref RolePoint outValue)
        //{
        //    switch (type)
        //    {
        //        case ePassiveEffect.提升内功:
        //            //outValue.Power += (ushort)tplValue;
        //            break;
        //        case ePassiveEffect.提升外功:
        //            //outValue.Body += (ushort)tplValue;
        //            break;
        //        case ePassiveEffect.提升身法:
        //            //outValue.Dex += (ushort)tplValue;
        //            break;
        //        case ePassiveEffect.伤害反射百分比:
        //            //outValue.DamageReflect += tplValue;
        //            break;
        //        case ePassiveEffect.提升伤害增加百分比:
        //            //outValue.UpHurtRate += tplValue;
        //            break;
        //        case ePassiveEffect.提升伤害减免百分比:
        //            //outValue.DownHurtRate += tplValue;
        //            break;
        //        case ePassiveEffect.提升致命一击几率:
        //            //outValue.DeadlyHitRate += tplValue;
        //            break;
        //        case ePassiveEffect.提升最大生命值:
        //            //outValue.MaxHP += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升最大法力值:
        //            //outValue.MaxMP += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升攻击力:
        //            //outValue.Atk += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升命中值:
        //            //outValue.Hit += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升闪避值:
        //            //outValue.Dodge += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升金防御:
        //            //outValue.Def[(int)eElemType.Gold] += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升木防御:
        //            //outValue.Def[(int)eElemType.Wood] += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升水防御:
        //            //outValue.Def[(int)eElemType.Water] += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升火防御:
        //            //outValue.Def[(int)eElemType.Fire] += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升土防御:
        //            //outValue.Def[(int)eElemType.Earth] += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升全防御:
        //            //outValue.AddAllDef((int)tplValue);
        //            break;
        //        case ePassiveEffect.提升移动值:
        //            //outValue.Speed += tplValue;
        //            break;
        //        case ePassiveEffect.提升异常抗性百分比:
        //            //outValue.UnusualDefRate += tplValue;
        //            break;
        //        case ePassiveEffect.提升暴击值:
        //            //outValue.Crit += (int)tplValue;
        //            break;
        //        case ePassiveEffect.提升暴抗值:
        //            //outValue.CritDef += (int)tplValue;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion


    }
}
