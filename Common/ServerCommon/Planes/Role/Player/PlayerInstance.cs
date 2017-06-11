using CSCommon;
using CSCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Planes
{

    /// <summary>
    /// 玩家类
    /// </summary>
    [RPC.RPCClassAttribute(typeof(PlayerInstance))]
    [System.ComponentModel.TypeConverterAttribute("System.ComponentModel.ExpandableObjectConverter")]
    public partial class PlayerInstance : RoleActor, RPC.RPCObject
    {
        #region RPC必须的基础定义
        public static RPC.RPCClassInfo smRpcClassInfo = new RPC.RPCClassInfo();
        public virtual RPC.RPCClassInfo GetRPCClassInfo()
        {
            return smRpcClassInfo;
        }
        #endregion

        public void SendSysMsg(string name)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            //ExamplePlugins.ZeusGame.H_IGame.smInstance.RPC_PushBackSysMsg(pkg, name);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }

        #region 一些重载

        protected override void OnPlacementUpdatePosition(ref SlimDX.Vector3 pos)
        {
            if (!this.HostMap.IsNullMap)
            {
                var mapCell = this.HostMap.GetMapCell(pos.X, pos.Z);
                if (mapCell != null)
                {
                    if (this.HostMapCell != mapCell)
                    {
                        mapCell.AddRole(this);

                        //// 向玩家发送周围格NPC
                        //this.HostMap.UpdateNPCsToClient(this);
                        //// 向玩家发送周围格玩家
                        //this.HostMap.UpdatePlayersToClient(this);
                    }
                }
            }


            if (!this.HostMap.IsNullMap && HostMap.IsSavePos(this) )
            {
                SetRoleDetailPos(pos.X, pos.Z);
                PlayerData.RoleDetail.Direction = this.Placement.GetDirection();
            }
        }

        public void SetRoleDetailPos(float x, float z)
        {
            PlayerData.RoleDetail.LocationX = x;
            PlayerData.RoleDetail.LocationY = 0;
            PlayerData.RoleDetail.LocationZ = z;
        }

        public override void OnEnterMap(MapInstance map)
        {
            if (!AllMapManager.IsInstanceMap(map.MapSourceId))
            {
                var loc = new SlimDX.Vector3(PlayerData.RoleDetail.LocationX, PlayerData.RoleDetail.LocationY, PlayerData.RoleDetail.LocationZ);
                Placement.SetLocation(ref loc);
            }
            else
            {
                var loc = new SlimDX.Vector3(map.MapInfo.MapData.startX, PlayerData.RoleDetail.LocationY, map.MapInfo.MapData.startY);
                Placement.SetLocation(ref loc);
            }

            //// 角色默认只有Y方向旋转量
            //var rot = SlimDX.Quaternion.RotationYawPitchRoll(PlayerData.RoleDetail.Direction, 0, 0);
            Placement.SetDirection(PlayerData.RoleDetail.Direction);

            base.OnEnterMap(map);
            mPlayerData.RoleDetail.RoleMaxHp = FinalRoleValue.MaxHP;
            mPlayerData.RoleDetail.RoleMaxMp = FinalRoleValue.MaxMP;
            mPlayerData.RoleDetail.DungeonID = 0;
        }

        public override void OnLeaveMap()
        {
            SaveAll();
            base.OnLeaveMap();
        }

        //bool bHasPlayer = false;
        bool OnTellClientNPC(RoleActor role, object arg)
        {
            var list = arg as List<RoleActor>;

            if (list.Count >= RPC.RPCNetworkMgr.Sync2ClientNPCsCountWhenEnterMap)
                return false;

            if (role is NPCInstance)
            {
                list.Add(role);
            }
            return true;
        }
        bool OnTellClientPlayer(RoleActor role, object arg)
        {
            var list = arg as List<RoleActor>;

            if (list.Count >= RPC.RPCNetworkMgr.Sync2ClientPlayersCountWhenEnterMap)
                return false;

            list.Add(role);
            
            return true;
        }



        public override void Tick(Int64 elapsedMiliSeccond)
        {
            base.Tick(elapsedMiliSeccond);
            //this.TaskManager.Tick(elapsedMiliSeccond);
            //if (PlayerData.RoleDetail.EvilValue > 0)
            //{
            //    mSubEvilTime += elapsedMiliSeccond;
            //    if (base.SubEvil(this, mSubEvilTime))
            //    {
            //        mSubEvilTime = 0;
            //    }
            //}
            //if (PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.BlueName)
            //    BlueChangeWhite(this);

            //if (PlayerData.RoleDetail.RoleState == (byte)CSCommon.Data.RState.PinkName)
            //    PinkChangeWhite(this);
            StateTick(elapsedMiliSeccond);
        }


        public override bool OnValueChanged(string name, RPC.DataWriter value)
        {
            if (true == base.OnValueChanged(name, value))
                return true;

            //这些数据只需要告诉玩家自己，不需要告诉别的客户端
            var pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_UpdateRoleValue(pkg, name, value);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
            return true;
        }

        public bool OnComValueChanged(string name, RPC.DataWriter value)
        {
            var pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_UpdateRoleComValue(pkg, this.Id, name, value);
            pkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
            return true;
        }

        public void GainExp(int exp)
        {
            //这里计算是否升级了
            var nexttpl = CSTable.StaticDataManager.PlayerLevel[PlayerData.RoleDetail.RoleLevel + 1, PlayerData.RoleDetail.Profession];
            if (nexttpl == null)//已到最大等级
            {
                return;
            }
            var temp = CSTable.StaticDataManager.PlayerLevel[PlayerData.RoleDetail.RoleLevel, PlayerData.RoleDetail.Profession];
            if (temp == null)//当前等级表错误
            {
                return;
            }
            PlayerData.RoleDetail.RoleExp += exp;
            var nextLevelExp = temp.exp;
            while (nextLevelExp <= PlayerData.RoleDetail.RoleExp)
            {
                nexttpl = CSTable.StaticDataManager.PlayerLevel[PlayerData.RoleDetail.RoleLevel + 1, PlayerData.RoleDetail.Profession];
                if (nexttpl == null)
                {
                    return;
                }
                RoleUpLv();
                PlayerData.RoleDetail.RoleExp -= nextLevelExp;
                temp = CSTable.StaticDataManager.PlayerLevel[PlayerData.RoleDetail.RoleLevel, PlayerData.RoleDetail.Profession];
                if (temp == null)
                {
                    return;
                }
                nextLevelExp = temp.exp;
            }
        }
        public void RoleUpLv()
        {
            PlayerData.RoleDetail.RoleLevel += 1;
            PlayerData.RoleDetail.RemainPoint += (ushort)CSTable.StaticDataManager.PlayerLevel[PlayerData.RoleDetail.RoleLevel, PlayerData.RoleDetail.Profession].point;
            CalcChangeValue(eValueType.Base);
            OpenSomething(PlayerData.RoleDetail.RoleLevel);
            PlayerData.RankData.Level = PlayerData.RoleDetail.RoleLevel;
            this.DispatchEvent(EventType.UpLevel, PlayerData.RoleDetail.RoleLevel);
        }

        private void OpenSomething(ushort roleLv)
        {
            var martialtpl = CSCommon.MartialClubCommon.Instance;
            if (martialtpl.MartialNeedRoleLv <= roleLv)
            {
                OpenMartial((byte)CSCommon.eBuildType.Martial);
            }
            if (martialtpl.PlantNeedRoleLv <= roleLv)
            {
                OpenMartial((byte)CSCommon.eBuildType.Plant);
            }
            if (martialtpl.SmeltNeedRoleLv <= roleLv)
            {
                OpenMartial((byte)CSCommon.eBuildType.Smelt);
            }
            if (martialtpl.TrainNeedRoleLv <= roleLv)
            {
                OpenMartial((byte)CSCommon.eBuildType.Train);
            }
        }

        #region 获取
        public void GetReward(string strMoney, string strItem)
        {
            var moneys = strMoney.Split('|');
            foreach (var m in moneys)
            {
                if (string.IsNullOrEmpty(m))
                {
                    continue;
                }
                var moeny = m.Split(',');
                if (!string.IsNullOrEmpty(moeny[0]) && !string.IsNullOrEmpty(moeny[1]))
                {
                    var type = (CSCommon.eCurrenceType)Convert.ToInt32(moeny[0]);
                    var count = Convert.ToInt32(moeny[1]);
                    _ChangeMoney(type, CSCommon.Data.eMoneyChangeType.DailyReward, count);
                }
            }

            string[] items = strItem.Split('|');
            foreach (var i in items)
            {
                if (string.IsNullOrEmpty(i))
                {
                    continue;
                }
                string[] idcount = i.Split(',');
                if (!string.IsNullOrEmpty(idcount[0]) && !string.IsNullOrEmpty(idcount[1]))
                {
                    int id = Convert.ToInt32(idcount[0]);
                    int count = Convert.ToInt32(idcount[1]);
                    CreateItemToBag(id, count);
                }
            }
        }
        //获取功勋
        public void GainExploit(int addvalue)
        {
            PlayerData.AchieveData.Exploit += (uint)addvalue;
            AddRankData(CSCommon.eRankType.Exploit, addvalue);
        }
        public void _ChangeMoney(CSCommon.eCurrenceType ctype, CSCommon.Data.eMoneyChangeType mtype, int value)
        {
            switch (ctype)
            {
                case eCurrenceType.Unknow:
                    break;
                case eCurrenceType.Gold:
                    PlayerData.RoleDetail.Gold += (uint)value;
                    break;
                case eCurrenceType.Rmb:
                    PlayerData.RoleDetail.Rmb += (uint)value;
                    break;
                case eCurrenceType.Reputation:
                    PlayerData.RoleDetail.Reputation += (uint)value;
                    break;
                case eCurrenceType.Exploit:
                    GainExploit(value);
                    break;
                case eCurrenceType.Activeness:
                    break;
                case eCurrenceType.Exp:
                    GainExp(value);
                    break;
                case eCurrenceType.Air:
                    PlayerData.RoleDetail.Air += (uint)value;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 判断是否满足条件
        public bool _IsMoneyEnough(CSCommon.eCurrenceType type, int count)
        {
            switch (type)
            {
                case CSCommon.eCurrenceType.Gold:
                    {
                        if (this.PlayerData.RoleDetail.Gold >= (uint)count)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CSCommon.eCurrenceType.Rmb:
                    {
                        if (this.PlayerData.RoleDetail.Rmb >= (uint)count)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CSCommon.eCurrenceType.Air:
                    {
                        if (this.PlayerData.RoleDetail.Air >= (uint)count)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case CSCommon.eCurrenceType.Reputation:
                    {
                        if (this.PlayerData.RoleDetail.Reputation >= (uint)count)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

            }
            return false;
        }  
        public bool _IsLevelEnough(ushort lv)
        {
            if (this.PlayerData.RoleDetail.RoleLevel >= lv)
            {
                return true;
            }
            return false;
        }
        public bool _IsCampMatch(byte camp)
        {
            if (camp == (byte)CSCommon.eCamp.None)
            {
                return true;
            }
            if (this.PlayerData.RoleDetail.Camp == camp)
            {
                return true;
            }
            return false;
        }
        public bool _IsProMatch(byte pro)
        {
            if (pro == (byte)CSCommon.eProfession.Unknow)
            {
                return true;
            }
            if (pro == this.PlayerData.RoleDetail.Profession)
            {
                return true;
            }
            return false;
        }
        #endregion

        #endregion

        #region 一些信息的获取
//         public PlanesInstance PlanesInstance
//         {
//             get
//             {
//                 if (mHostMap.IsNullMap)
//                     return null;
//                 return mHostMap.PlanesInstance;
//             }
//         }

        public CSTable.PlayerTplData RoleTemplate
        {
            get
            {
                if (mPlayerData == null)
                    return null;
                return mPlayerData.RoleDetail.Template;
            }
        }

        public override ulong Id
        {
            get
            {
                return mPlayerData.RoleDetail.RoleId;
            }
        }

        public override string RoleName
        {
            get
            {
                return mPlayerData.RoleDetail.RoleName;
            }
        }

        public override eElemType ElemType
        {
            get { return (eElemType)RoleTemplate.attribute; }
        }


        public ulong AccountId
        {
            get
            {
                return mPlayerData.RoleDetail.AccountId;
            }
        }

        Byte mVipLevel = 0;
        public Byte VipLevel
        {
            get { return mVipLevel; }
            set { mVipLevel = value; }
        }


        public override int RoleLevel
        {
            get { return PlayerData.RoleDetail.RoleLevel; }
            set { PlayerData.RoleDetail.RoleLevel = (byte)value; }
        }

        public CSCommon.eProfession RolePro
        {
            get { return (CSCommon.eProfession)PlayerData.RoleDetail.Profession; }
        }

        public override eCamp Camp
        {
            get { return (eCamp)PlayerData.RoleDetail.Camp; }
        }

        #endregion

        #region 初始化
        public static PlayerInstance CreatePlayerInstance(CSCommon.Data.PlayerData pd, Iocp.TcpConnect p2gConnect, UInt16 linkId)
        {
            IActorInitBase actInit = new IActorInitBase();
            actInit.GameType = eActorGameType.Player;
            PlayerInstance ret = new PlayerInstance();
            ret.Initialize(actInit);
            if (false == ret.InitRoleInstance(null, pd, p2gConnect, linkId))
                return null;

            return ret;
        }

        CSCommon.Data.PlayerData mPlayerData;
        public CSCommon.Data.PlayerData PlayerData
        {
            get { return mPlayerData; }
            set { mPlayerData = value; }
        }

        private bool InitRoleInstance(PlanesInstance planes, CSCommon.Data.PlayerData pd, Iocp.TcpConnect p2gConnect, UInt16 linkId)
        {
            mPlanes2GateConnect = p2gConnect;
            mClientLinkId = linkId;

            mPlayerData = pd;
            mPlayerData.RoleDetail._SetHostPlayer(this);
            Bag.InventoryType = CSCommon.eItemInventory.ItemBag;

            //var sp = CSCommon.RoleCommonData.Instance.SexProToTId.Find(x => ((byte)x.Sex == pd.RoleDetail.Sex && (byte)x.Pro == pd.RoleDetail.Profession));
            //if (sp == null)
            //{
            //    Log.Log.Common.Print("SexProToTId is Null sex={0},pro={1}", pd.RoleDetail.Sex, pd.RoleDetail.Profession);
            //    return false;
            //}

            mPlayerData.RoleDetail.TemplateId = CSCommon.CommonUtil.GetTemplateIDBySexAndPro(pd.RoleDetail.Sex, pd.RoleDetail.Profession);
            
            #region 背包初始化
            Bag.InitBag(this, mPlayerData.RoleDetail.BagSize, mPlayerData.BagItems);
            EquipBag.InitBag(this, (UInt16)CSCommon.eEquipType.MaxBagSize, mPlayerData.EquipedItems);
            FashionBag.InitBag(this, byte.MaxValue, mPlayerData.FashionItems);
            EquipGemBag.InitBag(this, (UInt16)CSCommon.eEquipType.MaxBagSize, mPlayerData.EquipGemItems);
            GemBag.InitBag(this, (UInt16)CSCommon.eEquipType.MaxBagSize, mPlayerData.GemItems);
            #endregion
            InitMartial();
            #region 任务初始化
            InitTask(mPlayerData.TaskData);
            mRecordMgr.Init(this, mPlayerData.AchieveData);
            #endregion
            mSkillMgr.InitBag(this, mPlayerData.SkillDatas);
            #region 状态机初始化
            InitState();
            #endregion
            CalcAllValues();

            //mPlayerData.RoleDetail.RoleMaxHp = FinalRoleValue.MaxHP;
            //暂时先这样,以后需要客户端操作复活
            //if (CurHP <= 0)
            //    Relive();

            //EventDispacth.Instance.AddListener(eGlobleEvent.Fight_KillActor, this);



            return true;
        }

        public void SaveAll()
        {
            // 物品背包
            mPlayerData.BagItems = Bag.GetBagSaver();
            mPlayerData.EquipedItems = EquipBag.GetBagSaver();
            mPlayerData.FashionItems = FashionBag.GetBagSaver();
            mPlayerData.EquipGemItems = EquipGemBag.GetBagSaver();
            mPlayerData.GemItems = GemBag.GetBagSaver();
            // 任务
            mPlayerData.TaskData = mCurTask.TaskData;
            mRecordMgr.Save(mPlayerData.AchieveData);
            mPlayerData.SkillDatas = mSkillMgr.GetBagSaver();
            SaveMartial();
            //发送存盘数据给数据服务器保存
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_DataServer(pkg).HGet_PlayerManager(pkg).SaveRole(pkg, mPlayerData.RoleDetail.RoleId, mPlayerData);
            pkg.DoCommand(IPlanesServer.Instance.DataConnect, RPC.CommandTargetType.DefaultType);
        }
        #endregion

        #region 网络通讯内部管理数据处理
        UInt16 mIndexInMap = System.UInt16.MaxValue;
        public UInt16 IndexInMap
        {
            get { return mIndexInMap; }
        }
        internal void _SetIndexInMap(UInt16 v)
        {
            mIndexInMap = v;
        }

        UInt16 mClientLinkId = UInt16.MaxValue;
        public UInt16 ClientLinkId
        {
            get { return mClientLinkId; }
            set { mClientLinkId = value; }
        }
        Iocp.TcpConnect mPlanes2GateConnect;
        public Iocp.TcpConnect Planes2GateConnect
        {
            get { return mPlanes2GateConnect; }
            set { mPlanes2GateConnect = value; }
        }
        #endregion


        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetRoleCreateInfo(ulong id, RPC.RPCForwardInfo fwd)
        {
            var map = this.HostMap;
            if (map == null)
                return;

            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            retPkg.SetSinglePkg();
            var roleActor = map.GetRole(id);
            if (roleActor == null)
            {
                retPkg.Write((sbyte)eRet_GetRoleCreateInfo.NotFindActor);//找不到角色
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            else if (roleActor.GameType == eActorGameType.Npc)
            {
                float fdistSq = SlimDX.Vector3.DistanceSquared(this.GetPosition(), roleActor.GetPosition());
                if (fdistSq > RPC.RPCNetworkMgr.Sync2ClientRangeSq)
                {
                    retPkg.Write((sbyte)eRet_GetRoleCreateInfo.OverDistance);//距离太远
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                //这里后面还需要判断是否已经死亡，是否对玩家可见
                else
                {
                    retPkg.Write((sbyte)eRet_GetRoleCreateInfo.OK_NPC);//距离太远
                    var npc = roleActor as NPCInstance;
                    retPkg.Write(npc.NPCData);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
            }
            else if (roleActor.GameType == eActorGameType.Player)
            {
                float fdistSq = SlimDX.Vector3.DistanceSquared(this.GetPosition(), roleActor.GetPosition());
                if (fdistSq > RPC.RPCNetworkMgr.Sync2ClientRangeSq)
                {
                    retPkg.Write((sbyte)eRet_GetRoleCreateInfo.OverDistance);//距离太远
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                //这里后面还需要判断是否已经死亡，是否对玩家可见
                else
                {
                    retPkg.Write((sbyte)eRet_GetRoleCreateInfo.OK_Player);//距离太远
                    var player = roleActor as PlayerInstance;
                    CSCommon.Data.RoleSyncInfo info = new CSCommon.Data.RoleSyncInfo(player);
                    retPkg.Write(info);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
            }
            else if (roleActor.GameType == eActorGameType.PlayerImage)
            {
                retPkg.Write((sbyte)eRet_GetRoleCreateInfo.OK_PlayerImage);
                var image = roleActor as PlayerImage;
                CSCommon.Data.RoleSyncInfo info = new CSCommon.Data.RoleSyncInfo(image);
                retPkg.Write(info);
                retPkg.DoReturnPlanes2Client(fwd);
            }
        }



        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GlobalMapFindPath(SlimDX.Vector3 from, SlimDX.Vector3 to, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {


        }
   }
}
