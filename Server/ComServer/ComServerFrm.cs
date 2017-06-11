using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComServer
{
    public partial class ComServerFrm : Form
    {
        const int cMaxShowLine = 3000;
        bool mAutoUpdate = false;

        static ComServerFrm mInst;
        static public ComServerFrm Inst
        {
            get { return mInst; }
        }

        public ComServerFrm()
        {
            mInst = this;
            InitializeComponent();
            ServerModule.Instance.Start();
            this.propertyGrid1.SelectedObject = ServerModule.Instance.Server;
        }

        Queue<string> mLogs = new Queue<string>();
        public void PushLog(string s)
        {
            mLogs.Enqueue(s);
            if (mLogs.Count > cMaxShowLine)
                mLogs.Dequeue();
            if (mAutoUpdate)
            {
                this.LogControl.Lines = mLogs.ToArray();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "ComServer:" + ServerModule.Instance.Server.LinkState.ToString();
            ServerModule.Instance.Tick();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerModule.Instance.Server.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.LogControl.Lines = mLogs.ToArray();
            Log.FileLog.Instance.Flush();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mLogs.Clear();
            this.LogControl.Lines = mLogs.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerModule.Instance.Server.UserRoleManager.UpdateToSql(null);
        }
    }
}
