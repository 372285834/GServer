using ServerFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{

    public partial class PlayerInstance : RPC.RPCObject
    {
        MartialBuild mMartialBuild = new MartialBuild();
        public MartialBuild MartialBuild
        {
            get { return mMartialBuild; }
        }

        PlantBuild mPlantBuild = new PlantBuild();
        public PlantBuild PlantBuild
        {
            get { return mPlantBuild; }
        }

        SmeltBuild mSmeltBuild = new SmeltBuild();
        public SmeltBuild SmeltBuild
        {
            get { return mSmeltBuild; }
        }

        TrainBuild mTrainBuild = new TrainBuild();
        public TrainBuild TrainBuild
        {
            get { return mTrainBuild; }
        }

        BuildOutPutManager mBuildOutPutManager = new BuildOutPutManager();
        public BuildOutPutManager BuildOutPutManager
        {
            get { return mBuildOutPutManager; }
        }

        public void InitMartial()
        {
            mMartialBuild.Init(this, this.PlayerData.MartialData.MartialLv);
            mPlantBuild.Init(this, this.PlayerData.MartialData.PlantLv);
            mSmeltBuild.Init(this, this.PlayerData.MartialData.SmeltLv);
            mTrainBuild.Init(this, this.PlayerData.MartialData.TrainLv);
            mBuildOutPutManager.MartialUnSerialize(this, this.PlayerData.MartialData.OutPutInfo);
        }

        public void SaveMartial()
        {
            BuildOutPutManager.MartialSerialize();
            PlayerData.MartialData.MartialLv = MartialBuild.Level;
            PlayerData.MartialData.PlantLv = PlantBuild.Level;
            PlayerData.MartialData.SmeltLv = SmeltBuild.Level;
            PlayerData.MartialData.TrainLv = TrainBuild.Level;
        }

        public BuildBase GetBuild(CSCommon.eBuildType type)
        {
            switch (type)
            {
                case CSCommon.eBuildType.Martial:
                    return mMartialBuild;
                case CSCommon.eBuildType.Smelt:
                    return mSmeltBuild;
                case CSCommon.eBuildType.Train:
                    return mTrainBuild;
                case CSCommon.eBuildType.Plant:
                    return mPlantBuild;
                default:
                    return null;
            }
        }

        //服务器开放
        public void OpenMartial(byte type)
        {
            var build = GetBuild((CSCommon.eBuildType)type);
            if (build == null)
            {
                Log.Log.Item.Info("OpenMartial GetBuild Failed eBuildType : {0}", type);
                return;
            }
            build.Open();
            CreateOutPutToDict(build.OutPutInfo);
            //通知客户端
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_MartialOpen(pkg, type);
            pkg.DoCommandPlanes2Client(this.Planes2GateConnect, this.ClientLinkId);
        }

        private void CreateOutPutToDict(Dictionary<int, int> info)
        {
            if (info == null)
            {
                return;
            }
            foreach (var i in info)
            {
                mBuildOutPutManager.CreateOutPutToAdd((byte)i.Key);
            }
        }

        //客户端调用建筑升级
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_UpMartialLevel(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var build = GetBuild((CSCommon.eBuildType)type);
            if (build == null)
            {
                Log.Log.Item.Info("UpMartialLevel GetBuild Failed eBuildType : {0}", type);
                pkg.Write((sbyte)CSCommon.eRet_UpMartialLevel.NoBuild);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            sbyte result = build.UpLevel();
            CreateOutPutToDict(build.OutPutInfo);
            pkg.Write(result);
            pkg.DoReturnPlanes2Client(fwd);
        }

        //返回当前加成状态
        //返回总次数
        //当前产出数量
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetMartialInfo(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var build = GetBuild((CSCommon.eBuildType)type);
            if (build == null)
            {
                Log.Log.Item.Info("GetMartialOutPut  Failed eBuildType : {0}", type);
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoBuild);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (build.Level < 1)
            {
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoOpen);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (build.OutPutInfo == null)
            {
                Log.Log.Item.Info("GetMartialOutPut  Failed OutPutInfo is null}");
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoOutPut);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.Succeed);
            pkg.Write(build.Level);
            byte count = (byte)build.OutPutInfo.Count;//几种类型
            pkg.Write(count);
            foreach (var i in build.OutPutInfo)
            {
                byte index = (byte)i.Key;//类型
                int num = i.Value;//数量
                var output = mBuildOutPutManager.GetBuildOutPut(index);
                byte total = output.mOutCount;//总次数
                float uprate = output.GetUpRate(System.DateTime.Now);//加成率
                float remainUpHour = output.mRemainUpHour;
                pkg.Write(index);
                pkg.Write(num);
                pkg.Write(total);
                pkg.Write(uprate);
                pkg.Write(remainUpHour);
            }
            pkg.DoReturnPlanes2Client(fwd);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetOutPutReward(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var build = GetBuild((CSCommon.eBuildType)type);
            if (build == null)
            {
                Log.Log.Item.Info("GetMartialOutPut  Failed eBuildType : {0}", type);
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoBuild);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (build.Level < 1)
            {
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoOpen);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }
            if (build.OutPutInfo == null)
            {
                Log.Log.Item.Info("GetMartialOutPut  Failed OutPutInfo is null}");
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.NoOutPut);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            Dictionary<byte, int> dict = new Dictionary<byte, int>();
            foreach (var i in build.OutPutInfo)
            {
                byte index = (byte)i.Key;//类型
                var output = mBuildOutPutManager.GetBuildOutPut(index);
                int reward = output.GetRewardCount();
                if (output.mOutCount > 0)
                {
                    dict[index] = reward;
                }
            }

            if (false == _CanGetOutPut(dict))//判断背包是否满
            {
                pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.BagFull);
                pkg.DoReturnPlanes2Client(fwd);
                return;
            }

            foreach (var r in dict)
            {
                _GetOutPut(r.Key, r.Value);
                var output = mBuildOutPutManager.GetBuildOutPut(r.Key);
                output.GetReward(r.Value);
            }

            pkg.Write((sbyte)CSCommon.eRet_GetMartialInfo.Succeed);
            pkg.Write(dict.Count);
            foreach (var r in dict)
            {
                pkg.Write(r.Key);
                pkg.Write(r.Value);//获得数量
            }
            pkg.DoReturnPlanes2Client(fwd);
        }

        bool _CanGetOutPut(Dictionary<byte, int> dict)
        {
            ushort count = 0;
            foreach(var i in dict)
            {
                var tpl = CSTable.StaticDataManager.MartialItem[(int)i.Key];
                if (tpl.type == (int)CSCommon.eMartialItemBigType.ItemType)
                {
                    count++;
                }
            }
            if (Bag.GetEmptyCount() < count)
            {
                return false;
            }
            return true;
        }

        void _GetOutPut(byte type, int count)
        {
            //根据类型获取物品等
            var tpl = CSTable.StaticDataManager.MartialItem[(int)type];
            CSCommon.eMartialItemBigType bType = (CSCommon.eMartialItemBigType)tpl.type;
            switch (bType)
            {
                case CSCommon.eMartialItemBigType.CurrenceType:
                    {
                        var CurrenceType = (CSCommon.eCurrenceType)tpl.tempId;
                        _ChangeMoney(CurrenceType, CSCommon.Data.eMoneyChangeType.GetMartialOut, count);
                    }
                    break;
                case CSCommon.eMartialItemBigType.ItemType:
                    {
                        CreateItemToBag(tpl.tempId, count);
                    }
                    break;
                default:
                    break;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_Visit(byte type, ulong otherId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_Visit(pkg, type, this.PlayerData.RoleDetail.RoleId, otherId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OpenVisit(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetTopAndFriend(pkg, this.PlayerData.RoleDetail.RoleId, this.PlayerData.RoleDetail.PlanesId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retPkg.Write(result);
                if (result == 1)
                {
                    byte WorldVisitCount = _io.ReadByte();
                    byte FriendVisitCount = _io.ReadByte();
                    byte BuyVisitCount = _io.ReadByte();
                    ushort ByVisitCount = _io.ReadByte();
                    retPkg.Write(WorldVisitCount);
                    retPkg.Write(FriendVisitCount);
                    retPkg.Write(BuyVisitCount);
                    retPkg.Write(ByVisitCount);

                    byte topcount = _io.ReadByte();
                    retPkg.Write(topcount);
                    for (byte i = 0; i < topcount; i++)
                    {
                        ulong id = _io.ReadUInt64();
                        string name = _io.ReadString();
                        byte state = _io.ReadByte();
                        retPkg.Write(id);
                        retPkg.Write(name);
                        retPkg.Write(state);
                    }
                    byte friendcount = _io.ReadByte();
                    retPkg.Write(friendcount);
                    for (byte j = 0; j < friendcount; j++)
                    {
                        ulong id = _io.ReadUInt64();
                        string name = _io.ReadString();
                        byte state = _io.ReadByte();
                        retPkg.Write(id);
                        retPkg.Write(name);
                        retPkg.Write(state);
                    }
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }
    }
}
