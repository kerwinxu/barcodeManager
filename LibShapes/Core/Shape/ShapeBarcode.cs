using Io.Github.Kerwinxu.LibShapes.Core.Converter;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{

    public class ShapeBarcode : ShapeVar
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShapeBarcode()
        {
            QrCodeErrorLevel = "容错7%";                // 默认这个
            Encoding = "CODE_39";                       // 默认编码
            StaticText = "690123456789";                // 默认数字
            isIncludeLabel = true;                      // 默认包含标签。
        }

        #region 一堆属性

        /// <summary>
        /// qr二维码的容错率
        /// </summary>
        [TypeConverter(typeof(QrCodeErrorCorrectionLevelConverter)), DescriptionAttribute("QR Code编码的容错率"), DisplayName("QR Code容错率"), CategoryAttribute("条形码设置")]
        public string QrCodeErrorLevel { get; set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        [TypeConverter(typeof(BarcodeEncodingConverter)), DescriptionAttribute("编码"), DisplayName("编码"), CategoryAttribute("条形码设置")]
        public string Encoding { get; set; }

        /// <summary>
        /// 字体
        /// </summary>
        [DescriptionAttribute("字体"), DisplayName("字体"), CategoryAttribute("字体")]
        public Font Font { get; set; }

        [DescriptionAttribute("是否包含标签"), DisplayName("包含标签"), CategoryAttribute("条形码设置")]
        public bool isIncludeLabel { get; set; }

        #endregion


        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeBarcode>(json);
            //throw new NotImplementedException();
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
            // 这里需要将尺寸单位更改一下，我的画布是mm，而这个zxing估计是像素吧
            var rect2 = new Rectangle()
            { 
                //X = (int)(rect.X / 25.4 * g.DpiX),
                //Y = (int)(rect.Y / 25.4 * g.DpiY),
                Width = (int)(rect.Width / 25.4 * g.DpiX),
                Height = (int)(rect.Height / 25.4 * g.DpiX),
            };
            // 中心点的坐标
            var centerPoint = new PointF()          
            {
                X = rect.X + rect.Width / 2,
                Y = rect.Y + rect.Height / 2
            };
            // 2. 取得条形码图像
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeEncodingConverter.dictBarcode[Encoding]; // 编码
            EncodingOptions options = new EncodingOptions()
            {  
                Width = rect2.Width,        // 图像的宽和高
                Height = rect2.Height,
                PureBarcode = !isIncludeLabel,  // 是否包括标签。
                Margin = 2,
            };
            if (Encoding == "QR_CODE")
            {
                options = new QrCodeEncodingOptions() {
                    Width = (int)rect.Width,        // 图像的宽和高
                    Height = (int)rect.Height,
                    PureBarcode = !isIncludeLabel,  // 是否包括标签。
                    Margin = 2,
                    ErrorCorrection=QrCodeErrorCorrectionLevelConverter.level[QrCodeErrorLevel],
                };

            }
            writer.Options = options;
            var bitmap = writer.Write(getText()); // 生成条形码图像。
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



        //public override GraphicsPath GetGraphicsPathWithAngle()
        //{
        //    // 这个直接是返回父类的。
        //    return base.GetGraphicsPathWithAngle();
        //}



    }
}
