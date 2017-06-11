namespace TemplateEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Open = new System.Windows.Forms.Button();
            this.button_SaveAs = new System.Windows.Forms.Button();
            this.comboBox_Templates = new System.Windows.Forms.ComboBox();
            this.button_New = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkBox_AutoSave = new System.Windows.Forms.CheckBox();
            this.textBox_SearchFolder = new System.Windows.Forms.TextBox();
            this.treeView_Files = new System.Windows.Forms.TreeView();
            this.button_ChooseFolder = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mTypeLabel = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button5 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.mExcelPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.mExcelToTxtPath = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(438, 562);
            this.propertyGrid.TabIndex = 0;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(190, 35);
            this.button_Save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(84, 25);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "储存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(98, 35);
            this.button_Open.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(84, 25);
            this.button_Open.TabIndex = 1;
            this.button_Open.Text = "打开";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // button_SaveAs
            // 
            this.button_SaveAs.Location = new System.Drawing.Point(282, 35);
            this.button_SaveAs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_SaveAs.Name = "button_SaveAs";
            this.button_SaveAs.Size = new System.Drawing.Size(84, 25);
            this.button_SaveAs.TabIndex = 1;
            this.button_SaveAs.Text = "另存为";
            this.button_SaveAs.UseVisualStyleBackColor = true;
            this.button_SaveAs.Click += new System.EventHandler(this.button_SaveAs_Click);
            // 
            // comboBox_Templates
            // 
            this.comboBox_Templates.FormattingEnabled = true;
            this.comboBox_Templates.Location = new System.Drawing.Point(6, 4);
            this.comboBox_Templates.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_Templates.Name = "comboBox_Templates";
            this.comboBox_Templates.Size = new System.Drawing.Size(493, 23);
            this.comboBox_Templates.TabIndex = 2;
            this.comboBox_Templates.SelectedIndexChanged += new System.EventHandler(this.comboBox_Templates_SelectedIndexChanged);
            // 
            // button_New
            // 
            this.button_New.Location = new System.Drawing.Point(6, 35);
            this.button_New.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_New.Name = "button_New";
            this.button_New.Size = new System.Drawing.Size(84, 25);
            this.button_New.TabIndex = 1;
            this.button_New.Text = "新建";
            this.button_New.UseVisualStyleBackColor = true;
            this.button_New.Click += new System.EventHandler(this.button_New_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(3, 2);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Size = new System.Drawing.Size(852, 629);
            this.splitContainer1.SplitterDistance = 438;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // checkBox_AutoSave
            // 
            this.checkBox_AutoSave.AutoSize = true;
            this.checkBox_AutoSave.Checked = true;
            this.checkBox_AutoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_AutoSave.Location = new System.Drawing.Point(524, 8);
            this.checkBox_AutoSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_AutoSave.Name = "checkBox_AutoSave";
            this.checkBox_AutoSave.Size = new System.Drawing.Size(89, 19);
            this.checkBox_AutoSave.TabIndex = 3;
            this.checkBox_AutoSave.Text = "自动保存";
            this.checkBox_AutoSave.UseVisualStyleBackColor = true;
            // 
            // textBox_SearchFolder
            // 
            this.textBox_SearchFolder.Location = new System.Drawing.Point(4, 10);
            this.textBox_SearchFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_SearchFolder.Name = "textBox_SearchFolder";
            this.textBox_SearchFolder.ReadOnly = true;
            this.textBox_SearchFolder.Size = new System.Drawing.Size(207, 25);
            this.textBox_SearchFolder.TabIndex = 1;
            // 
            // treeView_Files
            // 
            this.treeView_Files.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Files.HideSelection = false;
            this.treeView_Files.Location = new System.Drawing.Point(0, 0);
            this.treeView_Files.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView_Files.Name = "treeView_Files";
            this.treeView_Files.Size = new System.Drawing.Size(409, 561);
            this.treeView_Files.TabIndex = 0;
            this.treeView_Files.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Files_AfterSelect);
            // 
            // button_ChooseFolder
            // 
            this.button_ChooseFolder.Location = new System.Drawing.Point(233, 9);
            this.button_ChooseFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_ChooseFolder.Name = "button_ChooseFolder";
            this.button_ChooseFolder.Size = new System.Drawing.Size(84, 25);
            this.button_ChooseFolder.TabIndex = 1;
            this.button_ChooseFolder.Text = "选择目录";
            this.button_ChooseFolder.UseVisualStyleBackColor = true;
            this.button_ChooseFolder.Click += new System.EventHandler(this.button_ChooseFolder_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(866, 662);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(858, 633);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "工具修改";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.mTypeLabel);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(945, 699);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据导入导出";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "导出路径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 456);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "导出路径";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 428);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "文件名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 390);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "导入状态";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "导出状态";
            // 
            // mTypeLabel
            // 
            this.mTypeLabel.AutoSize = true;
            this.mTypeLabel.Location = new System.Drawing.Point(275, 55);
            this.mTypeLabel.Name = "mTypeLabel";
            this.mTypeLabel.Size = new System.Drawing.Size(67, 15);
            this.mTypeLabel.TabIndex = 3;
            this.mTypeLabel.Text = "文件类型";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(61, 485);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(144, 25);
            this.button4.TabIndex = 2;
            this.button4.Text = "导出到模版文件";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClickExportToFile);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(61, 385);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 25);
            this.button3.TabIndex = 2;
            this.button3.Text = "导入excel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClickImportExcel);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(72, 118);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 25);
            this.button2.TabIndex = 2;
            this.button2.Text = "导出excel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClickExportExcel);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(61, 18);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(211, 25);
            this.button6.TabIndex = 2;
            this.button6.Text = "打开多个同类型文件";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.OnClickOpenSelecetFile);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(315, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(253, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "打开同一文件夹所有同类型文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickOpenAllFile);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(164, 452);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(901, 25);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "D:\\tp\\npcforce\\";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(132, 82);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(776, 25);
            this.textBox2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer2);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(945, 699);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "配置";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button5);
            this.splitContainer2.Size = new System.Drawing.Size(945, 699);
            this.splitContainer2.SplitterDistance = 689;
            this.splitContainer2.TabIndex = 2;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(689, 699);
            this.propertyGrid1.TabIndex = 1;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(101, 24);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 22);
            this.button5.TabIndex = 0;
            this.button5.Text = "保存";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.mExcelPath);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.mExcelToTxtPath);
            this.tabPage4.Controls.Add(this.button7);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(945, 699);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "excel导出";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "excel所在目录";
            // 
            // mExcelPath
            // 
            this.mExcelPath.Location = new System.Drawing.Point(140, 141);
            this.mExcelPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mExcelPath.Name = "mExcelPath";
            this.mExcelPath.Size = new System.Drawing.Size(683, 25);
            this.mExcelPath.TabIndex = 6;
            this.mExcelPath.Text = "D:\\tp\\_data\\";
            this.mExcelPath.TextChanged += new System.EventHandler(this.mExcelPath_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "导出路径";
            // 
            // mExcelToTxtPath
            // 
            this.mExcelToTxtPath.Location = new System.Drawing.Point(140, 88);
            this.mExcelToTxtPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mExcelToTxtPath.Name = "mExcelToTxtPath";
            this.mExcelToTxtPath.Size = new System.Drawing.Size(776, 25);
            this.mExcelToTxtPath.TabIndex = 4;
            this.mExcelToTxtPath.Text = "D:\\tp\\_data\\tmp";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(356, 229);
            this.button7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(139, 46);
            this.button7.TabIndex = 0;
            this.button7.Text = "导出Text";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.comboBox_Templates);
            this.splitContainer3.Panel1.Controls.Add(this.checkBox_AutoSave);
            this.splitContainer3.Panel1.Controls.Add(this.button_Open);
            this.splitContainer3.Panel1.Controls.Add(this.button_Save);
            this.splitContainer3.Panel1.Controls.Add(this.button_SaveAs);
            this.splitContainer3.Panel1.Controls.Add(this.button_New);
            this.splitContainer3.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer3_Panel1_Paint);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer3.Size = new System.Drawing.Size(438, 629);
            this.splitContainer3.SplitterDistance = 63;
            this.splitContainer3.TabIndex = 4;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.textBox_SearchFolder);
            this.splitContainer4.Panel1.Controls.Add(this.button_ChooseFolder);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.treeView_Files);
            this.splitContainer4.Size = new System.Drawing.Size(409, 629);
            this.splitContainer4.SplitterDistance = 64;
            this.splitContainer4.TabIndex = 2;
            this.splitContainer4.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer4_SplitterMoved);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 662);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Open;
        private System.Windows.Forms.Button button_SaveAs;
        private System.Windows.Forms.ComboBox comboBox_Templates;
        private System.Windows.Forms.Button button_New;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView_Files;
        private System.Windows.Forms.TextBox textBox_SearchFolder;
        private System.Windows.Forms.Button button_ChooseFolder;
        private System.Windows.Forms.CheckBox checkBox_AutoSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label mTypeLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox mExcelPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox mExcelToTxtPath;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
    }
}

