using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCommon.Planes
{
    public partial class PlayerInstance : RPC.RPCObject
    {
        public static void NoticeNewMail2Client(PlayerInstance player)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_AddMail(pkg);
            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetMails(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetMails(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                retPkg.SetSinglePkg();
                List<CSCommon.Data.MailData> mails = new List<CSCommon.Data.MailData>();
                int count = 0;
                _io.Read(out count);
                retPkg.Write(count);
                for (int i = 0; i < count; i++)
                {
                    CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
                    _io.Read(mail);
                    retPkg.Write(mail);
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_DelMail(ulong mailId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_DelMail(pkg,mailId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OpenMail(ulong mailId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OpenMail(pkg, mailId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        //提取附件
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_GetMailItems(ulong mailId, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_GetMailItems(pkg, mailId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result;
                _io.Read(out result);
                if (result == 1)
                {
                    CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
                    _io.Read(mail);
                    if (_GetMailItems(mail.StrItems) == false)
                    {
                        pkg.Write((sbyte)-2); //背包空间不足
                        pkg.DoReturnPlanes2Client(fwd);
                        return;
                    }
                    _GetMailCurrencies(mail.StrCurrencies);
                    retPkg.Write((sbyte)1);

                    //删除邮件
                    RPC.PackageWriter delpkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_ComServer(delpkg).HGet_UserRoleManager(delpkg).RPC_DelMail(delpkg, mailId);
                    delpkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
                }
                else
                {
                    retPkg.Write((sbyte)-1);
                }
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        public bool _GetMailItems(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            Dictionary<int, int> items = ServerFrame.Util.ParsingStr(str);
            if (items != null && items.Count > 0)
            {
                if (this.Bag.GetEmptyCount() < items.Count)
                    return false;

                foreach (var i in items)
                {
                    CreateItemToBag(i.Key, i.Value);
                }
            }
            return true;
        }



        public void _GetMailCurrencies(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;
            Dictionary<int, int> items = ServerFrame.Util.ParsingStr(str);
            if (items != null)
            {
                foreach (var i in items)
                {
                    if (i.Key <= (int)CSCommon.eCurrenceType.Unknow || i.Key >= (int)CSCommon.eCurrenceType.MaxCurrenceTypeNum)
                        continue;
                    if (i.Value <= 0)
                        continue;
                    _ChangeMoney((CSCommon.eCurrenceType)i.Key, CSCommon.Data.eMoneyChangeType.Mail, i.Value);
                }
            }
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OneKeyDelMails(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OneKeyDelMails(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                sbyte result;
                _io.Read(out result);
                retPkg.Write(result);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_OneKeyGetItems(RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_OneKeyGetItems(pkg, PlayerData.RoleDetail.RoleId);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, null).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool isTimeOut)
            {
                if (isTimeOut)
                    return;
                RPC.PackageWriter retPkg = new RPC.PackageWriter();
                List<CSCommon.Data.MailData> mails = new List<CSCommon.Data.MailData>();
                int count = 0;
                _io.Read(out count);
                for (int i = 0; i < count; i++)
                {
                    CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
                    _io.Read(mail);
                    if (_GetMailItems(mail.StrItems) == false)
                    {
                        retPkg.Write((sbyte)-1); //背包空间不足
                        retPkg.DoReturnPlanes2Client(fwd);
                        return;
                    }
                    _GetMailCurrencies(mail.StrCurrencies);
                    //删除邮件
                    RPC.PackageWriter delpkg = new RPC.PackageWriter();
                    H_RPCRoot.smInstance.HGet_ComServer(delpkg).HGet_UserRoleManager(delpkg).RPC_DelMail(delpkg, mail.MailId);
                    delpkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
                }
                retPkg.Write((sbyte)1);
                retPkg.DoReturnPlanes2Client(fwd);
            };
        }

    }
}
