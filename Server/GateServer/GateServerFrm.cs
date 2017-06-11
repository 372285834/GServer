using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GateServer
{
    public partial class GateServerFrm : Form
    {
        const int cMaxShowLine = 3000;
        bool mAutoUpdate = false;

        static GateServerFrm mInst;
        static public GateServerFrm Inst
        {
            get { return mInst; }
        }

        public GateServerFrm()
        {
            mInst = this;
            InitializeComponent();
            ServerModule.Instance.Start();
            this.propertyGrid_ServerState.SelectedObject = ServerModule.Instance.Server;
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

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        extern static UInt32 timeGetTime();

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "Gate:" + ServerModule.Instance.Server.LinkState.ToString();
            ServerModule.Instance.Tick();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                this.treeView1.Nodes.Clear();
                ServerCommon.IGateServer gateServer = ServerModule.Instance.Server;

                TreeNode node = this.treeView1.Nodes.Add("Client");
                //foreach (ServerCommon.Gate.ClientLinker elem in gateServer.ClientLinkers)
                for (int i = 0; i < gateServer.ClientLinkers.Length; i++)
                {
                    var elem = gateServer.ClientLinkers[i];
                    if (elem != null)
                    {
                        var curClientLinker = elem.mForwardInfo.Gate2ClientConnect.m_BindData as ServerCommon.Gate.ClientLinker;
                        if (elem != curClientLinker)
                        {
                            Log.Log.Common.Print("Error!if (gateServer.ClientLinkers[elem.mForwardInfo.Handle] != curClientLinker)");
                            //gateServer.FreeLinker(elem);
                            continue;
                        }

                        Iocp.TcpConnect connect = elem.mForwardInfo.Gate2ClientConnect as Iocp.TcpConnect;
                        node.Nodes.Add(connect.IpAddress + ":" + connect.Port).Tag = elem;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.Common.Print(ex.ToString());

            }
            

        }

        private void GateServerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerModule.Instance.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.LogControl.Lines = mLogs.ToArray();
            Log.FileLog.Instance.Flush();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            mLogs.Clear();
            this.LogControl.Lines = mLogs.ToArray();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //RPC.RPCNetworkMgr.Instance.PrintAllCallCounter();
        }
    }
}
