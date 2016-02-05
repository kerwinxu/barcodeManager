namespace BarcodeTerminator
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblzhuCe = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnZhuCe = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.btnUpdateModel = new System.Windows.Forms.Button();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.chkIsFull = new System.Windows.Forms.CheckBox();
            this.chkIsMulti = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAutoPrint = new System.Windows.Forms.Button();
            this.chkSunHao = new System.Windows.Forms.CheckBox();
            this.btnPrint2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSunHao = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnQueuePrint = new System.Windows.Forms.Button();
            this.chkJianGe = new System.Windows.Forms.CheckBox();
            this.lblQtyWaitPrint = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadzPreviously = new System.Windows.Forms.Button();
            this.txtAlreadyPrinted = new System.Windows.Forms.TextBox();
            this.comboBoxQtyOfWantToPrinted = new System.Windows.Forms.ComboBox();
            this.txtQtyOfWantToPrinted = new System.Windows.Forms.TextBox();
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.btnSelectPrinter = new System.Windows.Forms.Button();
            this.txtCurrentPrintPage = new System.Windows.Forms.TextBox();
            this.btnLoadExcel = new System.Windows.Forms.Button();
            this.btnLoadBarcodeModel = new System.Windows.Forms.Button();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnTestPrint = new System.Windows.Forms.Button();
            this.btnNewBarcodeModel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEditBarcodeModel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxBarcodeModel = new System.Windows.Forms.ComboBox();
            this.dataGridViewPrintedRecords = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTipAutoPrint = new System.Windows.Forms.ToolTip(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.userControlCanvas1 = new VestShapes.UserControlCanvas();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrintedRecords)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblzhuCe);
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel1);
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btnZhuCe);
            this.splitContainer1.Panel1.Controls.Add(this.btnHelp);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(984, 662);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblzhuCe
            // 
            this.lblzhuCe.AutoSize = true;
            this.lblzhuCe.Location = new System.Drawing.Point(9, 636);
            this.lblzhuCe.Name = "lblzhuCe";
            this.lblzhuCe.Size = new System.Drawing.Size(41, 12);
            this.lblzhuCe.TabIndex = 31;
            this.lblzhuCe.Text = "未注册";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(3, 503);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(48, 43);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "kerwin.cn@gmail.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(11, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(40, 487);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "如有企业需要开发软件，请联系我。\r\n";
            // 
            // btnZhuCe
            // 
            this.btnZhuCe.Location = new System.Drawing.Point(5, 569);
            this.btnZhuCe.Name = "btnZhuCe";
            this.btnZhuCe.Size = new System.Drawing.Size(51, 23);
            this.btnZhuCe.TabIndex = 1;
            this.btnZhuCe.Text = "注册";
            this.btnZhuCe.UseVisualStyleBackColor = true;
            this.btnZhuCe.Click += new System.EventHandler(this.btnZhuCe_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(5, 603);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(51, 23);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(918, 662);
            this.splitContainer2.SplitterDistance = 427;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(916, 425);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.userControlCanvas1);
            this.splitContainer3.Panel1.Resize += new System.EventHandler(this.splitContainer3_Panel1_Resize);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(918, 231);
            this.splitContainer3.SplitterDistance = 339;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.btnUpdateModel);
            this.splitContainer4.Panel1.Controls.Add(this.btnSaveImage);
            this.splitContainer4.Panel1.Controls.Add(this.chkIsFull);
            this.splitContainer4.Panel1.Controls.Add(this.chkIsMulti);
            this.splitContainer4.Panel1.Controls.Add(this.label7);
            this.splitContainer4.Panel1.Controls.Add(this.btnAutoPrint);
            this.splitContainer4.Panel1.Controls.Add(this.chkSunHao);
            this.splitContainer4.Panel1.Controls.Add(this.btnPrint2);
            this.splitContainer4.Panel1.Controls.Add(this.label6);
            this.splitContainer4.Panel1.Controls.Add(this.txtSunHao);
            this.splitContainer4.Panel1.Controls.Add(this.label5);
            this.splitContainer4.Panel1.Controls.Add(this.btnQueuePrint);
            this.splitContainer4.Panel1.Controls.Add(this.chkJianGe);
            this.splitContainer4.Panel1.Controls.Add(this.lblQtyWaitPrint);
            this.splitContainer4.Panel1.Controls.Add(this.label1);
            this.splitContainer4.Panel1.Controls.Add(this.btnLoadzPreviously);
            this.splitContainer4.Panel1.Controls.Add(this.txtAlreadyPrinted);
            this.splitContainer4.Panel1.Controls.Add(this.comboBoxQtyOfWantToPrinted);
            this.splitContainer4.Panel1.Controls.Add(this.txtQtyOfWantToPrinted);
            this.splitContainer4.Panel1.Controls.Add(this.lblPrinterName);
            this.splitContainer4.Panel1.Controls.Add(this.btnSelectPrinter);
            this.splitContainer4.Panel1.Controls.Add(this.txtCurrentPrintPage);
            this.splitContainer4.Panel1.Controls.Add(this.btnLoadExcel);
            this.splitContainer4.Panel1.Controls.Add(this.btnLoadBarcodeModel);
            this.splitContainer4.Panel1.Controls.Add(this.txtInterval);
            this.splitContainer4.Panel1.Controls.Add(this.btnPrint);
            this.splitContainer4.Panel1.Controls.Add(this.btnTestPrint);
            this.splitContainer4.Panel1.Controls.Add(this.btnNewBarcodeModel);
            this.splitContainer4.Panel1.Controls.Add(this.label4);
            this.splitContainer4.Panel1.Controls.Add(this.label2);
            this.splitContainer4.Panel1.Controls.Add(this.btnEditBarcodeModel);
            this.splitContainer4.Panel1.Controls.Add(this.label3);
            this.splitContainer4.Panel1.Controls.Add(this.comboBoxBarcodeModel);
            this.splitContainer4.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer4_Panel1_Paint);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.dataGridViewPrintedRecords);
            this.splitContainer4.Size = new System.Drawing.Size(575, 231);
            this.splitContainer4.SplitterDistance = 369;
            this.splitContainer4.TabIndex = 0;
            // 
            // btnUpdateModel
            // 
            this.btnUpdateModel.Location = new System.Drawing.Point(3, 3);
            this.btnUpdateModel.Name = "btnUpdateModel";
            this.btnUpdateModel.Size = new System.Drawing.Size(63, 23);
            this.btnUpdateModel.TabIndex = 42;
            this.btnUpdateModel.Text = "模板更改";
            this.toolTipAutoPrint.SetToolTip(this.btnUpdateModel, "当左边的模板有更改后，只有点击这个才会更新的。左边的预览区可以做简单的更改，可以用键盘中的上下左右移动");
            this.btnUpdateModel.UseVisualStyleBackColor = true;
            this.btnUpdateModel.Click += new System.EventHandler(this.btnUpdateModel_Click);
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(281, 200);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(75, 23);
            this.btnSaveImage.TabIndex = 41;
            this.btnSaveImage.Text = "导出图像";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // chkIsFull
            // 
            this.chkIsFull.AutoSize = true;
            this.chkIsFull.Location = new System.Drawing.Point(281, 174);
            this.chkIsFull.Name = "chkIsFull";
            this.chkIsFull.Size = new System.Drawing.Size(72, 16);
            this.chkIsFull.TabIndex = 40;
            this.chkIsFull.Text = "充满打印";
            this.toolTipAutoPrint.SetToolTip(this.chkIsFull, "充满打印指的是每一个条形码都充满纸张");
            this.chkIsFull.UseVisualStyleBackColor = true;
            // 
            // chkIsMulti
            // 
            this.chkIsMulti.AutoSize = true;
            this.chkIsMulti.Location = new System.Drawing.Point(168, 7);
            this.chkIsMulti.Name = "chkIsMulti";
            this.chkIsMulti.Size = new System.Drawing.Size(72, 16);
            this.chkIsMulti.TabIndex = 39;
            this.chkIsMulti.Text = "选择多行";
            this.toolTipAutoPrint.SetToolTip(this.chkIsMulti, "选中这个，可以用ctrl和shift多选");
            this.chkIsMulti.UseVisualStyleBackColor = true;
            this.chkIsMulti.UseWaitCursor = true;
            this.chkIsMulti.CheckedChanged += new System.EventHandler(this.chkIsMulti_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(196, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 19);
            this.label7.TabIndex = 38;
            this.label7.Text = "加";
            // 
            // btnAutoPrint
            // 
            this.btnAutoPrint.Location = new System.Drawing.Point(281, 138);
            this.btnAutoPrint.Name = "btnAutoPrint";
            this.btnAutoPrint.Size = new System.Drawing.Size(75, 23);
            this.btnAutoPrint.TabIndex = 37;
            this.btnAutoPrint.Text = "自动打印";
            this.toolTipAutoPrint.SetToolTip(this.btnAutoPrint, "会自动打印当前行以及下面的行");
            this.btnAutoPrint.UseVisualStyleBackColor = true;
            this.btnAutoPrint.Click += new System.EventHandler(this.btnAutoPrint_Click);
            // 
            // chkSunHao
            // 
            this.chkSunHao.AutoSize = true;
            this.chkSunHao.Location = new System.Drawing.Point(143, 145);
            this.chkSunHao.Name = "chkSunHao";
            this.chkSunHao.Size = new System.Drawing.Size(120, 16);
            this.chkSunHao.TabIndex = 36;
            this.chkSunHao.Text = "自动打印时加损耗";
            this.chkSunHao.UseVisualStyleBackColor = true;
            // 
            // btnPrint2
            // 
            this.btnPrint2.Location = new System.Drawing.Point(281, 63);
            this.btnPrint2.Name = "btnPrint2";
            this.btnPrint2.Size = new System.Drawing.Size(75, 23);
            this.btnPrint2.TabIndex = 35;
            this.btnPrint2.Text = "按损耗打印";
            this.toolTipAutoPrint.SetToolTip(this.btnPrint2, "因为工人贴标签是有损耗的，这里可以加上这个损耗。");
            this.btnPrint2.UseVisualStyleBackColor = true;
            this.btnPrint2.Click += new System.EventHandler(this.btnPrint2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(260, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 19);
            this.label6.TabIndex = 34;
            this.label6.Text = "%";
            // 
            // txtSunHao
            // 
            this.txtSunHao.Location = new System.Drawing.Point(227, 65);
            this.txtSunHao.Name = "txtSunHao";
            this.txtSunHao.Size = new System.Drawing.Size(27, 21);
            this.txtSunHao.TabIndex = 33;
            this.txtSunHao.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "条形码模板";
            // 
            // btnQueuePrint
            // 
            this.btnQueuePrint.Location = new System.Drawing.Point(200, 200);
            this.btnQueuePrint.Name = "btnQueuePrint";
            this.btnQueuePrint.Size = new System.Drawing.Size(75, 23);
            this.btnQueuePrint.TabIndex = 30;
            this.btnQueuePrint.Text = "打印队列";
            this.btnQueuePrint.UseVisualStyleBackColor = true;
            this.btnQueuePrint.Click += new System.EventHandler(this.btnQueuePrint_Click);
            // 
            // chkJianGe
            // 
            this.chkJianGe.AutoSize = true;
            this.chkJianGe.Location = new System.Drawing.Point(10, 174);
            this.chkJianGe.Name = "chkJianGe";
            this.chkJianGe.Size = new System.Drawing.Size(180, 16);
            this.chkJianGe.TabIndex = 29;
            this.chkJianGe.Text = "是否在不同条形码间打印间隔";
            this.chkJianGe.UseVisualStyleBackColor = true;
            this.chkJianGe.CheckedChanged += new System.EventHandler(this.chkJianGe_CheckedChanged);
            // 
            // lblQtyWaitPrint
            // 
            this.lblQtyWaitPrint.AutoSize = true;
            this.lblQtyWaitPrint.Location = new System.Drawing.Point(108, 146);
            this.lblQtyWaitPrint.Name = "lblQtyWaitPrint";
            this.lblQtyWaitPrint.Size = new System.Drawing.Size(11, 12);
            this.lblQtyWaitPrint.TabIndex = 28;
            this.lblQtyWaitPrint.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "等待排队的打印：";
            // 
            // btnLoadzPreviously
            // 
            this.btnLoadzPreviously.Location = new System.Drawing.Point(246, 3);
            this.btnLoadzPreviously.Name = "btnLoadzPreviously";
            this.btnLoadzPreviously.Size = new System.Drawing.Size(110, 23);
            this.btnLoadzPreviously.TabIndex = 26;
            this.btnLoadzPreviously.Text = "选择以前导入的";
            this.toolTipAutoPrint.SetToolTip(this.btnLoadzPreviously, "就是以前导入的excel文件，已经保存在数据库中了");
            this.btnLoadzPreviously.UseVisualStyleBackColor = true;
            this.btnLoadzPreviously.Click += new System.EventHandler(this.btnLoadzPreviously_Click);
            // 
            // txtAlreadyPrinted
            // 
            this.txtAlreadyPrinted.Location = new System.Drawing.Point(144, 90);
            this.txtAlreadyPrinted.Name = "txtAlreadyPrinted";
            this.txtAlreadyPrinted.Size = new System.Drawing.Size(46, 21);
            this.txtAlreadyPrinted.TabIndex = 24;
            // 
            // comboBoxQtyOfWantToPrinted
            // 
            this.comboBoxQtyOfWantToPrinted.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQtyOfWantToPrinted.FormattingEnabled = true;
            this.comboBoxQtyOfWantToPrinted.Location = new System.Drawing.Point(66, 65);
            this.comboBoxQtyOfWantToPrinted.Name = "comboBoxQtyOfWantToPrinted";
            this.comboBoxQtyOfWantToPrinted.Size = new System.Drawing.Size(72, 20);
            this.comboBoxQtyOfWantToPrinted.TabIndex = 22;
            this.comboBoxQtyOfWantToPrinted.SelectedIndexChanged += new System.EventHandler(this.comboBoxVaribaleName_SelectedIndexChanged);
            // 
            // txtQtyOfWantToPrinted
            // 
            this.txtQtyOfWantToPrinted.Location = new System.Drawing.Point(144, 64);
            this.txtQtyOfWantToPrinted.Name = "txtQtyOfWantToPrinted";
            this.txtQtyOfWantToPrinted.Size = new System.Drawing.Size(46, 21);
            this.txtQtyOfWantToPrinted.TabIndex = 23;
            // 
            // lblPrinterName
            // 
            this.lblPrinterName.AutoSize = true;
            this.lblPrinterName.Location = new System.Drawing.Point(84, 207);
            this.lblPrinterName.Name = "lblPrinterName";
            this.lblPrinterName.Size = new System.Drawing.Size(65, 12);
            this.lblPrinterName.TabIndex = 21;
            this.lblPrinterName.Text = "默认打印机";
            // 
            // btnSelectPrinter
            // 
            this.btnSelectPrinter.Location = new System.Drawing.Point(3, 196);
            this.btnSelectPrinter.Name = "btnSelectPrinter";
            this.btnSelectPrinter.Size = new System.Drawing.Size(75, 23);
            this.btnSelectPrinter.TabIndex = 20;
            this.btnSelectPrinter.Text = "选择打印机";
            this.btnSelectPrinter.UseVisualStyleBackColor = true;
            this.btnSelectPrinter.Click += new System.EventHandler(this.btnSelectPrinter_Click);
            // 
            // txtCurrentPrintPage
            // 
            this.txtCurrentPrintPage.Location = new System.Drawing.Point(144, 114);
            this.txtCurrentPrintPage.Name = "txtCurrentPrintPage";
            this.txtCurrentPrintPage.Size = new System.Drawing.Size(46, 21);
            this.txtCurrentPrintPage.TabIndex = 18;
            // 
            // btnLoadExcel
            // 
            this.btnLoadExcel.Location = new System.Drawing.Point(71, 3);
            this.btnLoadExcel.Name = "btnLoadExcel";
            this.btnLoadExcel.Size = new System.Drawing.Size(91, 23);
            this.btnLoadExcel.TabIndex = 16;
            this.btnLoadExcel.Text = "导入Excel表格";
            this.toolTipAutoPrint.SetToolTip(this.btnLoadExcel, "可以导入EXCEL97-2003和EXCEL2007两种格式");
            this.btnLoadExcel.UseVisualStyleBackColor = true;
            this.btnLoadExcel.Click += new System.EventHandler(this.btnLoadExcel_Click);
            // 
            // btnLoadBarcodeModel
            // 
            this.btnLoadBarcodeModel.Location = new System.Drawing.Point(66, 32);
            this.btnLoadBarcodeModel.Name = "btnLoadBarcodeModel";
            this.btnLoadBarcodeModel.Size = new System.Drawing.Size(47, 23);
            this.btnLoadBarcodeModel.TabIndex = 15;
            this.btnLoadBarcodeModel.Text = "载入";
            this.toolTipAutoPrint.SetToolTip(this.btnLoadBarcodeModel, "载入模板");
            this.btnLoadBarcodeModel.UseVisualStyleBackColor = true;
            this.btnLoadBarcodeModel.Click += new System.EventHandler(this.btnLoadBarcodeModel_Click);
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(200, 174);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(75, 21);
            this.txtInterval.TabIndex = 12;
            this.txtInterval.Text = "间隔";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(281, 112);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 11;
            this.btnPrint.Text = "打印";
            this.toolTipAutoPrint.SetToolTip(this.btnPrint, "根据左边的“手动输入打印数量”打印");
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnTestPrint
            // 
            this.btnTestPrint.Location = new System.Drawing.Point(281, 88);
            this.btnTestPrint.Name = "btnTestPrint";
            this.btnTestPrint.Size = new System.Drawing.Size(75, 23);
            this.btnTestPrint.TabIndex = 10;
            this.btnTestPrint.Text = "测试打印";
            this.toolTipAutoPrint.SetToolTip(this.btnTestPrint, "只会打印一个");
            this.btnTestPrint.UseVisualStyleBackColor = true;
            this.btnTestPrint.Click += new System.EventHandler(this.btnTestPrint_Click);
            // 
            // btnNewBarcodeModel
            // 
            this.btnNewBarcodeModel.Location = new System.Drawing.Point(318, 32);
            this.btnNewBarcodeModel.Name = "btnNewBarcodeModel";
            this.btnNewBarcodeModel.Size = new System.Drawing.Size(40, 23);
            this.btnNewBarcodeModel.TabIndex = 0;
            this.btnNewBarcodeModel.Text = "新建";
            this.toolTipAutoPrint.SetToolTip(this.btnNewBarcodeModel, "新建模板");
            this.btnNewBarcodeModel.UseVisualStyleBackColor = true;
            this.btnNewBarcodeModel.Click += new System.EventHandler(this.btnNewBarcodeModel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "手动输入打印数量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "要打印数量";
            // 
            // btnEditBarcodeModel
            // 
            this.btnEditBarcodeModel.Location = new System.Drawing.Point(269, 31);
            this.btnEditBarcodeModel.Name = "btnEditBarcodeModel";
            this.btnEditBarcodeModel.Size = new System.Drawing.Size(43, 23);
            this.btnEditBarcodeModel.TabIndex = 3;
            this.btnEditBarcodeModel.Text = "编辑";
            this.toolTipAutoPrint.SetToolTip(this.btnEditBarcodeModel, "编辑模板");
            this.btnEditBarcodeModel.UseVisualStyleBackColor = true;
            this.btnEditBarcodeModel.Click += new System.EventHandler(this.btnEditBarcodeModel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "已打印数量";
            // 
            // comboBoxBarcodeModel
            // 
            this.comboBoxBarcodeModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBarcodeModel.FormattingEnabled = true;
            this.comboBoxBarcodeModel.Location = new System.Drawing.Point(119, 34);
            this.comboBoxBarcodeModel.Name = "comboBoxBarcodeModel";
            this.comboBoxBarcodeModel.Size = new System.Drawing.Size(144, 20);
            this.comboBoxBarcodeModel.TabIndex = 2;
            this.comboBoxBarcodeModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxBarcodeModel_SelectedIndexChanged);
            // 
            // dataGridViewPrintedRecords
            // 
            this.dataGridViewPrintedRecords.AllowUserToAddRows = false;
            this.dataGridViewPrintedRecords.AllowUserToDeleteRows = false;
            this.dataGridViewPrintedRecords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPrintedRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPrintedRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPrintedRecords.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPrintedRecords.Name = "dataGridViewPrintedRecords";
            this.dataGridViewPrintedRecords.ReadOnly = true;
            this.dataGridViewPrintedRecords.RowTemplate.Height = 23;
            this.dataGridViewPrintedRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPrintedRecords.Size = new System.Drawing.Size(200, 229);
            this.dataGridViewPrintedRecords.TabIndex = 0;
            this.dataGridViewPrintedRecords.DataSourceChanged += new System.EventHandler(this.dataGridViewPrintedRecords_DataSourceChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 2000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // userControlCanvas1
            // 
            //this.userControlCanvas1.arrlistKeyValue = ((System.Collections.ArrayList)(resources.GetObject("userControlCanvas1.arrlistKeyValue")));
            this.userControlCanvas1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.userControlCanvas1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userControlCanvas1.Location = new System.Drawing.Point(5, 3);
            this.userControlCanvas1.Name = "userControlCanvas1";
            this.userControlCanvas1.Option = "drawRect";
            this.userControlCanvas1.Size = new System.Drawing.Size(333, 235);
            this.userControlCanvas1.TabIndex = 0;
            this.userControlCanvas1.Zoom = 1F;
            this.userControlCanvas1.Resize += new System.EventHandler(this.userControlCanvas1_Resize);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 662);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "条形码设计打印管理专家 http://www.xuhengxiao.com/";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrintedRecords)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Button btnNewBarcodeModel;
        private System.Windows.Forms.Button btnEditBarcodeModel;
        private System.Windows.Forms.ComboBox comboBoxBarcodeModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnTestPrint;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Button btnLoadBarcodeModel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnLoadExcel;
        private System.Windows.Forms.TextBox txtCurrentPrintPage;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnSelectPrinter;
        private System.Windows.Forms.Label lblPrinterName;
        private System.Windows.Forms.DataGridView dataGridViewPrintedRecords;
        private System.Windows.Forms.ComboBox comboBoxQtyOfWantToPrinted;
        private System.Windows.Forms.TextBox txtQtyOfWantToPrinted;
        private System.Windows.Forms.TextBox txtAlreadyPrinted;
        private System.Windows.Forms.Button btnLoadzPreviously;
        private System.Windows.Forms.Label lblQtyWaitPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnZhuCe;
        private System.Windows.Forms.ToolTip toolTipAutoPrint;
        private System.Windows.Forms.CheckBox chkJianGe;
        private System.Windows.Forms.Button btnQueuePrint;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblzhuCe;
        private System.Windows.Forms.Label label5;
        private VestShapes.UserControlCanvas userControlCanvas1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSunHao;
        private System.Windows.Forms.Button btnPrint2;
        private System.Windows.Forms.CheckBox chkSunHao;
        private System.Windows.Forms.Button btnAutoPrint;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkIsMulti;
        private System.Windows.Forms.CheckBox chkIsFull;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnUpdateModel;

    }
}

