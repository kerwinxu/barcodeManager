using Io.Github.Kerwinxu.LibShapes.Core.Event;
using Io.Github.Kerwinxu.LibShapes.Core.Paper;
using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using Io.Github.Kerwinxu.LibShapes.Core.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Io.Github.Kerwinxu.LibShapes.Core
{
    public partial class UserControlToolbox : UserControl
    {
        public UserControlToolbox()
        {
            InitializeComponent();
            UserControlToolbox_Resize(this, null); // 更改尺寸。
            // 对象的属性更改时
            propertyGrid1.PropertyValueChanged += PropertyGrid1_PropertyValueChanged;
        }

        private void PropertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // 发送给外接
            if (this.PropertyValueChanged != null)
            {
                this.PropertyValueChanged(s, e);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 这个工具箱是操作画布的，这里用一个属性关联
        /// </summary>
        public UserControlCanvas canvas { get; set; }

        /// <summary>
        /// 选择更改事件的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void objectSelected(object sender, ObjectSelectEventArgs e)
        {
            propertyGrid1.SelectedObject = e.obj;
        }

        /// <summary>
        /// 状态更改事件的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stateChanged(object sender, StateChangedEventArgs e)
        {
            all_reset_1();
            // 
            if (e.CurrentState is StateCanvasZoom)
            {
                btn_zoom.Checked = true;
            }else if (e.CurrentState is StateCanvasMove)
            {
                btn_move_canvas.Checked = true;
            }
            else 
            {
                btn_select.Checked = true;
            }
            all_reset_2();
            if (e.CurrentState is StateCreate)
            {
                // 如果是创建模式，要看看是哪个形状的
                var tmp = e.CurrentState as StateCreate;
                if (tmp.shape is ShapePie)
                {
                    btn_pie.Checked = true;
                }
                else if (tmp.shape is ShapeArc)
                {
                    btn_arc.Checked = true;
                }
                else if (tmp.shape is ShapeEllipse)
                {
                    btn_ellipse.Checked = true;
                }
                else if (tmp.shape is ShapeRectangle)
                {
                    btn_rect.Checked = true;
                }else if (tmp.shape is ShapeRoundedRectangle)
                {
                    btn_roundedrect.Checked = true;
                }
                else if (tmp.shape is ShapeLine)
                {
                    btn_line.Checked = true;
                }
                else if (tmp.shape is ShapeImage)
                {
                    btn_img.Checked = true;
                }
                else if (tmp.shape is ShapeText)
                {
                    btn_text.Checked = true;
                }
                else if (tmp.shape is ShapeBarcode)
                {
                    btn_barcode.Checked = true;
                }

            }
        }

        /// <summary>
        /// 这里是属性更改事件
        /// </summary>
        public event System.Windows.Forms.PropertyValueChangedEventHandler PropertyValueChanged;




        /// <summary>
        /// 将所有的设置成没有选中的状态
        /// </summary>
        private void all_reset_1()
        {
            btn_select.Checked = false;
            btn_zoom.Checked = false;
            btn_move_canvas.Checked = false;
        }

        private void all_reset_2()
        {
            btn_rect.Checked = false;
            btn_roundedrect.Checked = false;
            btn_line.Checked = false;
            btn_arc.Checked = false;
            btn_pie.Checked = false;
            btn_img.Checked = false;
            btn_text.Checked = false;
            btn_barcode.Checked = false;
            btn_ellipse.Checked = false;
        }

        /// <summary>
        /// 新建形状，方便创建形状的
        /// </summary>
        /// <param name="shape"></param>
        private void create_shape(ShapeEle shape)
        {
            if (canvas != null)
            {
                canvas.state = new StateCreate(this.canvas, shape);
            }
        }

        /// <summary>
        /// 矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_rect_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeRectangle());
        }

        /// <summary>
        /// 圆角矩形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_roundedrect_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeRoundedRectangle());
        }

        /// <summary>
        /// 线段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_line_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeLine());
        }

        /// <summary>
        /// 椭圆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ellipse_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeEllipse());
        }

        /// <summary>
        /// 椭圆弧
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_arc_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeArc());
        }

        /// <summary>
        /// 扇形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_pie_Click(object sender, EventArgs e)
        {
            create_shape(new ShapePie());
        }

        private void btn_img_Click(object sender, EventArgs e)
        {
            // 这里首先要读取图片
            openFileDialog1.Filter = "图片文件|*.jpg;*.jpeg;*.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 这里要读取图片，然后转换。
                var shapeImg = new ShapeImage();
                shapeImg.Img = ShapeImage.ImgToBase64String((Bitmap)Bitmap.FromFile(openFileDialog1.FileName));
                create_shape(shapeImg);
            }
        }
        /// <summary>
        /// 文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_text_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeText());
        }

        /// <summary>
        /// 条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_barcode_Click(object sender, EventArgs e)
        {
            create_shape(new ShapeBarcode());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.deleteShapes();
            }
        }

        /// <summary>
        /// 组成群组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_merge_group_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.mergeGroup();
            }
        }

        /// <summary>
        /// 取消群组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_group_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.cancelGroup();
            }
        }

        /// <summary>
        /// 向前1位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_forward_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.forward();
            }
        }
        /// <summary>
        /// 移动到最前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_forwardtofront_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.forwardToFront();
            }
        }

        /// <summary>
        /// 往后1位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_backward_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.backward();
            }
        }

        private void btn_wardtoend_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.backwardToEnd();
            }
        }

        /// <summary>
        /// 对齐网格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_align_grid_Click(object sender, EventArgs e)
        {
            if(this.canvas != null)
            {
                this.btn_align_grid.Checked = !this.btn_align_grid.Checked; // 更改状态
                this.canvas.isAlignDridding = this.btn_align_grid.Checked;  // 应用状态
            }
        }

        /// <summary>
        /// 放大缩小状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_zoom_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.state = new StateCanvasZoom(this.canvas);
            }
        }

        /// <summary>
        /// 移动画布的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_move_canvas_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.state = new StateCanvasMove(this.canvas);
            }
        }

        /// <summary>
        /// 网格的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combo_grid_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (this.canvas != null)
            {
                switch (e.ClickedItem.Text)
                {
                    case "关闭":
                        this.canvas.isDrawDridding = false;
                        break;
                    case "1":
                        this.canvas.GriddingInterval = 1;
                        this.canvas.isDrawDridding = true;
                        break;
                    case "2":
                        this.canvas.GriddingInterval = 2;
                        this.canvas.isDrawDridding = true;
                        break;
                    case "5":
                        this.canvas.GriddingInterval = 5;
                        this.canvas.isDrawDridding = true;
                        break;
                    default:
                        break;
                }

                this.canvas.Refresh(); // 刷新。
            }
        }

        private void btn_page_setting_Click(object sender, EventArgs e)
        {
            // 
            IPaperSetting paperSetting;
            if (this.canvas.shapes.Paper != null)
            {
                paperSetting = new FrmPaperSetting(this.canvas.shapes.Paper);
            }
            else
            {
                paperSetting = new FrmPaperSetting();
            }
            // 打开窗口
            if (((Form)paperSetting).ShowDialog() == DialogResult.OK)
            {
                //返回新的纸张设置。
                this.canvas.shapes.Paper = paperSetting.GetPaper();
                this.canvas.Refresh();
            }

        }

        private void btn_zoom_screen_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.zoomToScreen();
            }
        }

        private void btn_align_top_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_top();
            }
        }

        private void btn_align_bottom_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_bottom();
            }
        }

        private void btn_align_left_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_left();
            }
        }

        private void btn_align_right_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_right();
            }
        }

        private void btn_align_center_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_center();
            }
        }

        private void btn_align_middle_Click(object sender, EventArgs e)
        {
            if (this.canvas != null)
            {
                this.canvas.align_midele();
            }
        }

        private void UserControlToolbox_Resize(object sender, EventArgs e)
        {
            // 这个会更改属性框的坐标
            var left_top_point = new Point(90, 28);   // 左上角的坐标
            propertyGrid1.Location = left_top_point;
            propertyGrid1.Width = this.Width - left_top_point.X - 10;
            propertyGrid1.Height = this.Height - left_top_point.Y - 10;

        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            // 转成待机模式
            this.canvas.SelectShape = null;
            this.canvas.state = new StateStandby(this.canvas);
        }
    }
}
