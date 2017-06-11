using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Com
{
    public partial class UserRoleManager : RPC.RPCObject
    {

        #region 好友操作

        public CSCommon.Data.SocialRoleInfo CreateSocialRoleInfo(CSCommon.Data.SocialData data, UserRole role)
        {
            CSCommon.Data.SocialRoleInfo info = new CSCommon.Data.SocialRoleInfo();
            info.id = data.OtherId;
            info.name = role.RoleData.Name;
            info.profession = role.RoleData.Profession;
            info.camp = role.RoleData.Camp;
            info.level = role.RoleData.Level;
            info.socialData = data;
            if (role.PlanesConnect != null)
            {
                info.state = (byte)CSCommon.eOnlineState.Online;
            }
            return info;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_GetSocialList(ulong roleId, byte type, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.SetSinglePkg();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Social.Print("role is null , {0}", roleId);
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            UpdateSocialInfoList((CSCommon.eSocialType)type, role);
            pkg.Write((sbyte)1);
            pkg.Write(role.mSocialInfoList.Count);
            foreach (var s in role.mSocialInfoList.Values)
            {
                pkg.Write(s);
                pkg.Write(s.socialData);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        private void UpdateSocialInfoList(CSCommon.eSocialType type, UserRole role)
        {
            var socials = role.SocialManager.GetSocialList(type);

            role.mSocialInfoList.Clear();

            foreach (var i in socials)
            {
                var retPlayer = this.GetRole(i.OtherId);
                if (retPlayer == null)
                {
                    //此角色不存在，建议删除
                    continue;
                }
                var info = CreateSocialRoleInfo(i, retPlayer);
                role.mSocialInfoList[info.id] = info;
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_AddSocial(ulong roleId, string otherName, byte type, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.SetSinglePkg();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Social.Print("role is null , {0}", roleId);
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = this.GetRole(otherName);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (roleId == other.RoleData.RoleId)
            {
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.RoleIsOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.SocialManager.IsSocial(other.RoleData.RoleId, (CSCommon.eSocialType)type))
            {
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.IsSocial);  //已经是这种关系
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.SocialManager.IsCoupleType((CSCommon.eSocialType)type))
            {
                if (false == role.SocialManager.IsSocial(other.RoleData.RoleId, CSCommon.eSocialType.Friend))
                {
                    pkg.Write((sbyte)CSCommon.eRet_AddSocial.NotFriend);//不是好友
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                //发送邀请
                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Social, _GetSocialMsgType((CSCommon.eSocialType)type), role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("玩家{0}请求与你成为{1}关系", role.RoleData.Name, _GetStrSocial((CSCommon.eSocialType)type));
                SendMessageToOther(other, msg);
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.AskSucceed);
            }
            else
            {
                CSCommon.Data.SocialData sd = role.SocialManager.AddSocial(other.RoleData.RoleId, (CSCommon.eSocialType)type);
                if (sd == null)
                {
                    pkg.Write((sbyte)CSCommon.eRet_AddSocial.IsSocial);   //已经是这种关系
                    pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                    return;
                }
                var info = CreateSocialRoleInfo(sd, other);
                pkg.Write((sbyte)CSCommon.eRet_AddSocial.AddSucceed);
                pkg.Write(info);
                pkg.Write(sd);

                if ((CSCommon.eSocialType)type == CSCommon.eSocialType.Friend)
                {
                    //发送消息
                    CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Social, _GetSocialMsgType((CSCommon.eSocialType)type), role.RoleData.Name, other.RoleData.RoleId);
                    msg.ShowInfo = string.Format("玩家{0}把你加为好友", role.RoleData.Name);
                    SendMessageToOther(other, msg);
                }
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }
        
        //同意或拒绝成为亲人关系  type信息类型 
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_OperateAddSocial(ulong roleId, string otherName, byte type, byte operate, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Social.Print("role is null , {0}", roleId);
                pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = this.GetRole(otherName);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (roleId == other.RoleData.RoleId)
            {
                pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.RoleIsOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            CSCommon.eSocialType sType = _GetSocialTypeFromMsg((CSCommon.eMessageType)type);
            if (sType == CSCommon.eSocialType.None)
            {
                pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.TypeError); //类型错误
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.SocialManager.IsSocial(other.RoleData.RoleId, sType))
            {
                pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.IsSocial);  //已经是这种关系
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if ((CSCommon.eOperateAsk)operate == CSCommon.eOperateAsk.Accept)
            {
                role.SocialManager.AddSocial(other.RoleData.RoleId, (CSCommon.eSocialType)type);
                role.SocialManager.AddSocial(other.RoleData.RoleId, CSCommon.eSocialType.Friend);
                other.SocialManager.AddSocial(role.RoleData.RoleId, (CSCommon.eSocialType)type);
                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Social, CSCommon.eMessageType.Couple_Accept, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("玩家{0}同意与你成为{1}关系", role.RoleData.Name, _GetStrSocial(sType));
                SendMessageToOther(other, msg);
            }
            else
            {
                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Social, CSCommon.eMessageType.Couple_Refuse, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("玩家{0}拒绝与你成为{1}关系", role.RoleData.Name, _GetStrSocial(sType));
                SendMessageToOther(other, msg);
            }
            pkg.Write((sbyte)CSCommon.eRet_OperateAddSocial.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }

        public void SendMessageToOther(UserRole other, CSCommon.Data.Message msg)
        {
            if (other.PlanesConnect != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerMsg(pkg, other.RoleData.RoleId, msg);
                pkg.DoCommand(other.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
            else
            {
                DB_CreateMessage(msg);
            }
        }

        public CSCommon.eSocialType _GetSocialTypeFromMsg(CSCommon.eMessageType type)
        {
            CSCommon.eSocialType mType = CSCommon.eSocialType.None;
            switch (type)
            {
                case CSCommon.eMessageType.Friend_Ask:
                    mType = CSCommon.eSocialType.Friend;
                    break;
                case CSCommon.eMessageType.ManWife_Ask:
                    mType = CSCommon.eSocialType.ManWife;
                    break;
                case CSCommon.eMessageType.Brother_Ask:
                    mType = CSCommon.eSocialType.Brother;
                    break;
                case CSCommon.eMessageType.BasicFriends_Ask:
                    mType = CSCommon.eSocialType.BasicFriends;
                    break;
                case CSCommon.eMessageType.Lover_Ask:
                    mType = CSCommon.eSocialType.Lover;
                    break;
                case CSCommon.eMessageType.Sister_Ask:
                    mType = CSCommon.eSocialType.Sister;
                    break;
                case CSCommon.eMessageType.Lily_Ask:
                    mType = CSCommon.eSocialType.Lily;
                    break;
                default:
                    break;
            }
            return mType;
        }

        public CSCommon.eMessageType _GetSocialMsgType(CSCommon.eSocialType type)
        {
            CSCommon.eMessageType mType = CSCommon.eMessageType.Friend_Ask;
            switch (type)
            {
                case CSCommon.eSocialType.Friend:
                    mType = CSCommon.eMessageType.Friend_Ask;
                    break;
                case CSCommon.eSocialType.ManWife:
                    mType = CSCommon.eMessageType.ManWife_Ask;
                    break;
                case CSCommon.eSocialType.Brother:
                    mType = CSCommon.eMessageType.Brother_Ask;
                    break;
                case CSCommon.eSocialType.BasicFriends:
                    mType = CSCommon.eMessageType.BasicFriends_Ask;
                    break;
                case CSCommon.eSocialType.Lover:
                    mType = CSCommon.eMessageType.Lover_Ask;
                    break;
                case CSCommon.eSocialType.Sister:
                    mType = CSCommon.eMessageType.Sister_Ask;
                    break;
                case CSCommon.eSocialType.Lily:
                    mType = CSCommon.eMessageType.Lily_Ask;
                    break;
                default:
                    break;
            }
            return mType;
        }

        public string _GetStrSocial(CSCommon.eSocialType type)
        {
            string name = "好友";
            switch (type)
            {
                case CSCommon.eSocialType.Friend:
                    name = "好友";
                    break;
                case CSCommon.eSocialType.ManWife:
                    name = "夫妻";
                    break;
                case CSCommon.eSocialType.Brother:
                    name = "兄弟";
                    break;
                case CSCommon.eSocialType.BasicFriends:
                    name = "基友";
                    break;
                case CSCommon.eSocialType.Lover:
                    name = "情人";
                    break;
                case CSCommon.eSocialType.Sister:
                    name = "姐妹";
                    break;
                case CSCommon.eSocialType.Lily:
                    name = "百合";
                    break;
                default:
                    break;
            }
            return name;
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_RemoveSocial(ulong roleId, ulong otherId, byte type, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Social.Print("role is null , {0}", roleId);
                pkg.Write((sbyte)CSCommon.eRet_RemoveSocial.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = this.GetRole(otherId);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveSocial.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (role.SocialManager.IsSocial(otherId, (CSCommon.eSocialType)type) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveSocial.NotSocial);  //不是这种关系
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (role.SocialManager.RemoveSocial(otherId, (CSCommon.eSocialType)type) == false)
            {
                pkg.Write((sbyte)CSCommon.eRet_RemoveSocial.RemoveError);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (type == (byte)CSCommon.eSocialType.Friend && role.SocialManager.IsCouple(otherId))
            {
                //删除双方亲人关系
                role.SocialManager.RemoveCouple(otherId);
                other.SocialManager.RemoveCouple(roleId);
                CSCommon.Data.Message msg = CreateMessage(CSCommon.eMessageFrom.Social, CSCommon.eMessageType.Couple_Remove, role.RoleData.Name, other.RoleData.RoleId);
                msg.ShowInfo = string.Format("玩家{0}与你成为解除亲人关系", role.RoleData.Name);
                SendMessageToOther(other, msg);
            }
            pkg.Write((sbyte)CSCommon.eRet_RemoveSocial.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        //[RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_VisitFriend(ulong roleId, ulong otherId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            return;
//             RPC.PackageWriter pkg = new RPC.PackageWriter();
//             var role = this.GetRole(roleId);
//             if (role == null)
//             {
//                 Log.Log.Social.Print("role is null , {0}", roleId);
//                 pkg.Write((sbyte)-1);
//                 pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
//                 return;
//             }
//             var other = this.GetRole(otherId);
//             if (other == null)
//             {
//                 pkg.Write((sbyte)-2);
//                 pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
//                 return;
//             }
// 
//             sbyte result = role.SocialManager.VisitFriend(otherId);
//             if (result == -1)
//             {
//                 pkg.Write((sbyte)-3);
//                 pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
//                 return;
//             }
// 
//             if (result == -2)
//             {
//                 pkg.Write((sbyte)-4);
//                 pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
//                 return;
//             }
// 
//             pkg.Write((sbyte)1);
//             pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, true)]
        public void RPC_SendGift(ulong roleId, ulong otherId, int index, int count, int addvalue, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = this.GetRole(roleId);
            if (role == null)
            {
                Log.Log.Social.Print("role is null , {0}", roleId);
                pkg.Write((sbyte)CSCommon.sRet_SendGift.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var other = this.GetRole(otherId);
            if (other == null)
            {
                pkg.Write((sbyte)CSCommon.sRet_SendGift.NoOther);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }

            if (role.SocialManager.IsCouple(otherId) == false)
            {
                pkg.Write((sbyte)CSCommon.sRet_SendGift.NotCouple);  //不是这种关系
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            role.SocialManager.AddIntimacy(otherId, addvalue);
            other.GiftData.AddGiftCount(index, count);

            if (other.PlanesConnect == null)
            {
                DB_SaveGiftData(other);
            }
            else
            {
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(retPkg).RPC_AddGiftCount(retPkg, otherId, index, count);
                retPkg.DoCommand(other.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
            pkg.Write((sbyte)CSCommon.sRet_SendGift.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }

        #endregion

        #region 好友相关

        private void DB_InitSocial(UserRole role)
        {
            string condition = "OwnerId=" + role.RoleData.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.SocialData(), "");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "SocialData");
            if (tab != null)
            {
                List<CSCommon.Data.SocialData> items = new List<CSCommon.Data.SocialData>();

                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.SocialData itemData = new CSCommon.Data.SocialData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(itemData, r))
                        continue;
                    items.Add(itemData);
                }
                role.Socials.Clear();
                role.Socials.AddRange(items);
                role.SocialManager.InitSocialList(role);
                items.Clear();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取关系数据库执行失败:" + dbOp.SqlCode);
            }
        }

        private void DB_InitGiftData(UserRole role)
        {
            string condition = "OwnerId=" + role.RoleData.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.GiftData(), "");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "GiftData");
            if (tab != null && tab.Rows.Count == 1)
            {
                CSCommon.Data.GiftData gd = new CSCommon.Data.GiftData();
                if (ServerFrame.DB.DBConnect.FillObject(gd, tab.Rows[0]))
                {
                    role.GiftData = gd;
                }

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取礼物数据库执行失败:" + dbOp.SqlCode);
            }

        }

        private void DB_SaveRoleData(UserRole role)
        {
            var condition = "RoleId=" + role.RoleData.RoleId;
            var dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, role.RoleData, null);
            mDBConnect._ExecuteUpdate(dbOp);
        }

        private void DB_SaveSocials(UserRole role)
        {
            var items = role.SocialManager.SaveSocialList();
            foreach (var i in items)
            {
                CSCommon.Data.SocialData outItem = null;
                foreach (var j in role.Socials)
                {
                    if (j.OtherId == i.OtherId)
                    {
                        outItem = j;
                        break;
                    }
                }
                if (outItem != null)
                {
                    string condition = "OtherId = " + i.OtherId;
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.UpdateData(condition, i, outItem);
                    mDBConnect._ExecuteUpdate(dbOp);

                }
                else
                {
                    string condition = "OtherId = " + i.OtherId;
                    ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, i, true);
                    mDBConnect._ExecuteInsert(dbOp);
                }
            }
            role.Socials = items;
        }

        private void DB_SaveGiftData(UserRole role)
        {
            string condition = "OtherId = " + role.RoleData.RoleId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, role.GiftData, true);
            mDBConnect._ExecuteInsert(dbOp);
        }
        #endregion

    }
}
