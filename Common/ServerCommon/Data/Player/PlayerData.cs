using ServerCommon.Planes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCommon.Data
{
    /// <summary>
    /// 自动同步角色所需数据
    /// </summary>
    
    public class RoleSyncInfo : RPC.IAutoSaveAndLoad
    {
        [RPC.AutoSaveLoad(true)]
        public ulong RoleId { get; set; }
        [RPC.AutoSaveLoad(true)]
        public string RoleName { get; set; }
        [RPC.AutoSaveLoad(true)]
        public UInt16 RoleLevel { get; set; }
        [RPC.AutoSaveLoad(true)]
        public Byte Camp { get; set; }
        [RPC.AutoSaveLoad(true)]
        public Byte Sex { get; set; }
        [RPC.AutoSaveLoad(true)]
        public Byte Profession { get; set; }
        [RPC.AutoSaveLoad(true)]
        public float LocationX { get; set; }
        [RPC.AutoSaveLoad(true)]
        public float LocationZ { get; set; }
        [RPC.AutoSaveLoad(true)]
        public float Direction { get; set; }
        [RPC.AutoSaveLoad(true)]
        public int RoleHp{ get; set; }
        [RPC.AutoSaveLoad(true)]
        public int RoleMaxHp{ get; set; }
        [RPC.AutoSaveLoad(true)]
        public int WeaponId { get; set; }
        [RPC.AutoSaveLoad(true)]
        public float Speed { get; set; }


        public RoleSyncInfo() { }
        public RoleSyncInfo(PlayerInstance pi)
        {
            RoleDetail rd = pi.PlayerData.RoleDetail;
            RoleId = rd.RoleId;
            RoleName = rd.RoleName;
            RoleLevel = rd.RoleLevel;
            Camp = rd.Camp;
            Sex = rd.Sex;
            Profession = rd.Profession;
            LocationX = rd.LocationX;
            LocationZ = rd.LocationZ;
            Direction = rd.Direction;
            RoleHp = rd.RoleHp;
            RoleMaxHp = rd.RoleMaxHp;
            var equip = pi.EquipBag.FindItemByPos((int)CSCommon.eEquipType.Soul);
            if (equip == null)
                equip = pi.EquipBag.FindItemByPos((int)CSCommon.eEquipType.Weapon);
            WeaponId = equip.ItemTemplate.id;
            Speed = rd.RoleSpeed;
        }

        public RoleSyncInfo(PlayerImage pi)
        {
            RoleCom rc = pi.RoleData;
            OffPlayerData pd = pi.PlayerData;
            NPCData nd = pi.NPCData;
            RoleId = pi.Id;
            RoleName = pi.RoleName;
            RoleLevel = (ushort)pi.RoleLevel;
            Camp = rc.Camp;
            Sex = rc.Sex;
            Profession = rc.Profession;
            LocationX = nd.Position.X;
            LocationZ = nd.Position.Z;
            Direction = nd.Direction;
            RoleHp = nd.RoleHp;
            RoleMaxHp = pd.value.MaxHP;
            WeaponId = pd.WeapFacdeid;
            Speed = pd.value.Speed;
        }
    }

    //玩家基本信息
    [ServerFrame.DB.DBBindTable("roleInfo")]
    public class RoleInfo : RPC.IAutoSaveAndLoad
    {
        IStateHost mHostPlayer;
        public void _SetHostPlayer(IStateHost player)
        {
            mHostPlayer = player;
        }

        protected void OnPropertyChanged(string proName)
        {
            if (mHostPlayer == null)
                return;
            var proInfo = this.GetType().GetProperty(proName);
            if (proInfo == null)
                return;

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            RPC.DataWriter dw = new RPC.DataWriter();
            if (proInfo.PropertyType == typeof(Byte))
                dw.Write(System.Convert.ToByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt16))
                dw.Write(System.Convert.ToUInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt32))
                dw.Write(System.Convert.ToUInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt64))
                dw.Write(System.Convert.ToUInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(SByte))
                dw.Write(System.Convert.ToSByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int16))
                dw.Write(System.Convert.ToInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int32))
                dw.Write(System.Convert.ToInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int64))
                dw.Write(System.Convert.ToInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(float))
                dw.Write(System.Convert.ToSingle(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(double))
                dw.Write(System.Convert.ToDouble(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(System.Guid))
                dw.Write(System.Guid.Parse(proInfo.GetValue(this, null).ToString()));
            else
                return;

            mHostPlayer.OnValueChanged(proName, dw);
        }

        protected void OnComPropertyChanged(string proName)
        {
            if (mHostPlayer == null)
                return;
            var proInfo = this.GetType().GetProperty(proName);

            RPC.PackageWriter pkg = new RPC.PackageWriter();

            RPC.DataWriter dw = new RPC.DataWriter();
            if (proInfo.PropertyType == typeof(Byte))
                dw.Write(System.Convert.ToByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt16))
                dw.Write(System.Convert.ToUInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt32))
                dw.Write(System.Convert.ToUInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(UInt64))
                dw.Write(System.Convert.ToUInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(SByte))
                dw.Write(System.Convert.ToSByte(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int16))
                dw.Write(System.Convert.ToInt16(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int32))
                dw.Write(System.Convert.ToInt32(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(Int64))
                dw.Write(System.Convert.ToInt64(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(float))
                dw.Write(System.Convert.ToSingle(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(double))
                dw.Write(System.Convert.ToDouble(proInfo.GetValue(this, null)));
            else if (proInfo.PropertyType == typeof(System.Guid))
                dw.Write(System.Guid.Parse(proInfo.GetValue(this, null).ToString()));
            else
                return;


            var host = mHostPlayer as ServerCommon.Planes.PlayerInstance;
            if (host!=null)
                host.OnComValueChanged(proName, dw);
        }

        CSTable.PlayerTplData mTemplate = null;
        public CSTable.PlayerTplData Template
        {
            get { return mTemplate; }
        }

        int mTemplateId;
        public int TemplateId
        {
            get { return mTemplateId; }
            set
            {
                mTemplateId = value;
                mTemplate = CSTable.StaticDataManager.PlayerTpl[value];
            }
        }
        ulong mRoleId;
        [ServerFrame.DB.DBBindField("RoleId")]
        [RPC.AutoSaveLoad(true)]
        public ulong RoleId
        {
            get { return mRoleId; }
            set { mRoleId = value; }
        }
        string mRoleName;
        [ServerFrame.DB.DBBindField("RoleName")]
        [RPC.AutoSaveLoad(true)]
        public string RoleName
        {
            get { return mRoleName; }
            set { mRoleName = value; }
        }

        UInt16 mRoleLevel = 1;
        [ServerFrame.DB.DBBindField("RoleLevel")]
        [RPC.AutoSaveLoad(true)]
        public UInt16 RoleLevel
        {
            get { return mRoleLevel; }
            set
            {
                mRoleLevel = value;
                OnPropertyChanged("RoleLevel");
                OnComPropertyChanged("RoleLevel");
            }
        }

        eCamp mCamp;//角色阵营
        [ServerFrame.DB.DBBindField("Camp")]
        [RPC.AutoSaveLoad(true)]
        public Byte Camp
        {
            get { return (Byte)mCamp; }
            set
            {
                mCamp = (eCamp)value;
                OnPropertyChanged("Camp");
                OnComPropertyChanged("Camp");
            }
        }

        ushort mPlanesId;
        [ServerFrame.DB.DBBindField("PlanesId")]
        [RPC.AutoSaveLoad]
        public ushort PlanesId
        {
            get { return mPlanesId; }
            set
            {
                mPlanesId = value;
                OnComPropertyChanged("PlanesId");
            }
        }

        #region 核心信息
        UInt16 mLimitLevel;//GM权限
        [ServerFrame.DB.DBBindField("LimitLevel")]
        [RPC.AutoSaveLoad(true)]
        public UInt16 LimitLevel
        {
            get { return mLimitLevel; }
            set { mLimitLevel = value; }
        }

        ulong mAccountId;
        [ServerFrame.DB.DBBindField("AccountId")]
        [RPC.AutoSaveLoad]
        public ulong AccountId
        {
            get { return mAccountId; }
            set { mAccountId = value; }
        }

        Byte mSex;// 角色性别
        [ServerFrame.DB.DBBindField("Sex")]
        [RPC.AutoSaveLoad(true)]
        public Byte Sex
        {
            get { return mSex; }
            set
            {
                mSex = value;
                OnPropertyChanged("Sex");
            }
        }

        Byte mProfession;// 角色职业
        [ServerFrame.DB.DBBindField("Profession")]
        [RPC.AutoSaveLoad(true)]
        public Byte Profession
        {
            get { return mProfession; }
            set
            {
                mProfession = value;
                OnPropertyChanged("Profession");
                OnComPropertyChanged("Profession");
            }
        }

        int mRoleHp = int.MaxValue;
        [ServerFrame.DB.DBBindField("RoleHp")]
        [RPC.AutoSaveLoad(true)]
        public int RoleHp
        {
            get { return mRoleHp; }
            set
            {
                mRoleHp = value;
                OnPropertyChanged("RoleHp");
            }
        }

        int mRoleMaxHp = 100;
        [ServerFrame.DB.DBBindField("RoleMaxHp")]
        [RPC.AutoSaveLoad(true)]
        public int RoleMaxHp
        {
            get { return mRoleMaxHp; }
            set
            {
                mRoleMaxHp = value;
                OnPropertyChanged("RoleMaxHp");
            }
        }


        int mRoleMp = int.MaxValue;
        [ServerFrame.DB.DBBindField("RoleMp")]
        [RPC.AutoSaveLoad(true)]
        public int RoleMp
        {
            get { return mRoleMp; }
            set
            {
                mRoleMp = value;
                OnPropertyChanged("RoleMp");
            }
        }

        int mRoleMaxMp = 100;
        [ServerFrame.DB.DBBindField("RoleMaxMp")]
        [RPC.AutoSaveLoad(true)]
        public int RoleMaxMp
        {
            get { return mRoleMaxMp; }
            set
            {
                mRoleMaxMp = value;
                OnPropertyChanged("RoleMaxMp");
            }
        }

        float mRoleSpeed;
        [RPC.AutoSaveLoad(true)]
        public float RoleSpeed
        {
            get { return mRoleSpeed; }
            set { mRoleSpeed = value; }
        }
        #endregion

        #region 货币
        UInt32 mGold;//角色金币
        [ServerFrame.DB.DBBindField("Gold")]
        [RPC.AutoSaveLoad(true)]
        public UInt32 Gold
        {
            get { return mGold; }
            set
            {
                mGold = value;
                OnPropertyChanged("Gold");
            }
        }

        UInt32 mRmb;//角色人民币
        [ServerFrame.DB.DBBindField("Rmb")]
        [RPC.AutoSaveLoad(true)]
        public UInt32 Rmb
        {
            get { return mRmb; }
            set
            {
                mRmb = value;
                OnPropertyChanged("Rmb");
            }
        }

        UInt32 mBindRmb;//角色钻石
        [ServerFrame.DB.DBBindField("BindRmb")]
        [RPC.AutoSaveLoad(true)]
        public UInt32 BindRmb
        {
            get { return mBindRmb; }
            set
            {
                mBindRmb = value;
                OnPropertyChanged("BindRmb");
            }
        }

        UInt32 mAir;                    //技能点
        [ServerFrame.DB.DBBindField("Air")]
        [RPC.AutoSaveLoad(true)]
        public UInt32 Air
        {
            get { return mAir; }
            set
            {
                mAir = value;
                OnPropertyChanged("Air");
            }
        }

        UInt32 mReputation;                    //声望
        [ServerFrame.DB.DBBindField("Reputation")]
        [RPC.AutoSaveLoad(true)]
        public UInt32 Reputation
        {
            get { return mReputation; }
            set
            {
                mReputation = value;
                OnPropertyChanged("Reputation");
            }
        }

        #endregion

        #region 地图信息
        string mPlanesName;
        //[ServerFrame.DB.DBBindField("PlanesName")]
        [RPC.AutoSaveLoad]
        public string PlanesName
        {
            get { return mPlanesName; }
            set { mPlanesName = value; }
        }

        string mMapName;
        //[ServerFrame.DB.DBBindField("MapName")]
        //[RPC.AutoSaveLoad]
        public string MapName
        {
            get { return mMapName; }
            set { mMapName = value; }
        }

        ushort mMapSourceId;
        [ServerFrame.DB.DBBindField("MapId")]
        [RPC.AutoSaveLoad(true)]
        public ushort MapSourceId
        {
            get { return mMapSourceId; }
            set { mMapSourceId = value; }
        }
        #endregion
    }

    [ServerFrame.DB.DBBindTable("RoleInfo")]//完整描述
    public class RoleDetail : RoleInfo
    {
        #region 基础信息
        Int64 mRoleExp;//主角当前经验
        [ServerFrame.DB.DBBindField("RoleExp")]
        [RPC.AutoSaveLoad(true)]
        public Int64 RoleExp
        {
            get { return mRoleExp; }
            set
            {
                mRoleExp = value;

                OnPropertyChanged("RoleExp");
            }
        }

        byte mPkMode = (byte)ePKMode.Peace;
        [ServerFrame.DB.DBBindField("PkMode")]
        [RPC.AutoSaveLoad(true)]
        public byte PkMode  //ePKMode
        {
            get { return mPkMode; }
            set
            {
                mPkMode = value;
            }
        }

        string mAchieveName;
        [ServerFrame.DB.DBBindField("AchieveName")]
        [RPC.AutoSaveLoad(true)]
        public string AchieveName
        {
            get { return mAchieveName; }
            set
            {
                mAchieveName = value;
                OnPropertyChanged("AchieveName");
            }
        }


        #endregion

        #region 加点，筋脉
        ushort mRemainPoint; //剩余潜能点数
        [ServerFrame.DB.DBBindField("RemainPoint")]
        [RPC.AutoSaveLoad(true)]
        public ushort RemainPoint
        {
            get { return mRemainPoint; }
            set
            {
                mRemainPoint = value;
                OnPropertyChanged("RemainPoint");
            }
        }

        ushort mPowerPoint;  //分配的内功
        [ServerFrame.DB.DBBindField("PowerPoint")]
        [RPC.AutoSaveLoad(true)]
        public ushort PowerPoint
        {
            get { return mPowerPoint; }
            set
            {
                mPowerPoint = value;
                OnPropertyChanged("PowerPoint");
            }
        }

        ushort mBodyPoint;   //分配的外功点数
        [ServerFrame.DB.DBBindField("BodyPoint")]
        [RPC.AutoSaveLoad(true)]
        public ushort BodyPoint
        {
            get { return mBodyPoint; }
            set
            {
                mBodyPoint = value;
                OnPropertyChanged("BodyPoint");
            }
        }

        ushort mDexPoint;    //分配的身法
        [ServerFrame.DB.DBBindField("DexPoint")]
        [RPC.AutoSaveLoad(true)]
        public ushort DexPoint
        {
            get { return mDexPoint; }
            set
            {
                mDexPoint = value;
                OnPropertyChanged("DexPoint");
            }
        }

        byte mFreeUpMuscleTimes;    //免费筋脉突破次数
        [ServerFrame.DB.DBBindField("FreeUpMuscleTimes")]
        [RPC.AutoSaveLoad(true)]
        public byte FreeUpMuscleTimes
        {
            get { return mFreeUpMuscleTimes; }
            set
            {
                mFreeUpMuscleTimes = value;
                OnPropertyChanged("FreeUpMuscleTimes");
            }
        }

        byte mUpMuscleTimes;    //花钱筋脉突破次数
        [ServerFrame.DB.DBBindField("UpMuscleTimes")]
        [RPC.AutoSaveLoad(true)]
        public byte UpMuscleTimes
        {
            get { return mUpMuscleTimes; }
            set
            {
                mUpMuscleTimes = value;
                OnPropertyChanged("UpMuscleTimes");
            }
        }

        [ServerFrame.DB.DBBindField("LastUpMuscleTime")] //最后一次筋脉突破次数
        [RPC.AutoSaveLoad]
        public System.DateTime LastUpMuscleTime { get; set; }
        #endregion

        #region 统计信息
        System.DateTime mCreateTime = System.DateTime.Now;
        [ServerFrame.DB.DBBindField("CreateTime")]
        [RPC.AutoSaveLoad(false)]
        public System.DateTime CreateTime
        {
            get { return mCreateTime; }
            set { mCreateTime = value; }
        }
        // 上次登录的日期
        DateTime mLastLoginDate;
        [ServerFrame.DB.DBBindField("LastLoginDate")]
        [RPC.AutoSaveLoad(false)]
        public DateTime LastLoginDate
        {
            get { return mLastLoginDate; }
            set { mLastLoginDate = value; }
        }

        // 今天累计登陆经过的秒数
        int mTodayPassSecond;
        [ServerFrame.DB.DBBindField("TodayPassSecond")]
        [RPC.AutoSaveLoad(false)]
        public int TodayPassSecond
        {
            get { return mTodayPassSecond; }
            set { mTodayPassSecond = value; }
        }
        #endregion

        #region 位置信息
        ulong mDungeonID = 0;
        [ServerFrame.DB.DBBindField("DungeonID")]
        [RPC.AutoSaveLoad(false)]
        public ulong DungeonID
        {
            get { return mDungeonID; }
            set { mDungeonID = value; }
        }
        float mLocationX = 0;
        [ServerFrame.DB.DBBindField("LocationX")]
        [RPC.AutoSaveLoad(true)]
        public float LocationX
        {
            get { return mLocationX; }
            set { mLocationX = value; }
        }
        float mLocationY = 0;
        [ServerFrame.DB.DBBindField("LocationY")]
        [RPC.AutoSaveLoad(true)]
        public float LocationY
        {
            get { return mLocationY; }
            set { mLocationY = value; }
        }
        float mLocationZ = 0;
        [ServerFrame.DB.DBBindField("LocationZ")]
        [RPC.AutoSaveLoad(true)]
        public float LocationZ
        {
            get { return mLocationZ; }
            set { mLocationZ = value; }
        }

        float mDirection = 0;
        [ServerFrame.DB.DBBindField("Direction")]
        [RPC.AutoSaveLoad(true)]
        public float Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }
        #endregion

        #region 背包处理

        UInt16 mBagSize;//主背包容量
        [ServerFrame.DB.DBBindField("BagSize")]
        [RPC.AutoSaveLoad(true)]
        public UInt16 BagSize
        {
            get { return mBagSize; }
            set { mBagSize = value; }
        }

        #endregion

        #region PVP

        byte mAttackModel;      //攻击模式
        [ServerFrame.DB.DBBindField("AttackModel")]
        public byte AttackModel
        {
            get { return mAttackModel; }
            set
            {
                mAttackModel = value;
                OnPropertyChanged("AttackModel");
            }
        }
        #endregion

    }

    public class PlayerData : RPC.IAutoSaveAndLoad
    {
        #region Data
        RoleDetail mRoleDetail = new RoleDetail();
        public RoleDetail RoleDetail
        {
            get { return mRoleDetail; }
            set { mRoleDetail = value; }
        }

        List<ItemData> mBagItems = new List<ItemData>();
        public List<ItemData> BagItems
        {
            get { return mBagItems; }
            set
            {
                mBagItems = value;
            }
        }

        List<ItemData> mFashionItems = new List<ItemData>();
        public List<ItemData> FashionItems
        {
            get { return mFashionItems; }
            set
            {
                mFashionItems = value;
            }
        }

        List<ItemData> mStoreItems = new List<ItemData>();
        public List<ItemData> StoreItems
        {
            get { return mStoreItems; }
            set
            {
                mStoreItems = value;
            }
        }
        List<ItemData> mEquipedItems = new List<ItemData>();
        public List<ItemData> EquipedItems
        {
            get { return mEquipedItems; }
            set
            {
                mEquipedItems = value;
            }
        }
        List<ItemData> mEquipGemItems = new List<ItemData>();
        public List<ItemData> EquipGemItems
        {
            get { return mEquipGemItems; }
            set
            {
                mEquipGemItems = value;
            }
        }
        List<ItemData> mGemItems = new List<ItemData>();
        public List<ItemData> GemItems
        {
            get { return mGemItems; }
            set
            {
                mGemItems = value;
            }
        }
        TaskData mTaskData = new TaskData();   // 玩家主线任务
        public TaskData TaskData
        {
            get { return mTaskData; }
            set
            {
                mTaskData = value;
            }
        }

        AchieveData mAchieveData = new AchieveData();   // 玩家成就
        public AchieveData AchieveData
        {
            get { return mAchieveData; }
            set
            {
                mAchieveData = value;
            }
        }

        List<SkillData> mSkillDatas = new List<SkillData>();
        public List<SkillData> SkillDatas
        {
            get { return mSkillDatas; }
            set { mSkillDatas = value; }
        }

        MartialData mMartialData = new MartialData();
        public MartialData MartialData
        {
            get { return mMartialData; }
            set { mMartialData = value; }
        }

        RankData mRankData = new RankData();
        public RankData RankData
        {
            get { return mRankData; }
            set { mRankData = value; }
        }

        #endregion

        #region SaveAndLoad
        public override void PackageWrite(RPC.PackageWriter pkg)
        {
            mRoleDetail.PackageWrite(pkg);
            PackageWriteList<ItemData>(mBagItems, pkg);
            PackageWriteList<ItemData>(mEquipedItems, pkg);
            PackageWriteList<ItemData>(mEquipGemItems, pkg);
            PackageWriteList<ItemData>(mGemItems, pkg);
            PackageWriteList<ItemData>(mFashionItems, pkg);
            mTaskData.PackageWrite(pkg);
            PackageWriteList<SkillData>(mSkillDatas, pkg);
            mMartialData.PackageWrite(pkg);
            if (!pkg.IsSinglePkg)
            {
                mAchieveData.PackageWrite(pkg);
                mRankData.PackageWrite(pkg);
            }
        }


        public override void PackageRead(RPC.PackageProxy pkg)
        {
            mRoleDetail.PackageRead(pkg);
            PackageReadList<ItemData>(mBagItems, pkg);
            PackageReadList<ItemData>(mEquipedItems, pkg);
            PackageReadList<ItemData>(mEquipGemItems, pkg);
            PackageReadList<ItemData>(mGemItems, pkg);
            PackageReadList<ItemData>(mFashionItems, pkg);
            mTaskData.PackageRead(pkg);
            PackageReadList<SkillData>(mSkillDatas, pkg);
            mMartialData.PackageRead(pkg);
            mAchieveData.PackageRead(pkg);
            mRankData.PackageRead(pkg);
            
        }

        //送给玩家的数据，以后需要采用RLE压缩，所以DataWrite和DataRead要特殊处理，一定要注意，服务器客户端的对应
        public override void DataWrite(RPC.DataWriter pkg, bool bToClient)
        {
            mRoleDetail.DataWrite(pkg, bToClient);
            DaraWriteList<ItemData>(mBagItems, pkg, bToClient);
            DaraWriteList<ItemData>(mEquipedItems, pkg, bToClient);
            DaraWriteList<ItemData>(mEquipGemItems, pkg, bToClient);
            DaraWriteList<ItemData>(mGemItems, pkg, bToClient);
            DaraWriteList<ItemData>(mFashionItems, pkg, bToClient);
            mTaskData.DataWrite(pkg, bToClient);
            DaraWriteList<SkillData>(mSkillDatas, pkg, bToClient);
            mMartialData.DataWrite(pkg, bToClient);
            if (!bToClient)
            {
                mAchieveData.DataWrite(pkg, bToClient);
                mRankData.DataWrite(pkg, bToClient);
            }
        }
        public override void DataRead(RPC.DataReader pkg, bool bToClient)
        {
            mRoleDetail.DataRead(pkg, bToClient);
            DaraReadList<ItemData>(mBagItems, pkg, bToClient);
            DaraReadList<ItemData>(mEquipedItems, pkg, bToClient);
            DaraReadList<ItemData>(mEquipGemItems, pkg, bToClient);
            DaraReadList<ItemData>(mGemItems, pkg, bToClient);
            DaraReadList<ItemData>(mFashionItems, pkg, bToClient);
            mTaskData.DataRead(pkg, bToClient);
            mAchieveData.DataRead(pkg, bToClient);
            DaraReadList<SkillData>(mSkillDatas, pkg, bToClient);
            mMartialData.DataRead(pkg, bToClient);
            mRankData.DataRead(pkg, bToClient);
            
        }
        #endregion

    }

//     public enum PlayerLandStep
//     {
//         SelectRole,
//         EnterGame,
//     }
    public class PlayerDataEx : PlayerData
    {
        public object CurMap;
        public object CurPlanes;
        public AccountInfo mAccountInfo;
        public Iocp.NetConnection mGateConnect;
        public ushort mGateConnectIndex;
        public ulong mLinkSerialId;
        public Iocp.NetConnection mPlanesConnect;
        public object mSaverThread;

//         public PlayerLandStep mLandStep = PlayerLandStep.SelectRole;
//         public ulong mLinkSerialId;
//         public AccountInfo mAccountInfo;
//         public Iocp.NetConnection mGateConnect;
//         public Iocp.NetConnection mPlanesConnect;
//         public UInt16 mGateConnectIndex;//客户端在gate上的tcpconnect编号

 //       public object mSaverThread;
    }
}
