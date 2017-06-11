using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPCCodeBuilder
{
    public delegate void Callfunc(string moduleName);
    public partial class RPCCodeBuilderFrm : Form
    {
        public string ServCallerToClient = "";

        Callfunc func;
        public RPCCodeBuilderFrm()
        {
            InitializeComponent();
        }

        // 加载模块
        private void button_LoadAssembly_Click(object sender, EventArgs e)
        {
            //if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    List<string> slst = new List<string>();
            //    slst.Add(this.openFileDialog1.FileName);
            //    MakeCodeFromModule(slst);
            //}
        }

        void MakeCodeFromModule(List<string> modules, bool bIsClient)
        {
            if (modules.Count == 0)
                return;

            this.treeView1.Nodes.Clear();

            if (bIsClient)
                func = LoadMoudleForClient;
            else
                func = LoadMoudleForServ;

            foreach (var xmoduleName in modules)
                func(xmoduleName);

            if (bIsClient)
                ClientMakeCode(bIsClient);
            else
                ServerMakeCode(bIsClient);
        }


        private void LoadMoudleForClient(string moduleName)
        {
            if (moduleName == "")
                return;

            System.Reflection.Assembly assembly;
            assembly = System.Reflection.Assembly.LoadFrom(moduleName);


            this.treeView1.Nodes.Clear();
            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                object[] attrs = t.GetCustomAttributes(false);

                if (t.Name.Contains("RpcRoot"))
                {

                }

                bool bFindClass = false;
                if (attrs != null && attrs.Length > 0)
                {
                    foreach (object attr in attrs)
                    {
                        if (((Attribute)attr).ToString() == "RPC.RPCClassAttribute")
                        {
                            bFindClass = true;
                            break;
                        }
                    }

                    if (!bFindClass)
                        continue;

                    TreeNode cNode = this.treeView1.Nodes.Add(t.FullName);
                    cNode.Tag = t;
                    cNode.Name = t.FullName;

                    System.Reflection.PropertyInfo[] props = t.GetProperties();
                    foreach (System.Reflection.PropertyInfo p in props)
                    {
                        object[] propAttrs = p.GetCustomAttributes(false);

                        if (propAttrs != null && propAttrs.Length > 0)
                        {
                            foreach (object propAtt in propAttrs)
                            {
                                if (p.Name == "Item")
                                {
                                    if (((Attribute)propAtt).ToString() =="RPC.RPCIndexObjectAttribute")
                                    {
                                        int ChildIndex = (int)((Attribute)propAtt).TypeId;
                                        TreeNode coNode = cNode.Nodes.Add("(I:" + ChildIndex.ToString() + ")" + p.Name);
                                        coNode.Tag = p;
                                        coNode.Name = ChildIndex.ToString();
                                    }
                                }
                                else
                                {
                                    if (((Attribute)propAtt).ToString() =="RPC.RPCChildObjectAttribute")
                                    {
                                        int ChildIndex = (int)((Attribute)propAtt).TypeId;
                                        TreeNode coNode = cNode.Nodes.Add("(P:" + ChildIndex.ToString() + ")" + p.Name);
                                        coNode.Tag = p;
                                        coNode.Name = ChildIndex.ToString();
                                    }
                                }
                            }
                        }
                    }

                    System.Reflection.MethodInfo[] methords = t.GetMethods();
                    Byte indexOfMethod = 0;
                    foreach (System.Reflection.MethodInfo m in methords)
                    {
                        object[] mtdAttrs = m.GetCustomAttributes(false);

                        bool bFindMethord = false;
                        if (mtdAttrs != null && mtdAttrs.Length > 0)
                        {
                            foreach (object mtd in mtdAttrs)
                            {
                                if (((Attribute)mtd).ToString() =="RPC.RPCMethodAttribute")
                                {
                                    bFindMethord = true;
                                    break;
                                }
                            }
                        }

                        if (!bFindMethord)
                            continue;

                        MethodDesc desc = new MethodDesc();
                        desc.mi = m;
                        desc.hostType = t;
                        desc.MethodIndex = indexOfMethod;
                        indexOfMethod++;
                        desc.HashcodeOfMethod = RPC.RPCEntrance.GetMethodHashCode(m, t.FullName);
                        TreeNode cmNode = cNode.Nodes.Add(m.Name);
                        cmNode.Tag = desc;
                    }
                }
            }
        }

        private void LoadMoudleForServ(string moduleName)
        {
            System.Reflection.Assembly assembly;
            assembly = System.Reflection.Assembly.LoadFrom(moduleName);

            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                object[] attrs = t.GetCustomAttributes(typeof(RPC.RPCClassAttribute), false);
                if (attrs != null && attrs.Length == 1)
                {
                    RPC.RPCClassAttribute att = attrs[0] as RPC.RPCClassAttribute;
                    if (att == null)
                        continue;


                    TreeNode cNode = this.treeView1.Nodes.Add(t.FullName);
                    cNode.Tag = att;
                    cNode.Name = t.FullName;

                    System.Reflection.PropertyInfo[] props = t.GetProperties();
                    foreach (System.Reflection.PropertyInfo p in props)
                    {
                        if (p.Name == "Item")
                        {
                            object[] propAttrs = p.GetCustomAttributes(typeof(RPC.RPCIndexObjectAttribute), false);
                            if (propAttrs != null && propAttrs.Length == 1)
                            {
                                RPC.RPCIndexObjectAttribute propAtt = propAttrs[0] as RPC.RPCIndexObjectAttribute;
                                if (propAtt == null)
                                    continue;

                                TreeNode coNode = cNode.Nodes.Add("(I:" + propAtt.ChildIndex.ToString() + ")" + p.Name);
                                coNode.Tag = p;
                                coNode.Name = propAtt.ChildIndex.ToString();
                            }
                        }
                        else
                        {
                            object[] propAttrs = p.GetCustomAttributes(typeof(RPC.RPCChildObjectAttribute), false);
                            if (propAttrs != null && propAttrs.Length == 1)
                            {
                                RPC.RPCChildObjectAttribute propAtt = propAttrs[0] as RPC.RPCChildObjectAttribute;
                                if (propAtt == null)
                                    continue;

                                TreeNode coNode = cNode.Nodes.Add("(P:" + propAtt.ChildIndex.ToString() + ")" + p.Name);
                                coNode.Tag = p;
                                coNode.Name = propAtt.ChildIndex.ToString();
                            }
                        }
                    }
                    {
                        System.Reflection.MethodInfo[] methords = t.GetMethods();
                        Byte indexOfMethod = 0;
                        foreach (System.Reflection.MethodInfo m in methords)
                        {
                            object[] mtdAttrs = m.GetCustomAttributes(typeof(RPC.RPCMethodAttribute), false);
                            if (mtdAttrs != null && mtdAttrs.Length == 1)
                            {
                                RPC.RPCMethodAttribute propAtt = mtdAttrs[0] as RPC.RPCMethodAttribute;
                                MethodDesc desc = new MethodDesc();
                                desc.mi = m;
                                desc.rpcAttr = propAtt;
                                desc.hostType = t;
                                desc.MethodIndex = indexOfMethod;
                                indexOfMethod++;
                                desc.HashcodeOfMethod = RPC.RPCEntrance.GetMethodHashCode(m, t.FullName);
                                TreeNode cmNode = cNode.Nodes.Add(m.Name);
                                cmNode.Tag = desc;
                            }
                        }
                    }
                }
            }
        }

        private void ServerMakeCode(bool bIsClient)
        {
            // Server Caller
            this.textBoxCaller.Text = "//Server Caller\r\n";
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                string strClient = "";
                this.textBoxCaller.Text += RPCClassBuilder.MakeClientClassCode(node, bIsClient, out strClient);
                ServCallerToClient += strClient;
            }

            // Server Callee
            this.textBoxCallee.Text += "//Server Callee\r\n";
            this.textBoxCallee.Text += "namespace RPC_ExecuterNamespace{" + "\r\n";
            string mappingCode = "";
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                foreach (TreeNode mnode in node.Nodes)
                {
                    MethodDesc mi = mnode.Tag as MethodDesc;
                    if (mi != null)
                    {
                        this.textBoxCallee.Text += RPCClassBuilder.MakeMethodExecuterCode(mi, bIsClient);//RPCClassBuilder.MakeMethodExecuterCode(mi, ca.RPCType);
                        mappingCode += "    RPC.RPCNetworkMgr.AddExecuterIndxer(" + mi.HashcodeOfMethod + " , " + mi.MethodIndex + ");\r\n";
                    }
                    else
                    {
                        System.Reflection.PropertyInfo pi = mnode.Tag as System.Reflection.PropertyInfo;
                        if (pi != null && pi.Name == "Item")
                        {
                            this.textBoxCallee.Text += RPCClassBuilder.MakeIndexerExecuterCode(pi, node, bIsClient);
                        }
                    }
                }
            }

            this.textBoxCallee.Text += "public class MappingHashCode2Index{\r\n";

            this.textBoxCallee.Text += "public static void BuildMapping(){\r\n";
            this.textBoxCallee.Text += mappingCode;
            this.textBoxCallee.Text += "}\r\n";

            this.textBoxCallee.Text += "}\r\n\r\n";

            this.textBoxCallee.Text += "}" + "\r\n";

        }

        private void ClientMakeCode(bool bIsClient)
        {
            // Server Caller
            this.textBoxCaller.Text = "//Server Caller\r\n";
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                this.textBoxCaller.Text += RPCClassBuilder.MakeClientClassCode(node, bIsClient);
            }

            // Server Callee
            this.textBoxCallee.Text += "//Server Callee\r\n";
            this.textBoxCallee.Text += "namespace RPC_ExecuterNamespace{" + "\r\n";
            string mappingCode = "";
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                foreach (TreeNode mnode in node.Nodes)
                {
                    MethodDesc mi = mnode.Tag as MethodDesc;
                    if (mi != null)
                    {
                        this.textBoxCallee.Text += RPCClassBuilder.MakeMethodExecuterCode(mi, bIsClient);
                        //if (!bIsClient)
                            mappingCode += "    RPC.RPCNetworkMgr.AddExecuterIndxer(" + mi.HashcodeOfMethod + " , " + mi.MethodIndex + ");\r\n";
                        //else
                        //    mappingCode += "    RPC.RPCNetworkMgr.AddExecuterIndxer(\"" +  mi.mi.Name + "\" , " + mi.MethodIndex + ");\r\n";
                    }
                    else
                    {
                        System.Reflection.PropertyInfo pi = mnode.Tag as System.Reflection.PropertyInfo;
                        if (pi != null && pi.Name == "Item")
                        {
                            this.textBoxCallee.Text += RPCClassBuilder.MakeIndexerExecuterCode(pi, node, bIsClient);
                        }
                    }
                }
            }
            
            this.textBoxCallee.Text += "public class MappingHashCode2Index{\r\n";

            this.textBoxCallee.Text += "public static void BuildMapping(){\r\n";
            this.textBoxCallee.Text += mappingCode;
            this.textBoxCallee.Text += "}\r\n";

            this.textBoxCallee.Text += "}\r\n\r\n";

            this.textBoxCallee.Text += "}" + "\r\n";

        }

        // 加载头文件
        private void button1_Click(object sender, EventArgs e)
        {
            //this.textBoxCaller.Text = "";
            //this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            //if (this.folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    string strFolder = this.folderBrowserDialog1.SelectedPath;
            //    CppMakeCodeFromFolder(strFolder);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Caller.Text = this.openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_Callee.Text = this.openFileDialog1.FileName;
            }
        }

        // 生成代码
        private void button4_Click(object sender, EventArgs e)
        {
            bool bIsClient = this.textBox_Callee.Text == Config.Instance.CurProjectConfig.ClientCallee;
            if (System.IO.File.Exists(this.textBox_Callee.Text))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.textBox_Callee.Text);
                if (sw != null)
                {
                    if(bIsClient)
                        sw.Write(this.textBoxCallee.Text.Replace("SlimDX", "UnityEngine"));
                    else
                        sw.Write(this.textBoxCallee.Text);
                    sw.Close();
                }
            }

            if (System.IO.File.Exists(this.textBox_Caller.Text))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(this.textBox_Caller.Text);
                if (sw != null)
                {
                    if(bIsClient)
                        sw.Write(this.textBoxCaller.Text.Replace("SlimDX", "UnityEngine"));
                    else
                        sw.Write(this.textBoxCaller.Text);
                    sw.Close();
                }
            }

            if (bIsClient)
            {
                if (System.IO.File.Exists(Config.Instance.CurProjectConfig.ServCppCaller))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(Config.Instance.CurProjectConfig.ServCppCaller);
                    if (sw != null)
                    {
                        sw.Write(this.textBoxCaller.Text.Replace("UnityEngine", "SlimDX"));
                        sw.Close();
                    }
                }
            }
            else
            {
                if (System.IO.File.Exists(Config.Instance.CurProjectConfig.ClientServCaller))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(Config.Instance.CurProjectConfig.ClientServCaller);
                    if (sw != null)
                    {
                        sw.Write(ServCallerToClient.Replace("SlimDX", "UnityEngine"));
                        sw.Close();
                    }
                }
            }

            //====================== 客户端同步服务器的类结构 ======================

            string outStruct = "";

            //GetCppStructureDefine只发送基础属性 byte[] x = new byte[3], list<int> y = new list<int>();类似属性都将不发送
            outStruct += new CSCommon.Data.RoleInfo().GetCppStructureDefine();
            outStruct += new CSCommon.Data.NPCData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.RoleDetail().GetCppStructureDefine();
            outStruct += new CSCommon.Data.RoleSyncInfo().GetCppStructureDefine();
            outStruct += new CSCommon.Data.ItemData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.SocialData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.SocialRoleInfo().GetCppStructureDefine();
            outStruct += new CSCommon.Data.GiftData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.TaskData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.MailData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.GuildCom().GetCppStructureDefine();
            outStruct += new CSCommon.Data.RoleCom().GetCppStructureDefine();
            outStruct += new CSCommon.Data.Message().GetCppStructureDefine();
            outStruct += new CSCommon.Data.SkillData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.MartialData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.AchieveData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.AttrStruct().GetCppStructureDefine();
            outStruct += new CSCommon.Data.CityForceData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.EfficiencyData().GetCppStructureDefine();
            outStruct += new CSCommon.Data.CampForceData().GetCppStructureDefine();
            //===================================================================
            
            if (System.IO.File.Exists(Config.Instance.CurProjectConfig.ClientHeader))
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(Config.Instance.CurProjectConfig.ClientHeader);
                if (sw != null)
                {
                    sw.Write(outStruct);
                    sw.Close();
                }
            }
        }

        // 读取C#文件
        private void button5_Click(object sender, EventArgs e)
        {//读取上次CSharp代码生成配置
            if (Config.Instance.CurProjectConfig == null)
                return;

            this.textBoxCaller.Text = "";
            this.textBoxCallee.Text = "";
            string[] segs = Config.Instance.CurProjectConfig.ServModuleName.Split(';');
            List<string> modules = new List<string>();
            foreach(var i in segs)
            {
                modules.Add(i);
            }
            MakeCodeFromModule(modules, false);
            
            this.textBox_Caller.Text = Config.Instance.CurProjectConfig.ServerCaller;
            this.textBox_Callee.Text = Config.Instance.CurProjectConfig.ServerCallee;
        }

        //string m_strCppCodeClient;

        // 读取C++文件
        private void button6_Click(object sender, EventArgs e)
        {//读取上次C++代码生成配置

            if (Config.Instance.CurProjectConfig == null)
                return;

            this.textBoxCaller.Text = "";
            this.textBoxCallee.Text = "";
            //CppMakeCodeFromFolder(Config.Instance.CurProjectConfig.CppFolder);
            string[] segs = Config.Instance.CurProjectConfig.ClientModuleName.Split(';');
            List<string> lst = new List<string>();
            foreach (var i in segs) 
            {
                lst.Add(i);
            }
            MakeCodeFromModule(lst, true);
            this.textBox_Caller.Text = Config.Instance.CurProjectConfig.ClientCaller;
            this.textBox_Callee.Text = Config.Instance.CurProjectConfig.ClientCallee;
        }

        // 保存配置
        private void button7_Click(object sender, EventArgs e)
        {
            Config.Instance.Save(Config.Instance.ConfigFile);
        }

        private void RPCCodeBuilderFrm_Load(object sender, EventArgs e)
        {
            button_RefreshConfig_Click(sender, e);
        }

        private void comboBox_Projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.Instance.SetCurProject(comboBox_Projects.SelectedItem.ToString());
        }

        private void button_RefreshConfig_Click(object sender, EventArgs e)
        {
            comboBox_Projects.Items.Clear();

            Config.Instance.Load(Config.Instance.ConfigFile);
            foreach (var proj in Config.Instance.ProjConfigList)
            {
                comboBox_Projects.Items.Add(proj.ProjectName);
            }
            if (comboBox_Projects.Items.Count > 0)
                comboBox_Projects.SelectedIndex = 0;

            comboBox_Projects.Update();
        }

        private void textBox_cs_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

    