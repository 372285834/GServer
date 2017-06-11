using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{

    public partial class PlayerInstance : RPC.RPCObject
    {
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetSocialList(byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetSocialList(pkg,this.PlayerData.RoleDetail.RoleId, type);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                sbyte result = -1;
                _io.Read(out result);

                int count;
                if (result == -1)
                {
                    count = 0;
                    retPkg.Write(count);
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }
                else if (result == 1)
                {
                    _io.Read(out count);
                    retPkg.Write(count);

                    for (int i = 0; i < count; i++)
                    {
                        CSCommon.Data.SocialRoleInfo s = new CSCommon.Data.SocialRoleInfo();
                        _io.Read(s);
                        _io.Read(s.socialData);
                        retPkg.Write(s);
                        retPkg.Write(s.socialData);
                    }
                    retPkg.DoReturnPlanes2Client(fwd);
                    return;
                }


            };     
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_AddSocialByName(string name, byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_AddSocial(pkg,this.PlayerData.RoleDetail.RoleId, name, type);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retpkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retpkg.Write(result);
                if (result == (sbyte)CSCommon.eRet_AddSocial.AddSucceed)
                {
                    CSCommon.Data.SocialRoleInfo s = new CSCommon.Data.SocialRoleInfo();
                    _io.Read(s);
                    _io.Read(s.socialData);
                    retpkg.Write(s);
                    retpkg.Write(s.socialData);
                }
                retpkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OperateAddSocial(string otherName, byte type, byte operate, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OperateAddSocial(pkg, this.PlayerData.RoleDetail.RoleId, otherName, type, operate);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retpkg = new RPC.PackageWriter();
                sbyte result = _io.ReadSByte();
                retpkg.Write(result);
                retpkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_RemoveSocial(ulong roleId, byte type, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_RemoveSocial(pkg, this.PlayerData.RoleDetail.RoleId, roleId, type);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte result = -1;
                _io.Read(out result);

                RPC.PackageWriter retpkg = new RPC.PackageWriter();
                retpkg.Write(result);
                retpkg.DoReturnPlanes2Client(fwd);
            };
        }

        //拜访好友
//         [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
//         public void RPC_VisitFriend(ulong roleId, RPC.RPCForwardInfo fwd)
//         {
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_VisitFriend(pkg, this.PlayerData.RoleDetail.RoleId, roleId);
//             pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
//             {
//                 sbyte result = -1;
//                 _io.Read(out result);
// 
//                 RPC.PackageWriter retpkg = new RPC.PackageWriter();
//                 retpkg.Write(result);
//                 retpkg.DoReturnPlanes2Client(fwd);
//             };
//         }

        //送礼物
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_SendGift(ulong roleId, int templateId, int count, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter retPkg = new RPC.PackageWriter();
            var itemtemplate = CSTable.ItemUtil.GetItem(templateId) as CSTable.ItemGiftData;
            if (itemtemplate == null)
            {
                Log.Log.Social.Print("itemtemplate is null:{0}", templateId);
                retPkg.Write((sbyte)CSCommon.sRet_SendGift.NoGftTpl);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (itemtemplate.ItemType != (int)CSCommon.eItemType.Gift)
            {
                retPkg.Write((sbyte)CSCommon.sRet_SendGift.NotGiftType);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (CSCommon.SocialCommon.Instance.GiftList.Contains(templateId) == false)
            {
                retPkg.Write((sbyte)CSCommon.sRet_SendGift.ListNoGift);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }

            if (_IsMoneyEnough((CSCommon.eCurrenceType)itemtemplate.BuyCurrenceType, itemtemplate.PurchasePrice * count) == false)
            {
                retPkg.Write((sbyte)CSCommon.sRet_SendGift.LessMoney);
                retPkg.DoReturnPlanes2Client(fwd);
                return;
            }
            int index = CSCommon.SocialCommon.Instance.GiftList.IndexOf(templateId);
            int addvalue = itemtemplate.AddIntimacy * count;

            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_SendGift(pkg, this.PlayerData.RoleDetail.RoleId, roleId, index, count, addvalue);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                RPC.PackageWriter retpkg = new RPC.PackageWriter();
                sbyte result = -1;
                _io.Read(out result);
                if (result == (sbyte)CSCommon.sRet_SendGift.Succeed)
                {
                    _ChangeMoney((CSCommon.eCurrenceType)itemtemplate.BuyCurrenceType, CSCommon.Data.eMoneyChangeType.SendGift, -count);
                }
                retpkg.Write(result);
                retpkg.DoReturnPlanes2Client(fwd);
            };
        }

        public static void SendGift2Client(PlayerInstance player, int index, int count)
        {
            RPC.PackageWriter retpkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_AddGiftCount(retpkg, index, count);
            retpkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }

    }
}
