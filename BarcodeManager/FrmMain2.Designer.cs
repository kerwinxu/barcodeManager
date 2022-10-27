
namespace BarcodeManager
{
    partial class FrmMain2
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.canvas = new Io.Github.Kerwinxu.LibShapes.Core.UserControlCanvas();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_load_excel = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.combo_printers = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_load_model = new System.Windows.Forms.Button();
            this.combo_models = new System.Windows.Forms.ComboBox();
            this.btn_edit_model = new System.Windows.Forms.Button();
            this.btn_new_model = new System.Windows.Forms.Button();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.combo_qty_var_names = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_print_qty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_loss_ratio = new System.Windows.Forms.TextBox();
            this.txt_print_qty2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_increase_print = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_qty2 = new System.Windows.Forms.TextBox();
            this.btn_print_2 = new System.Windows.Forms.Button();
            this.btn_all_print = new System.Windows.Forms.Button();
            this.chk_isFull = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.linkLabel1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(892, 479);
            this.splitContainer1.SplitterDistance = 52;
            this.splitContainer1.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("SimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel1.Location = new System.Drawing.Point(7, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(37, 432);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "软件定制请淘宝搜索\"鑫意雅\"";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("SimSun", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 408);
            this.label1.TabIndex = 0;
            // 
            // splitContainer2
            // 
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
            this.splitContainer2.Size = new System.Drawing.Size(836, 479);
            this.splitContainer2.SplitterDistance = 294;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(836, 294);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.canvas);
            this.splitContainer3.Panel1.SizeChanged += new System.EventHandler(this.splitContainer3_Panel1_SizeChanged);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer3.Panel2.SizeChanged += new System.EventHandler(this.splitContainer3_Panel2_SizeChanged);
            this.splitContainer3.Size = new System.Drawing.Size(836, 181);
            this.splitContainer3.SplitterDistance = 306;
            this.splitContainer3.TabIndex = 0;
            // 
            // canvas
            // 
            this.canvas.GriddingInterval = 2;
            this.canvas.isAlignDridding = false;
            this.canvas.isDrawDridding = false;
            this.canvas.IsEdit = false;
            this.canvas.isShift = false;
            this.canvas.Location = new System.Drawing.Point(3, 12);
            this.canvas.Name = "canvas";
            this.canvas.SelectShape = null;
            this.canvas.Size = new System.Drawing.Size(300, 150);
            this.canvas.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel5, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(511, 162);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btn_load_excel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(505, 26);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btn_load_excel
            // 
            this.btn_load_excel.Location = new System.Drawing.Point(3, 3);
            this.btn_load_excel.Name = "btn_load_excel";
            this.btn_load_excel.Size = new System.Drawing.Size(105, 23);
            this.btn_load_excel.TabIndex = 0;
            this.btn_load_excel.Text = "导入excel表格";
            this.btn_load_excel.UseVisualStyleBackColor = true;
            this.btn_load_excel.Click += new System.EventHandler(this.btn_load_excel_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label6);
            this.flowLayoutPanel2.Controls.Add(this.combo_printers);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 35);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(505, 26);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "选择打印机";
            // 
            // combo_printers
            // 
            this.combo_printers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_printers.FormattingEnabled = true;
            this.combo_printers.Location = new System.Drawing.Point(74, 3);
            this.combo_printers.Name = "combo_printers";
            this.combo_printers.Size = new System.Drawing.Size(331, 20);
            this.combo_printers.TabIndex = 3;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.btn_load_model);
            this.flowLayoutPanel3.Controls.Add(this.combo_models);
            this.flowLayoutPanel3.Controls.Add(this.btn_edit_model);
            this.flowLayoutPanel3.Controls.Add(this.btn_new_model);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 67);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(505, 26);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "条形码模板";
            // 
            // btn_load_model
            // 
            this.btn_load_model.Location = new System.Drawing.Point(74, 3);
            this.btn_load_model.Name = "btn_load_model";
            this.btn_load_model.Size = new System.Drawing.Size(53, 23);
            this.btn_load_model.TabIndex = 1;
            this.btn_load_model.Text = "导入";
            this.btn_load_model.UseVisualStyleBackColor = true;
            this.btn_load_model.Click += new System.EventHandler(this.btn_load_model_Click);
            // 
            // combo_models
            // 
            this.combo_models.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_models.FormattingEnabled = true;
            this.combo_models.Location = new System.Drawing.Point(133, 3);
            this.combo_models.Name = "combo_models";
            this.combo_models.Size = new System.Drawing.Size(154, 20);
            this.combo_models.TabIndex = 2;
            this.combo_models.SelectedIndexChanged += new System.EventHandler(this.combo_models_SelectedIndexChanged);
            // 
            // btn_edit_model
            // 
            this.btn_edit_model.Location = new System.Drawing.Point(293, 3);
            this.btn_edit_model.Name = "btn_edit_model";
            this.btn_edit_model.Size = new System.Drawing.Size(53, 23);
            this.btn_edit_model.TabIndex = 3;
            this.btn_edit_model.Text = "编辑";
            this.btn_edit_model.UseVisualStyleBackColor = true;
            this.btn_edit_model.Click += new System.EventHandler(this.btn_edit_model_Click);
            // 
            // btn_new_model
            // 
            this.btn_new_model.Location = new System.Drawing.Point(352, 3);
            this.btn_new_model.Name = "btn_new_model";
            this.btn_new_model.Size = new System.Drawing.Size(53, 23);
            this.btn_new_model.TabIndex = 4;
            this.btn_new_model.Text = "新建";
            this.btn_new_model.UseVisualStyleBackColor = true;
            this.btn_new_model.Click += new System.EventHandler(this.btn_new_model_Click);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.label3);
            this.flowLayoutPanel4.Controls.Add(this.combo_qty_var_names);
            this.flowLayoutPanel4.Controls.Add(this.label7);
            this.flowLayoutPanel4.Controls.Add(this.btn_print_qty);
            this.flowLayoutPanel4.Controls.Add(this.label4);
            this.flowLayoutPanel4.Controls.Add(this.txt_loss_ratio);
            this.flowLayoutPanel4.Controls.Add(this.txt_print_qty2);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 99);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(505, 26);
            this.flowLayoutPanel4.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "要打印数量";
            // 
            // combo_qty_var_names
            // 
            this.combo_qty_var_names.FormattingEnabled = true;
            this.combo_qty_var_names.Location = new System.Drawing.Point(74, 3);
            this.combo_qty_var_names.Name = "combo_qty_var_names";
            this.combo_qty_var_names.Size = new System.Drawing.Size(74, 20);
            this.combo_qty_var_names.TabIndex = 3;
            this.combo_qty_var_names.TextChanged += new System.EventHandler(this.combo_qty_var_names_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "打印数量";
            // 
            // btn_print_qty
            // 
            this.btn_print_qty.Location = new System.Drawing.Point(213, 3);
            this.btn_print_qty.Name = "btn_print_qty";
            this.btn_print_qty.Size = new System.Drawing.Size(42, 21);
            this.btn_print_qty.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "加损耗";
            // 
            // txt_loss_ratio
            // 
            this.txt_loss_ratio.Location = new System.Drawing.Point(308, 3);
            this.txt_loss_ratio.Name = "txt_loss_ratio";
            this.txt_loss_ratio.Size = new System.Drawing.Size(30, 21);
            this.txt_loss_ratio.TabIndex = 6;
            this.txt_loss_ratio.Text = "0";
            // 
            // txt_print_qty2
            // 
            this.txt_print_qty2.AutoSize = true;
            this.txt_print_qty2.Location = new System.Drawing.Point(344, 0);
            this.txt_print_qty2.Name = "txt_print_qty2";
            this.txt_print_qty2.Size = new System.Drawing.Size(143, 12);
            this.txt_print_qty2.TabIndex = 7;
            this.txt_print_qty2.Text = "%,损耗超过0至少打印一张";
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Controls.Add(this.btn_increase_print);
            this.flowLayoutPanel5.Controls.Add(this.label8);
            this.flowLayoutPanel5.Controls.Add(this.txt_qty2);
            this.flowLayoutPanel5.Controls.Add(this.btn_print_2);
            this.flowLayoutPanel5.Controls.Add(this.btn_all_print);
            this.flowLayoutPanel5.Controls.Add(this.chk_isFull);
            this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 131);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(505, 28);
            this.flowLayoutPanel5.TabIndex = 4;
            // 
            // btn_increase_print
            // 
            this.btn_increase_print.Location = new System.Drawing.Point(3, 3);
            this.btn_increase_print.Name = "btn_increase_print";
            this.btn_increase_print.Size = new System.Drawing.Size(87, 23);
            this.btn_increase_print.TabIndex = 9;
            this.btn_increase_print.Text = "按损耗打印";
            this.toolTip1.SetToolTip(this.btn_increase_print, "数据按照表格中已经选择的一行或者多行");
            this.btn_increase_print.UseVisualStyleBackColor = true;
            this.btn_increase_print.Click += new System.EventHandler(this.btn_increase_print_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(96, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "手动输入打印数量:";
            // 
            // txt_qty2
            // 
            this.txt_qty2.Location = new System.Drawing.Point(209, 3);
            this.txt_qty2.Name = "txt_qty2";
            this.txt_qty2.Size = new System.Drawing.Size(42, 21);
            this.txt_qty2.TabIndex = 11;
            // 
            // btn_print_2
            // 
            this.btn_print_2.Location = new System.Drawing.Point(257, 3);
            this.btn_print_2.Name = "btn_print_2";
            this.btn_print_2.Size = new System.Drawing.Size(70, 23);
            this.btn_print_2.TabIndex = 12;
            this.btn_print_2.Text = "手动打印";
            this.btn_print_2.UseVisualStyleBackColor = true;
            this.btn_print_2.Click += new System.EventHandler(this.btn_print_2_Click);
            // 
            // btn_all_print
            // 
            this.btn_all_print.Location = new System.Drawing.Point(333, 3);
            this.btn_all_print.Name = "btn_all_print";
            this.btn_all_print.Size = new System.Drawing.Size(61, 23);
            this.btn_all_print.TabIndex = 8;
            this.btn_all_print.Text = "全部打印";
            this.toolTip1.SetToolTip(this.btn_all_print, "数据按照这个表格中的所有行");
            this.btn_all_print.UseVisualStyleBackColor = true;
            this.btn_all_print.Click += new System.EventHandler(this.btn_all_print_Click);
            // 
            // chk_isFull
            // 
            this.chk_isFull.AutoSize = true;
            this.chk_isFull.Location = new System.Drawing.Point(400, 3);
            this.chk_isFull.Name = "chk_isFull";
            this.chk_isFull.Size = new System.Drawing.Size(72, 16);
            this.chk_isFull.TabIndex = 13;
            this.chk_isFull.Text = "充满打印";
            this.toolTip1.SetToolTip(this.chk_isFull, "比如一张纸上打印2行2列的模型，而输入的打印数量为1，如果为充满打印则表示，实际打印4个。");
            this.chk_isFull.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FrmMain2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 479);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmMain2";
            this.Text = "条形码打印管理专家 - 专注于快速高效的打印条形码";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain2_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Button btn_load_excel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox combo_printers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_load_model;
        private System.Windows.Forms.ComboBox combo_models;
        private System.Windows.Forms.Button btn_edit_model;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox combo_qty_var_names;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox btn_print_qty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_loss_ratio;
        private System.Windows.Forms.Label txt_print_qty2;
        private System.Windows.Forms.Button btn_increase_print;
        private System.Windows.Forms.Button btn_all_print;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Io.Github.Kerwinxu.LibShapes.Core.UserControlCanvas canvas;
        private System.Windows.Forms.Button btn_new_model;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_qty2;
        private System.Windows.Forms.Button btn_print_2;
        private System.Windows.Forms.CheckBox chk_isFull;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}