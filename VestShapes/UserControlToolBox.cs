using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Xuhengxiao.MyDataStructure;

namespace VestShapes
{
    public partial class UserControlToolBox : UserControl
    {
        protected UserControlCanvas _canvas; 
        public UserControlToolBox()
        {
            InitializeComponent();
        }

        public void setCanvas(UserControlCanvas canvas)
        {
            _canvas = canvas;
            _canvas.objectSelected += new UserControlCanvas.ObjectSelected(_canvas_objectSelected);
            _canvas.optionChanged += new UserControlCanvas.OptionChanged(_canvas_optionChanged);

            toolStripButton4.Checked = UserControlCanvas.isAlignGridding;

        }

        void _canvas_optionChanged(object sender, UserControlCanvas.OptionEventArgs e)
        {
            //这个只要判断是否是select就可以了
            if (e.option == "select")
            {
                deselectAll();
                SelectBtn.Checked = true;

            }
            //throw new NotImplementedException();
        }

        private void deselectAll()
        {
            SelectBtn.Checked = false;
            RectBtn.Checked = false;
            btnArc.Checked = false;
            btnEllipse.Checked = false;
            LineBtn.Checked = false;
            btnSingleText.Checked = false;
            ImageBtn.Checked = false;
            btnMultiLineText.Checked = false;
            btnBarcode.Checked = false;
            btnEllipse.Checked = false;
            btnPie.Checked = false;
            btnRoundRect.Checked = false;
            btnZoom.Checked = false;
            btnHand.Checked = false;

        }

        void _canvas_objectSelected(object sender, UserControlCanvas.PropertyEventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObjects = e.arrShapeEleSelect;
            }
            catch (System.Exception ex)
            {
            	
            }
            
            //throw new NotImplementedException();
        }

