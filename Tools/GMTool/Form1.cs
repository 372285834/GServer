using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using MySql.Data.MySqlClient;

namespace Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ToolThread.Instance.Start();

            textBox1.Text = @"SELECT RoleId FROM RoleInfo  where Deleted = 0 and RoleName =";
            //textBox1.Text = @"SELECT RoleId FROM Account INNER JOIN RoleInfo ON Account.Id = RoleInfo.AccountId where UserName not like '%etang%' and username not like 'robot%' and UserName not like 'test5___' order by UserName";
            if (tabControl1.SelectedIndex == 0)
                propertyGrid1.SelectedObject = mSendInfo;
        }
        MailDetail mSendInfo = new MailDetail();
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                propertyGrid1.SelectedObject = mSendInfo;
            }

        }

        public string ToString(int a, int b)
        {
            return a.ToString() + "," + b.ToString() + "|";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string items = "";
            if (mSendInfo.ItemANum > 0 && mSendInfo.ItemAName != "")
            {
                var temp = ToolThread.Instance.Dict[mSendInfo.ItemAName];
                if (temp != null)
                {
                    int tid = temp.id;
                    items += ToString(tid, mSendInfo.ItemANum);
                }
            }

            if (mSendInfo.ItemBNum > 0 && mSendInfo.ItemBName != "")
            {
                var temp = ToolThread.Instance.Dict[mSendInfo.ItemBName];
                if (temp != null)
                {
                    int tid = temp.id;
                    items += ToString(tid, mSendInfo.ItemBNum);
                }
            }

            if (mSendInfo.ItemCNum > 0 && mSendInfo.ItemCName != "")
            {
                var temp = ToolThread.Instance.Dict[mSendInfo.ItemCName];
                if (temp != null)
                {
                    int tid = temp.id;
                    items += ToString(tid, mSendInfo.ItemCNum);
                }
            }

            if (mSendInfo.ItemDNum > 0 && mSendInfo.ItemDName != "")
            {
                var temp = ToolThread.Instance.Dict[mSendInfo.ItemDName];
                if (temp != null)
                {
                    int tid = temp.id;
                    items += ToString(tid, mSendInfo.ItemDNum);
                }
            }

            string currencies = "";
            if (mSendInfo.Gold > 0)
            {
                string id = (int)CSCommon.eCurrenceType.Gold + ",";
                currencies += id;
                string inum = mSendInfo.Gold + "|";
                currencies += inum;
            }

            if (mSendInfo.Rmb > 0)
            {
                string id = (int)CSCommon.eCurrenceType.Rmb + ",";
                currencies += id;
                string inum = mSendInfo.Rmb + "|";
                currencies += inum;
            }

            if (mSendInfo.Reputation > 0)
            {
                string id = (int)CSCommon.eCurrenceType.Reputation + ",";
                currencies += id;
                string inum = mSendInfo.Reputation + "|";
                currencies += inum;
            }

            if (mSendInfo.Activeness > 0)
            {
                string id = (int)CSCommon.eCurrenceType.Activeness + ",";
                currencies += id;
                string inum = mSendInfo.Activeness + "|";
                currencies += inum;
            }

            if (mSendInfo.Exploit > 0)
            {
                string id = (int)CSCommon.eCurrenceType.Exploit + ",";
                currencies += id;
                string inum = mSendInfo.Exploit + "|";
                currencies += inum;
            }

            string strSQL = "";

            if (checkBox1.Checked)
            {
                strSQL = textBox1.Text;
            }
            else
            {
                strSQL = "SELECT RoleId FROM RoleInfo WHERE Deleted = 0";
                do
                {
                    if (mSendInfo.RoleIds.Count > 0)
                    {
                        strSQL = strSQL + " and " + "RoleId = "; 
                        for (int i = 0; i < mSendInfo.RoleIds.Count; i++)
                        {
                            if (i == mSendInfo.RoleIds.Count - 1)
                                strSQL += string.Format("'{0}'", mSendInfo.RoleIds[i].ToString());
                            else
                                strSQL += string.Format("'{0}' OR RoleId = ", mSendInfo.RoleIds[i].ToString());
                        }
                        break;
                    }

                    if (mSendInfo.PlanesIds.Count > 0)
                    {
                        strSQL = strSQL + " and " + "PlanesId = "; 
                        for (int i = 0; i < mSendInfo.PlanesIds.Count; i++)
                        {
                            if (i == mSendInfo.PlanesIds.Count - 1)
                                strSQL += string.Format("'{0}'", mSendInfo.PlanesIds[i].ToString());
                            else
                                strSQL += string.Format("'{0}' OR PlanesId = ", mSendInfo.PlanesIds[i].ToString());
                        }
                    }

                } while (false);
            }

            CSCommon.Data.SystemMailData mail = new CSCommon.Data.SystemMailData();
            mail.MailId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Email);
            mail.CreateTime = System.DateTime.Now;
            mail.EndTime = mail.CreateTime.AddDays(mSendInfo.ShelfLife);
            mail.Type = mSendInfo.Type;
            mail.Title = mSendInfo.Title;
            mail.ContentStr = mSendInfo.Content;
            mail.StrItems = items;
            mail.StrCurrencies = currencies;
            mail.SelectSql = strSQL;
            CeateSystemMail(mail);
        }
        void CeateSystemMail(CSCommon.Data.SystemMailData mail)
        {
            var condition = "MailId=" + mail.MailId;
            var dbOp = ServerFrame.DB.DBConnect.InsertData(condition, mail, true);
            ToolThread.Instance.DBConnect._ExecuteUpdate(dbOp);
            ServerCommon.IComServer.Instance.UserRoleManager.AddSystemMailAndSend(mail, ToolThread.Instance.DBConnect);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ServerCommon.IDataServer.Instance.ClearMail(ToolThread.Instance.DBConnect);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ToolThread.Instance.DBConnect.CloseConnect();
            Application.Exit();
        }
        MySqlDataAdapter mAdap = null;
        System.Data.DataSet mDataSet = null;


        private void button3_Click(object sender, EventArgs e)
        {
            CSCommon.Data.MailData mail = new CSCommon.Data.MailData();
            mail.CreateTime = System.DateTime.Now;
            mail.EndTime = mail.CreateTime.AddDays(mSendInfo.ShelfLife);
            mail.Type = "1";
            mail.Title = "1";
            mail.ContentStr = "1";
            mail.StrItems = "111";
            mail.StrCurrencies = "111";
            mail.Deleted = 0;
            int count = Convert.ToInt32(this.textBox5.Text);
            var start = System.DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                mail.MailId = ServerFrame.Util.GenerateObjID(ServerFrame.GameObjectType.Email);
                mail.OwnerId = (ulong)i;
                DataRow row = mDataSet.Tables["MailData"].NewRow();
                ServerFrame.DB.DBConnect.FillDataRow(row, mail);
                //mDataSet.Tables["MailData"].ImportRow(row);
                mDataSet.Tables["MailData"].Rows.Add(row);
//                 var dbOp = ServerFrame.DB.DBConnect.InsertData("", mail, true);
//                 ToolThread.Instance.DBConnect._ExecuteInsert(dbOp);
            }
            var end = System.DateTime.Now;
            var time = (end - start).TotalSeconds;
            this.textBox2.Text = Convert.ToString(time) + "秒" + mDataSet.Tables["MailData"].Rows.Count.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var start = System.DateTime.Now;
            string sql = "OwnerId = " + this.textBox3.Text;
            var dbOp = ServerFrame.DB.DBConnect.SelectData(sql, new CSCommon.Data.MailData(), "");
            var tab = ToolThread.Instance.DBConnect._ExecuteSelect(dbOp, "MailData");
            var end = System.DateTime.Now;
            var time = (end - start).TotalSeconds;
            this.textBox2.Text = Convert.ToString(time) + "秒";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //mDataSet.Reset();
            mDataSet = new System.Data.DataSet();
            string strSQL = "SELECT * FROM MailData";
            MySqlCommand command = new MySqlCommand(strSQL, ToolThread.Instance.DBConnect.mConnection);
            mAdap = new MySqlDataAdapter(command);
            MySqlCommandBuilder msb = new MySqlCommandBuilder(mAdap);
            mDataSet = new DataSet();
            mAdap.Fill(mDataSet, "MailData");
            this.textBox4.Text = mDataSet.Tables["MailData"].Rows.Count.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
                    //更新到数据库
            if (mAdap == null || mDataSet == null)
            {
                return;
            }

            var start = System.DateTime.Now;
            mAdap.Update(mDataSet, "MailData");
            
            var end = System.DateTime.Now;
            var time = (end - start).TotalSeconds;
            this.textBox4.Text = Convert.ToString(time) + "秒";
        }



    }
}
