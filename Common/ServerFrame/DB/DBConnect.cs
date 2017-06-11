using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerFrame;
using MySql.Data.MySqlClient;

namespace ServerFrame.DB
{
    public enum SqlExeType
    {
        Update,
        Insert,
        Select,
        Delete,
        Destroy,
    }

    public class AsyncExecuter
    {
        public delegate void FOnExec();
        public FOnExec Exec;
    }

    public class AsyncExecuteThread
    {
        public AsyncExecuter AsyncExe(bool pushQueue)
        {
            AsyncExecuter holder = new AsyncExecuter();
            if (pushQueue)
            {
                lock (this)
                {
                    mLoginQueue.Add(holder);
                }
            }
            return holder;
        }

        List<AsyncExecuter> mLoginQueue = new List<AsyncExecuter>();
        public int GetNumber()
        {
            return mLoginQueue.Count;
        }
        void Tick()
        {
            if (mLoginQueue.Count > 0)
            {
                AsyncExecuter atom = null;
                lock (this)
                {
                    atom = mLoginQueue[0];
                    if (atom.Exec != null)
                        mLoginQueue.RemoveAt(0);
                    else
                        return;
                }
                try
                {
                    atom.Exec();
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                    Log.Log.Common.Print(ex.StackTrace.ToString());
                }
            }
        }
        bool mRunning;
        private System.Threading.Thread mThread;
        public void ThreadLoop()
        {
            while (mRunning)
            {
                Tick();
                System.Threading.Thread.Sleep(5);
            }
            mThread = null;

            Log.Log.Common.Print("AsyncExecuteThread Thread exit!");
        }
        public void StartThread()
        {
            mRunning = true;
            mThread = new System.Threading.Thread(new System.Threading.ThreadStart(this.ThreadLoop));
            mThread.Start();
        }
        public void StopThread()
        {
            mRunning = false;
        }
    }

    public class AsyncExecuteThreadManager
    {
        static AsyncExecuteThreadManager smInstance = new AsyncExecuteThreadManager();
        public static AsyncExecuteThreadManager Instance
        {
            get { return smInstance; }
        }

        List<AsyncExecuteThread> mExes = new List<AsyncExecuteThread>();

        public AsyncExecuteThread GetAsyncExecuteThread(int i)
        {
            return mExes[i];
        }

        public AsyncExecuter AsyncExe(bool pushQueue)
        {
            lock (this)
            {
                int MinNumber = Int32.MaxValue;
                AsyncExecuteThread exe = null;
                foreach (var i in mExes)
                {
                    if (i.GetNumber() < MinNumber)
                    {
                        MinNumber = i.GetNumber();
                        exe = i;
                    }
                }
                if (exe != null)
                {
                    return exe.AsyncExe(pushQueue);
                }
                return null;
            }
        }

        public void InitManager(int nThread)
        {
            mExes.Clear();
            for (int i = 0; i < nThread; i++)
            {
                mExes.Add(new AsyncExecuteThread());
            }
        }

        public void StartThread()
        {
            foreach (var i in mExes)
            {
                i.StartThread();
            }
        }
        public void StopThread()
        {
            foreach (var i in mExes)
            {
                i.StopThread();
            }
        }
    }

    public class DBOperator
    {
        public List<MySql.Data.MySqlClient.MySqlParameter> SqlParameters = new List<MySql.Data.MySqlClient.MySqlParameter>();
        public string SqlCode;
        public SqlExeType ExeType;

        public AsyncExecuter Executer = null;

        public MySqlCommand ToSqlCommand(MySqlConnection dbConnect)
        {
            var cmd = new MySql.Data.MySqlClient.MySqlCommand(SqlCode, dbConnect);
            if (SqlParameters != null)
            {
                foreach (var i in SqlParameters)
                {
                    cmd.Parameters.Add(i);
                }
            }
            return cmd;
        }
    }


    public class DBConnect
    {
        public static string ConnectIP;

