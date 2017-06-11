using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public class byteIntGroup
    {
        byte mGetNum;
        [ServerFrame.Config.DataValueAttribute("GetNum")]
        [System.ComponentModel.DisplayName("01.获取数量")]
        public byte GetNum
        {
            get { return mGetNum; }
            set { mGetNum = value; }
        }

        int mNeedNum;
        [ServerFrame.Config.DataValueAttribute("NeedNum")]
        [System.ComponentModel.DisplayName("02.需求数量")]
        public int NeedNum
        {
            get { return mNeedNum; }
            set { mNeedNum = value; }
        }
    }

    //基础属性转化
    public class BaseValueConvert
    {
        [System.ComponentModel.DisplayName("01.外功转生命")]
        [ServerFrame.Config.DataValueAttribute("BodyToHP")]
        public float BodyToHP { get; set; }
        [System.ComponentModel.DisplayName("02.外功转防御")]
        [ServerFrame.Config.DataValueAttribute("BodyToDef")]
        public float BodyToDef { get; set; }
        [System.ComponentModel.DisplayName("03.内功转攻击")]
        [ServerFrame.Config.DataValueAttribute("PowerToAtk")]
        public float PowerToAtk { get; set; }
        [System.ComponentModel.DisplayName("04.身法转命中")]
        [ServerFrame.Config.DataValueAttribute("DexToHit")]
        public float DexToHit { get; set; }
        [System.ComponentModel.DisplayName("05.身法转闪避")]
        [ServerFrame.Config.DataValueAttribute("DexToDodge")]
        public float DexToDodge { get; set; }
        [System.ComponentModel.DisplayName("06.身法转致命")]
        [ServerFrame.Config.DataValueAttribute("DexToCrit")]
        public float DexToCrit { get; set; }
      
    }

    

    [ServerFrame.Editor.CDataEditorAttribute(".rco")]
    [ServerFrame.Editor.Template]
    public class RoleCommon : ServerFrame.CommonTemplate<RoleCommon>
    {
        byte mFreeCount;
        [System.ComponentModel.Category("01.经脉")]
        [System.ComponentModel.DisplayName("01.每日免费突破经脉次数")]
        [ServerFrame.Config.DataValueAttribute("FreeCount")]
        public byte FreeCount
        {
            get { return mFreeCount; }
            set { mFreeCount = value; }
        }

        byte mPayCount = 1;
        [System.ComponentModel.Category("01.经脉")]
        [System.ComponentModel.DisplayName("02.每日元宝突破经脉次数")]
        [ServerFrame.Config.DataValueAttribute("PayCount")]
        public byte PayCount
        {
            get { return mPayCount; }
            set { mPayCount = value; }
        }

        int mPayMoneyNum = 1;
        [System.ComponentModel.Category("01.经脉")]
        [System.ComponentModel.DisplayName("03.突破经脉花费元宝数量")]
        [ServerFrame.Config.DataValueAttribute("PayMoneyNum")]
        public int PayMoneyNum
        {
            get { return mPayMoneyNum; }
            set { mPayMoneyNum = value; }
        }

        byte mReset = 1;
        [System.ComponentModel.Category("01.经脉")]
        [System.ComponentModel.DisplayName("04.经脉次数重置时间")]
        [ServerFrame.Config.DataValueAttribute("Reset")]
        public byte Reset
        {
            get { return mReset; }
            set { mReset = value; }
        }

        int mExploitBoxId;
        [System.ComponentModel.Category("02.功勋")]
        [System.ComponentModel.DisplayName("01.奖励箱子Id")]
        [ServerFrame.Config.DataValueAttribute("ExploitBoxId")]
        public int ExploitBoxId
        {
            get { return mExploitBoxId; }
            set { mExploitBoxId = value; }
        }

        List<byteIntGroup> mExploitReward = new List<byteIntGroup>();
        [System.ComponentModel.Category("02.功勋")]
        [System.ComponentModel.DisplayName("02.功勋奖励箱子详情")]
        [ServerFrame.Config.DataValueAttribute("ExploitReward")]
        public List<byteIntGroup> ExploitReward
        {
            get { return mExploitReward; }
            set { mExploitReward = value; }
        }

        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("01.少林男模型id")]
        [ServerFrame.Config.DataValueAttribute("ShaoLin_Men")]
        public int ShaoLin_Men{get;set;}
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("02.武当男模型id")]
        [ServerFrame.Config.DataValueAttribute("Wudang_Men")]
        public int Wudang_Men { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("03.武当女模型id")]
        [ServerFrame.Config.DataValueAttribute("Wudang_Women")]
        public int Wudang_Women { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("04.峨眉女模型id")]
        [ServerFrame.Config.DataValueAttribute("Emei_Women")]
        public int Emei_Women { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("05.唐门男模型id")]
        [ServerFrame.Config.DataValueAttribute("Tangmen_Men")]
        public int Tangmen_Men { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("06.唐门女模型id")]
        [ServerFrame.Config.DataValueAttribute("Tangmen_Women")]
        public int Tangmen_Women { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("07.明教男模型id")]
        [ServerFrame.Config.DataValueAttribute("Mingjiao_Men")]
        public int Mingjiao_Men { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("08.明教女模型id")]
        [ServerFrame.Config.DataValueAttribute("Mingjiao_Women")]
        public int Mingjiao_Women { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("09.丐帮男模型id")]
        [ServerFrame.Config.DataValueAttribute("Gaibang_Men")]
        public int Gaibang_Men { get; set; }
        [System.ComponentModel.Category("03.人物职业相关")]
        [System.ComponentModel.DisplayName("10.丐帮女模型id")]
        [ServerFrame.Config.DataValueAttribute("Gaibang_Women")]
        public int Gaibang_Women { get; set; }


        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("01.少林属性转换")]
        [ServerFrame.Config.DataValueAttribute("ShaoLin_ValueConvert")]
        public BaseValueConvert ShaoLin_ValueConvert { get; set; }
        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("02.武当属性转换")]
        [ServerFrame.Config.DataValueAttribute("Wudang_ValueConvert")]
        public BaseValueConvert Wudang_ValueConvert { get; set; }
        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("03.峨眉属性转换")]
        [ServerFrame.Config.DataValueAttribute("Emei_ValueConvert")]
        public BaseValueConvert Emei_ValueConvert { get; set; }
        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("04.唐门属性转换")]
        [ServerFrame.Config.DataValueAttribute("Tangmen_ValueConvert")]
        public BaseValueConvert Tangmen_ValueConvert { get; set; }
        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("05.明教属性转换")]
        [ServerFrame.Config.DataValueAttribute("Mingjiao_ValueConvert")]
        public BaseValueConvert Mingjiao_ValueConvert { get; set; }
        [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
        [System.ComponentModel.Category("04.一级属性转换参数")]
        [System.ComponentModel.DisplayName("06.丐帮属性转换")]
        [ServerFrame.Config.DataValueAttribute("Gaibang_ValueConvert")]
        public BaseValueConvert Gaibang_ValueConvert { get; set; }

        public override string GetFilePath()
        {
            return "Common/RoleCommon.rco";
        }
    }

  
}
