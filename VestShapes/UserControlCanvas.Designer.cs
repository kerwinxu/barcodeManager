namespace VestShapes
{
    partial class UserControlCanvas
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
            this.components = new System.ComponentModel.Container();
            this.timerLianXuAnJian = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerLianXuAnJian
            // 
            this.timerLianXuAnJian.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UserControlCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DoubleBuffered = true;
            this.Name = "UserControlCanvas";
            this.Size = new System.Drawing.Size(327, 259);
            this.Click += new System.EventHandler(this.UserControlCanvas_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControl1_Paint);
            this.DoubleClick += new System.EventHandler(this.UserControlCanvas_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserControlCanvas_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UserControlCanvas_KeyUp);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlCanvas_Layout);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseUp);
            this.Resize += new System.EventHandler(this.UserControlCanvas_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerLianXuAnJian;



    }
}
