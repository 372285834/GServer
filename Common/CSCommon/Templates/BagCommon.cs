using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    [ServerFrame.Editor.CDataEditorAttribute(".bag")]
    [ServerFrame.Editor.Template]
    public class BagCommon : ServerFrame.CommonTemplate<BagCommon>
    {
        Byte mBagStartSize = 24;
        [System.ComponentModel.Category("1.背包")]
        [System.ComponentModel.DisplayName("<1>背包开始容量")]
        [ServerFrame.Config.DataValueAttribute("BagStartSize")]
        public Byte BagStartSize
        {
            get { return mBagStartSize; }
            set { mBagStartSize = value; }
        }


        List<Byte> mBagSize = new List<Byte>();
        [System.ComponentModel.Category("1.背包")]
        [System.ComponentModel.DisplayName("<2>背包容量")]
        [System.ComponentModel.Description("不同vip等级的背包容量;0代表vip1")]
        [ServerFrame.Config.DataValueAttribute("BagSize")]
        public List<Byte> BagSize
        {
            get { return mBagSize; }
            set { mBagSize = value; }
        }

        public override string GetFilePath()
        {
            return "Common/Bag.bag";
        }
    }

}
