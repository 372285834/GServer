using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterServer
{
    public partial class RegisterServerFrm : Form
    {
        const int cMaxShowLine = 3000;
        bool mAutoUpdate = false;

        static RegisterServerFrm mInst;
        static public RegisterServerFrm Inst
        {
            get { return mInst; }
        }

        public RegisterServerFrm()
        {
            mInst = this;
            InitializeComponent();
            ServerModule.Instance.Start();
            propertyGrid1.SelectedObject = ServerModule.Instance.Server;
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

        private void button1_Click(object sender, EventArgs e)
        {
            ServerModule.Instance.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServerModule.Instance.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "Reg:" + ServerModule.Instance.Server.LinkState.ToString();
            ServerModule.Instance.Tick();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();

            ServerCommon.IRegisterServer regServer = ServerModule.Instance.Server;//.DataServer

            TreeNode node = this.treeView1.Nodes.Add("DataServer");
            if (null!=regServer.DataServer)
                node.Nodes.Add(regServer.DataServer.IpAddress+":"+regServer.DataServer.Port).Tag = regServer.DataServer;
            node = this.treeView1.Nodes.Add("GateServer");
            foreach (var ep in regServer.GateServers)
            {
                node.Nodes.Add(ep.Value.Ip + ":" + ep.Value.Port).Tag = ep.Value;
            }
            node = this.treeView1.Nodes.Add("PlanesServer");
            foreach (var ep in regServer.PlanesServers)
            {
                node.Nodes.Add(ep.Value.Ip + ":" + ep.Value.Port).Tag = ep.Value;
            }
        }

        private void RegisterServerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerModule.Instance.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.label1.Text = this.folderBrowserDialog1.SelectedPath+"\\";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.label2.Text = this.folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.LogControl.Lines = mLogs.ToArray();
            Log.FileLog.Instance.Flush();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            mLogs.Clear();
            this.LogControl.Lines = mLogs.ToArray();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            //if (checkbox1.Checked)
            //{
            //    if (System.IO.Directory.Exists(label3.Text))
            //        System.IO.Directory.Delete(label3.Text, true);
            //    System.IO.Directory.CreateDirectory(label3.Text);
            //}

            //ServerCommon.Data.PatchFileListBuilder pfBuilder = new ServerCommon.Data.PatchFileListBuilder();
            //pfBuilder.BuildPatch(label1.Text, label2.Text, label3.Text);
            //System.Windows.Forms.MessageBox.Show("版本比对完成！");
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.label1.Text = this.folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.label2.Text = this.folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.label3.Text = this.folderBrowserDialog1.SelectedPath + "\\";
            }
        }

        private void RegisterServerFrm_Load(object sender, EventArgs e)
        {
            label1.Text = ServerCommon.IServer.Instance.ExePath + "..\\DemaciaGame\\";
            label2.Text = "D:\\wwwroot\\Demacia_Update\\";
            label3.Text = "D:\\wwwroot\\Demacia_Backup\\";
        }
    }
}
