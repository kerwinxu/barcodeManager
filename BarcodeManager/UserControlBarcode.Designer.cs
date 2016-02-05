namespace BarcodeTerminator
{
    partial class UserControlBarcode
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.comboBoxEncoding = new System.Windows.Forms.ComboBox();
            this.comboBoxVaribaleName = new System.Windows.Forms.ComboBox();
            this.txtBarcodeData = new System.Windows.Forms.TextBox();
            this.chkIncludeLabel = new System.Windows.Forms.CheckBox();
            this.btnFrmDispose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.txtX);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.txtY);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.txtWidth);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.txtHeight);
            this.flowLayoutPanel1.Controls.Add(this.comboBoxEncoding);
            this.flowLayoutPanel1.Controls.Add(this.comboBoxVaribaleName);
            this.flowLayoutPanel1.Controls.Add(this.txtBarcodeData);
            this.flowLayoutPanel1.Controls.Add(this.chkIncludeLabel);
            this.flowLayoutPanel1.Controls.Add(this.btnFrmDispose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(886, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "左边距";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(50, 3);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(37, 21);
            this.txtX.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(93, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "顶边距";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(140, 3);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(37, 21);
            this.txtY.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "宽度";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(218, 3);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(37, 21);
            this.txtWidth.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "高度";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(296, 3);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(37, 21);
            this.txtHeight.TabIndex = 4;
            // 
            // comboBoxEncoding
            // 
            this.comboBoxEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEncoding.FormattingEnabled = true;
            this.comboBoxEncoding.Items.AddRange(new object[] {
            "EAN13",
            "EAN8",
            "UPCA",
            "UPCE",
            "UPC_SUPPLEMENTAL_2DIGIT",
            "UPC_SUPPLEMENTAL_5DIGIT",
            "CODE39",
            "CODE39Extended",
            "CODE128",
            "CODE128A",
            "CODE128B",
            "CODE128C",
            "Codabar",
            "ISBN",
            "Interleaved2of5",
            "Standard2of5",
            "Industrial2of5",
            "PostNet",
            "BOOKLAND",
            "JAN13",
            "MSI_Mod10",
            "MSI_2Mod10",
            "MSI_Mod11",
            "MSI_Mod11_Mod10",
            "Modified_Plessey",
            "CODE11",
            "USD8",
            "UCC12",
            "UCC13",
            "LOGMARS,",
            "ITF14",
            "CODE93",
            "TELEPEN",
            "FIM"});
            this.comboBoxEncoding.Location = new System.Drawing.Point(339, 3);
            this.comboBoxEncoding.Name = "comboBoxEncoding";
            this.comboBoxEncoding.Size = new System.Drawing.Size(109, 20);
            this.comboBoxEncoding.TabIndex = 5;
            // 
            // comboBoxVaribaleName
            // 
            this.comboBoxVaribaleName.FormattingEnabled = true;
            this.comboBoxVaribaleName.Location = new System.Drawing.Point(454, 3);
            this.comboBoxVaribaleName.Name = "comboBoxVaribaleName";
            this.comboBoxVaribaleName.Size = new System.Drawing.Size(95, 20);
            this.comboBoxVaribaleName.TabIndex = 6;
            this.comboBoxVaribaleName.SelectedIndexChanged += new System.EventHandler(this.comboBoxVaribaleName_SelectedIndexChanged);
            // 
            // txtBarcodeData
            // 
            this.txtBarcodeData.Location = new System.Drawing.Point(555, 3);
            this.txtBarcodeData.Name = "txtBarcodeData";
            this.txtBarcodeData.Size = new System.Drawing.Size(187, 21);
            this.txtBarcodeData.TabIndex = 7;
            // 
            // chkIncludeLabel
            // 
            this.chkIncludeLabel.AutoSize = true;
            this.chkIncludeLabel.Location = new System.Drawing.Point(748, 3);
            this.chkIncludeLabel.Name = "chkIncludeLabel";
            this.chkIncludeLabel.Size = new System.Drawing.Size(72, 16);
            this.chkIncludeLabel.TabIndex = 8;
            this.chkIncludeLabel.Text = "包含数字";
            this.chkIncludeLabel.UseVisualStyleBackColor = true;
            // 
            // btnFrmDispose
            // 
            this.btnFrmDispose.Location = new System.Drawing.Point(826, 3);
            this.btnFrmDispose.Name = "btnFrmDispose";
            this.btnFrmDispose.Size = new System.Drawing.Size(48, 23);
            this.btnFrmDispose.TabIndex = 9;
            this.btnFrmDispose.Text = "删除";
            this.btnFrmDispose.UseVisualStyleBackColor = true;
            this.btnFrmDispose.Click += new System.EventHandler(this.btnFrmDispose_Click);
            // 
            // UserControlBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UserControlBarcode";
            this.Size = new System.Drawing.Size(886, 29);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox comboBoxEncoding;
        private System.Windows.Forms.TextBox txtBarcodeData;
        private System.Windows.Forms.CheckBox chkIncludeLabel;
        private System.Windows.Forms.Button btnFrmDispose;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.ComboBox comboBoxVaribaleName;
    }
}
