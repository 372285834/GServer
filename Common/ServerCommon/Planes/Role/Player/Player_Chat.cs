using System;
using System.Collections.Generic;

namespace ServerCommon.Planes
{
    public class HyperLinkData
    {
        string mMessages;
        public string Messages
        {
            get { return mMessages; }
            set { mMessages = value; }
        }

        RPC.DataWriter mDatas = new RPC.DataWriter();
        public RPC.DataWriter Datas
        {
            get { return mDatas; }
            set { mDatas = value; }
        }
    }

    public partial class PlayerInstance : RPC.RPCObject
    {
        #region 聊天
        public static void SendTalkMsg2Client(PlayerInstance player, sbyte channel, string sender, string msg, RPC.DataWriter hyperLink)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            Wuxia.H_RpcRoot.smInstance.RPC_AddTalkMsg(pkg, channel, sender, msg, hyperLink);
            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_Say(sbyte channel, string msg, RPC.DataReader hyperlink, RPC.RPCForwardInfo fwd)
        {
            msg = CheckMaskWord(msg);
            RPC.DataWriter data = new RPC.DataWriter();
            data.Write(hyperlink.mHandle);
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            SayToComServer(channel, msg, data);
        }

        public void SayToComServer(sbyte channel, string msg, RPC.DataWriter data)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_Say(pkg, this.Id, channel, msg, data);
            pkg.DoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType);
        }
        public string CheckMaskWord(string msg)
        {
            var dict = CSTable.StaticDataManager.MaskWord.Dict;
            foreach (var i in dict.Values)
            {
                msg = msg.Replace(i.word, ChangToStar(i.word.Length));
            }
            return msg;
        }
        public string ChangToStar(int len)
        {
            string str = "";
            for (int i = 0; i < len;i++ )
            {
                str += "*";
            }
            return str;
        }
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Player, false)]
        public void RPC_Whisper(string targetName, string msg, RPC.DataReader hyperlink, RPC.RPCForwardInfo fwd)
        {
            sbyte success = 0;
            RPC.DataWriter data = new RPC.DataWriter();
            if (hyperlink != null)
            {
                hyperlink.Read(out success);
                data.Write(success);
                if (success == (sbyte)1)//物品
                {
                    CSCommon.Data.ItemData item = new CSCommon.Data.ItemData();
                    hyperlink.Read(item, true);
                    data.Write(item, true);
                }
            }
            msg = CheckMaskWord(msg);
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            H_RPCRoot.smInstance.HGet_ComServer(pkg).HGet_UserRoleManager(pkg).RPC_SayToRole(pkg, this.Id, targetName, msg, data);
            pkg.WaitDoCommand(IPlanesServer.Instance.ComConnect, RPC.CommandTargetType.DefaultType, new System.Diagnostics.StackTrace(1, true)).OnFarCallFinished = delegate(RPC.PackageProxy _io, bool bTimeOut)
            {
                sbyte result = -1;
                _io.Read(out result);
                if (result < 0)
                {
                    SendTalkMsg2Client(this, (sbyte)CSCommon.eSayChannel.WhisperChannel, "", "无效发送对象", new RPC.DataWriter());
                }
                else
                {
                    SendTalkMsg2Client(this, (sbyte)CSCommon.eSayChannel.WhisperChannel, this.RoleName, msg, data);
                }
            };
        }


        #endregion

        #region 消息
        public static void SendMsg2Client(PlayerInstance player, CSCommon.Data.Message msg)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            pkg.SetSinglePkg();
            Wuxia.H_RpcRoot.smInstance.RPC_ReceiveMsg(pkg, msg);
            pkg.DoCommandPlanes2Client(player.Planes2GateConnect, player.ClientLinkId);
        }
        #endregion
    }
}