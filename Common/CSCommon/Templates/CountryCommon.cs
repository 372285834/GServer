using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    [ServerFrame.Editor.CDataEditorAttribute(".sco")]
    [ServerFrame.Editor.Template]
    public class CountryCommon : ServerFrame.CommonTemplate<CountryCommon>
    {
        int mBarrackMaxForceCount = 9999;
        [System.ComponentModel.Category("01.兵营")]
        [System.ComponentModel.DisplayName("01.兵营最大储兵数")]
        [ServerFrame.Config.DataValueAttribute("BarrackMaxForceCount")]
        public int BarrackMaxForceCount
        {
            get { return mBarrackMaxForceCount; }
            set { mBarrackMaxForceCount = value; }
        }

        int mBarrackMaxLevel = 200;
        [System.ComponentModel.Category("01.兵营")]
        [System.ComponentModel.DisplayName("02.兵营最高等级数")]
        [ServerFrame.Config.DataValueAttribute("BarrackMaxLevel")]
        public int BarrackMaxLevel
        {
            get { return mBarrackMaxLevel; }
            set { mBarrackMaxLevel = value; }
        }

        public override string GetFilePath()
        {
            return "Common/CountryCommon.sco";
        }
    }

}
