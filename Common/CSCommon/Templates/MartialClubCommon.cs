using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon
{
    public class UpGain
    {
        int mPrice;
        [System.ComponentModel.DisplayName("1.价格")]
        [ServerFrame.Config.DataValueAttribute("Price")]
        public int Price
        {
            get { return mPrice; }
            set { mPrice = value; }
        }

        byte mValidity;
        [System.ComponentModel.DisplayName("2.有效期天")]
        [ServerFrame.Config.DataValueAttribute("Validity")]
        public byte Validity
        {
            get { return mValidity; }
            set { mValidity = value; }
        }

        float mUpRate;
        [System.ComponentModel.DisplayName("3.倍数")]
        [ServerFrame.Config.DataValueAttribute("UpRate")]
        public float UpRate
        {
            get { return mUpRate; }
            set { mUpRate = value; }
        }


    }


    public class RoleAtt
    {
        ushort mPower;
        [System.ComponentModel.DisplayName("1.内功")]
        [ServerFrame.Config.DataValueAttribute("Power")]
        public ushort Power
        {
            get { return mPower; }
            set { mPower = value; }
        }

        ushort mBody;
        [System.ComponentModel.DisplayName("2.外功")]
        [ServerFrame.Config.DataValueAttribute("Body")]
        public ushort Body
        {
            get { return mBody; }
            set { mBody = value; }
        }

        ushort mDex;
        [System.ComponentModel.DisplayName("3.身法")]
        [ServerFrame.Config.DataValueAttribute("Dex")]
        public ushort Dex
        {
            get { return mDex; }
            set { mDex = value; }
        }
    }

    public class ClonedInfo
    {
        ushort mPrice;
        [System.ComponentModel.DisplayName("1.价格")]
        [ServerFrame.Config.DataValueAttribute("Price")]
        public ushort Price
        {
            get { return mPrice; }
            set { mPrice = value; }
        }

        float mPowerf;
        [System.ComponentModel.DisplayName("2.能力")]
        [ServerFrame.Config.DataValueAttribute("Powerf")]
        public float Powerf
        {
            get { return mPowerf; }
            set { mPowerf = value; }
        }
    }

    [ServerFrame.Editor.CDataEditorAttribute(".mtcc")]
    [ServerFrame.Editor.Template]
    public class MartialClubCommon : ServerFrame.CommonTemplate<MartialClubCommon>
    {
        List<RoleAtt> mAddAtt = new List<RoleAtt>();
        [System.ComponentModel.DisplayName("01.武馆升级增加角色属性")]
        [ServerFrame.Config.DataValueAttribute("AddAtt")]
        public List<RoleAtt> AddAtt
        {
            get { return mAddAtt; }
            set { mAddAtt = value; }
        }

        byte mOutPutMaxTime;
        [System.ComponentModel.DisplayName("02.产出最大次数")]
        [ServerFrame.Config.DataValueAttribute("OutPutMaxTime")]
        public byte OutPutMaxTime
        {
            get { return mOutPutMaxTime; }
            set { mOutPutMaxTime = value; }
        }

        byte mOutPutPer = 1;
        [System.ComponentModel.DisplayName("02.产出间隔时间")]
        [ServerFrame.Config.DataValueAttribute("OutPutPer")]
        public byte OutPutPer
        {
            get { return mOutPutPer; }
            set { mOutPutPer = value; }
        }

        byte mPostOwnMaxCount;
        [System.ComponentModel.DisplayName("03.玩家拥有据点最大数量")]
        [ServerFrame.Config.DataValueAttribute("PostOwnMaxCount")]
        public byte PostOwnMaxCount
        {
            get { return mPostOwnMaxCount; }
            set { mPostOwnMaxCount = value; }
        }

        byte mPostMaxCount;
        [System.ComponentModel.DisplayName("03.据点最大数量")]
        [ServerFrame.Config.DataValueAttribute("PostMaxCount")]
        public byte PostMaxCount
        {
            get { return mPostMaxCount; }
            set { mPostMaxCount = value; }
        }

        int mByVisitMaxCount;
        [System.ComponentModel.DisplayName("04.被拜访最大次数")]
        [ServerFrame.Config.DataValueAttribute("ByVisitMaxCount")]
        public int ByVisitMaxCount
        {
            get { return mByVisitMaxCount; }
            set { mByVisitMaxCount = value; }
        }

        byte mVisitToActionCount;
        [System.ComponentModel.DisplayName("04.拜访兑换声望数量")]
        [ServerFrame.Config.DataValueAttribute("VisitToActionCount")]
        public byte VisitToActionCount
        {
            get { return mVisitToActionCount; }
            set { mVisitToActionCount = value; }
        }

        byte mByVisitToActionCount;
        [System.ComponentModel.DisplayName("04.被拜访兑换声望数量")]
        [ServerFrame.Config.DataValueAttribute("ByVisitToActionCount")]
        public byte ByVisitToActionCount
        {
            get { return mByVisitToActionCount; }
            set { mByVisitToActionCount = value; }
        }

        byte mWorldVisitMaxCount;
        [System.ComponentModel.DisplayName("04.世界拜访上限")]
        [ServerFrame.Config.DataValueAttribute("WorldVisitMaxCount")]
        public byte WorldVisitMaxCount
        {
            get { return mWorldVisitMaxCount; }
            set { mWorldVisitMaxCount = value; }
        }

        byte mFriendVisitMaxCount;
        [System.ComponentModel.DisplayName("04.好友拜访上限")]
        [ServerFrame.Config.DataValueAttribute("FriendVisitMaxCount")]
        public byte FriendVisitMaxCount
        {
            get { return mFriendVisitMaxCount; }
            set { mFriendVisitMaxCount = value; }
        }

        byte mBuyVisitMaxCount;
        [System.ComponentModel.DisplayName("04.派发红包拜访上限")]
        [ServerFrame.Config.DataValueAttribute("BuyVisitMaxCount")]
        public byte BuyVisitMaxCount
        {
            get { return mBuyVisitMaxCount; }
            set { mBuyVisitMaxCount = value; }
        }

        int mVisitGetMoney;
        [System.ComponentModel.DisplayName("04.点击主界面红包主人拜访获取银两数量")]
        [ServerFrame.Config.DataValueAttribute("VisitGetMoney")]
        public int VisitGetMoney
        {
            get { return mVisitGetMoney; }
            set { mVisitGetMoney = value; }
        }

        List<ClonedInfo> mCloneInfos = new List<ClonedInfo>();
        [System.ComponentModel.DisplayName("05.分身信息列表")]
        [ServerFrame.Config.DataValueAttribute("CloneInfos")]
        public List<ClonedInfo> CloneInfos
        {
            get { return mCloneInfos; }
            set { mCloneInfos = value; }
        }

        byte mFreePrayCount;
        [System.ComponentModel.DisplayName("06.免费祈福次数")]
        [ServerFrame.Config.DataValueAttribute("FreePrayCount")]
        public byte FreePrayCount
        {
            get { return mFreePrayCount; }
            set { mFreePrayCount = value; }
        }

        byte mRankMaxCount;
        [System.ComponentModel.DisplayName("07.武馆排行最多名次")]
        [ServerFrame.Config.DataValueAttribute("RankMaxCount")]
        public byte RankMaxCount
        {
            get { return mRankMaxCount; }
            set { mRankMaxCount = value; }
        }

        byte mFlushHour;
        [System.ComponentModel.DisplayName("08.刷新时间")]
        [ServerFrame.Config.DataValueAttribute("FlushHour")]
        public byte FlushHour
        {
            get { return mFlushHour; }
            set { mFlushHour = value; }
        }

        List<UpGain> mUpGains = new List<UpGain>();
        [System.ComponentModel.DisplayName("09.收益方式")]
        [ServerFrame.Config.DataValueAttribute("UpGains")]
        public List<UpGain> UpGains
        {
            get { return mUpGains; }
            set { mUpGains = value; }
        }

        ushort mMartialNeedRoleLv = 3;
        [System.ComponentModel.DisplayName("10.武馆开放需要角色等级")]
        [ServerFrame.Config.DataValueAttribute("MartialNeedRoleLv")]
        public ushort MartialNeedRoleLv
        {
            get { return mMartialNeedRoleLv; }
            set { mMartialNeedRoleLv = value; }
        }

        ushort mPlantNeedRoleLv = 4;
        [System.ComponentModel.DisplayName("10.种植场开放需要角色等级")]
        [ServerFrame.Config.DataValueAttribute("PlantNeedRoleLv")]
        public ushort PlantNeedRoleLv
        {
            get { return mPlantNeedRoleLv; }
            set { mPlantNeedRoleLv = value; }
        }

        ushort mTrainNeedRoleLv = 5;
        [System.ComponentModel.DisplayName("10.训练场开放需要角色等级")]
        [ServerFrame.Config.DataValueAttribute("TrainNeedRoleLv")]
        public ushort TrainNeedRoleLv
        {
            get { return mTrainNeedRoleLv; }
            set { mTrainNeedRoleLv = value; }
        }

        ushort mSmeltNeedRoleLv = 7;
        [System.ComponentModel.DisplayName("10.矿场开放需要角色等级")]
        [ServerFrame.Config.DataValueAttribute("SmeltNeedRoleLv")]
        public ushort SmeltNeedRoleLv
        {
            get { return mSmeltNeedRoleLv; }
            set { mSmeltNeedRoleLv = value; }
        }

        ushort mSportsNeedRoleLv = 8;
        [System.ComponentModel.DisplayName("10.竞技台开放需要角色等级")]
        [ServerFrame.Config.DataValueAttribute("SportsNeedRoleLv")]
        public ushort SportsNeedRoleLv
        {
            get { return mSportsNeedRoleLv; }
            set { mSportsNeedRoleLv = value; }
        }

        public override string GetFilePath()
        {
            return "Common/Martial.mtcc";
        }

    }
}
