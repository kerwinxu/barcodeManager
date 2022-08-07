namespace VestShapes
{
    partial class FrmPageSettings
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPagePaper = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboPrinters = new System.Windows.Forms.ComboBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.comboPaperSizes = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btn_paper_ok = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_paper_width = new System.Windows.Forms.TextBox();
            this.txt_paper_height = new System.Windows.Forms.TextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tabPageLayout = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkCustomInterval = new System.Windows.Forms.CheckBox();
            this.txtHorizontalInterval = new System.Windows.Forms.TextBox();
            this.txtVerticalInterval = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCustomModelSize = new System.Windows.Forms.CheckBox();
            this.txtModelWidth = new System.Windows.Forms.TextBox();
            this.txtModelHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMaginsTop = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMaginsLeft = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMaginsRight = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMaginsBottom = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UpDownNumberOfColumn = new System.Windows.Forms.NumericUpDown();
            this.UpDownNumberOfLine = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPageShapes = new System.Windows.Forms.TabPage();
            this.groupBoxRect = new System.Windows.Forms.GroupBox();
            this.chkEllipseHole = new System.Windows.Forms.CheckBox();
            this.txtEllipseHoleRadius = new System.Windows.Forms.TextBox();
            this.lblEllipseHoleRadius = new System.Windows.Forms.Label();
            this.txtRoundRadius = new System.Windows.Forms.TextBox();
            this.lblRoundRadius = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.radioButtonRoundRect = new System.Windows.Forms.RadioButton();
            this.radioButtonEllipse = new System.Windows.Forms.RadioButton();
            this.radioButtonRect = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPaperSize = new System.Windows.Forms.Label();
            this.lblModelSize = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPagePaper.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.tabPageLayout.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfLine)).BeginInit();
            this.tabPageShapes.SuspendLayout();
            this.groupBoxRect.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPagePaper);
            this.tabControl1.Controls.Add(this.tabPageLayout);
            this.tabControl1.Controls.Add(this.tabPageShapes);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(318, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPagePaper
            // 
            this.tabPagePaper.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPagePaper.Controls.Add(this.flowLayoutPanel1);
            this.tabPagePaper.Location = new System.Drawing.Point(4, 22);
            this.tabPagePaper.Name = "tabPagePaper";
            this.tabPagePaper.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePaper.Size = new System.Drawing.Size(310, 393);
            this.tabPagePaper.TabIndex = 0;
            this.tabPagePaper.Text = "纸张";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox6);
            this.flowLayoutPanel1.Controls.Add(this.groupBox9);
            this.flowLayoutPanel1.Controls.Add(this.groupBox7);
            this.flowLayoutPanel1.Controls.Add(this.groupBox10);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(304, 387);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboPrinters);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(298, 58);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "打印机";
            // 
            // comboPrinters
            // 
            this.comboPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPrinters.FormattingEnabled = true;
            this.comboPrinters.Location = new System.Drawing.Point(6, 20);
            this.comboPrinters.Name = "comboPrinters";
            this.comboPrinters.Size = new System.Drawing.Size(286, 20);
            this.comboPrinters.TabIndex = 1;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.comboPaperSizes);
            this.groupBox9.Location = new System.Drawing.Point(3, 67);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(298, 58);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "纸张大小";
            // 
            // comboPaperSizes
            // 
            this.comboPaperSizes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPaperSizes.FormattingEnabled = true;
            this.comboPaperSizes.Location = new System.Drawing.Point(6, 20);
            this.comboPaperSizes.Name = "comboPaperSizes";
            this.comboPaperSizes.Size = new System.Drawing.Size(286, 20);
            this.comboPaperSizes.TabIndex = 1;
            this.comboPaperSizes.SelectedValueChanged += new System.EventHandler(this.comboPaperSizes_SelectedValueChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btn_paper_ok);
            this.groupBox7.Controls.Add(this.tableLayoutPanel1);
            this.groupBox7.Location = new System.Drawing.Point(3, 131);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(298, 97);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "纸张自定义";
            // 
            // btn_paper_ok
            // 
            this.btn_paper_ok.Location = new System.Drawing.Point(189, 55);
            this.btn_paper_ok.Name = "btn_paper_ok";
            this.btn_paper_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_paper_ok.TabIndex = 1;
            this.btn_paper_ok.Text = "确定";
            this.btn_paper_ok.UseVisualStyleBackColor = true;
            this.btn_paper_ok.Click += new System.EventHandler(this.btn_paper_ok_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.3871F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.6129F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txt_paper_width, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label14, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label15, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_paper_height, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(177, 62);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "宽度：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "高度：";
            // 
            // txt_paper_width
            // 
            this.txt_paper_width.Location = new System.Drawing.Point(76, 3);
            this.txt_paper_width.Name = "txt_paper_width";
            this.txt_paper_width.Size = new System.Drawing.Size(63, 21);
            this.txt_paper_width.TabIndex = 2;
            // 
            // txt_paper_height
            // 
            this.txt_paper_height.Location = new System.Drawing.Point(76, 34);
            this.txt_paper_height.Name = "txt_paper_height";
            this.txt_paper_height.Size = new System.Drawing.Size(63, 21);
            this.txt_paper_height.TabIndex = 3;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.radioButton2);
            this.groupBox10.Controls.Add(this.radioButton1);
            this.groupBox10.Location = new System.Drawing.Point(3, 234);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(298, 69);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "方向";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.Text = "横向";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.Text = "纵向";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // tabPageLayout
            // 
            this.tabPageLayout.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPageLayout.Controls.Add(this.groupBox4);
            this.tabPageLayout.Controls.Add(this.groupBox3);
            this.tabPageLayout.Controls.Add(this.groupBox2);
            this.tabPageLayout.Controls.Add(this.groupBox1);
            this.tabPageLayout.Location = new System.Drawing.Point(4, 22);
            this.tabPageLayout.Name = "tabPageLayout";
            this.tabPageLayout.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLayout.Size = new System.Drawing.Size(310, 393);
            this.tabPageLayout.TabIndex = 1;
            this.tabPageLayout.Text = "布局";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkCustomInterval);
            this.groupBox4.Controls.Add(this.txtHorizontalInterval);
            this.groupBox4.Controls.Add(this.txtVerticalInterval);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Location = new System.Drawing.Point(6, 284);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(291, 100);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "间距";
            // 
            // chkCustomInterval
            // 
            this.chkCustomInterval.AutoSize = true;
            this.chkCustomInterval.Checked = true;
            this.chkCustomInterval.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCustomInterval.Location = new System.Drawing.Point(193, 45);
            this.chkCustomInterval.Name = "chkCustomInterval";
            this.chkCustomInterval.Size = new System.Drawing.Size(72, 16);
            this.chkCustomInterval.TabIndex = 18;
            this.chkCustomInterval.Text = "手动设置";
            this.chkCustomInterval.UseVisualStyleBackColor = true;
            this.chkCustomInterval.CheckedChanged += new System.EventHandler(this.chkCustomInterval_CheckedChanged);
            // 
            // txtHorizontalInterval
            // 
            this.txtHorizontalInterval.Location = new System.Drawing.Point(56, 23);
            this.txtHorizontalInterval.Name = "txtHorizontalInterval";
            this.txtHorizontalInterval.Size = new System.Drawing.Size(76, 21);
            this.txtHorizontalInterval.TabIndex = 14;
            this.txtHorizontalInterval.TextChanged += new System.EventHandler(this.txtHorizontalInterval_TextChanged);
            // 
            // txtVerticalInterval
            // 
            this.txtVerticalInterval.Location = new System.Drawing.Point(56, 60);
            this.txtVerticalInterval.Name = "txtVerticalInterval";
            this.txtVerticalInterval.Size = new System.Drawing.Size(76, 21);
            this.txtVerticalInterval.TabIndex = 17;
            this.txtVerticalInterval.TextChanged += new System.EventHandler(this.txtVerticalInterval_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "垂直";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 16;
            this.label13.Text = "水平";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkCustomModelSize);
            this.groupBox3.Controls.Add(this.txtModelWidth);
            this.groupBox3.Controls.Add(this.txtModelHeight);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(6, 178);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(291, 100);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "模板大小";
            // 
            // chkCustomModelSize
            // 
            this.chkCustomModelSize.AutoSize = true;
            this.chkCustomModelSize.Location = new System.Drawing.Point(193, 43);
            this.chkCustomModelSize.Name = "chkCustomModelSize";
            this.chkCustomModelSize.Size = new System.Drawing.Size(72, 16);
            this.chkCustomModelSize.TabIndex = 18;
            this.chkCustomModelSize.Text = "手动设置";
            this.chkCustomModelSize.UseVisualStyleBackColor = true;
            this.chkCustomModelSize.CheckedChanged += new System.EventHandler(this.chkCustomModelSize_CheckedChanged);
            // 
            // txtModelWidth
            // 
            this.txtModelWidth.Enabled = false;
            this.txtModelWidth.Location = new System.Drawing.Point(53, 23);
            this.txtModelWidth.Name = "txtModelWidth";
            this.txtModelWidth.Size = new System.Drawing.Size(76, 21);
            this.txtModelWidth.TabIndex = 14;
            this.txtModelWidth.TextChanged += new System.EventHandler(this.txtModelWidth_TextChanged);
            // 
            // txtModelHeight
            // 
            this.txtModelHeight.Enabled = false;
            this.txtModelHeight.Location = new System.Drawing.Point(53, 60);
            this.txtModelHeight.Name = "txtModelHeight";
            this.txtModelHeight.Size = new System.Drawing.Size(76, 21);
            this.txtModelHeight.TabIndex = 17;
            this.txtModelHeight.TextChanged += new System.EventHandler(this.txtModelHeight_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "高度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "宽度";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMaginsTop);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtMaginsLeft);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtMaginsRight);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtMaginsBottom);
            this.groupBox2.Location = new System.Drawing.Point(3, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 96);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "边距";
            // 
            // txtMaginsTop
            // 
            this.txtMaginsTop.Location = new System.Drawing.Point(56, 20);
            this.txtMaginsTop.Name = "txtMaginsTop";
            this.txtMaginsTop.Size = new System.Drawing.Size(76, 21);
            this.txtMaginsTop.TabIndex = 6;
            this.txtMaginsTop.TextChanged += new System.EventHandler(this.txtMaginsTop_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(149, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "右";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 8;
            this.label9.Text = "下";
            // 
            // txtMaginsLeft
            // 
            this.txtMaginsLeft.Location = new System.Drawing.Point(196, 20);
            this.txtMaginsLeft.Name = "txtMaginsLeft";
            this.txtMaginsLeft.Size = new System.Drawing.Size(76, 21);
            this.txtMaginsLeft.TabIndex = 13;
            this.txtMaginsLeft.TextChanged += new System.EventHandler(this.txtMaginsLeft_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(149, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 9;
            this.label10.Text = "左";
            // 
            // txtMaginsRight
            // 
            this.txtMaginsRight.Location = new System.Drawing.Point(196, 57);
            this.txtMaginsRight.Name = "txtMaginsRight";
            this.txtMaginsRight.Size = new System.Drawing.Size(76, 21);
            this.txtMaginsRight.TabIndex = 12;
            this.txtMaginsRight.TextChanged += new System.EventHandler(this.txtMaginsRight_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 10;
            this.label11.Text = "上";
            // 
            // txtMaginsBottom
            // 
            this.txtMaginsBottom.Location = new System.Drawing.Point(56, 57);
            this.txtMaginsBottom.Name = "txtMaginsBottom";
            this.txtMaginsBottom.Size = new System.Drawing.Size(76, 21);
            this.txtMaginsBottom.TabIndex = 11;
            this.txtMaginsBottom.TextChanged += new System.EventHandler(this.txtMaginsBottom_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UpDownNumberOfColumn);
            this.groupBox1.Controls.Add(this.UpDownNumberOfLine);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 64);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "布局";
            // 
            // UpDownNumberOfColumn
            // 
            this.UpDownNumberOfColumn.Location = new System.Drawing.Point(196, 20);
            this.UpDownNumberOfColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownNumberOfColumn.Name = "UpDownNumberOfColumn";
            this.UpDownNumberOfColumn.Size = new System.Drawing.Size(76, 21);
            this.UpDownNumberOfColumn.TabIndex = 6;
            this.UpDownNumberOfColumn.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.UpDownNumberOfColumn.ValueChanged += new System.EventHandler(this.UpDownNumberOfColumn_ValueChanged);
            // 
            // UpDownNumberOfLine
            // 
            this.UpDownNumberOfLine.Location = new System.Drawing.Point(56, 21);
            this.UpDownNumberOfLine.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownNumberOfLine.Name = "UpDownNumberOfLine";
            this.UpDownNumberOfLine.Size = new System.Drawing.Size(76, 21);
            this.UpDownNumberOfLine.TabIndex = 5;
            this.UpDownNumberOfLine.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.UpDownNumberOfLine.ValueChanged += new System.EventHandler(this.UpDownNumberOfLine_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "行数：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(149, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "列数：";
            // 
            // tabPageShapes
            // 
            this.tabPageShapes.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPageShapes.Controls.Add(this.groupBoxRect);
            this.tabPageShapes.Controls.Add(this.groupBox5);
            this.tabPageShapes.Location = new System.Drawing.Point(4, 22);
            this.tabPageShapes.Name = "tabPageShapes";
            this.tabPageShapes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageShapes.Size = new System.Drawing.Size(310, 393);
            this.tabPageShapes.TabIndex = 2;
            this.tabPageShapes.Text = "形状";
            // 
            // groupBoxRect
            // 
            this.groupBoxRect.Controls.Add(this.chkEllipseHole);
            this.groupBoxRect.Controls.Add(this.txtEllipseHoleRadius);
            this.groupBoxRect.Controls.Add(this.lblEllipseHoleRadius);
            this.groupBoxRect.Controls.Add(this.txtRoundRadius);
            this.groupBoxRect.Controls.Add(this.lblRoundRadius);
            this.groupBoxRect.Location = new System.Drawing.Point(6, 119);
            this.groupBoxRect.Name = "groupBoxRect";
            this.groupBoxRect.Size = new System.Drawing.Size(298, 139);
            this.groupBoxRect.TabIndex = 1;
            this.groupBoxRect.TabStop = false;
            this.groupBoxRect.Text = "选项";
            // 
            // chkEllipseHole
            // 
            this.chkEllipseHole.AutoSize = true;
            this.chkEllipseHole.Location = new System.Drawing.Point(15, 73);
            this.chkEllipseHole.Name = "chkEllipseHole";
            this.chkEllipseHole.Size = new System.Drawing.Size(36, 16);
            this.chkEllipseHole.TabIndex = 7;
            this.chkEllipseHole.Text = "孔";
            this.chkEllipseHole.UseVisualStyleBackColor = true;
            this.chkEllipseHole.CheckedChanged += new System.EventHandler(this.chkEllipseHole_CheckedChanged);
            // 
            // txtEllipseHoleRadius
            // 
            this.txtEllipseHoleRadius.Location = new System.Drawing.Point(72, 95);
            this.txtEllipseHoleRadius.Name = "txtEllipseHoleRadius";
            this.txtEllipseHoleRadius.Size = new System.Drawing.Size(100, 21);
            this.txtEllipseHoleRadius.TabIndex = 5;
            // 
            // lblEllipseHoleRadius
            // 
            this.lblEllipseHoleRadius.AutoSize = true;
            this.lblEllipseHoleRadius.Location = new System.Drawing.Point(34, 98);
            this.lblEllipseHoleRadius.Name = "lblEllipseHoleRadius";
            this.lblEllipseHoleRadius.Size = new System.Drawing.Size(29, 12);
            this.lblEllipseHoleRadius.TabIndex = 4;
            this.lblEllipseHoleRadius.Text = "孔径";
            // 
            // txtRoundRadius
            // 
            this.txtRoundRadius.Location = new System.Drawing.Point(72, 42);
            this.txtRoundRadius.Name = "txtRoundRadius";
            this.txtRoundRadius.Size = new System.Drawing.Size(100, 21);
            this.txtRoundRadius.TabIndex = 2;
            // 
            // lblRoundRadius
            // 
            this.lblRoundRadius.AutoSize = true;
            this.lblRoundRadius.Location = new System.Drawing.Point(13, 48);
            this.lblRoundRadius.Name = "lblRoundRadius";
            this.lblRoundRadius.Size = new System.Drawing.Size(53, 12);
            this.lblRoundRadius.TabIndex = 1;
            this.lblRoundRadius.Text = "圆角半径";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.radioButtonRoundRect);
            this.groupBox5.Controls.Add(this.radioButtonEllipse);
            this.groupBox5.Controls.Add(this.radioButtonRect);
            this.groupBox5.Location = new System.Drawing.Point(6, 18);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(298, 95);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "模板形状";
            // 
            // radioButtonRoundRect
            // 
            this.radioButtonRoundRect.AutoSize = true;
            this.radioButtonRoundRect.Checked = true;
            this.radioButtonRoundRect.Location = new System.Drawing.Point(15, 44);
            this.radioButtonRoundRect.Name = "radioButtonRoundRect";
            this.radioButtonRoundRect.Size = new System.Drawing.Size(71, 16);
            this.radioButtonRoundRect.TabIndex = 2;
            this.radioButtonRoundRect.TabStop = true;
            this.radioButtonRoundRect.Text = "圆角矩形";
            this.radioButtonRoundRect.UseVisualStyleBackColor = true;
            this.radioButtonRoundRect.CheckedChanged += new System.EventHandler(this.radioButtonRoundRect_CheckedChanged);
            // 
            // radioButtonEllipse
            // 
            this.radioButtonEllipse.AutoSize = true;
            this.radioButtonEllipse.Location = new System.Drawing.Point(15, 66);
            this.radioButtonEllipse.Name = "radioButtonEllipse";
            this.radioButtonEllipse.Size = new System.Drawing.Size(47, 16);
            this.radioButtonEllipse.TabIndex = 1;
            this.radioButtonEllipse.Text = "椭圆";
            this.radioButtonEllipse.UseVisualStyleBackColor = true;
            this.radioButtonEllipse.CheckedChanged += new System.EventHandler(this.radioButtonEllipse_CheckedChanged);
            // 
            // radioButtonRect
            // 
            this.radioButtonRect.AutoSize = true;
            this.radioButtonRect.Location = new System.Drawing.Point(15, 20);
            this.radioButtonRect.Name = "radioButtonRect";
            this.radioButtonRect.Size = new System.Drawing.Size(47, 16);
            this.radioButtonRect.TabIndex = 0;
            this.radioButtonRect.Text = "方框";
            this.radioButtonRect.UseVisualStyleBackColor = true;
            this.radioButtonRect.CheckedChanged += new System.EventHandler(this.radioButtonRect_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(336, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 335);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(336, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "预览";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // lblPaperSize
            // 
            this.lblPaperSize.AutoSize = true;
            this.lblPaperSize.Location = new System.Drawing.Point(371, 389);
            this.lblPaperSize.Name = "lblPaperSize";
            this.lblPaperSize.Size = new System.Drawing.Size(65, 12);
            this.lblPaperSize.TabIndex = 6;
            this.lblPaperSize.Text = "纸张尺寸：";
            // 
            // lblModelSize
            // 
            this.lblModelSize.AutoSize = true;
            this.lblModelSize.Location = new System.Drawing.Point(371, 415);
            this.lblModelSize.Name = "lblModelSize";
            this.lblModelSize.Size = new System.Drawing.Size(65, 12);
            this.lblModelSize.TabIndex = 7;
            this.lblModelSize.Text = "模板大小：";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(113, 445);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(467, 445);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(155, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 4;
            this.label14.Text = "mm";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(155, 40);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 5;
            this.label15.Text = "mm";
            // 
            // FrmPageSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(684, 480);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblModelSize);
            this.Controls.Add(this.lblPaperSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FrmPageSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "页面设置";
            this.Load += new System.EventHandler(this.FrmPageSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPagePaper.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.tabPageLayout.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownNumberOfLine)).EndInit();
            this.tabPageShapes.ResumeLayout(false);
            this.groupBoxRect.ResumeLayout(false);
            this.groupBoxRect.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPagePaper;
        private System.Windows.Forms.TabPage tabPageLayout;
        private System.Windows.Forms.TabPage tabPageShapes;
        private System.Windows.Forms.ComboBox comboPaperSizes;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtMaginsLeft;
        private System.Windows.Forms.TextBox txtMaginsRight;
        private System.Windows.Forms.TextBox txtMaginsBottom;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMaginsTop;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkCustomInterval;
        private System.Windows.Forms.TextBox txtHorizontalInterval;
        private System.Windows.Forms.TextBox txtVerticalInterval;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkCustomModelSize;
        private System.Windows.Forms.TextBox txtModelWidth;
        private System.Windows.Forms.TextBox txtModelHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButtonRoundRect;
        private System.Windows.Forms.RadioButton radioButtonEllipse;
        private System.Windows.Forms.RadioButton radioButtonRect;
        private System.Windows.Forms.GroupBox groupBoxRect;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.NumericUpDown UpDownNumberOfColumn;
        private System.Windows.Forms.NumericUpDown UpDownNumberOfLine;
        private System.Windows.Forms.TextBox txtRoundRadius;
        private System.Windows.Forms.Label lblRoundRadius;
        private System.Windows.Forms.Label lblEllipseHoleRadius;
        private System.Windows.Forms.TextBox txtEllipseHoleRadius;
        private System.Windows.Forms.CheckBox chkEllipseHole;
        private System.Windows.Forms.Label lblPaperSize;
        private System.Windows.Forms.Label lblModelSize;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox comboPrinters;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_paper_ok;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_paper_width;
        private System.Windows.Forms.TextBox txt_paper_height;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
    }
}