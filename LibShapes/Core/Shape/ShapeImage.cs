using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    

    /// <summary>
    /// 图片
    /// </summary>
    public class ShapeImage : ShapeVar
    {

        public ShapeImage()
        {
 
        }

        // 这个不用ShapeVar中的StaticText，是因为我不想显示，并且也不用GetText，是因为这个默认情况下，变量意味着路径，而Img意味着静态的图片。
        [Browsable(false)]//不在PropertyGrid上显示
        public string Img { get; set; }

        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeImage>(json);
        }

        public override void Draw(Graphics g, Matrix matrix)
        {
            // 请注意，我这个算法是有瑕疵的，
            // 这个角度实际上应该是最小内接矩形的角度，
            // 这个是个小项目，应用场景是简单的图形操作，
            // 如果群组里套图形加上群组有角度，会产生偏差。
            // 1.0首先取得没有变换前的坐标
            var path = GetGraphicsPath(matrix);
            var rect = path.GetBounds();            // 外接矩形，如果是内接矩形是最准的。
            var centerPoint = new PointF()          // 中心点的坐标
            {
                X = rect.X + rect.Width/2,
                Y= rect.Y + rect.Height/2
            };
            // 2. 取得图片对象
            var bitmap = getImg();
            if (bitmap != null)
            {
                // 3. 转换。
                Matrix matrix1 = new Matrix();
                matrix1.RotateAt(this.Angle, centerPoint);
                g.Transform = matrix1; // 应用这个变换。
                // 4. 
                // todo 以后添加上拉伸的判断。
                g.DrawImage(bitmap, rect.X, rect.Y, rect.Width, rect.Height);

                //5. 
                g.ResetTransform(); // 取消这个变换
            }
           
            //base.Draw(g, matrix);
        }

        

        private Bitmap getImg()
        {
            try
            {
                if (string.IsNullOrEmpty(this.VarName))
                {
                    return Base64StringToImage(this.Img);
                }
                else
                {
                    // 这里表示是有路径
                    if (File.Exists(this.VarValue))
                    {
                        // 如果路径存在
                        return (Bitmap)Image.FromFile(this.VarValue);
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                //throw;
            }
           
            return null;
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            return base.GetGraphicsPathWithAngle();
        }

        #region 文本和图像的转换
        public static  string ImgToBase64String(Bitmap bmp)
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
                return "";
            }
        }

        //base64编码的文本 转为图片
        public static Bitmap Base64StringToImage(string strbaser64)
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
                return new Bitmap(10, 10);
            }
        }



        #endregion
    }
}
