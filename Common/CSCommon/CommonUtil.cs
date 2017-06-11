using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public static class CommonUtil
    {
        public static int GetTemplateIDBySexAndPro(Byte sex, Byte pro)
        {
            var s = (eRoleSex)sex;
            var p = (eProfession)pro;
            if (s == eRoleSex.Men)
            {
                switch (p)
                {
                    case eProfession.Shaolin_Changzhang://少林 长杖
                        return RoleCommon.Instance.ShaoLin_Men;
                    case eProfession.Tangmen_Xiujian://唐门 袖箭
                        return RoleCommon.Instance.Tangmen_Men;
                    case eProfession.Emei_Guqing:   //峨眉 古琴
                        return RoleCommon.Instance.Emei_Women;
                    case eProfession.Gaibang_Dadao://丐帮 大刀
                        return RoleCommon.Instance.Gaibang_Men;
                    case eProfession.Wudang_Qingjian: //武当 轻剑
                        return RoleCommon.Instance.Wudang_Men;
                    case eProfession.Mingjiao_ShuangYue://明教 双钺                
                        return RoleCommon.Instance.Mingjiao_Men;
                }
            }
            else
            {
                switch (p)
                {
                    case eProfession.Shaolin_Changzhang://少林 长杖
                        return RoleCommon.Instance.ShaoLin_Men;
                    case eProfession.Tangmen_Xiujian://唐门 袖箭
                        return RoleCommon.Instance.Tangmen_Women;
                    case eProfession.Emei_Guqing:   //峨眉 古琴
                        return RoleCommon.Instance.Emei_Women;
                    case eProfession.Gaibang_Dadao://丐帮 大刀
                        return RoleCommon.Instance.Gaibang_Women;
                    case eProfession.Wudang_Qingjian: //武当 轻剑
                        return RoleCommon.Instance.Wudang_Women;
                    case eProfession.Mingjiao_ShuangYue://明教 双钺                
                        return RoleCommon.Instance.Mingjiao_Women;
                }
            }

            return RoleCommon.Instance.ShaoLin_Men;
        }

        /// <summary>
        /// 将装备属性类型转换为技能属性类型
        /// </summary>
        public static eSkillAttrIndex EquipTypeTranslate(eEquipValueType type)
        {
            return (eSkillAttrIndex)type;
        }

        /// <summary>
        /// 将被动技能属性类型转换为技能属性类型
        /// </summary>
        public static eSkillAttrIndex SkillTypeTranslate(ePassiveEffect type)
        {
            return (eSkillAttrIndex)type;
        }

        /// <summary>
        /// 将buff效果类型转换为技能属性类型
        /// </summary>
        public static eSkillAttrIndex BufferTypeTranslate(eBuffEffectType type)
        {
            switch (type)
            {
                case eBuffEffectType.影响生命:
                    return eSkillAttrIndex.HP;
                case eBuffEffectType.影响生命百分比:
                    return eSkillAttrIndex.HPRate;
            }
            if (type >= eBuffEffectType.影响生命上限百分比 && type <= eBuffEffectType.影响移动速度百分比)
                return (eSkillAttrIndex)((int)eSkillAttrIndex.MaxHPRate + (int)(type - eBuffEffectType.影响生命上限百分比));

            return eSkillAttrIndex.None;
        }

        /// <summary>
        /// 将五行属性类型转换为技能属性类型
        /// </summary>
        public static eSkillAttrIndex ElemTypeTranslate(eElemType elemType)
        {
            return (eSkillAttrIndex)((int)eSkillAttrIndex.Def_Gold + (int)elemType - 1);
        }

        /// <summary>
        /// 通过类型获取地图路线模板表
        /// </summary>
        public static CSTable.MapWayData GetMapWayDataByType(eCamp camp, eNpcType type)
        {
            var tplMapWays = CSTable.StaticDataManager.MapWay.Dict;
            foreach (var tpl in tplMapWays.Values)
            {
                eCarType carType = eCarType.None;
                switch (type)
                {
                    case eNpcType.FoodCar:
                        carType = eCarType.FoodCar;
                        break;
                    case eNpcType.MoneyCar:
                        carType = eCarType.MoneyCar;
                        break;
                }
                if (tpl.camp == (int)camp && tpl.cartype == (int)carType)
                    return tpl;
            }
            return null;
        }

    }
}
