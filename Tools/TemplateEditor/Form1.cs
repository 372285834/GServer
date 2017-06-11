using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TemplateEditor
{
    public partial class Form1 : Form
    {
        //ServerCommon.Data.BuildConditionManager m_buildConditionManager = new ServerCommon.Data.BuildConditionManager();
        public System.Reflection.Assembly m_CSCommonAssembly;
        public System.Reflection.Assembly m_ServerCommonAssembly;
        System.Reflection.Assembly m_UIEditorAssembly;
        System.Object m_Instance;
        string m_opendFileName = "";

        static Form1 mInst;
        public static Form1 Inst { get { return mInst; } }

        public string SearchFolderText
        {
            get
            {
                if (textBox_SearchFolder.Text.Contains(':'))
                {
                    if (textBox_SearchFolder.Text[textBox_SearchFolder.Text.Length - 1] == '/')
                        return textBox_SearchFolder.Text;
                    else
                        return textBox_SearchFolder.Text + "/";
                }
                else
                    return System.IO.Directory.GetCurrentDirectory() + "/" + textBox_SearchFolder.Text;
            }
        }

        public class CComboBoxItem
        {
            public Type m_Type;
            public String m_strExt;

            public override string ToString()
            {
                return m_Type.ToString();
            }
        }

        public ExportConfig mConfig = new ExportConfig();
        public Form1()
        {
            InitializeComponent();
            mInst = this;
            
            string fullPathname = ServerCommon.IServer.Instance.ExePath + "TemplateConfig.cfg";
            if (false == ServerFrame.Config.IConfigurator.FillProperty(mConfig, fullPathname))
            {
                //System.Windows.Forms.MessageBox.Show("请检查服务器配置文件");
                ServerFrame.Config.IConfigurator.SaveProperty(mConfig, "TemplateConfig", fullPathname);
            }
            propertyGrid1.SelectedObject = mConfig;

            ApplyConfig();

            
            string dir = System.AppDomain.CurrentDomain.BaseDirectory + "CodeLinker.dll";
            m_UIEditorAssembly = System.Reflection.Assembly.LoadFile(dir);

            var aTypes = m_UIEditorAssembly.GetTypes();

            //foreach (var type in aTypes)
            //{
            //    var atts = type.GetCustomAttributes(typeof(Device.CDataEditorAttribute), true);

            //    if (atts.Length > 0)
            //    {
            //        Device.CDataEditorAttribute dea = atts[0] as Device.CDataEditorAttribute;

            //        CComboBoxItem item = new CComboBoxItem()
            //        {
            //            m_Type = type,
            //            m_strExt = dea.m_strFileExt
            //        };

            //        comboBox_Templates.Items.Add(item);
            //    }
            //}

            dir = System.AppDomain.CurrentDomain.BaseDirectory + "ServerCommon.dll";
            //var dir = System.IO.Directory.GetCurrentDirectory() + "\\ServerCommon.dll";
            m_ServerCommonAssembly = System.Reflection.Assembly.LoadFile(dir);

            aTypes = m_ServerCommonAssembly.GetTypes();

            foreach (var type in aTypes)
            {
                var atts = type.GetCustomAttributes(typeof(ServerFrame.Editor.CDataEditorAttribute), true);

                if (atts.Length > 0)
                {
                    ServerFrame.Editor.CDataEditorAttribute dea = atts[0] as ServerFrame.Editor.CDataEditorAttribute;

                    CComboBoxItem item = new CComboBoxItem()
                    {
                        m_Type = type,
                        m_strExt = dea.m_strFileExt
                    };

                    comboBox_Templates.Items.Add(item);
                }
            }

            dir = System.AppDomain.CurrentDomain.BaseDirectory + "CSCommon.dll";
            //var dir = System.IO.Directory.GetCurrentDirectory() + "\\ServerCommon.dll";
            m_CSCommonAssembly = System.Reflection.Assembly.LoadFile(dir);

            aTypes = m_CSCommonAssembly.GetTypes();

            foreach (var type in aTypes)
            {
                var atts = type.GetCustomAttributes(typeof(ServerFrame.Editor.CDataEditorAttribute), true);

                if (atts.Length > 0)
                {
                    ServerFrame.Editor.CDataEditorAttribute dea = atts[0] as ServerFrame.Editor.CDataEditorAttribute;

                    CComboBoxItem item = new CComboBoxItem()
                    {
                        m_Type = type,
                        m_strExt = dea.m_strFileExt
                    };

                    comboBox_Templates.Items.Add(item);
                }
            }


            comboBox_Templates.SelectedIndex = 0;
        }

        private void ApplyConfig()
        {
            textBox_SearchFolder.Text = mConfig.SearchDefalutFolder;



            if (string.IsNullOrEmpty(mConfig.ExportExcelPath))
                textBox2.Text = "D:\\";
            else
                textBox2.Text = mConfig.ExportExcelPath;

            if (string.IsNullOrEmpty(mConfig.ExportFilePath))
                textBox1.Text =  "D:\\";
            else
                textBox1.Text = mConfig.ExportFilePath;
        }
        
        private void button_New_Click(object sender, EventArgs e)
        {
            m_opendFileName = null;
            m_Instance = m_ServerCommonAssembly.CreateInstance(comboBox_Templates.SelectedItem.ToString());
            if (m_Instance == null)
                m_Instance = m_UIEditorAssembly.CreateInstance(comboBox_Templates.SelectedItem.ToString());
            if(m_Instance == null )
                m_Instance = m_CSCommonAssembly.CreateInstance(comboBox_Templates.SelectedItem.ToString());
            propertyGrid.SelectedObject = m_Instance;
            this.Text = "新建";
        }

        public object OpenFile(string strFile)
        {
            
            string strLine = "";
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            using(StreamReader sr = new StreamReader(strFile))
            {
                try
                {
                    doc.Load(sr);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                	return null;
                }
            }
            strLine = doc.DocumentElement.Name;

            foreach (var itm in comboBox_Templates.Items)
            {
                if (!(itm is CComboBoxItem))
                    continue;
                CComboBoxItem item = itm as CComboBoxItem;

                if (item.m_Type.ToString() == strLine)
                {
                    comboBox_Templates.SelectedItem = item;
                    break;
                }
            }

            m_Instance = m_ServerCommonAssembly.CreateInstance(strLine);
            if (m_Instance == null)
                m_Instance = m_UIEditorAssembly.CreateInstance(strLine);
            if (m_Instance == null)
                m_Instance = m_CSCommonAssembly.CreateInstance(comboBox_Templates.SelectedItem.ToString());

            if (m_Instance == null)
            {
                MessageBox.Show("无法创建实例:" + strLine);
                return null;
            }
            //Common.AutoSaveLoad.LoadProperty(m_Instance, strFile);
            ServerFrame.Config.IConfigurator.FillProperty(m_Instance, strFile);
            propertyGrid.SelectedObject = m_Instance;
            this.Text = strFile;
            m_opendFileName = strFile;
            return m_Instance;
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofD = new OpenFileDialog();

            var curPath = System.IO.Directory.GetCurrentDirectory();
            var subPath = curPath.Remove(curPath.IndexOf("Kiang") + 5);
            ofD.InitialDirectory = subPath + "\\Release\\Template\\";
            ofD.Filter = "All Files(*.*)|*.*";

            foreach (var itm in comboBox_Templates.Items)
            {
                if (!(itm is CComboBoxItem))
                    continue;
                CComboBoxItem item = itm as CComboBoxItem;
                ofD.Filter += "|" + item.m_Type.ToString() + " Files(*" + item.m_strExt + ")|*" + item.m_strExt;
            }

            if (ofD.ShowDialog() == DialogResult.OK)
            {
                OpenFile(ofD.FileName);
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (m_Instance == null)
                return;

            if (string.IsNullOrEmpty(m_opendFileName))
            {
                MessageBox.Show("请点击另存为按钮保存!");
                return;
            }

            //Common.AutoSaveLoad.SaveProperty(m_Instance, m_Instance.GetType().ToString(), m_opendFileName);
            ServerFrame.Config.IConfigurator.SaveProperty(m_Instance, m_Instance.GetType().ToString(), m_opendFileName);
        }

        private void button_SaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfD = new SaveFileDialog();
            CComboBoxItem item = comboBox_Templates.SelectedItem as CComboBoxItem;
            if(item != null)
                sfD.Filter = item.m_Type.ToString() + " Files(*" + item.m_strExt + ")|*" + item.m_strExt + "|All files(*.*)|*.*";
            if (sfD.ShowDialog() == DialogResult.OK)
            {
                m_opendFileName = sfD.FileName;

                //Common.AutoSaveLoad.SaveProperty(m_Instance, m_Instance.GetType().ToString(), sfD.FileName);
                ServerFrame.Config.IConfigurator.SaveProperty(m_Instance, m_Instance.GetType().ToString(), sfD.FileName);
                this.Text = sfD.FileName;
            }
        }

        private void comboBox_Templates_SelectedIndexChanged(object sender, EventArgs e)
        {
            CComboBoxItem item = comboBox_Templates.SelectedItem as CComboBoxItem;
            if (item == null)
                return;

            treeView_Files.Nodes.Clear();

            SearchFolder(SearchFolderText, item);
            treeView_Files.ExpandAll();
            treeView_Files.Update();
        }

        //private void AddToTreeView(string str)
        //{
        //    foreach ()
        //    {
        //    }
        //}

        private TreeNode GetTreeNode(string name, TreeView tree)
        {
            foreach (TreeNode node in tree.Nodes)
            {
                var retNode = GetTreeNode(name, node);
                if (retNode != null)
                    return retNode;
            }

            return null;
        }
        private TreeNode GetTreeNode(string name, TreeNode parNode)
        {
            if (parNode.Text == name)
                return parNode;

            foreach (TreeNode node in parNode.Nodes)
            {
                var retNode = GetTreeNode(name, node);
                if (retNode != null)
                    return retNode;
            }

            return null;
        }

        private void SearchFolder(string folder, CComboBoxItem item)
        {
            if (!System.IO.Directory.Exists(folder))
                return;
            var files = System.IO.Directory.EnumerateFiles(folder);
            foreach (var file in files)
            {
                var dotIdx = file.LastIndexOf('.');
                if (dotIdx < 0)
                    continue;

                if (file.Substring(dotIdx) != item.m_strExt)
                    continue;

                //string strLine = "";

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                using (StreamReader sr = new StreamReader(file))
                {
                    try
                    {
                        doc.Load(sr);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        continue;
                    }
                    //do
                    //{
                    //    strLine += sr.ReadLine();
                    //} while (!(strLine.Length > 1 && strLine[0] == '<'));

                    //strLine = strLine.Remove(0, 1);
                    //strLine = strLine.Remove(strLine.Length - 1);
                }

                if (doc.DocumentElement != null && doc.DocumentElement.Name == item.m_Type.ToString())//strLine.Contains("<" + item.m_Type.ToString() + ">"))//(item.m_Type.ToString() == strLine)
                {
                    // 加入TreeView
                    var searchFolderIdx = file.IndexOf(textBox_SearchFolder.Text);
                    var tempFile = file.Remove(0, searchFolderIdx + textBox_SearchFolder.Text.Length);
                    tempFile = tempFile.Replace("\\", "/");

                    var splits = tempFile.Split('/');
                    TreeNode parentNode = null;
                    TreeNode treeNode = null;
                    foreach (var split in splits)
                    {
                        if (string.IsNullOrEmpty(split))
                            continue;

                        if (parentNode == null)
                            treeNode = GetTreeNode(split, treeView_Files);
                        else
                            treeNode = GetTreeNode(split, parentNode);

                        if (treeNode == null)
                        {
                            if (parentNode == null)
                            {
                                treeNode = new TreeNode();
                                treeNode.Text = split;
                                treeView_Files.Nodes.Add(treeNode);
                                //treeNode.Expand();
                            }
                            else
                            {
                                treeNode = new TreeNode();
                                treeNode.Text = split;
                                parentNode.Nodes.Add(treeNode);
                            }
                        }

                        parentNode = treeNode;
                    }
                }
            }

            var folders = System.IO.Directory.EnumerateDirectories(folder);
            foreach (var fd in folders)
            {
                SearchFolder(fd, item);
            }
        }

        private void treeView_Files_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strFile = "";
            var node = treeView_Files.SelectedNode;
            while(node != null)
            {
                if (string.IsNullOrEmpty(strFile))
                    strFile = node.Text;
                else
                    strFile = node.Text + "/" + strFile;
                node = node.Parent;
            }

            if (checkBox_AutoSave.Checked && !string.IsNullOrEmpty(m_opendFileName) && m_Instance != null)
            {
                ServerFrame.Config.IConfigurator.SaveProperty(m_Instance, m_Instance.GetType().ToString(), m_opendFileName);
                //Common.AutoSaveLoad.SaveProperty(m_Instance, m_Instance.GetType().ToString(), m_opendFileName);
            }
            OpenFile(SearchFolderText + strFile);
        }

        private void button_ChooseFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                treeView_Files.Nodes.Clear();

                textBox_SearchFolder.Text = fbd.SelectedPath;
                if (comboBox_Templates.SelectedItem != null)
                {
                    SearchFolder(SearchFolderText, (CComboBoxItem)(comboBox_Templates.SelectedItem));
                    treeView_Files.ExpandAll();
                    treeView_Files.Update();
                }
            }
        }

        private void OnClickOpenAllFile(object sender, EventArgs e)
        {
            OpenFileDialog ofD = new OpenFileDialog();

            var curPath = System.IO.Directory.GetCurrentDirectory();
            var subPath = curPath.Remove(curPath.IndexOf("Bin"));

            if (string.IsNullOrEmpty(mConfig.OpenTemplatePath))
                ofD.InitialDirectory = subPath + "DemaciaGame\\Template\\";
            else
                ofD.InitialDirectory = mConfig.OpenTemplatePath;
            ofD.Filter = "All Files(*.*)|*.*";

            foreach (var itm in comboBox_Templates.Items)
            {
                if (!(itm is CComboBoxItem))
                    continue;
                CComboBoxItem item = itm as CComboBoxItem;
                ofD.Filter += "|" + item.m_Type.ToString() + " Files(*" + item.m_strExt + ")|*" + item.m_strExt;
            }

            if (ofD.ShowDialog() == DialogResult.OK)
            {
                LoadSuffixInfo(ofD.FileName);
            }
            else
                return;
        }

        void OnClickOpenSelecetFile(object sender, EventArgs e)
        {
            OpenFileDialog ofD = new OpenFileDialog();

            var curPath = System.IO.Directory.GetCurrentDirectory();
            var subPath = curPath.Remove(curPath.IndexOf("Bin"));
            ofD.Multiselect = true;

            if (string.IsNullOrEmpty(mConfig.OpenTemplatePath))
                ofD.InitialDirectory = subPath + "DemaciaGame\\Template\\";
            else
                ofD.InitialDirectory = mConfig.OpenTemplatePath;
            ofD.Filter = "All Files(*.*)|*.*";

            foreach (var itm in comboBox_Templates.Items)
            {
                if (!(itm is CComboBoxItem))
                    continue;
                CComboBoxItem item = itm as CComboBoxItem;
                ofD.Filter += "|" + item.m_Type.ToString() + " Files(*" + item.m_strExt + ")|*" + item.m_strExt;
            }

            if (ofD.ShowDialog() == DialogResult.OK)
            {
                LoadSuffixInfo(ofD.FileNames);
            }
        }

        void OnClickExportExcel(object sender, EventArgs e)
        {
            
            //TemplateEditor.ExportToExcel.doExport(mDataset, textBox2.Text);
            SetExportState("正在导出中..........");

            mExportToExcel.DatasetExportToExcel(mDataset, textBox2.Text);
            SetExportState("导出结束！！！");
        }

        void OnClickImportExcel(object sender, EventArgs e)
        {
            //TemplateEditor.ExportToExcel.doExport(mDataset, textBox2.Text);
            label3.Text = "正在导入excel中..........";
            mExportToExcel.CreateDataSetFromExcel();
            label3.Text = "导入完成";
        }

        public void SetExcelFileShowName(string name)
        {
            label5.Text = name;
        }

        void OnClickExportToFile(object sender, EventArgs e)
        {
            //TemplateEditor.ExportToExcel.doExport(mDataset, textBox2.Text);
            label3.Text = "正在导出到文件中..........";
            mExportToExcel.DatasetExportToTemplateFile(textBox1.Text);
            label3.Text = "导出文件完成";
        }


        public void SetExportState(string state)
        {
            label2.Text = state;
        }


        DataSet mDataset;
        TemplateEditor.ExportToExcel mExportToExcel = new TemplateEditor.ExportToExcel();
        public void LoadSuffixInfo(string fileName)
        {
            var suffix = fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
            var folder = fileName.Remove(fileName.LastIndexOf('\\'));
            Type curType = null;
            mDataset = mExportToExcel.CreateDataSetFromFileBySuffix(suffix, folder, out curType);
            mTypeLabel.Text = curType.ToString();
        }

        public void LoadSuffixInfo(string[] fileNames)
        {

            var suffix = fileNames[0].Substring(fileNames[0].LastIndexOf('.'), fileNames[0].Length - fileNames[0].LastIndexOf('.'));
            Type curType = null;
            mDataset = mExportToExcel.CreateDataSetFromFileBySuffix(suffix, fileNames, out curType);
            mTypeLabel.Text = curType.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string fullPathname = ServerCommon.IServer.Instance.ExePath + "TemplateConfig.cfg";
            ServerFrame.Config.IConfigurator.SaveProperty(mConfig, "TemplateConfig", fullPathname);
            ApplyConfig();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            LoadTableFromExcel.ExportByOle(mExcelPath.Text, mExcelToTxtPath.Text);
        }

        private void mExcelPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer3_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer4_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

    }
}
