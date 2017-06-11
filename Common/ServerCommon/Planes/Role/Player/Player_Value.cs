using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        RoleValue mFinalRoleValue = new RoleValue();

        ////一级属性
        //public RolePoint FinalPoint;   //最终点数
        //public RolePoint BasePoint;     //人物基础点数
        //public RolePoint EquipPoint;    //装备附加点数
        //public RolePoint BuffPoint;    //buff附加点数
        //public RolePoint GemPoint;      //宝石叠加数据
        //public RolePoint SkillPoint;    //被动技能附加数据;

        ////二级属性
        public override RoleValue FinalRoleValue { get { return mFinalRoleValue; } }    //最终结果
        //public RoleValue EquipRoleValue = new RoleValue();//装备叠加数据
        //public RoleValue GemRoleValue = new RoleValue();//宝石叠加数据
        //public RoleValue SkillRoleValue = new RoleValue();    //被动技能附加数据;

        //public RoleValue PointToValue = new RoleValue();//由人物属性转化的数值
        //public RoleValue BaseRoleValue = new RoleValue();//人物基础数据


        public override int CurHP
        {
            set { this.PlayerData.RoleDetail.RoleHp = value; }
            get { return this.PlayerData.RoleDetail.RoleHp; }
        }
        
        public override int CurMP
        {
            set { this.PlayerData.RoleDetail.RoleMp = value; }
            get { return this.PlayerData.RoleDetail.RoleMp; }
        }
       
        /// <summary>
        /// 某类变化，单独计算
        /// </summary>
        /// <param name="eType"></param>
        public void CalcChangeValue(eValueType eType)
        {
            //CalcChangeType(eType);
            //switch (eType)
            //{
            //    case eValueType.BaseRole:
            //        CalcBaseRoleValue();
            //        break;
            //    case eValueType.Equip:
            //        CalcEquipValue();
            //        break;
            //    case eValueType.Gem:
            //        CalcGemValue();
            //        break;
            //    case eValueType.Buff:
            //        CalcBuffValue();
            //        break;
            //    case eValueType.Skill:
            //        CalcSkillValue();
            //        break;
            //    default:
            //        break;
            //}
            //CalcPointToValue();
            //_CalcFinalValue();
        }

        public void CalcChangeType(eValueType eType)
        {
            //switch (eType)
            //{
            //    case eValueType.Equip:
            //        CalcEquipPoint();
            //        break;
            //    case eValueType.Gem:
            //        CalcGemPoint();
            //        break;
            //    case eValueType.Buff:
            //        CalcBuffPoint();
            //        break;
            //    case eValueType.Skill:
            //        CalcSkillPoint();
            //        break;
            //    default:
            //        break;
            //}
            //_CaclFinalPoint();
        }

        public void CalcAllValues()
        {
            CalcAllPoint();
            CalcBaseRoleValue();
            CalcPointToValue();
            CalcEquipValue();
            CalcBuffValue();
            CalcSkillPoint();
            _CalcFinalValue();
        }

        public void CalcAllOffValues()
        {
            CalcAllOffPoint();
            CalcBaseRoleValue();
            CalcPointToValue();
            CalcEquipValue();
            CalcSkillPoint();
            _CalcFinalValue();
        }

        private void _CalcFinalValue()
        {
            //FinalRoleValue.Reset();
            //FinalRoleValue.AddValue(BaseRoleValue);
            //FinalRoleValue.AddValue(PointToValue);
            //FinalRoleValue.AddValue(EquipRoleValue);
            //FinalRoleValue.AddValue(BuffRoleValue);
            //FinalRoleValue.AddValue(SkillRoleValue);

            if (mPlayerData.RoleDetail.RoleMaxHp != FinalRoleValue.MaxHP)
                mPlayerData.RoleDetail.RoleMaxHp = FinalRoleValue.MaxHP;
            if (mPlayerData.RoleDetail.RoleMaxMp != FinalRoleValue.MaxMP)
                mPlayerData.RoleDetail.RoleMaxMp = FinalRoleValue.MaxMP;
            if (mPlayerData.RoleDetail.RoleSpeed != FinalRoleValue.Speed)
                mPlayerData.RoleDetail.RoleSpeed = FinalRoleValue.Speed;

            if (CurHP > FinalRoleValue.MaxHP)
                CurHP = FinalRoleValue.MaxHP;
            if (CurMP > FinalRoleValue.MaxMP)
                CurMP = FinalRoleValue.MaxMP;
        }

        #region 一级属性计算
        /// <summary>
        /// 合成所有一级属性
        /// </summary>
        public void CalcAllPoint()
        {
            CalcBasePoint();
            CalcEquipPoint();
            CalcGemPoint();
            CalcBuffPoint();
            CalcSkillPoint();
            _CaclFinalPoint();
        }

        public void CalcAllOffPoint()
        {
            CalcBasePoint();
            CalcEquipPoint();
            CalcGemPoint();
            CalcSkillPoint();
            _CaclFinalPoint();
        }

        private void _CaclFinalPoint()
        {
            //FinalPoint = BasePoint + EquipPoint + BuffPoint + GemPoint + SkillPoint;
        }

        /// <summary>
        /// 计算装备带来的一级属性
        /// </summary>
        public void CalcBasePoint()
        {
            var lvData = CSTable.StaticDataManager.PlayerLevel[this.RoleLevel, (byte)this.RolePro];
            //BasePoint.Power = (ushort)(PlayerData.RoleDetail.PowerPoint + lvData.power);
            //BasePoint.Body = (ushort)(PlayerData.RoleDetail.BodyPoint + lvData.body);
            //BasePoint.Dex = (ushort)(PlayerData.RoleDetail.DexPoint + lvData.dex);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Power, PlayerData.RoleDetail.PowerPoint);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Body, PlayerData.RoleDetail.BodyPoint);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Dex, PlayerData.RoleDetail.DexPoint);

            AddAttrBase(eValueType.Base, eSkillAttrIndex.Power, (int)lvData.power);
            AddAttrBase(eValueType.Base, eSkillAttrIndex.Body, (int)lvData.body);
            AddAttrBase(eValueType.Base, eSkillAttrIndex.Dex, (int)lvData.dex);
        }


        /// <summary>
        /// 计算buff技能带来的一级属性
        /// </summary>
        public void CalcBuffPoint()
        {
            //BuffPoint.Reset();
            //foreach (var buff in mBuffMgr.Buffs())
            //{
            //    AddBuffValue((eBuffEffectType)buff.LevelData.attri1, buff.LevelData.value1, ref BuffPoint);
            //    AddBuffValue((eBuffEffectType)buff.LevelData.attri2, buff.LevelData.value2, ref BuffPoint);
            //    AddBuffValue((eBuffEffectType)buff.LevelData.attri3, buff.LevelData.value3, ref BuffPoint);
            //}
        }

        /// <summary>
        /// 计算skill技能带来的一级属性
        /// </summary>
        public void CalcSkillPoint()
        {
            //SkillPoint.Reset();
            //foreach (var sk in mSkillMgr.list)
            //{
            //    if (sk.BaseType != eSkillType.BodyChannel && sk.BaseType != eSkillType.Passive)
            //        continue;
            //    var passive = sk as SkillPassive;
            //    if (passive != null)
            //    {
            //        AddSkillValue((ePassiveEffect)passive.LevelData.attri1, passive.LevelData.value1, ref SkillPoint);
            //        AddSkillValue((ePassiveEffect)passive.LevelData.attri2, passive.LevelData.value2, ref SkillPoint);
            //        AddSkillValue((ePassiveEffect)passive.LevelData.attri3, passive.LevelData.value3, ref SkillPoint);
            //    }
            //}
        }

        /// <summary>
        /// 计算装备带来的一级属性
        /// </summary>
        public void CalcEquipPoint()
        {
            //EquipPoint.Reset();
            foreach (var item in EquipBag.IterItems())
            {
                var temp = CSTable.ItemUtil.GetEquipLvTpl(item.ItemTemplate.ItemProfession, (item.ItemTemplate as CSTable.ItemEquipData).EquipType, item.ItemData.ItemLv);
                if (temp != null)
                {
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attri1), (int)temp.value1);
                }

                //精炼表
                var temp1 = CSTable.ItemUtil.GetEquipRefineLvTpl((item.ItemTemplate as CSTable.ItemEquipData).EquipType, item.ItemData.ItemRefineLv);
                if (temp1 != null)
                {
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri1), temp1.value1);
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri2), temp1.value2);
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri3), temp1.value3);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri4), temp1.value4);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri5), temp1.value5);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri6), temp1.value6);
                }
            }
        }

        /// <summary>
        /// 计算宝石带来的一级属性
        /// </summary>
        public void CalcGemPoint()
        {
            //GemPoint.Reset();
            foreach (var item in EquipGemBag.IterItems())
            {
                var temp = CSTable.ItemUtil.GetItem(item.ItemData.ItemTemlateId) as CSTable.ItemGemData;
                if (temp != null)
                {
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attr), (int)temp.value);
                }
            }
        }

        /// <summary>
        /// 计算成就带来的一级属性
        /// </summary>
        public void CalcAchievePoint()
        {
            //AchievePoint.Reset();
            var list = mRecordMgr.GetAchieveList(eAchieveType.AchieveName);
            foreach (var i in list)
            {
                if (i.IsFinished())
                {
                    var temp = i.mTemplate as CSTable.AchieveNameData;
                    if (temp != null)
                    {
                        AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attri1), (int)temp.value1);
                    }
                }
            }

        }
        #endregion

        #region 二级属性计算

        /// <summary>
        /// 计算人物等级基础属性
        /// </summary>
        public void CalcBaseRoleValue()
        {
            //BaseRoleValue.Reset();
            var lvData = CSTable.StaticDataManager.PlayerLevel[this.RoleLevel, (byte)this.RolePro];
            //BaseRoleValue.MaxHP += lvData.hp;
            //BaseRoleValue.MaxMP += lvData.mp;

            //BaseRoleValue.Atk += lvData.atk;
            //BaseRoleValue.Def[(int)eElemType.Gold] += lvData.gold;
            //BaseRoleValue.Def[(int)eElemType.Wood] += lvData.wood;
            //BaseRoleValue.Def[(int)eElemType.Water] += lvData.water;
            //BaseRoleValue.Def[(int)eElemType.Fire] += lvData.fire;
            //BaseRoleValue.Def[(int)eElemType.Earth] += lvData.earth;
            //BaseRoleValue.Hit += lvData.hit;
            //BaseRoleValue.Dodge += lvData.dodge;

            SetAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, lvData.hp);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.MaxMP, lvData.mp);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Atk, lvData.atk);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Gold, lvData.gold);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Wood, lvData.wood);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Water, lvData.water);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Fire, lvData.fire);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Def_Earth, lvData.earth);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Hit, lvData.hit);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, lvData.dodge);
            SetAttrBase(eValueType.Base, eSkillAttrIndex.MoveSpeed, (int)RoleTemplate.speed);
        }

        /// <summary>
        /// 计算由一级属性转化为二级属性
        /// </summary>
        public void CalcPointToValue()
        {
            //PointToValue.Reset();

            //switch (RolePro)
            //{
            //    case eProfession.Shaolin_Changzhang:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.ShaoLin_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToHit);
            //        break;
            //    case eProfession.Wudang_Qingjian:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.Wudang_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.Wudang_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.Wudang_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToHit);
            //        break;
            //    case eProfession.Emei_Guqing:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.Emei_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.Emei_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.Emei_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToHit);
            //        break;
            //    case eProfession.Tangmen_Xiujian:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.Tangmen_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.Tangmen_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.Tangmen_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToHit);
            //        break;
            //    case eProfession.Gaibang_Dadao:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.Gaibang_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.Gaibang_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.Gaibang_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToHit);
            //        break;
            //    case eProfession.Mingjiao_ShuangYue:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.Mingjiao_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.Mingjiao_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.Mingjiao_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToHit);
            //        break;
            //    default:
            //        PointToValue.MaxHP += (int)(FinalPoint.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToHP);
            //        PointToValue.AddAllDef((int)(FinalPoint.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToDef));
            //        PointToValue.Atk += (int)(FinalPoint.Power * RoleCommon.Instance.ShaoLin_ValueConvert.PowerToAtk);
            //        PointToValue.Crit += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToCrit);
            //        PointToValue.Dodge += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToDodge);
            //        PointToValue.Hit += (int)(FinalPoint.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToHit);
            //        break;
            //}

            switch (RolePro)
            {
                case eProfession.Shaolin_Changzhang:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.ShaoLin_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToHit));
                    break;
                case eProfession.Wudang_Qingjian:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.Wudang_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.Wudang_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.Wudang_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Wudang_ValueConvert.DexToHit));
                    break;
                case eProfession.Emei_Guqing:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.Emei_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.Emei_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.Emei_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Emei_ValueConvert.DexToHit));
                    break;
                case eProfession.Tangmen_Xiujian:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.Tangmen_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.Tangmen_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.Tangmen_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Tangmen_ValueConvert.DexToHit));
                    break;
                case eProfession.Gaibang_Dadao:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.Gaibang_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.Gaibang_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.Gaibang_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Gaibang_ValueConvert.DexToHit));
                    break;
                case eProfession.Mingjiao_ShuangYue:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.Mingjiao_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.Mingjiao_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.Mingjiao_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.Mingjiao_ValueConvert.DexToHit));
                    break;
                default:
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.MaxHP, (int)(FinalRoleValue.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToHP));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Def_All, (int)(FinalRoleValue.Body * RoleCommon.Instance.ShaoLin_ValueConvert.BodyToDef));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Atk, (int)(FinalRoleValue.Power * RoleCommon.Instance.ShaoLin_ValueConvert.PowerToAtk));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Crit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToCrit));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Dodge, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToDodge));
                    AddAttrBase(eValueType.Base, eSkillAttrIndex.Hit, (int)(FinalRoleValue.Dex * RoleCommon.Instance.ShaoLin_ValueConvert.DexToHit));
                    break;
            }
        }

        /// <summary>
        /// 计算装备带的二级属性
        /// </summary>
        public void CalcEquipValue()
        {
            //EquipRoleValue.Reset();
            //TODO:EquipRoleValue
            foreach (var item in EquipBag.IterItems())
            {
                //等级表
                var temp = CSTable.ItemUtil.GetEquipLvTpl(item.ItemTemplate.ItemProfession, (item.ItemTemplate as CSTable.ItemEquipData).EquipType, item.ItemData.ItemLv);
                if (temp != null)
                {
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attri1), temp.value1);
                }

                //精炼表
                var temp1 = CSTable.ItemUtil.GetEquipRefineLvTpl((item.ItemTemplate as CSTable.ItemEquipData).EquipType, item.ItemData.ItemRefineLv);
                if (temp1 != null)
                {
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri1), temp1.value1);
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri2), temp1.value2);
                    AddAttrBase(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri3), temp1.value3);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri4), temp1.value4);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri5), temp1.value5);
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp1.attri6), temp1.value6);
                }
            }
        }



        /// <summary>
        /// 计算宝石带的二级属性
        /// </summary>
        public void CalcGemValue()
        {
            //GemRoleValue.Reset();
            foreach (var item in GemBag.IterItems())
            {
                var temp = CSTable.ItemUtil.GetItem(item.ItemData.ItemTemlateId) as CSTable.ItemGemData;
                if (temp != null)
                {
                    AddAttrBasePer(eValueType.Equip, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attr), temp.value);
                }
            }
        }

        /// <summary>
        /// 计算技能带的二级属性
        /// </summary>
        public void CalcSkillValue()
        {
            //SkillRoleValue.Reset();
            foreach (var sk in mSkillMgr.list)
            {
                if (sk.BaseType != eSkillType.Cheats)
                    continue;
                var passive = sk as SkillCheats;
                if (passive != null)
                {
                    AddAttrBasePer(eValueType.Skill, CommonUtil.SkillTypeTranslate((ePassiveEffect)passive.LevelData.attri1), passive.LevelData.value1);
                    AddAttrBasePer(eValueType.Skill, CommonUtil.SkillTypeTranslate((ePassiveEffect)passive.LevelData.attri2), passive.LevelData.value2);
                    AddAttrBasePer(eValueType.Skill, CommonUtil.SkillTypeTranslate((ePassiveEffect)passive.LevelData.attri3), passive.LevelData.value3);
                }
            }
        }

        /// <summary>
        /// 计算成就称号带的二级属性
        /// </summary>
        public void CalcAchieveValue()
        {
            //AchieveValue.Reset();
            var list = mRecordMgr.GetAchieveList(eAchieveType.AchieveName);
            foreach (var i in list)
            {
                if (i.IsFinished())
                {
                    var temp = i.mTemplate as CSTable.AchieveNameData;
                    if (temp != null)
                    {
                        AddAttrBasePer(eValueType.Achieve, CommonUtil.EquipTypeTranslate((eEquipValueType)temp.attri1), temp.value1);
                    }
                }
            }
        }

        /// <summary>
        /// 计算buff带的二级属性
        /// </summary>
        public virtual void CalcBuffValue()
        {
            //BuffRoleValue.Reset();
            foreach (var buff in mBuffMgr.Buffers.Values)
            {
                buff.AllEffectValue.Clear();
                buff.ReComputeAffectValue();
            }
        }

        #endregion


    }
}
