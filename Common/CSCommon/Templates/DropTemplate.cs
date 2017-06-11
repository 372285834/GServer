using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public enum eDropType
    {
        Drop_1,
        Drop_2,
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class RandomRangeInt32
    {
        int mMin;
        [ServerFrame.Config.DataValueAttribute("Min")]
        public int Min
        {
            get { return mMin; }
            set { mMin = value; }
        }
        int mMax;
        [ServerFrame.Config.DataValueAttribute("Max")]
        public int Max
        {
            get { return mMax; }
            set { mMax = value; }
        }
        public int RandValue(System.Random _rand)
        {
            return (int)_rand.Next((int)mMin, (int)mMax);
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class TwoValueInt32
    {
        int mDropNum;
        [ServerFrame.Config.DataValueAttribute("DropNum")]
        public int DropNum
        {
            get { return mDropNum; }
            set { mDropNum = value; }
        }
        int mDropValue;
        [ServerFrame.Config.DataValueAttribute("DropValue")]
        public int DropValue
        {
            get { return mDropValue; }
            set { mDropValue = value; }
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class DropElement
    {
        List<int> mItems = new List<int>();
        [ServerFrame.Config.DataValueAttribute("Items")]
        public List<int> Items
        {
            get { return mItems; }
            set { mItems = value; }
        }

        int mDropValue;
        [ServerFrame.Config.DataValueAttribute("DropValue")]
        public int DropValue
        {
            get { return mDropValue; }
            set { mDropValue = value; }
        }

        RandomRangeInt32 mDropNum = new RandomRangeInt32();
        [ServerFrame.Config.DataValueAttribute("DropNum")]
        public RandomRangeInt32 DropNum
        {
            get { return mDropNum; }
            set { mDropNum = value; }
        }
    }

    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class DropItem
    {
        int mItemId;
        [ServerFrame.Config.DataValueAttribute("ItemId")]
        public int ItemId
        {
            get { return mItemId; }
            set { mItemId = value; }
        }

        int mDropValue;
        [ServerFrame.Config.DataValueAttribute("DropValue")]
        public int DropValue
        {
            get { return mDropValue; }
            set { mDropValue = value; }
        }

        RandomRangeInt32 mDropNum = new RandomRangeInt32();
        [ServerFrame.Config.DataValueAttribute("DropNum")]
        public RandomRangeInt32 DropNum
        {
            get { return mDropNum; }
            set { mDropNum = value; }
        }

    }

    [ServerFrame.Editor.CDataEditorAttribute(".drop")]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public class DropTemplate
    {

        int mTemplateId;
        [ServerFrame.Config.DataValueAttribute("TemplateId")]
        [System.ComponentModel.DisplayName("<1>掉落Id")]
        [System.ComponentModel.Category("1.掉落基本属性")]
        public int TemplateId
        {
            get { return mTemplateId; }
            set { mTemplateId = value; }
        }

        eDropType mDropType;
        [ServerFrame.Config.DataValueAttribute("DropType")]
        [System.ComponentModel.DisplayName("<2>掉落方式")]
        [System.ComponentModel.Category("1.掉落基本属性")]
        [System.ComponentModel.Description("Drop_1(权值掉落);Drop_2(非权值掉落)")]
        public eDropType DropType
        {
            get { return mDropType; }
            set { mDropType = value; }
        }

        List<DropElement> mDropElems = new List<DropElement>();
        [ServerFrame.Config.DataValueAttribute("DropElems")]
        [System.ComponentModel.DisplayName("<1>掉落组")]
        [System.ComponentModel.Category("2.权值掉落")]
        public List<DropElement> DropElems
        {
            get { return mDropElems; }
            set { mDropElems = value; }
        }

        List<TwoValueInt32> mDropNumRate = new List<TwoValueInt32>();
        [ServerFrame.Config.DataValueAttribute("DropNumRate")]
        [System.ComponentModel.DisplayName("<2>掉落次数权值")]
        [System.ComponentModel.Category("2.权值掉落")]
        public List<TwoValueInt32> DropNumRate
        {
            get { return mDropNumRate; }
            set { mDropNumRate = value; }
        }

        bool mNoRepeatElem = false;
        [ServerFrame.Config.DataValueAttribute("NoRepeatElem")]
        [System.ComponentModel.DisplayName("<3>能否重复掉落")]
        [System.ComponentModel.Category("2.权值掉落")]
        public bool NoRepeatElem
        {
            get { return mNoRepeatElem; }
            set { mNoRepeatElem = value; }
        }

        List<TwoValueInt32> mDropMultiple = new List<TwoValueInt32>();
        [ServerFrame.Config.DataValueAttribute("DropMultiple")]
        [System.ComponentModel.DisplayName("<5>掉落倍数权值")]
        [System.ComponentModel.Category("2.权值掉落")]
        public List<TwoValueInt32> DropMultiple
        {
            get { return mDropMultiple; }
            set { mDropMultiple = value; }
        }

        bool mIsMultiple = false;
        [ServerFrame.Config.DataValueAttribute("IsMultiple")]
        [System.ComponentModel.DisplayName("<4>是否使用倍数")]
        [System.ComponentModel.Category("2.权值掉落")]
        public bool IsMultiple
        {
            get { return mIsMultiple; }
            set { mIsMultiple = value; }
        }

        RandomRangeInt32 mDropNum = new RandomRangeInt32();
        [ServerFrame.Config.DataValueAttribute("DropNum")]
        [System.ComponentModel.DisplayName("<1>掉落随机数范围")]
        [System.ComponentModel.Category("3.非权值掉落")]
        public RandomRangeInt32 DropNum
        {
            get { return mDropNum; }
            set { mDropNum = value; }
        }


        List<DropItem> mDropList = new List<DropItem>();
        [ServerFrame.Config.DataValueAttribute("DropList")]
        [System.ComponentModel.DisplayName("<2>掉落列表")]
        [System.ComponentModel.Category("3.非权值掉落")]
        public List<DropItem> DropList
        {
            get { return mDropList; }
            set { mDropList = value; }
        }
    }

    [ServerFrame.Editor.Template]
    public class DropTemplateManager : ServerFrame.TemplateManager<DropTemplateManager, DropTemplate>
    {
        public override string GetFilePath()
        {
            return "Drop";
        }

        public override void OnLoadedItem(DropTemplate item, int templateId)
        {
            item.TemplateId = templateId;
        }

    }


}
