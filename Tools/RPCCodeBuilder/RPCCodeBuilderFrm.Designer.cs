namespace RPCCodeBuilder
{
    partial class RPCCodeBuilderFrm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_RefreshConfig = new System.Windows.Forms.Button();
            this.comboBox_Projects = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Callee = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Caller = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button_LoadAssembly = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxCaller = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxCallee = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button_RefreshConfig);
            this.splitContainer1.Panel1.Controls.Add(this.comboBox_Projects);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.button7);
            this.splitContainer1.Panel1.Controls.Add(this.button6);
            this.splitContainer1.Panel1.Controls.Add(this.button5);
            this.splitContainer1.Panel1.Controls.Add(this.button4);
            this.splitContainer1.Panel1.Controls.Add(this.button3);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_Callee);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox_Caller);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.button_LoadAssembly);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1417, 974);
            this.splitContainer1.SplitterDistance = 545;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // button_RefreshConfig
            // 
            this.button_RefreshConfig.Location = new System.Drawing.Point(289, 8);
            this.button_RefreshConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_RefreshConfig.Name = "button_RefreshConfig";
            this.button_RefreshConfig.Size = new System.Drawing.Size(51, 25);
            this.button_RefreshConfig.TabIndex = 16;
            this.button_RefreshConfig.Text = "刷新";
            this.button_RefreshConfig.UseVisualStyleBackColor = true;
            this.button_RefreshConfig.Click += new System.EventHandler(this.button_RefreshConfig_Click);
            // 
            // comboBox_Projects
            // 
            this.comboBox_Projects.FormattingEnabled = true;
            this.comboBox_Projects.Location = new System.Drawing.Point(87, 8);
            this.comboBox_Projects.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBox_Projects.Name = "comboBox_Projects";
            this.comboBox_Projects.Size = new System.Drawing.Size(193, 23);
            this.comboBox_Projects.TabIndex = 15;
            this.comboBox_Projects.SelectedIndexChanged += new System.EventHandler(this.comboBox_Projects_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "工程选择";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(435, 110);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(85, 29);
            this.button7.TabIndex = 13;
            this.button7.Text = "保存配置";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(141, 110);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(109, 29);
            this.button6.TabIndex = 12;
            this.button6.Text = "分析客户端";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(21, 110);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(109, 29);
            this.button5.TabIndex = 11;
            this.button5.Text = "分析服务器";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(348, 110);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(83, 29);
            this.button4.TabIndex = 10;
            this.button4.Text = "生成代码";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(357, 74);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(27, 26);
            this.button3.TabIndex = 9;
            this.button3.Text = "。";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(357, 40);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 26);
            this.button2.TabIndex = 8;
            this.button2.Text = "。";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Callee";
            // 
            // textBox_Callee
            // 
            this.textBox_Callee.Location = new System.Drawing.Point(216, 74);
            this.textBox_Callee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_Callee.Name = "textBox_Callee";
            this.textBox_Callee.Size = new System.Drawing.Size(132, 25);
            this.textBox_Callee.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Caller";
            // 
            // textBox_Caller
            // 
            this.textBox_Caller.Location = new System.Drawing.Point(216, 40);
            this.textBox_Caller.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox_Caller.Name = "textBox_Caller";
            this.textBox_Caller.Size = new System.Drawing.Size(132, 25);
            this.textBox_Caller.TabIndex = 4;
            this.textBox_Caller.TextChanged += new System.EventHandler(this.textBox_cs_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 71);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "加载头文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.LineColor = System.Drawing.Color.BlanchedAlmond;
            this.treeView1.Location = new System.Drawing.Point(0, 152);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(537, 815);
            this.treeView1.TabIndex = 1;
            // 
            // button_LoadAssembly
            // 
            this.button_LoadAssembly.Location = new System.Drawing.Point(21, 38);
            this.button_LoadAssembly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_LoadAssembly.Name = "button_LoadAssembly";
            this.button_LoadAssembly.Size = new System.Drawing.Size(100, 29);
            this.button_LoadAssembly.TabIndex = 0;
            this.button_LoadAssembly.Text = "加载模块";
            this.button_LoadAssembly.UseVisualStyleBackColor = true;
            this.button_LoadAssembly.Click += new System.EventHandler(this.button_LoadAssembly_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(867, 974);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxCaller);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(859, 945);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "H";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxCaller
            // 
            this.textBoxCaller.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCaller.Location = new System.Drawing.Point(4, 4);
            this.textBoxCaller.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCaller.Multiline = true;
            this.textBoxCaller.Name = "textBoxCaller";
            this.textBoxCaller.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCaller.Size = new System.Drawing.Size(851, 937);
            this.textBoxCaller.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxCallee);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(859, 945);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cpp";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxCallee
            // 
            this.textBoxCallee.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCallee.Location = new System.Drawing.Point(4, 4);
            this.textBoxCallee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCallee.Multiline = true;
            this.textBoxCallee.Name = "textBoxCallee";
            this.textBoxCallee.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCallee.Size = new System.Drawing.Size(851, 937);
            this.textBoxCallee.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "dll";
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // RPCCodeBuilderFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1417, 974);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RPCCodeBuilderFrm";
            this.Text = "RPCCodeBuilder";
            this.Load += new System.EventHandler(this.RPCCodeBuilderFrm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button_LoadAssembly;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Callee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Caller;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBoxCaller;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxCallee;
        private System.Windows.Forms.Button button_RefreshConfig;
        private System.Windows.Forms.ComboBox comboBox_Projects;
        private System.Windows.Forms.Label label3;
    }
}

