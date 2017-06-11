using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{

    [ServerFrame.Editor.CDataEditorAttribute(".sco")]
    [ServerFrame.Editor.Template]
    public class SocialCommon : ServerFrame.CommonTemplate<SocialCommon>
    {
        int mAddIntimacyBySend = 25;
        [System.ComponentModel.Category("1.好友")]
        [System.ComponentModel.DisplayName("<1>赠送礼物增加亲密度")]
        [ServerFrame.Config.DataValueAttribute("AddIntimacyBySend")]
        public int AddIntimacyBySend
        {
            get { return mAddIntimacyBySend; }
            set { mAddIntimacyBySend = value; }
        }


        List<int> mGiftList = new List<int>();
        [System.ComponentModel.Category("2.礼物列表")]
        [ServerFrame.Config.DataValueAttribute("GiftList")]
        public List<int> GiftList
        {
            get { return mGiftList; }
            set { mGiftList = value; }
        }

        public override string GetFilePath()
        {
            return "Common/Social.sco";
        }

    }
}
