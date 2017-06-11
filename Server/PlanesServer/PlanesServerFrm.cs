using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ServerCommon.Planes;
using ServerFrame;
namespace PlanesServer
{
    public partial class PlanesServerFrm : Form
    {
        const int cMaxShowLine = 3000;
        bool mAutoUpdate = false;

        static PlanesServerFrm mInst;
        static public PlanesServerFrm Inst
        {
            get { return mInst; }
        }

        public PlanesServerFrm()
        {
            mInst = this;
            InitializeComponent();
            ServerModule.Instance.Start();
            this.propertyGrid_ServerState.SelectedObject = ServerModule.Instance;

            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            //RemoteServer.Init();
        }


        Queue<string> mLogs = new Queue<string>();
        public void PushLog(string s)
        {
            mLogs.Enqueue(s);
            if (mLogs.Count > cMaxShowLine)
                mLogs.Dequeue();
            if (mAutoUpdate)
            {
                //this.LogControl.Lines = mLogs.ToArray();
                //Log.FileLog.Instance.Flush();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerModule.Instance.Start();
        }

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        extern static UInt32 timeGetTime();
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "Planes:" + ServerModule.Instance.Server.LinkState.ToString();
            ServerModule.Instance.Tick();
            if (mAutoRefresh)
                pictureBox1.Invalidate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();

            ServerCommon.IPlanesServer server = ServerModule.Instance.Server;

            TreeNode node = this.treeView1.Nodes.Add("GateServer");
            foreach (var elem in server.GateServers)
            {
                node.Nodes.Add(elem.Value.IpAddress + ":" + elem.Value.Port).Tag = elem;
            }
        }

        private void PlanesServerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerModule.Instance.Server.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //ServerCommon.IPlanesServer.Instance.RPC_PublicNotice(n);
            //n++;
            //if (n > 1)
            //{
            //    n = 0;
            //}
        }

        private void SendTempNotice(object sender, EventArgs e)
        {

            //List<Guid> planesIds = ServerCommon.Data.Activity.NoticeTemplateManager.Instance.CommonTemplate.PlanesIds;
            //string msg = mInputNotice.Text;
            //ServerCommon.IPlanesServer.Instance.SendNotice(planesIds, msg); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.Activity.NoticeTemplateManager.Instance.LoadCommonItem();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.LogControl.Lines = mLogs.ToArray();
            Log.FileLog.Instance.Flush();
            mAutoUpdate = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mLogs.Clear();
            this.LogControl.Lines = mLogs.ToArray();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //string cmd = mCodeInput.Text;
            //var cxt = new ServerCommon.Com.Context();
            //Guid roleid = Guid.NewGuid();
            ////if (!String.IsNullOrEmpty(cmd))
            //{
            //    cxt.Invoke(cmd, roleid);
            //}
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //var maps = ServerCommon.Planes.Map.MapManager.Instance.MapInstance;
            //foreach (var i in maps)
            //{
            //    if (i == null)
            //        continue;
            //    foreach (var player in i.PlayerPool)
            //    {
            //        if (player == null)
            //            continue;
            //        //player.SaveAll(player.Planes2GateConnect);
            //        player.DisconnectPlane(); 

            //    }
            //}
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //ushort dropid = ushort.Parse(mInputDrop.Text);
            //ServerCommon.Planes.Creative.DropManager.Instance.ForceFreshItem(dropid, 0); 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ushort npcForceid = ushort.Parse(mInputNpcForce.Text);
            //ServerCommon.Data.Fight.SceneNPCForceManager.Instance.ReloadNPCForceTemplate(npcForceid);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            CSCommon.ShopCommon.Instance.LoadCommonTemplate(ServerCommon.ServerConfig.Instance.TemplatePath);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.Item.GiftBagTemplateManager.Instance.LoadAllTemplate();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.ItemTemplateManager.Instance.LoadAllTemplate();
        }

        float mScale = 5;   //1米mScale个像素，
        float mStartX = 0;      //画面左下角起始地图坐标对应屏幕坐标
        float mStartY = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            var gdi = e.Graphics;
            gdi.Clear(Color.Black);
            if (mSelectMap == null)
                return;

            var size = new Size((int)mSelectMap.MapInfo.MapData.sizeX, (int)mSelectMap.MapInfo.MapData.sizeZ);
            var h = pictureBox1.Size.Height;
            var w = pictureBox1.Size.Width;

            int mapOffsetX = 0;//size.Width;
            int mapOffsetZ = 0;// size.Height;

            if (mSelectMap == null)
                return;

            using (var pen = new Pen(Color.Yellow))
            {
                if (mCollision != null)
                {
                    //                     var beginXIndex = (int)((mStartX + 128)/ mCollisionInv);
                    //                     var endXIndex = (int)((mStartX + 128) + w / mScale / mCollisionInv);
                    //                     var beginYIndex = (int)((mStartY + 128) / mCollisionInv);
                    //                     var endYIndex = (int)((mStartY + 128) + w / mScale / mCollisionInv);
                    for (int j = 0; j < mCollisionH; j++)
                    {
                        for (int i = 0; i < mCollisionW; i++)
                        {
                            if (i - 1 < 0 || j - 1 < 0 || i + 1 >= mCollisionW || j + 1 >= mCollisionH)
                                continue;
                            if (mCollision[j * mCollisionW + i] == '1'
                                 && (mCollision[(j + 1) * mCollisionW + (i + 1)] == '0'
                                 || mCollision[(j - 1) * mCollisionW + (i - 1)] == '0'
                                 || mCollision[(j + 1) * mCollisionW + (i - 1)] == '0'
                                 || mCollision[(j - 1) * mCollisionW + (i + 1)] == '0')
                                )
                            {
                                gdi.DrawRectangle(pen, ((i + mOffsetx) * mCollisionInv - mapOffsetX) * mScale - mStartX, h - ((j + mOffsetz) * mCollisionInv - mapOffsetZ) * mScale - mStartY, mCollisionInv * mScale, mCollisionInv * mScale);
                            }
                        }

                    }
                }

            }


