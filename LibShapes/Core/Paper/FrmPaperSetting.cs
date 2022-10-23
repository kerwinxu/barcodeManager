using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Io.Github.Kerwinxu.LibShapes.Core.Paper
{

    public partial class FrmPaperSetting : Form , IPaperSetting
    {

        /// <summary>
        /// 修改的是这个变量
        /// </summary>
        private Paper paper = new Paper();

        /// <summary>
        /// 这个是要返回的
        /// </summary>
        private Paper result;

        public FrmPaperSetting()
        {
            InitializeComponent();
            init_printer();
        }

        public FrmPaperSetting(Paper paper):base()
        {
            this.paper = paper;
            paper_to_ui();
        }

        /// <summary>
        /// 初始化打印机
        /// </summary>
        private void init_printer()
        {
            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                comboPrinters.Items.Add(item);
            }
            
        }

        public Paper GetPaper()
        {
            return this.result;
            //throw new NotImplementedException();
        }


        private bool isOnlyOne;

        /// <summary>
        /// 界面上数据发生改变
        /// </summary>
        private void change()
        {
            // 有这个标志，表示当前只能有一个在修改。
            if (isOnlyOne)
            {
                return;
            }

            isOnlyOne = true;
            // 先更新数据
            if (ui_to_paper() && compute())
            {
                paper_to_ui();  // 然后计算，最后更新到界面
                this.pictureBox1.Refresh();
                this.lblModelSize.Text = $"模板大小:{this.paper.ModelWidth} * {this.paper.ModelHeight}";
                this.lblPaperSize.Text = $"纸张大小:{this.paper.PaperWidth} * {this.paper.PaperHeight}";
            }
            isOnlyOne = false;
        }



        private bool compute()
        {
            // 我这里简化一下，自定义模板尺寸和自定义模板的间距，两个得有一个是可以手动设置，而另一个是自动的
            // 解方程 纸张的宽度 = 左边距 + 列数 * 模板的宽度 + （列数-1）*模板的间距 + 右边距
            if (chkCustomInterval.Checked)
            {
                // 这个模板间距是手动设置的，那么我就要计算的是模板的大小了
                this.paper.ModelWidth = (this.paper.PaperWidth - this.paper.Left - this.paper.Right - (this.paper.Cols - 1) * this.paper.HorizontalIntervalDistance) / this.paper.Cols;
                this.paper.ModelHeight = (this.paper.PaperHeight - this.paper.Top - this.paper.Bottom - (this.paper.Rows - 1) * this.paper.VerticalIntervalDistance) / this.paper.Rows;
                if (this.paper.ModelHeight < 0 || this.paper.ModelWidth < 0)
                {
                    return false;
                }

            }else if (chkCustomModelSize.Checked)
            {
                // 自定义了模板，要求的是模板的边距了
                this.paper.HorizontalIntervalDistance = (this.paper.PaperWidth - this.paper.Left - this.paper.Right - this.paper.Cols * this.paper.ModelWidth) / (this.paper.Cols - 1);
                this.paper.VerticalIntervalDistance = (this.paper.PaperHeight - this.paper.Top - this.paper.Bottom - this.paper.Rows * this.paper.ModelHeight) / (this.paper.Rows - 1);
                if (this.paper.HorizontalIntervalDistance < 0 || this.paper.VerticalIntervalDistance < 0)
                {
                    return false;
                }
            }


            return true;
        }

        private bool ui_to_paper()
        {
            // 这里将ui中的信息全部转到paper中
            try
            {
                this.paper.PaperWidth = float.Parse(txt_paper_width.Text);
                this.paper.PaperHeight = float.Parse(txt_paper_height.Text);
                this.paper.Landscape = radioButton1.Checked;// 纵向
                this.paper.Rows = (int)UpDownNumberOfLine.Value;
                this.paper.Cols = (int)UpDownNumberOfColumn.Value;
                this.paper.Top = float.Parse(txtMaginsTop.Text);
                this.paper.Bottom = float.Parse(txtMaginsBottom.Text);
                this.paper.Left = float.Parse(txtMaginsLeft.Text);
                this.paper.Right = float.Parse(txtMaginsRight.Text);
                this.paper.ModelWidth = float.Parse(txtModelWidth.Text);
                this.paper.ModelHeight = float.Parse(txtModelHeight.Text);
                this.paper.HorizontalIntervalDistance = float.Parse(txtHorizontalInterval.Text);
                this.paper.VerticalIntervalDistance = float.Parse(txtVerticalInterval.Text);
                if (radioButtonRect.Checked)
                {
                    this.paper.ModelShape = new Shape.ShapeRectangle();        // 矩形

                }else if (radioButtonRoundRect.Checked)
                {
                    this.paper.ModelShape = new Shape.ShapeRoundedRectangle(); // 圆角矩形
                }
                else
                {
                    this.paper.ModelShape = new Shape.ShapeEllipse(); // 椭圆
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换失败:" + ex.Message);
                return false;
                //throw;
            }
        }

        private void paper_to_ui()
        {
            txt_paper_width.Text = this.paper.PaperWidth.ToString();
            txt_paper_height.Text = this.paper.PaperHeight.ToString();
            if (this.paper.Landscape) radioButton1.Checked = true; else radioButton2.Checked = true;
            UpDownNumberOfLine.Value = this.paper.Rows;
            UpDownNumberOfColumn.Value = this.paper.Cols;
            txtMaginsTop.Text = this.paper.Top.ToString();
            txtMaginsBottom.Text = this.paper.Bottom.ToString();
            txtMaginsLeft.Text = this.paper.Left.ToString();
            txtMaginsLeft.Text = this.paper.Left.ToString();
            txtMaginsRight.Text = this.paper.Right.ToString();
            txtModelWidth.Text = this.paper.ModelWidth.ToString();
            txtModelHeight.Text = this.paper.ModelHeight.ToString();
            txtHorizontalInterval.Text = this.paper.HorizontalIntervalDistance.ToString();
            txtVerticalInterval.Text = this.paper.VerticalIntervalDistance.ToString();

            if (this.paper.ModelShape is Shape.ShapeRectangle)
            {
                radioButtonRect.Checked = true;
            }
            else if (this.paper.ModelShape is Shape.ShapeRoundedRectangle)
            {
                radioButtonRoundRect.Checked = true;
            }
            else
            {
                radioButtonEllipse.Checked = true;
            }

        }

        Dictionary<string, System.Drawing.Printing.PaperSize> dict_paperSize = new Dictionary<string, PaperSize>();

        private void comboPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {
            dict_paperSize.Clear();
            // 打印机更改后需要更改有多少的纸张。
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = comboPrinters.Text;// 打印机的名称
            foreach (System.Drawing.Printing.PaperSize item in printDocument.PrinterSettings.PaperSizes)
            {
                dict_paperSize[item.ToString()] = item;
            }

            // 加载到组合框
            string _old = comboPaperSizes.Text;
            comboPaperSizes.Items.Clear();
            comboPaperSizes.Items.AddRange(dict_paperSize.Keys.ToArray());
            
            // 我这里看看是否有这个纸张
            if (dict_paperSize.ContainsKey(_old))
            {
                //comboPaperSizes.Text = _old;
            }
            else
            {
                // 这里有新的纸张
                comboPaperSizes.SelectedIndex = 0;
            }
        }


        private void comboPaperSizes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (dict_paperSize.ContainsKey(comboPaperSizes.Text))
            {
                var _papersize = dict_paperSize[comboPaperSizes.Text];
                txt_paper_height.Text = _papersize.Height.ToString();
                txt_paper_width.Text = _papersize.Width.ToString();
                change();
            }

        }

        private void chkCustomInterval_CheckedChanged(object sender, EventArgs e)
        {
            txtHorizontalInterval.Enabled = chkCustomInterval.Checked;
            txtVerticalInterval.Enabled = chkCustomInterval.Checked;

            if (chkCustomInterval.Checked)
            {
                chkCustomModelSize.Checked = false;
            }
             
        }

        private void chkCustomModelSize_CheckedChanged(object sender, EventArgs e)
        {
            // 这里要变成灰色的
            txtModelHeight.Enabled = chkCustomModelSize.Checked;
            txtModelWidth.Enabled = chkCustomModelSize.Checked;

            if (chkCustomModelSize.Checked)
            {
                chkCustomInterval.Checked = false;
            }
            

        }

        private void btn_paper_ok_Click(object sender, EventArgs e)
        {
            change();
        }

        private void UpDownNumberOfLine_ValueChanged(object sender, EventArgs e)
        {
            change();
        }

        private void txtMaginsTop_TextChanged(object sender, EventArgs e)
        {
            change();
        }

        private void txtModelWidth_TextChanged(object sender, EventArgs e)
        {
            // 要判断是否是手动
            if(chkCustomModelSize.Checked) change(); 
        }

        private void txtHorizontalInterval_TextChanged(object sender, EventArgs e)
        {
            // 要判断是否是手动
            if (chkCustomInterval.Checked) change();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //
            this.DialogResult = DialogResult.OK; // 返回ok
            this.Close();                        // 关闭
            result = paper;                      // 设置这个要返回的。
        }

        private void radioButtonRect_CheckedChanged(object sender, EventArgs e)
        {
            change();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // 做一个图形
                Shapes shapes = new Shapes();
                // 做一张纸
                Shape.ShapeRectangle rect1 = new Shape.ShapeRectangle()
                {
                    X = 0,
                    Y = 0,
                    Width = this.paper.PaperWidth,     // todo 这里要注意横向
                    Height = this.paper.PaperHeight,
                    PenColor = Color.Red,
                    PenWidth = 1,
                    IsFill = true,
                    FillColor = Color.White,
                };
                shapes.lstShapes.Add(rect1);
                ////如下是绘制模板。
                for (int i = 0; i < this.paper.Cols; i++)
                {
                    for (int j = 0; j < this.paper.Rows; j++)
                    {
                        var tmp = this.paper.ModelShape.DeepClone();
                        tmp.X = this.paper.Left + i * (this.paper.ModelWidth + this.paper.HorizontalIntervalDistance);
                        tmp.Y = this.paper.Top + j * (this.paper.ModelHeight + this.paper.VerticalIntervalDistance);
                        tmp.Width = this.paper.ModelWidth;
                        tmp.Height = this.paper.ModelHeight;
                        tmp.PenColor = Color.Black;
                        tmp.PenWidth = 1;
                        tmp.IsFill = true;
                        tmp.FillColor = Color.AliceBlue;
                        shapes.lstShapes.Add(tmp);
                    }

                }
                // 这里做一个放大
                shapes.zoomTo(e.Graphics.DpiX, e.Graphics.DpiY, this.pictureBox1.Width, this.pictureBox1.Height, 5);
                // 显示
                
                shapes.Draw(e.Graphics, shapes.GetMatrix(), false);
            }
            catch (Exception)
            {

                //throw;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