        #region 数据库基础操作
        const string EndLine = "\r\n";
        public MySqlConnection mConnection;
        MySqlDataAdapter mDataAdapter;
        public bool OpenConnect(string connectSql = "")
        {
            string ip;
            if (connectSql == "")
            {
                ip = ConnectIP;
            }
            else
            {
                ip = connectSql;
            }
            string name = System.String.Format("server={0};database=wuxia;uid=root;pwd=123456;", ip);
            
            try
            {
                mConnection = new MySql.Data.MySqlClient.MySqlConnection(name);
                mConnection.Open();
                mDataAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();
            }
            catch (System.Exception exp)
            {
                System.Diagnostics.Debug.Write(exp.ToString());
                return false;
            }

            return false;
        }
        public void CloseConnect()
        {
            try
            {
                if (mConnection != null)
                    mConnection.Close();
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
            }
        }
        public bool IsValidConnect()
        {
            if (mConnection.State != System.Data.ConnectionState.Open)
                return false;
            return true;
        }
        public void Tick()
        {
            if (mConnection != null && IsValidConnect() == false)
            {
                Log.Log.Server.Print("要命，数据库断开了！");
                ReOpen();
            }
            if (reOpen && IsValidConnect())
            {
                Log.Log.Server.Print("重新连上数据库");
                reOpen = false;
            }
        }

        bool reOpen = false;
        public void ReOpen()
        {
            reOpen = true;
            mConnection.Open();
        }



        //后面都是产生code后，如果update,insert都会保存在一个队列里面，每个玩家一个sqlcode保存队列
        //如果sqlcode队列在另外线程执行空后，才能从删除列表彻底把对象释放，以后再需要的时候从数据库读取
        //如果执行队列没有为空，说明还有存储数据没有完成，玩家不能彻底卸载，万一他在这过程中上来了，那么
        //直接从内存里面把他的数据取出来用就好了

        public static string SqlSafeString(string str)
        {
            string result = str.Replace("\'", "\'\'");
            //result = result.Replace("\"", "\"\"");
            //result = result.Replace("[", "[[]");//这个可能不需要，这是用like语法的时候的通配符的问题，我们没有用到like语句
            //result = result.Replace("%", "[%]");//这个可能不需要
            //result = result.Replace("_", "[_]");//这个可能不需要
            return result;
        }
        #endregion

        public static ulong Guid2SqlString(ulong id)
        {
            return id;
        }
        
