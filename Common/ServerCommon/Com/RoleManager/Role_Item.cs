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
        public bool DB_CreateGird(CSCommon.Data.ConsignGridData data)
        {
            string condition = "GirdId = " + data.GirdId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.InsertData(condition, data, true);
            return mDBConnect._ExecuteInsert(dbOp);
        }

        public void DB_DelGird(CSCommon.Data.ConsignGridData data)
        {
            string condition = "GirdId =" + data.GirdId;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.DestroyData(condition, data);
            mDBConnect._ExecuteDestroy(dbOp);
            return;
        }

        public CSCommon.Data.ConsignGridData CreateConsignGird(ulong ownerId, int templateId, int stack, int price)
        {
            var role = GetRole(ownerId);
            if (role == null)
                return null;
            
            CSCommon.Data.ConsignGridData gird = new CSCommon.Data.ConsignGridData();
            gird.GirdId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.ConsignGird);
            gird.OwnerId = ownerId;
            gird.TemplateId = templateId;
            gird.StackNum = stack;
            gird.Price = price;
            gird.PlanesId = role.RoleData.PlanesId;
            float hour = CSCommon.ItemCommon.Instance.MaxConsignTime;
            gird.DelTime = System.DateTime.Now.AddHours(hour);
            if (false == DB_CreateGird(gird))
            {
                Log.Log.Item.Print("DB_CreateGird failed");
                return null;
            }
            return gird;
        }

        public CSCommon.Data.ConsignGridData GetGird(ulong id)
        {
            CSCommon.Data.ConsignGridData gird = new CSCommon.Data.ConsignGridData();
            string condition = "GirdId =" + id;
            ServerFrame.DB.DBOperator dbOp = ServerFrame.DB.DBConnect.SelectData(condition, gird,"");
            System.Data.DataTable tab = mDBConnect._ExecuteSelect(dbOp, "ConsignGirdData");
            if (tab != null && tab.Rows.Count == 1)
            {
                if (false == ServerFrame.DB.DBConnect.FillObject(gird, tab.Rows[0]))
                    return null;
                return gird;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("GetGird failed:{0}" + id);
                return null;
            }
        }

        //24小时内寄售失败，通过邮件返回给玩家
        public void DownGirdOutDate()
        {
            string sql = "select * from ConsignGirdData where DelTime > " + System.DateTime.Now;
            System.Data.DataTable tab = mDBConnect._ExecuteSql(sql, "ConsignGirdData");
            if (tab != null)
            {
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.ConsignGridData data = new CSCommon.Data.ConsignGridData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(data, r))
                        continue;

                    DownGird(data);

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("DownGirdOutDate failed:" + sql);
            }
        }

        private void DownGird(CSCommon.Data.ConsignGridData gird)
        {
            string items = GetStr(gird.TemplateId, gird.StackNum);
            CreateMailAndSend(gird.OwnerId, CSCommon.eMailFromType.ConsignFailedReturn, "", items);
            var role = GetRole(gird.OwnerId);
            DB_DelGird(gird);
        }



        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_ConsignItem(ulong roleId, int templateId, int stack, int price, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_ConsignItem.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            if (CSCommon.ItemCommon.Instance == null)
            {
                Log.Log.Item.Print("CSCommon.ItemCommon.Instance == null");
                pkg.Write((sbyte)CSCommon.eRet_ConsignItem.LessTemp);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            byte roleMaxGirdNum = CSCommon.ItemCommon.Instance.MaxConsignNum;

            string strSQL = "select * from ConsignGirdData where OwnerId = " + roleId;
            List<CSCommon.Data.ConsignGridData> datas = SelectGird(strSQL);
            if (datas.Count >= roleMaxGirdNum)
            {
                pkg.Write((sbyte)CSCommon.eRet_ConsignItem.OverMaxNum);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var gird = CreateConsignGird(roleId, templateId, stack, price);
            if (gird == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_ConsignItem.DBError);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_ConsignItem.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_BuyConsignItem(ulong roleId, ulong itemId,Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var gird = GetGird(itemId);
            if (gird == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.NoItem);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            
            string items = GetStr(gird.TemplateId, gird.StackNum);
            CreateMailAndSend(roleId, CSCommon.eMailFromType.BuyConsignSucceed, "", items);
            DB_DelGird(gird);
            pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.Succeed);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetConsignItem(ulong roleId, ulong itemId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.NoRole);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            var gird = GetGird(itemId);
            if (gird == null)
            {
                pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.NoItem);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)CSCommon.eRet_BuyConsignItem.Succeed);
            pkg.Write(gird.Price);
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGird(ulong roleId, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            string strSQL = "select * from ConsignGirdData where OwnerId = " + roleId;
            List<CSCommon.Data.ConsignGridData> datas = SelectGird(strSQL);
            int count = datas.Count;
            pkg.Write(count);
            for (int i = 0; i < count; i++)
            {
                pkg.Write(datas[i]);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        //Limit(Start,Count) 是从Start + 1 条记录开始读出 Count 条记录
        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGirdByName(ulong roleId, string name, byte findType, byte page, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            byte pageNum = 5;
            int start = (page - 1) * pageNum;
            string strSQL;
            if (findType == (byte)CSCommon.eFindType.Exact)
                strSQL = "select * from ConsignGirdData where name = " + name + "limit start,pageNum";
            else
                strSQL = "select * from ConsignGirdData where name like %" + name + "%" + "limit start,pageNum";      
            List<CSCommon.Data.ConsignGridData> datas = SelectGird(strSQL);
            int count = datas.Count;
            pkg.Write(count);
            for (int i = 0; i < count; i++)
            {
                pkg.Write(datas[i]);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        [RPC.RPCMethodAttribute((int)RPC.RPCExecuteLimitLevel.Developer, false)]
        public void RPC_GetRoleGirdByType(ulong roleId, byte itemType, byte page, Iocp.NetConnection connect, RPC.RPCForwardInfo fwd)
        {
            RPC.PackageWriter pkg = new RPC.PackageWriter();
            var role = GetRole(roleId);
            if (role == null)
            {
                pkg.Write((sbyte)-1);
                pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
                return;
            }
            pkg.Write((sbyte)1);
            byte pageNum = 5;
            int start = (page - 1) * pageNum;
            string strSQL = "select * from ConsignGirdData where Type = " + itemType.ToString() + "limit start,pageNum";
            List<CSCommon.Data.ConsignGridData> datas = SelectGird(strSQL);
            int count = datas.Count;
            pkg.Write(count);
            for (int i = 0; i < count; i++)
            {
                pkg.Write(datas[i]);
            }
            pkg.DoReturnCommand2(connect, fwd.ReturnSerialId);
        }

        public List<CSCommon.Data.ConsignGridData> SelectGird(string sql)
        {
            List<CSCommon.Data.ConsignGridData> datas = new List<CSCommon.Data.ConsignGridData>();
            datas.Clear();
            System.Data.DataTable tab = mDBConnect._ExecuteSql(sql, "ConsignGirdData");
            if (tab != null)
            {
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    CSCommon.Data.ConsignGridData data = new CSCommon.Data.ConsignGridData();
                    if (false == ServerFrame.DB.DBConnect.FillObject(data, r))
                        continue;
                    datas.Add(data);
                }
                return datas;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("角色获取关系数据库执行失败:" + sql);
                return null;
            }
        }
    }
}
