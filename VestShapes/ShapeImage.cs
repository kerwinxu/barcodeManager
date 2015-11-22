using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{
    /// <summary>
    /// ShapeImage 的变量类型
    /// </summary>
    public enum enumImageVar { 从文件插入, 从变量获取文件名 };

    [Serializable]
    //[ProtoContract]
    public class ShapeImage : ShapeEle
    {

        private Bitmap _image = new Bitmap(10, 10);//预防没有初始化吧
        private bool _isOriginalSize = false;//原始大小
        private bool _isScaled = true;//按比例缩放。

        private bool isChangeed;//受否改变了，

        /**
        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("这个加载图片默认是将字段的值当成图片文件路径"), DisplayName("变量名"), CategoryAttribute("变量设置")]
        [XmlElement]
        public string VarName
        {
            get
            {
                return _strVarName;
            }
            set
            {
                _strVarName = value;

            }

        }
         * */

        [DescriptionAttribute("图片"), DisplayName("图片"), CategoryAttribute("图片设置")]
        [XmlIgnore]
        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                //_image = new Bitmap(value);

                try
                {
                    using (System.IO.MemoryStream memory3 = new System.IO.MemoryStream())
                    {

                        (new Bitmap(value)).Save(memory3, System.Drawing.Imaging.ImageFormat.Jpeg);
                        Bitmap bitmapTemp = new Bitmap(memory3);
                        //这里强制最大的图像宽度是800*600，等比例缩放。
                        //条件是宽度和高度大于
                        // 俺比例缩放首先要求出图像的宽和高

                        if ((bitmapTemp.Width > 800) ||
                            (bitmapTemp.Height > 600))
                        {
                            float fltx = 0;
                            float flty = 0;
                            float fltw = 800;
                            float flth = 600;


                            float fltImageWidth = bitmapTemp.Width;
                            float fltImageHeight = bitmapTemp.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算



                            if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                            {
                                //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                                //图片的X坐标也在选择框的Y上。

                                float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                                Bitmap bitmap2 = new Bitmap((int)fltw, (int)fltImageRealHeight);

                                Graphics g = Graphics.FromImage(bitmap2);


                                //如下就是画图了
                                g.DrawImage(bitmapTemp, fltx, flty, fltw, fltImageRealHeight);//

                                _image = new Bitmap(bitmap2);

                            }
                            else
                            {
                                //到这里可以算是图片的高等比例到选择框的高
                                float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                                Bitmap bitmap2 = new Bitmap((int)fltImageRealWidth, (int)flth);

                                Graphics g = Graphics.FromImage(bitmap2);


                                //如下就是画图了
                                g.DrawImage(bitmapTemp, fltx, flty, fltImageRealWidth, flth);

                                _image = new Bitmap(bitmap2);

                            }

                        }
                        else
                        {
                            _image = bitmapTemp;//如果小于那个尺寸就用原先的啦。
                        }





                    }
                }
                catch (Exception exception)
                {
                    ////ClsErrorFile.WriteLine(exception);
                    //Console.Error.Write(exception.ToString());
                }

            }
        }

        [DescriptionAttribute("原始图片大小"), DisplayName("原始图片大小"), CategoryAttribute("图片设置")]
        [XmlElement]
        public bool OriginalSize
        {
            get
            {
                return _isOriginalSize;
            }

            set
            {
                _isOriginalSize = value;
            }

        }
        [DescriptionAttribute("按比例缩放"), DisplayName("按比例缩放"), CategoryAttribute("图片设置")]
        [XmlElement]
        public bool Scaled
        {
            get
            {
                return _isScaled;
            }
            set
            {
                _isScaled = value;

                if (_isScaled == true)
                {
                    _isOriginalSize = false;
                }
            }
        }


        public override ShapeEle DeepClone()
        {
            ShapeImage shapeEle = new ShapeImage();
            shapeEle.Zoom = Zoom;
            shapeEle.X = X;
            shapeEle.Y = Y;
            shapeEle.Width = Width;
            shapeEle.Height = Height;
            shapeEle.isFill = isFill;
            shapeEle.PenColor = PenColor;
            shapeEle.PenWidth = PenWidth;
            shapeEle.PenDashStyle = PenDashStyle;
            shapeEle.Route = Route;

            //如下是子类单独的
            shapeEle.Image = new Bitmap(Image);
            shapeEle.OriginalSize = OriginalSize;
            shapeEle.Scaled = Scaled;

            return shapeEle;
            //throw new NotImplementedException();
        }

        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]//如下是单独序列化保存图片时用的属性
        public string SerializerBitmap
        {
            get
            {
                return ImgToBase64String(_image);
            }
            set
            {
                _image = Base64StringToImage(value);
            }

        }

        private string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                //如下是为了预防GDI一般性错误而深度复制
                Bitmap bmp2 = new Bitmap(bmp);

                MemoryStream ms = new MemoryStream();
                bmp2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);

                bmp2.Dispose();

                return strbaser64;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("ImgToBase64String 转换失败/nException:" + ex.Message);
                ////ClsErrorFile.WriteLine("ImgToBase64String 转换失败/nException:" + ex.Message);
                ////ClsErrorFile.WriteLine("ImgToBase64String 转换失败/nException:", ex);
                //Console.Error.WriteLine("ImgToBase64String 转换失败/nException:" + ex.Message);
                return "";
            }
        }

        //base64编码的文本 转为图片
        private Bitmap Base64StringToImage(string strbaser64)
        {
            try
            {

                byte[] arr = Convert.FromBase64String(strbaser64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();

                return bmp;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Base64StringToImage 转换失败/nException：" + ex.Message);
                ////ClsErrorFile.WriteLine("Base64StringToImage 转换失败/nException：" + ex.Message);
                ////ClsErrorFile.WriteLine("Base64StringToImage 转换失败/nException：", ex);
                //Console.Error.WriteLine("Base64StringToImage 转换失败/nException：" + ex.Message);
                return new Bitmap(10, 10);
            }
        }

        public override void Draw(Graphics g, List<Matrix> listMatrix)
        {
            RectangleF rect = getGraphicsPath(listMatrix).GetBounds();
            float fltx = rect.X;
            float flty = rect.Y;
            float fltw = rect.Width;
            float flth = rect.Height;

            /**
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;
             * */

            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置


            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //RectangleF rect = getRect();


            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx, flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//

                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }


            g.ResetTransform();
            //base.Draw(g, arrlistMatrix);

        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置
            g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);

            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            RectangleF rect = getRect();


            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx, flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//

                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }

            //如下的这个是恢复原先的，负数.
            g.TranslateTransform(-fltKongX, -fltKongY);
            g.ResetTransform();//恢复原先的坐标系。

        }


        public override void Draw(Graphics g)
        {
            float fltx = _X + _XAdd;
            float flty = _Y + _YAdd;
            float fltw = _Width + _WidthAdd;
            float flth = _Height + _HeightAdd;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //如果没有图片，那么就直接跳过了。
            if (_image == null)
                return;

            //旋转图形
            if (Route != 0)
            {
                PointF pZhongXin = new PointF(fltx + (fltw) / 2, flty + (flth) / 2);
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            RectangleF rect = getRect();

            //首先判断是否是原始图片大小
            if (_isOriginalSize)
            {
                g.DrawImage(_image, new PointF(fltx, flty));
            }
            else if (_isScaled)//按比例缩放
            {
                // 俺比例缩放首先要求出图像的宽和高
                float fltImageWidth = _image.Width;
                float fltImageHeight = _image.Height;//因为只需要比例，所以不需要放大率和分辨率之类的东西换算

                if ((fltImageWidth / fltImageHeight) > (fltw / flth))
                {
                    //如果图片的宽高比例大于用户的选择框的宽高比例，那么也就是说得按照图片的宽等比例放大到用户的款。
                    //图片的X坐标也在选择框的Y上。

                    float fltImageRealHeight = fltw * fltImageHeight / fltImageWidth;//图片实际的高，

                    //我想要把图片放在正中的位置,那么图片的X点就是用户的fltx, 图片的Y点计算如下，

                    float fltImageRealY = flty + (flth - fltImageRealHeight) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltx, fltImageRealY, fltw, fltImageRealHeight);//


                }
                else
                {
                    //到这里可以算是图片的高等比例到选择框的高
                    float fltImageRealWidth = fltImageWidth * flth / fltImageHeight;

                    //我同样想把这个图片放在正中的位置。
                    float fltImageRealX = fltx + (fltw - fltImageRealWidth) / 2;

                    //如下就是画图了
                    g.DrawImage(_image, fltImageRealX, flty, fltImageRealWidth, flth);

                }

            }
            else//不按比例缩放。
            {
                g.DrawImage(_image, fltx, flty, fltw, flth);

            }
            g.ResetTransform();
            //throw new NotImplementedException();
        }

    }
}