        //当属性发生更改是要强制绘制图形
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _canvas.saveOperatingRecord();
            _canvas.Refresh();
        }

        private void RectBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            RectBtn.Checked = true;
            _canvas.addShapeEle(new ShapeRect());

        }

        private void toolStripDropDownButton2_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            try
            {
                if (e.ClickedItem.Text == "纸张放大到屏幕")
                {
                    _canvas.ZoomPaperToScreen();
                }
                else
                {
                    _canvas.Zoom = Convert.ToSingle(e.ClickedItem.Text);
                }
            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //Console.Error.WriteLine(ex.Message);
            }
            
        }

        private void LineBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            LineBtn.Checked = true;
            _canvas.addShapeEle(new ShapeLine());
        }

        private void TextBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnSingleText.Checked = true;
            _canvas.addShapeEle(new shapeSingleText());
        }

        private void CirciBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnEllipse.Checked = true;
            _canvas.addShapeEle(new ShapeEllipse());
            //这里要添加 _canvas.addShapeEle
        }

        private void ImageBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            ImageBtn.Checked = true;
            //这里要添加 _canvas.addShapeEle

            ShapeImage simage = new ShapeImage();

             //这里要选择图片

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    simage.Image = new Bitmap(openFileDialog1.FileName);
                    /**
                    using (System.IO.MemoryStream memory3 = new System.IO.MemoryStream())
                    {

                        (new Bitmap(openFileDialog1.FileName)).Save(memory3, System.Drawing.Imaging.ImageFormat.Png);
                        simage.Image = new Bitmap(memory3);

                    }
                     * */
                }
                catch (Exception exception)
                {
                    //ClsErrorFile.WriteLine(exception);
                    //Console.Error.Write(exception.ToString());
                }
                finally
                {

                }

            }
            else
            {
                //我实在是不知道该怎么提示。
                
            }

            _canvas.addShapeEle(simage);


        }



        private void btnBarcode_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnBarcode.Checked = true;
            //这里要添加 _canvas.addShapeEle
            _canvas.addShapeEle(new ShapeBarcode());
        }

        private void btnVarText_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnMultiLineText.Checked = true;
            _canvas.addShapeEle(new shapeMultiText());

        }

        //如下这个是椭圆的。
        private void RRectBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnArc.Checked = true;
            _canvas.addShapeEle(new ShapeArc());
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _canvas.Loader(openFileDialog1.FileName);

            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _canvas.Saver(saveFileDialog1.FileName);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            _canvas.deleteSelectShapeEle();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _canvas.deleteSelectShapeEle();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _canvas.deleteSelectShapeEle();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            _canvas.forward();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _canvas.forward2();
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            _canvas.backward();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            _canvas.backward2();
        }

        private void GroupBtn_Click(object sender, EventArgs e)
        {
            _canvas.doGroup();

        }

        private void deGroupBtn_Click(object sender, EventArgs e)
        {
            _canvas.DeGroup();
        }

        private void btnPie_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnPie.Checked = true;
            _canvas.addShapeEle(new ShapePie());
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "关闭":
                    _canvas.isDrawDridding = false;
                    break;
                    /**
                case "1":
                    UserControlCanvas.isDrawDridding = true;
                    UserControlCanvas.GriddingInterval = 1;
                    break;
                case "2":
                    UserControlCanvas.isDrawDridding = true;
                    UserControlCanvas.GriddingInterval = 2;
                    break;
                case "5":
                    UserControlCanvas.isDrawDridding = true;
                    UserControlCanvas.GriddingInterval = 5;
                    break;
                     * */
                default:

                    try
                    {
                        float f = Convert.ToSingle(e.ClickedItem.Text);
                        _canvas.isDrawDridding = true;
                        UserControlCanvas.GriddingInterval = f;

                    }
                    catch (Exception ex)
                    {
                        //ClsErrorFile.WriteLine(ex);
                        //Console.Error.WriteLine(ex.Message);
                        throw;
                    }


                    break;
            }

            _canvas.Refresh();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            toolStripButton4.Checked = !toolStripButton4.Checked;
            UserControlCanvas.isAlignGridding =toolStripButton4.Checked;

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (_canvas.myShapes==null)
                {
                    _canvas.myShapes = new Shapes();
                }

                if (_canvas.myShapes.BarcodePageSettings == null)
                {
                    _canvas.myShapes.BarcodePageSettings = new ClsPageSettings();
                }

                FrmPageSettings frm = new FrmPageSettings(_canvas.myShapes.BarcodePageSettings);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _canvas.myShapes.BarcodePageSettings = frm.BarcodePageSettings;
                    _canvas.Refresh();

                }
            }
            catch (System.Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
            }

        }

        private void btnRoundRect_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnRoundRect.Checked = true;
            _canvas.addShapeEle(new ShapeRoundRect());
        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnZoom.Checked = true;
            _canvas.Option = "Zoom";
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            deselectAll();
            btnHand.Checked = true;
            _canvas.Option = "Hand";

        }



        private void toolStripButton8_Click_1(object sender, EventArgs e)
        {
            _canvas.ZoomPaperToScreen();
        }

        private void SelectBtn_Click(object sender, EventArgs e)
        {
            deselectAll();
            _canvas.Option = "";

        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            _canvas.doHorizontalAlignWithMainObject(System.Windows.Forms.VisualStyles.HorizontalAlign.Left);

        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            _canvas.doHorizontalAlignWithMainObject(System.Windows.Forms.VisualStyles.HorizontalAlign.Right);
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            _canvas.doHorizontalAlignWithMainObject(System.Windows.Forms.VisualStyles.HorizontalAlign.Center);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            _canvas.doVerticalAlignWithMainObject(System.Windows.Forms.VisualStyles.VerticalAlignment.Top);
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            _canvas.doVerticalAlignWithMainObject(System.Windows.Forms.VisualStyles.VerticalAlignment.Bottom);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            _canvas.doVerticalAlignWithMainObject(System.Windows.Forms.VisualStyles.VerticalAlignment.Center);

        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            _canvas.doHorizontalAlignWithModel(System.Windows.Forms.VisualStyles.HorizontalAlign.Center);
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            _canvas.doVerticalAlignWithModel(System.Windows.Forms.VisualStyles.VerticalAlignment.Center);
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