        //存储过程
        public int CallSqlSP(string spName, params System.Object[] args)
        {
            System.Data.DataSet result = new System.Data.DataSet();
            mDataAdapter.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand("select * from sys.parameters where object_id=object_id('" + spName + "')", mConnection);
            MySql.Data.MySqlClient.MySqlCommandBuilder myCB = new MySql.Data.MySqlClient.MySqlCommandBuilder(mDataAdapter);
            mDataAdapter.Fill(result, "SP");
            List<string> list = new List<string>();
            foreach (System.Data.DataRow r in result.Tables[0].Rows)
            {
                list.Add(r["name"].ToString());
            }
            if (args.Count() != list.Count())
            {  //参数不符
                System.Diagnostics.Debug.WriteLine("运行存储过程失败:参数不符");
                return -1;           
            }                        
            int reNum = -1;
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(spName, mConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            for (int i = 0; i < list.Count(); i++)
            {
                cmd.Parameters.AddWithValue(list[i], args[i].ToString());
            }

            try
            {                
                reNum = cmd.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
            	return -1;
            }                        
            return reNum;
        }

        #region Sql Select

        public static bool FillDataRow(System.Data.DataRow row, object obj)
        {
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return false;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            foreach(var dbBind in clsDesc.Fields)
            {
                var p = dbBind.Property;
                try
                {
                    row[dbBind.Field] = p.GetValue(obj, null);
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                }

            }



            return true;
        }



        public static bool FillObject(object obj, System.Data.DataRow row)
        {
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return false;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            foreach(var dbBind in clsDesc.Fields)
            {
                var p = dbBind.Property;
                try
                {
                    if (row[dbBind.Field] is System.DBNull)
                        continue;

                    if (p.PropertyType == typeof(string))
                    {
                        p.SetValue(obj, row[dbBind.Field].ToString(), null);
                    }
                    else if (p.PropertyType == typeof(Guid))
                    {
                        /*string guidStr = row[dbBind.Field].ToString();
                        if (guidStr == "0")
                            p.SetValue(obj, Guid.NewGuid(), null);
                        else
                            p.SetValue(obj, Guid.Parse(guidStr), null);*/

                        //CONVERT(uniqueidentifier,'313A857C-7251-45A0-A6A7-8482AC5D9B1D')

                        p.SetValue(obj, Guid.Parse(row[dbBind.Field].ToString()), null);
                    }
                    else if (p.PropertyType == typeof(byte[]))
                    {
                        p.SetValue(obj, (byte[])row[dbBind.Field], null);
                    }
                    else if (p.PropertyType == typeof(SByte))
                    {
                        p.SetValue(obj, (SByte)System.Convert.ToInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(Int16))
                    {
                        p.SetValue(obj, (Int16)System.Convert.ToInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(Int32))
                    {
                        p.SetValue(obj, System.Convert.ToInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(System.DateTime))
                    {
                        p.SetValue(obj, row[dbBind.Field], null);
                    }
                    else if (p.PropertyType == typeof(Int64))
                    {
                        p.SetValue(obj, System.Convert.ToInt64(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(Byte))
                    {
                        p.SetValue(obj, (Byte)System.Convert.ToUInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(UInt16))
                    {
                        p.SetValue(obj, (UInt16)System.Convert.ToUInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(UInt32))
                    {
                        p.SetValue(obj, System.Convert.ToUInt32(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(UInt64))
                    {
                        p.SetValue(obj, System.Convert.ToUInt64(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(Single))
                    {
                        p.SetValue(obj, System.Convert.ToSingle(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(Double))
                    {
                        p.SetValue(obj, System.Convert.ToDouble(row[dbBind.Field]), null);
                    }
                    else if (p.PropertyType == typeof(System.DateTime))
                    {
                        p.SetValue(obj, row[dbBind.Field], null);
                    }
                    else if (p.PropertyType == typeof(System.Byte[]))
                    {
                        p.SetValue(obj, row[dbBind.Field], null);
                    }
                    else if (p.PropertyType.IsEnum)
                    {
                        p.SetValue(obj, System.Convert.ToInt32(row[dbBind.Field]), null);
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Log.Common.Print(ex.ToString());
                }
            }
            return true;
        }

        public System.Data.DataTable _ExecuteSelect(DBOperator dbOp,string tabName)
        {
            System.Data.DataSet result = new System.Data.DataSet();
            try
            {
                mDataAdapter.SelectCommand = dbOp.ToSqlCommand(mConnection);
                //System.Data.SqlClient.SqlCommandBuilder myCB = new System.Data.SqlClient.SqlCommandBuilder(mDataAdapter);
                //int iRet = mDataAdapter.SelectCommand.ExecuteReader()

                mDataAdapter.Fill(result, tabName);
                mDataAdapter.SelectCommand = null;
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(e.StackTrace.ToString());
                return null;
            }
            return result.Tables[0];
        }

        public System.Data.DataTable _ExecuteSql(string sql, string tabName)
        {
            System.Data.DataSet result = new System.Data.DataSet();
            try
            {
                DBOperator dbOp = new DBOperator();
                dbOp.ExeType = SqlExeType.Select;
                dbOp.SqlCode = sql;
                mDataAdapter.SelectCommand = dbOp.ToSqlCommand(mConnection);
                mDataAdapter.Fill(result, tabName);
                mDataAdapter.SelectCommand = null;
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(e.StackTrace.ToString());
                return null;
            }
            return result.Tables[0];
        }

        public static DBOperator SelectData(string condition, object obj, string prefix)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Select;
            string result = "";
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            bool first = true;
            foreach (var dbBind in clsDesc.Fields)
            {
                if (first)
                {
                    result += dbBind.Field;
                    first = false;
                }
                else
                {
                    result += "," + dbBind.Field;
                }
            }
            if (string.IsNullOrEmpty(condition))
                //return "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0" + EndLine;
                dbOp.SqlCode = "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0" + EndLine;
            else
                //return "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0 and " + condition + EndLine;
                dbOp.SqlCode = "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0 and " + condition + EndLine;

            return dbOp;
        }

        //重载select函数
        public static DBOperator SelectData(string condition, object obj, string prefix, string aszAfter)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Select;
            string result = "";
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            bool first = true;
            foreach (var dbBind in clsDesc.Fields)
            {
                if (first)
                {
                    result += dbBind.Field;
                    first = false;
                }
                else
                {
                    result += "," + dbBind.Field;
                }
            }
            if (string.IsNullOrEmpty(condition))
                //return "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0" + EndLine;
                dbOp.SqlCode = "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0" + " " + aszAfter + EndLine;
            else
                //return "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0 and " + condition + EndLine;
                dbOp.SqlCode = "select " + prefix + result + " from " + bindTab.Table + " where Deleted=0 and " + condition + " " + aszAfter + EndLine;

            return dbOp;
        }




        public UInt64 GetDataCount(string tabName)
        {
            try
            {
                mDataAdapter.SelectCommand = new MySql.Data.MySqlClient.MySqlCommand("select count(*) totalCount from " + tabName, mConnection);
                UInt64 retNum = Convert.ToUInt64(mDataAdapter.SelectCommand.ExecuteScalar());
                return retNum;
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());
                Log.Log.Common.Print(ex.StackTrace.ToString());
                return 0;
            }
        }

        #endregion

        #region Sql Update

        public void _ExecuteUpdate(DBOperator dbOp)
        {
            if (dbOp == null)
            {
                return;
            }
            try
            {
                mDataAdapter.UpdateCommand = dbOp.ToSqlCommand(mConnection);
                
                mDataAdapter.UpdateCommand.ExecuteNonQuery();
                mDataAdapter.UpdateCommand = null;
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(dbOp.SqlCode);
            }
        }

        public static DBOperator UpdateData(string condition, object obj, object templateobj)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Update;

            if (templateobj != null && obj.GetType() != templateobj.GetType())
                return null;

            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);

            bool first = true;
            string result = "";
            foreach (var dbBind in clsDesc.Fields)
            {
                System.Reflection.PropertyInfo p = dbBind.Property;               

                object fv = p.GetValue(obj, null);
                if (fv == null)
                    continue;

                if (templateobj != null && fv.Equals(p.GetValue(templateobj, null)))
                    continue;
                string valueSql;
                bool needStringFlag = true;
                if (p.PropertyType.IsEnum)
                {
                    valueSql = System.Convert.ToInt32(fv).ToString();
                }
                else if (p.PropertyType == typeof(System.DateTime))
                {
                    valueSql = System.String.Format("\'{0}\'", fv.ToString());
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(System.Guid))
                {
                    //valueSql = System.String.Format("convert(uniqueidentifier,\'{0}\')", fv.ToString());
                    valueSql = System.String.Format("\'{0}\'", ((System.Guid)fv).ToString("N"));
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(byte[]))
                {
                    valueSql = System.String.Format("@{0}", dbBind.Field);
                    needStringFlag = false;

                    //string sql = "update T_Employee set ImageLogo=@ImageLogo where EmpId=@EmpId"; 
                    //byte[] imgSourse = new byte[100];
                    var param = new MySql.Data.MySqlClient.MySqlParameter(valueSql, fv);
                    dbOp.SqlParameters.Add(param);
                }
                else
                {
                    valueSql = fv.ToString();
                    //防止SQL注入处理
                    valueSql = SqlSafeString(valueSql);
                }
                if (first)
                {
                    if (needStringFlag)
                        result += "    set " + dbBind.Field + " = \'" + valueSql + "\'";
                    else
                        result += "    set " + dbBind.Field + " = " + valueSql;
                    first = false;
                }
                else
                {
                    if (needStringFlag)
                        result += "," + dbBind.Field + " = \'" + valueSql + "\'";
                    else
                        result += "," + dbBind.Field + " = " + valueSql;
                }
            }
            if (result == "")
                return null;

            dbOp.SqlCode = "update " + bindTab.Table + "\r\n" + result + "\r\n where " + condition;
            return dbOp;
        }

        #endregion
        
        #region Sql Insert
        public bool _ExecuteInsert(DBOperator dbOp)
        {
            try
            {
                mDataAdapter.InsertCommand = dbOp.ToSqlCommand(mConnection);
                mDataAdapter.InsertCommand.ExecuteNonQuery();
                mDataAdapter.InsertCommand = null;
            }
            catch (System.Exception e)
            {
                if (e.ToString().Contains("for key")) //包含主键或唯一键值
                    return false;

                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(dbOp.SqlCode);
                return false;
            }
            return true;
        }
        public static DBOperator ReplaceData(string keyCondition, object obj, bool existUpdate)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Insert;

            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            string fieldStr = "";
            string valueStr = "";
            string setStr = "";
            bool first = true;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            foreach (var dbBind in clsDesc.Fields)
            {
                System.Reflection.PropertyInfo p = dbBind.Property;               
                object v = p.GetValue(obj, null);
                string valueSql;
                bool needStringFlag = true;
                if (p.PropertyType.IsEnum)
                {
                    valueSql = System.Convert.ToInt32(v).ToString();
                }
                else if (p.PropertyType == typeof(System.DateTime))
                {
                    valueSql = System.String.Format("\'{0}\'", v.ToString());
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(System.Guid))
                {
                    //valueSql = System.String.Format("convert(uniqueidentifier,\'{0}\')", v.ToString());

                    valueSql = string.Format("\'{0}\'", ((System.Guid)v).ToString("N"));
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(byte[]))
                {
                    valueSql = System.String.Format("@{0}", dbBind.Field);
                    needStringFlag = false;

                    var param = new MySql.Data.MySqlClient.MySqlParameter(valueSql,v);
                    dbOp.SqlParameters.Add(param);
                }
                else
                {
                    if (v != null)
                        valueSql = v.ToString();
                    else
                        valueSql = "";
                    //valueSql这个地方要处理数据库攻击，SQL注入
                    valueSql = SqlSafeString(valueSql);
                }
                if (first)
                {
                    fieldStr += dbBind.Field;
                    if (needStringFlag)
                        valueStr += "\'" + valueSql + "\'";
                    else
                        valueStr += valueSql;
                    if (needStringFlag)
                        setStr += " set " + dbBind.Field + "= \'" + valueSql + "\'";
                    else
                        setStr += " set " + dbBind.Field + "= " + valueSql;
                    first = false;
                }
                else
                {
                    fieldStr += "," + dbBind.Field;
                    if (needStringFlag)
                        valueStr += ",\'" + valueSql + "\'";
                    else
                        valueStr += "," + valueSql;
                    if (needStringFlag)
                        setStr += "," + dbBind.Field + "=\'" + valueSql + "\'";
                    else
                        setStr += "," + dbBind.Field + "=" + valueSql;
                }
            }
            string finalStr = "REPLACE into " + bindTab.Table + " (" + fieldStr + ") values (" + valueStr + ")\r\n";
            dbOp.SqlCode = finalStr;
            return dbOp;
        }

        public static DBOperator InsertData(string keyCondition, object obj, bool existUpdate)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Insert;

            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            string fieldStr = "";
            string valueStr = "";
            string setStr = "";
            bool first = true;
            var clsDesc = RPC.IAutoSLClassDescManager.Instance.GetDBClassDesc(objType);
            foreach (var dbBind in clsDesc.Fields)
            {
                System.Reflection.PropertyInfo p = dbBind.Property;               

                object v = p.GetValue(obj, null);
                string valueSql;
                bool needStringFlag = true;
                if (p.PropertyType.IsEnum)
                {
                    valueSql = System.Convert.ToInt32(v).ToString();
                }
                else if (p.PropertyType == typeof(System.DateTime))
                {
                    valueSql = System.String.Format("\'{0}\'", v.ToString());
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(System.Guid))
                {
                    //valueSql = System.String.Format("convert(uniqueidentifier,\'{0}\')", v.ToString());

                    valueSql = string.Format("\'{0}\'", ((System.Guid)v).ToString("N"));
                    needStringFlag = false;
                }
                else if (p.PropertyType == typeof(byte[]))
                {
                    valueSql = System.String.Format("@{0}", dbBind.Field);
                    needStringFlag = false;

                    var param = new MySql.Data.MySqlClient.MySqlParameter(valueSql, v);
                    dbOp.SqlParameters.Add(param);
                }
                else
                {
                    if (v != null)
                        valueSql = v.ToString();
                    else
                        valueSql = "";
                    //valueSql这个地方要处理数据库攻击，SQL注入
                    valueSql = SqlSafeString(valueSql);
                }
                if (first)
                {
                    fieldStr += dbBind.Field;
                    if (needStringFlag)
                        valueStr += "\'" + valueSql + "\'";
                    else
                        valueStr += valueSql;
                    if (needStringFlag)
                        setStr += " set " + dbBind.Field + "= \'" + valueSql + "\'";
                    else
                        setStr += " set " + dbBind.Field + "= " + valueSql;
                    first = false;
                }
                else
                {
                    fieldStr += "," + dbBind.Field;
                    if (needStringFlag)
                        valueStr += ",\'" + valueSql + "\'";
                    else
                        valueStr += "," + valueSql;
                    if (needStringFlag)
                        setStr += "," + dbBind.Field + "=\'" + valueSql + "\'";
                    else
                        setStr += "," + dbBind.Field + "=" + valueSql;
                }
            }
            string finalStr = "insert into " + bindTab.Table + " (" + fieldStr + ") values (" + valueStr + ")\r\n";
            dbOp.SqlCode = finalStr;
            return dbOp;
        }
        #endregion
        
        #region Sql Del
        public static DBOperator DelData(string condition, object obj)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Delete;
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            dbOp.SqlCode = "update " + bindTab.Table + " set Deleted=1 where " + condition;
            //return "update " + bindTab.Table + " set Deleted=1 where " + condition;
            return dbOp;
        }
        public bool _ExecuteDelete(DBOperator dbOp)
        {
            try
            {
                mDataAdapter.UpdateCommand = dbOp.ToSqlCommand(mConnection);
                mDataAdapter.UpdateCommand.ExecuteNonQuery();
                mDataAdapter.UpdateCommand = null;
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(dbOp.SqlCode);
                return false;
            }
            return true;
        }

        public static DBOperator DestroyData(string condition, object obj)
        {
            DBOperator dbOp = new DBOperator();
            dbOp.ExeType = SqlExeType.Destroy;
            Type objType = obj.GetType();
            object[] propsTab = objType.GetCustomAttributes(typeof(DBBindTable), false);
            if (propsTab == null)
                return null;
            DBBindTable bindTab = propsTab[0] as DBBindTable;
            dbOp.SqlCode = "delete from " + bindTab.Table + " where " + condition;
            return dbOp;
            //return "delete from " + bindTab.Table + " where " + condition;
        }
        public bool _ExecuteDestroy(DBOperator dbOp)
        {
            try
            {
                mDataAdapter.DeleteCommand = dbOp.ToSqlCommand(mConnection);
                mDataAdapter.DeleteCommand.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                Log.Log.Common.Print(e.ToString());
                Log.Log.Common.Print(dbOp.SqlCode);
                return false;
            }
            return true;
        }
        #endregion
        
        
    }
}