            for (int i = 0; i < mSelectMap.CellZCount; i++)
            {
                gdi.DrawLine(Pens.White,
                    0 - mStartX, h - i * MapInstance.mServerMapCellWidth * mScale - mStartY,
                    size.Height * mScale - mStartX, h - i * MapInstance.mServerMapCellWidth * mScale - mStartY);
            }

            for (int i = 0; i < mSelectMap.CellXCount; i++)
            {
                gdi.DrawLine(Pens.White,
                    i * MapInstance.mServerMapCellHeight * mScale - mStartX, h - 0 - mStartY,
                    i * MapInstance.mServerMapCellHeight * mScale - mStartX, h - size.Width * mScale - mStartY);
            }


            using (Pen p = new Pen(Color.Red, 3))
            {
                SolidBrush drawBrush = new SolidBrush(Color.Red);

                var ls = mSelectMap.NpcDictionary.Values.ToArray<NPCInstance>();

                foreach (var npc in ls)
                {
                    var loc = npc.GetPosition();
                    var x = loc.X * mScale - mStartX - 1;
                    var z = h - loc.Z * mScale - mStartY - 1;
                    gdi.DrawString(npc.RoleName, this.Font, drawBrush, x - 5, z - 18);
                    gdi.DrawEllipse(p, x, z, 3, 3);
                }


                using (Pen p2 = new Pen(Color.Blue, 3))
                {
                    SolidBrush drawBrush2 = new SolidBrush(Color.Blue);
                    var lstr = mSelectMap.TriggerDictionary.Values.ToArray<TriggerInstance>();
                    foreach (var npc in lstr)
                    {
                        var loc = npc.Placement.GetLocation();
                        var x = loc.X * mScale - mStartX - 1;
                        var z = h - loc.Z * mScale - mStartY - 1;
                        gdi.DrawString("传送到:" + npc.TriggerData.mapId, this.Font, drawBrush2, x - 5, z - 18);
                        gdi.DrawEllipse(p2, x, z, 5, 5);
                    }
                }

            }


            gdi.DrawLine(Pens.Gray, 0, h - mStartY, w, h - mStartY);
            gdi.DrawLine(Pens.Gray, -mStartX, 0, -mStartX, h);


            using (Pen p = new Pen(Color.Green, 2))
            {
                SolidBrush drawBrush = new SolidBrush(Color.Green);
                
                foreach (var player in mSelectMap.PlayerPool)
                {
                    if (player == null)
                        continue;

                    var loc = player.GetPosition();
                    var x = loc.X * mScale - mStartX - 4;
                    var z = h - loc.Z * mScale - mStartY - 4;
                    gdi.DrawString(player.RoleName, this.Font, drawBrush, x - 5, z - 18);
                    gdi.DrawEllipse(p, x, z, 8, 8);
                }

            }
            
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            mSelectMap = e.Node.Tag as ServerCommon.Planes.MapInstance;
            //mCollision = Util.GetCollision(mSelectMap.MapInfo.MapData.id, out mCollisionW, out mCollisionH, out mCollisionInv, out mOffsetx, out mOffsetz);
        }

        ServerCommon.Planes.MapInstance mSelectMap;
        byte[] mCollision;
        float mCollisionInv;    //每个点间隔米数;
        int mCollisionW, mCollisionH, mOffsetx, mOffsetz;
        private void treeView2_Enter(object sender, EventArgs e)
        {
            treeView2.Nodes.Clear();
            foreach (var map in ServerModule.Instance.Server.MapManager.AllMaps)
            {
                if (map == null)
                    continue;

                var node = treeView2.Nodes.Add(map.Planes.PlanesId+"-"+map.MapSourceId + ":" + map.MapInfo.MapData.nickName);
                node.Tag = map;
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            float.TryParse((sender as TextBox).Text, out mScale);
            pictureBox1.Invalidate();
        }


        Point mDownPt;
        Point mSavePt;
        bool mMouseDown = false;
        bool mAutoRefresh = false;
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            var h = pictureBox1.Size.Height;

            mDownPt = new Point(e.X, h - e.Y);
            mSavePt = new Point((int)mStartX, (int)mStartY);
            mMouseDown = true;
            pictureBox1.Focus();
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            var h = pictureBox1.Size.Height;
            var x = (int)(e.X + mStartX) / mScale;
            var y = (int)((h - e.Y) - mStartY) / mScale;
            mLocText.Text = string.Format("鼠标当前坐标:x={0},z={1}", x, y);

            pictureBox1.Invalidate();

            if (!mMouseDown)
                return;

            var deltaX = e.X - mDownPt.X;
            var deltaY = h - e.Y - mDownPt.Y;
            mStartX = mSavePt.X - deltaX;
            mStartY = mSavePt.Y + deltaY;

        }

        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            var t = e.Delta;
            if (e.Delta > 0)
                mScale *= 1.1f;
            else if (e.Delta < 0)
                mScale *= 0.9f;

            if (mScale < 0.1)
            {
                mScale = 0.1f;
            }
            pictureBox1.Invalidate();
            textBox1.Text = mScale.ToString();
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mMouseDown = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            mAutoRefresh = !mAutoRefresh;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            mAutoUpdate = true;
        }

    }
}
