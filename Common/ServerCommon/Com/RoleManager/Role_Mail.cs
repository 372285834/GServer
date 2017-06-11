using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace ServerCommon.Com
{
    public partial class UserRoleManager : RPC.RPCObject
    {
        #region DB
        private bool DB_CreateMail(CSCommon.Data.MailData mail)
        {
            string condition = "MailId = " + mail.MailId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, mail, true);
            return mDBConnect._ExecuteInsert(dbOp);
        }

        private void DB_UpdateMail(CSCommon.Data.MailData mail)
        {
            string condition = "MailId = " + mail.MailId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.ReplaceData(condition, mail, true);
            mDBConnect._ExecuteInsert(dbOp);
            return;
        }

        private bool DB_DelMail(ulong mailId)
        {
            string condition = "MailId = " + mailId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DelData(condition, new CSCommon.Data.MailData());
            return mDBConnect._ExecuteDelete(dbOp);
        }

        private bool DB_DelMails(ulong roleId)
        {
            string condition = "OwnerId=" + roleId + " and StrItems = " + "\'" + "\'" + " and StrCurrencies = " + "\'" + "\'";
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DelData(condition, new CSCommon.Data.MailData());
            return mDBConnect._ExecuteDelete(dbOp);
        }

        private List<CSCommon.Data.MailData> DB_GetMails(string condition)
        {
            List<CSCommon.Data.MailData> mails = new List<CSCommon.Data.MailData>();
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.MailData(), "");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "MailData");
            if (tab != null)
            {
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(mail, r))
                        continue;
                    mails.Add(mail);
                }
            }
            else
            {
                Log.Log.Mail.Print("RPC_GetRoleMails failed :{0}", condition);
            }
            return mails;
        }

        private CSCommon.Data.MailData DB_GetMail(ulong mailId)
        {
            string condition = "MailId = " + mailId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, new CSCommon.Data.MailData(), "");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "MailData");
            if (tab == null || tab.Rows.Count != 1)
            {
                return null;
            }
            CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
            if (false == ServerFrame.DB.DBConnect.FillObject(mail, tab.Rows[0]))
                return null;
            return mail;
        }
        #endregion

        private List<CSCommon.Data.MailData> GetAllMails(ulong roleId)
        {
            string condition = "OwnerId=" + roleId;
            return DB_GetMails(condition);
        }

        private List<CSCommon.Data.MailData> GetMailsWithItems(ulong roleId)
        {
            string condition = "OwnerId=" + roleId + " and " + "(" + "StrItems != " + "\'" + "\'" + " or " + "StrCurrencies != " + "\'" + "\'" + ")";
            return DB_GetMails(condition);
        }

        public string GetStr(int id, int count)
        {
            return string.Format("{0},{1}|", id, count);
        }

        public void AddSystemMailAndSend(CSCommon.Data.SystemMailData data, ServerFrame.DB.DBConnect connect)
        {
            if (this.mDBConnect.mConnection == null)
            {
                this.mDBConnect = connect;
            }
            DataTable tab = connect._ExecuteSql(data.SelectSql, "roleinfo");
            if (tab != null)
            {
                foreach(DataRow i in tab.Rows)
                {
                    ulong id = Convert.ToUInt64(i["RoleId"]);
                    var role = GetRole(id);
                    if (role != null && role.PlanesConnect != null)
                    {
                        SendPlayerMail(id);
                    }
                }
            }
        }

        /// <summary>
        /// 生成邮件
        /// </summary>
        /// <param name="targetId"></param> 主人Id
        /// <param name="type"></param> 邮件来源枚举，用于从模板表里查询邮件的标题、类型、内容等信息
        /// <param name="arg"></param> 有些邮件内容携带参数
        /// <param name="items"></param> 邮件附件物品
        /// <param name="currencies"></param> 邮件附件货币
        /// <returns></returns>
        public void CreateMailAndSend(ulong targetId, CSCommon.eMailFromType type, string arg = "", string items = "", string currencies = "")
        {
            CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
            mail.MailId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Email);
            mail.OwnerId = targetId;
            mail.StrItems = items;
            mail.StrCurrencies = currencies;
            var temp = CSTable.StaticDataManager.MailInfo[(int)type];
            if (temp != null)
            {
                mail.Type = temp.Type;
                mail.Title = temp.Title;
                mail.ContentStr = string.Format(temp.Content, arg);
            }
            else
            {
                Log.Log.Mail.Print("MailInfo[(int){0}] is null", type.ToString());
            }
            DB_CreateMail(mail);
            SendPlayerMail(targetId);
        }

        public void SendPlayerMail(ulong roleId)
        {
            var role = GetRole(roleId);
            if (role == null)
                return;

            if (role.PlanesConnect != null)
            {
                RPC.PackageWriter pkg = new RPC.PackageWriter();
                H_RPCRoot.smInstance.HGet_PlanesServer(pkg).RPC_SendPlayerMail(pkg, roleId);
                pkg.DoCommand(role.PlanesConnect, RPC.CommandTargetType.DefaultType);
            }
        }

        #region RPC

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetMails(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            List<CSCommon.Data.MailData> mails = GetAllMails(roleId);
            int count = mails.Count;
            pkg.Write(count);
            for (int i = 0; i < count; i++)
            {
                pkg.Write(mails[i]);
            }
            mails.Clear();
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_DelMail(ulong mailId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            if (DB_DelMail(mailId) == false)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_OpenMail(ulong mailId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            CSCommon.Data.MailData mail = DB_GetMail(mailId);
            if (mail == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            mail.State = (byte)CSCommon.eMailState.Opened;
            DB_UpdateMail(mail);
            pkg.Write((sbyte)1);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetMailItems(ulong mailId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            CSCommon.Data.MailData mail = DB_GetMail(mailId);
            if (mail == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            pkg.Write(mail);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);

        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_OneKeyDelMails(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            if (DB_DelMails(roleId) == false)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_OneKeyGetItems(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            List<CSCommon.Data.MailData> mails = GetMailsWithItems(roleId);
            pkg.Write(mails.Count);
            for (int i = 0; i < mails.Count; i++)
            {
                pkg.Write(mails[i]);
            }
            mails.Clear();
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        #endregion
        
    }
}
