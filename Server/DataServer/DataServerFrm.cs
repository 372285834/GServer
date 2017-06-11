using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataServer
{
    public partial class DataServerFrm : Form
    {
		const int cMaxShowLine = 3000;
        bool mAutoUpdate = false;


        static DataServerFrm mInst;
        static public DataServerFrm Inst
        {
            get { return mInst; }
        }
        public DataServerFrm()
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

        //[System.Runtime.InteropServices.DllImport("winmm.dll")]
        //extern static UInt32 timeGetTime();

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Text = "Data:" + ServerModule.Instance.Server.LinkState.ToString();
            ServerModule.Instance.Tick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();

            ServerCommon.IDataServer dataServer = ServerModule.Instance.Server;

            TreeNode node = this.treeView1.Nodes.Add("GateServer");
            foreach (var elem in dataServer.GateServers)
            {
                node.Nodes.Add(elem.Value.IpAddress + ":" + elem.Value.Port).Tag = elem;
            }

            node = this.treeView1.Nodes.Add("PlanesServer");
            foreach (var elem in dataServer.PlanesServers)
            {
                node.Nodes.Add(elem.Value.EndPoint.IpAddress + ":" + elem.Value.EndPoint.Port).Tag = elem;
            }
        }

        private void DataServerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ServerModule.Instance.Stop();
        }

        //#region 物品编辑
        //void AddItemToTree(TreeNode node, ServerCommon.Data.ItemTemplate item)
        //{
        //    string[] segs = item.ItemName.Split('.');
        //    for (int i = 0; i < segs.Length; i++)
        //    {
        //        if (i == segs.Length - 1)
        //        {
        //            node = node.Nodes.Add(segs[i]);
        //            break;
        //        }
        //        bool bFind = false;
        //        foreach (TreeNode e in node.Nodes)
        //        {
        //            if (e.Text == segs[i])
        //            {
        //                node = e;
        //                bFind = true;
        //                break;
        //            }
        //        }
        //        if (bFind == false)
        //            node = node.Nodes.Add(segs[i]);
        //    }
        //    node.Tag = item;
        //    if (item.IsDirty)
        //        node.ImageIndex = 1;
        //    else
        //        node.ImageIndex = 0;
        //}

        //void FreshItemTree()
        //{
        //    this.treeView_Items.Nodes.Clear();
        //    TreeNode node = this.treeView_Items.Nodes.Add("Items");
        //    Dictionary<UInt16, ServerCommon.Data.ItemTemplate> items = ServerCommon.Data.ItemTemplateManager.Instance.AllItems;
        //    foreach (KeyValuePair<UInt16, ServerCommon.Data.ItemTemplate> kv in items)
        //    {
        //        AddItemToTree(node, kv.Value);
        //    }
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    FreshItemTree();
        //}

        //UInt16 NewItemId()
        //{
        //    var files = System.IO.Directory.EnumerateFiles(ServerCommon.IServer.smInstance.ExePath + "/../template/item");
        //    UInt16[] slots = new UInt16[65534];
        //    foreach (var i in files)
        //    {
        //        if (i.Substring(i.Length - 5, 5) == ".item")
        //        {
        //            int pos = i.LastIndexOf('\\') + 1;
        //            string idText = i.Substring(pos, i.Length - 5 - pos);
        //            int id = System.Convert.ToUInt16(idText);
        //            slots[id] = 1;
        //        }
        //    }
        //    for (UInt16 idx = 0; idx < 65534; idx++)
        //    {
        //        if (slots[idx] == 0)
        //        {
        //            if (ServerCommon.Data.ItemTemplateManager.Instance.GetTemplate(idx) == null)
        //                return idx;
        //        }
        //    }
        //    return UInt16.MaxValue;
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    ServerCommon.Data.ItemTemplate item = new ServerCommon.Data.ItemTemplate();
        //    item.ItemId = NewItemId();
        //    this.propertyGrid_ItemEditor.SelectedObject = item;
        //    ServerCommon.Data.ItemTemplateManager.Instance.AddItem(item);
        //    //FreshItemTree();
        //    if (this.treeView_Items.Nodes.Count == 1 && this.treeView_Items.Nodes[0].Text == "Items")
        //        AddItemToTree(this.treeView_Items.Nodes[0], item);
        //}

        //private void propertyGrid_ItemEditor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        //{
        //    ServerCommon.Data.ItemTemplate item = this.propertyGrid_ItemEditor.SelectedObject as ServerCommon.Data.ItemTemplate;
        //    if (item == null)
        //        return;
        //    item.IsDirty = true;
        //    if (this.treeView_Items.SelectedNode == null)
        //        return;
        //    ServerCommon.Data.ItemTemplate treeitem = this.treeView_Items.SelectedNode.Tag as ServerCommon.Data.ItemTemplate;
        //    if (treeitem == item)
        //        this.treeView_Items.SelectedNode.ImageIndex = 1;
        //}

        //private void treeView_Items_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    ServerCommon.Data.ItemTemplate item = e.Node.Tag as ServerCommon.Data.ItemTemplate;
        //    if (item == null)
        //        return;
        //    this.propertyGrid_ItemEditor.SelectedObject = item;
        //    this.propertyGrid_ItemTpl.SelectedObject = item.StandTemplateItem;
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    ServerCommon.Data.ItemTemplateManager.Instance.SaveDirtyItems(ServerModule.Instance.Server);
        //    FreshItemTree();
        //}

        //ServerCommon.Data.ItemTemplate mCopySrc;
        //private void toolStripMenuItem_copy_Click(object sender, EventArgs e)
        //{
        //    if (this.treeView_Items.SelectedNode == null)
        //        return;
        //    mCopySrc = this.treeView_Items.SelectedNode.Tag as ServerCommon.Data.ItemTemplate;
        //}

        //private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    ServerCommon.Data.ItemTemplate mCopyDst = this.treeView_Items.SelectedNode.Tag as ServerCommon.Data.ItemTemplate;
        //    if (mCopyDst != null && mCopyDst != mCopySrc)
        //    {
        //        if (mCopySrc!=null)
        //            mCopyDst.CopyFrom(mCopySrc);
        //    }
        //}
        
        //private void button6_Click(object sender, EventArgs e)
        //{
        //    if (this.treeView_Items.SelectedNode == null)
        //        return;
        //    ServerCommon.Data.ItemTemplate item = this.treeView_Items.SelectedNode.Tag as ServerCommon.Data.ItemTemplate;
        //    if (item == null)
        //        return;

        //    ServerCommon.Data.ItemTemplateManager.Instance.DeleteItem(ServerModule.Instance.Server, item.ItemId);
        //    FreshItemTree();
        //}

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    if (this.treeView_Items.SelectedNode == null)
        //        return;
        //    ServerCommon.Data.ItemTemplate item = this.treeView_Items.SelectedNode.Tag as ServerCommon.Data.ItemTemplate;
        //    if (item == null)
        //        return;
        //    item.IsDirty = true;
        //    item.StandTemplateItem.ItemTemlateId = item.ItemId;
        //    //item.StandTemplateItem.DangrousReInitData();
        //    this.propertyGrid_ItemTpl.SelectedObject = item.StandTemplateItem;
        //}
        //#endregion

        #region 武将编辑
        //void AddKnightToTree(TreeNode node, ServerCommon.Data.KnightTemplate item)
        //{
        //    string[] segs = item.KnightName.Split('.');
        //    for (int i = 0; i < segs.Length; i++)
        //    {
        //        if (i == segs.Length - 1)
        //        {
        //            node = node.Nodes.Add(segs[i]);
        //            break;
        //        }
        //        bool bFind = false;
        //        foreach (TreeNode e in node.Nodes)
        //        {
        //            if (e.Text == segs[i])
        //            {
        //                node = e;
        //                bFind = true;
        //                break;
        //            }
        //        }
        //        if (bFind == false)
        //            node = node.Nodes.Add(segs[i]);
        //    }
        //    node.Tag = item;
        //    if (item.IsDirty)
        //        node.ImageIndex = 1;
        //    else
        //        node.ImageIndex = 0;
        //}

        //void FreshKnightTree()
        //{
        //    this.treeView_Knights.Nodes.Clear();
        //    TreeNode node = this.treeView_Knights.Nodes.Add("Knights");
        //    Dictionary<UInt16, ServerCommon.Data.KnightTemplate> items = ServerCommon.Data.KnightTemplateManager.Instance.AllItems;
        //    foreach (KeyValuePair<UInt16, ServerCommon.Data.KnightTemplate> kv in items)
        //    {
        //        AddKnightToTree(node, kv.Value);
        //    }
        //}

        private void button11_Click(object sender, EventArgs e)
        {
            //FreshKnightTree();
        }

        //UInt16 NewKnightId()
        //{
        //    var files = System.IO.Directory.EnumerateFiles(ServerCommon.IServer.smInstance.ExePath + "/../template/knight");
        //    UInt16[] slots = new UInt16[65534];
        //    foreach (var i in files)
        //    {
        //        if (i.Substring(i.Length - 7, 7) == ".knight")
        //        {
        //            int pos = i.LastIndexOf('\\') + 1;
        //            string idText = i.Substring(pos, i.Length - 7 - pos);
        //            int id = System.Convert.ToUInt16(idText);
        //            slots[id] = 1;
        //        }
        //    }
        //    for (UInt16 idx = 0; idx < 65534; idx++)
        //    {
        //        if (slots[idx] == 0)
        //        {
        //            if(ServerCommon.Data.KnightTemplateManager.Instance.FindItem(idx)==null)
        //                return idx;
        //        }
        //    }
        //    return UInt16.MaxValue;
        //}

        private void button10_Click(object sender, EventArgs e)
        {

            //ServerCommon.Data.KnightTemplate item = new ServerCommon.Data.KnightTemplate();
            //item.KnightId = NewKnightId();
            //item.IsDirty = true;
            //this.propertyGrid_KnightEditor.SelectedObject = item;
            //ServerCommon.Data.KnightTemplateManager.Instance.AddItem(item);
            ////FreshItemTree();
            //if (this.treeView_Knights.Nodes.Count == 1 && this.treeView_Knights.Nodes[0].Text == "Knights")
            //    AddKnightToTree(this.treeView_Knights.Nodes[0], item);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.KnightTemplateManager.Instance.SaveDirtyItems(ServerModule.Instance.Server);
            //FreshKnightTree();
        }

        private void treeView_Knights_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //ServerCommon.Data.KnightTemplate item = e.Node.Tag as ServerCommon.Data.KnightTemplate;
            //if (item == null)
            //    return;
            //this.propertyGrid_KnightEditor.SelectedObject = item;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //if (this.treeView_Knights.SelectedNode == null)
            //    return;
            //ServerCommon.Data.KnightTemplate item = this.treeView_Knights.SelectedNode.Tag as ServerCommon.Data.KnightTemplate;
            //if (item == null)
            //    return;

            //ServerCommon.Data.KnightTemplateManager.Instance.DeleteItem(ServerModule.Instance.Server, item.KnightId);
            //FreshKnightTree();
        }

        private void propertyGrid_KnightEditor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //ServerCommon.Data.KnightTemplate item = this.propertyGrid_KnightEditor.SelectedObject as ServerCommon.Data.KnightTemplate;
            //if (item == null)
            //    return;
            //item.IsDirty = true;
            //if (this.treeView_Knights.SelectedNode == null)
            //    return;
            //ServerCommon.Data.KnightTemplate treeitem = this.treeView_Knights.SelectedNode.Tag as ServerCommon.Data.KnightTemplate;
            //if (treeitem == item)
            //    this.treeView_Knights.SelectedNode.ImageIndex = 1;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //if (this.treeView_Knights.SelectedNode == null)
            //    return;
            //ServerCommon.Data.KnightTemplate item = this.treeView_Knights.SelectedNode.Tag as ServerCommon.Data.KnightTemplate;
            //if (item == null)
            //    return;
            //ServerCommon.Data.KnightData kd = new ServerCommon.Data.KnightData();
            //kd.TemplateId = item.KnightId;
            //kd.DangrousReInitData();
            //this.propertyGrid_RandKnight.SelectedObject = kd;
        }
        #endregion

        #region 任务编辑
        //void AddTaskToTree(TreeNode node, ServerCommon.Data.TaskTemplate item)
        //{
        //    string[] segs = item.TaskName.Split('.');
        //    for (int i = 0; i < segs.Length; i++)
        //    {
        //        if (i == segs.Length - 1)
        //        {
        //            node = node.Nodes.Add(segs[i]);
        //            break;
        //        }
        //        bool bFind = false;
        //        foreach (TreeNode e in node.Nodes)
        //        {
        //            if (e.Text == segs[i])
        //            {
        //                node = e;
        //                bFind = true;
        //                break;
        //            }
        //        }
        //        if (bFind == false)
        //            node = node.Nodes.Add(segs[i]);
        //    }
        //    node.Tag = item;
        //    if (item.IsDirty)
        //        node.ImageIndex = 1;
        //    else
        //        node.ImageIndex = 0;
        //}

        //void FreshTaskTree()
        //{
        //    this.treeView_Tasks.Nodes.Clear();
        //    TreeNode node = this.treeView_Tasks.Nodes.Add("Tasks");
        //    ServerCommon.Data.TaskTemplate[] items = ServerCommon.Data.TaskTemplateManager.Instance.AllTasks;
        //    foreach (ServerCommon.Data.TaskTemplate kv in items)
        //    {
        //        if (kv!=null)
        //            AddTaskToTree(node, kv);
        //    }
        //}
        private void button16_Click(object sender, EventArgs e)
        {
            //FreshTaskTree();
        }
        //UInt16 NewTaskId()
        //{
        //    var files = System.IO.Directory.EnumerateFiles(ServerCommon.IServer.smInstance.ExePath + "/../template/task");
        //    UInt16[] slots = new UInt16[ServerCommon.Data.TaskTemplateManager.MaxTaskTemplateNumber];
        //    foreach (var i in files)
        //    {
        //        if (i.Substring(i.Length - 5, 5) == ".task")
        //        {
        //            int pos = i.LastIndexOf('\\') + 1;
        //            string idText = i.Substring(pos, i.Length - 5 - pos);
        //            int id = System.Convert.ToUInt16(idText);
        //            slots[id] = 1;
        //        }
        //    }
        //    for (UInt16 idx = 0; idx < ServerCommon.Data.TaskTemplateManager.MaxTaskTemplateNumber; idx++)
        //    {
        //        if (slots[idx] == 0)
        //        {
        //            if (ServerCommon.Data.TaskTemplateManager.Instance.FindItem(idx) == null)
        //                return idx;
        //        }
        //    }
        //    return UInt16.MaxValue;
        //}
        private void button15_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.TaskTemplate item = new ServerCommon.Data.TaskTemplate();
            //item.TaskId = NewTaskId();
            //item.IsDirty = true;
            //this.propertyGrid_TaskEditor.SelectedObject = item;
            //ServerCommon.Data.TaskTemplateManager.Instance.AddItem(item);
            ////FreshItemTree();
            //if (this.treeView_Tasks.Nodes.Count == 1 && this.treeView_Tasks.Nodes[0].Text == "Tasks")
            //    AddTaskToTree(this.treeView_Tasks.Nodes[0], item);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //ServerCommon.Data.TaskTemplateManager.Instance.SaveDirtyItems(ServerModule.Instance.Server);
            //FreshTaskTree();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            //if (this.treeView_Tasks.SelectedNode == null)
            //    return;
            //ServerCommon.Data.TaskTemplate item = this.treeView_Tasks.SelectedNode.Tag as ServerCommon.Data.TaskTemplate;
            //if (item == null)
            //    return;

            //ServerCommon.Data.TaskTemplateManager.Instance.DeleteItem(ServerModule.Instance.Server, item.TaskId);
            //FreshKnightTree();
        }
        private void treeView_Tasks_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //ServerCommon.Data.TaskTemplate item = e.Node.Tag as ServerCommon.Data.TaskTemplate;
            //if (item == null)
            //    return;
            //this.propertyGrid_TaskEditor.SelectedObject = item;
        }
        private void propertyGrid_TaskEditor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //ServerCommon.Data.TaskTemplate item = this.propertyGrid_TaskEditor.SelectedObject as ServerCommon.Data.TaskTemplate;
            //if (item == null)
            //    return;
            //item.IsDirty = true;
            //if (this.treeView_Tasks.SelectedNode == null)
            //    return;
            //ServerCommon.Data.TaskTemplate treeitem = this.treeView_Tasks.SelectedNode.Tag as ServerCommon.Data.TaskTemplate;
            //if (treeitem == item)
            //    this.treeView_Tasks.SelectedNode.ImageIndex = 1;
        }
        #endregion        

        private void UpdatePvP_Click(object sender, EventArgs e)
        {
            //ServerCommon.IDataServer dataServer = ServerModule.Instance.Server;

            //dataServer.UpdatePvPRank("PvPRank_PreDay", 10, dataServer.mRankingPreDay);
            //dataServer.AutoAnsyRankingPreDay();

            //if (System.DateTime.Now.DayOfWeek == System.DayOfWeek.Sunday)
            //{
            //    dataServer.UpdatePvPRank("PvPRank_PreWeek", 0, dataServer.mRankingPreWeek);
            //    dataServer.AutoAnsyRankingPreWeek();
            //}
        }

        private void DataServerFrm_Load(object sender, EventArgs e)
        {
            System.DateTime nowTime = new System.DateTime();

            nowTime = System.DateTime.Now;
            var date = nowTime.AddDays(1);
            System.DateTime ExcuteTime = new System.DateTime(date.Year, date.Month, date.Day, 0, 0, 5);
            var needSeconds = (ExcuteTime - nowTime).TotalMilliseconds;

        }

        private void UpdatePvPTimer_Tick(object sender, EventArgs e)
        {
            System.DateTime nowTime = new System.DateTime();

            nowTime = System.DateTime.Now;
            var date = nowTime.AddDays(1);
            System.DateTime ExcuteTime = new System.DateTime(date.Year, date.Month, date.Day, 0, 0, 5);
            var needSeconds = (ExcuteTime - nowTime).TotalMilliseconds;

            UpdatePvP_Click(sender, e);
        }

        public void SetServerConfigProgrid(object obj)
        {
            this.propertyGrid1.SelectedObject = obj;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string namePre = textBox2.Text;
            int num = int.Parse(textBox1.Text);
            int startIndex = int.Parse(mInputStartId.Text);
            var db = ServerCommon.IDataServer.Instance.DBLoaderConnect;
            for (int i = startIndex; i < startIndex+num; i++)
            {
                var ps = Guid.NewGuid().ToString("N").Substring(3,6);
                ServerCommon.IDataServer.Instance.PlayerManager.RegAccount(db, namePre + i.ToString("0000"), ps, "");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.LogControl.Lines = mLogs.ToArray();
            Log.FileLog.Instance.Flush();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            mLogs.Clear();
            this.LogControl.Lines = mLogs.ToArray();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //ServerFrame.Config.IConfigurator.SaveProperty(ServerModule.Instance.mParameter, "DataServer", ServerModule.Instance.mFullPathname);            
        }

    }
}
