//using BarcodeLib;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
//using ZXing.qrcode.decoder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Xml;
using COMMON = ZXing.Common;

namespace VestShapes
{

    [Serializable]
    //[ProtoContract]
    public class ShapeBarcode : ShapeEle
    {
        //就是一个普通的矩形，但是
        protected string _BarcodeEncoding;//条形码编码
        protected bool _isIncludeLabel;//
        protected string _strBarcodeNumber;
        protected Font _fontLabelFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold); //数字的字体。
        protected Font _RealFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        //private LabelPositions _LabelPosition = LabelPositions.BOTTOMCENTER;//取消这个属性啦2016/10/7

        private float _fltOldW, _fltOldh;//这两个参数用在条形码中，特别是二维码运算很繁琐，这两个保存旧的宽度和高度，如果这两个都相同，就不用重新计算
        private Image _imageOld;//直接用这个来绘图就可以了。

        /// <summary>
        /// 这里分情况，当出现错误的时候，判断是否也原先的错误一样，如果不一样，就显示，这里已经包含了原先没有错误，为空字符串的情况
        /// 而要重新验证，只需要将这个重置为空值字符串，在两个地方需要重置
        /// 一个是 BarcodeNumber ，当用户输入后当然得重置了。
        /// 另一个是变量更新的时候，updateVarValue ，在这个方法中，如果变量值更新了，就得重置字符串。
        /// </summary>
        private string _strBarcodeErrorMessage;//条形码返回的错误，这个为空字符串时，就会重新验证。

        public ShapeBarcode()
            : base()
        {
            _BarcodeEncoding = "CODE_39";
            _strBarcodeNumber = "690123456789";
            _isIncludeLabel = true;


        }

        /** 取消这个属性啦2016/10/7
        /// <summary>
        /// 条形码文字的位置，默认为下边中间。
        /// </summary>
        [DescriptionAttribute("文字位置"), DisplayName("文字位置"), CategoryAttribute("条形码设置")]
        public LabelPositions LabelPosition
        {
            get { return _LabelPosition; }
            set
            {
                _LabelPosition = value;
                isChangeed = true;
            }
        }//LabelPosition
        **/

        [TypeConverter(typeof(VarNameDetails)), DescriptionAttribute("就是EXCEL表格的第一行当变量名"), DisplayName("变量名"), CategoryAttribute("变量设置")]
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

        [DescriptionAttribute("条形码数字"), DisplayName("默认条形码数字"), CategoryAttribute("条形码设置")]
        public string BarcodeNumber
        {
            get
            {
                return _strBarcodeNumber;
            }
            set
            {
                _strBarcodeNumber = value;
                _strBarcodeErrorMessage = "";//用这个来再次判定是否符合
                isChangeed = true;
            }
        }

        [DescriptionAttribute("包含数字"), DisplayName("包含文字"), CategoryAttribute("条形码设置")]
        public bool isIncludeLabel
        {
            get
            {
                return _isIncludeLabel;
            }

            set
            {
                _isIncludeLabel = value;
            }

        }



        [TypeConverter(typeof(BarcodeEncoding)), DescriptionAttribute("编码"), DisplayName("编码"), CategoryAttribute("条形码设置")]
        public string Encoding
        {
            get
            {
                return _BarcodeEncoding;
            }
            set
            {
                _BarcodeEncoding = value;
                isChangeed = true;
            }
        }

        /// <summary>
        /// 这个是设置条形码的字体的。
        /// </summary>
        [DescriptionAttribute("字体"), DisplayName("字体"), CategoryAttribute("条形码设置")]
        [XmlIgnore]
        public Font LabelFont
        {
            get
            {
                return _fontLabelFont;
            }
            set
            {
                _fontLabelFont = value;
                _RealFont = ChangeFontSize(_fontLabelFont, _fontLabelFont.Size * _Zoom);//设置真实的字体大小
                isChangeed = true;


            }
        }

        private string _strLanguageEncodingName;

        [TypeConverter(typeof(LanguageEncoding)), DescriptionAttribute("语言编码,中文的话：国外的最好用UTF-8编码，而国内的用GB2312的多"), DisplayName("语言编码"), CategoryAttribute("二维码设置")]
        [XmlElement]
        public string LanguageEncodingDisplayName
        {
            get
            {
                foreach (EncodingInfo item in System.Text.Encoding.GetEncodings())
                {
                    if (item.Name == _strLanguageEncodingName)
                        return item.DisplayName;
                }

                return "";

            }
            set
            {
                isChangeed = true;
                foreach (EncodingInfo item in System.Text.Encoding.GetEncodings())
                {
                    if (item.DisplayName == value)
                    {
                        _strLanguageEncodingName = item.Name;
                        break;
                    }
                }
            }
        }

        private bool isChangeed;//受否改变了，

        private ErrorCorrectionLevel _QRCODEErrorLevel = ErrorCorrectionLevel.L;
        [TypeConverter(typeof(QrCodeErrorLevel)), DescriptionAttribute("容错率"), DisplayName("容错率"), CategoryAttribute("QR Code 设置")]
        [XmlElement]
        public string QrCodeErrorLevel
        {
            get
            {
                if (_QRCODEErrorLevel == ErrorCorrectionLevel.L)
                {
                    return "容错7%";
                }
                else if (_QRCODEErrorLevel == ErrorCorrectionLevel.M)
                {
                    return "容错15%";
                }
                else if (_QRCODEErrorLevel == ErrorCorrectionLevel.Q)
                {
                    return "容错25%";
                }
                else
                {
                    return "容错30%";
                }
            }

            set
            {
                isChangeed = true;//不管怎样，都是改变了

                switch (value)
                {
                    case "容错7%":
                        _QRCODEErrorLevel = ErrorCorrectionLevel.L;
                        break;
                    case "容错15%":
                        _QRCODEErrorLevel = ErrorCorrectionLevel.M;
                        break;
                    case "容错25%":
                        _QRCODEErrorLevel = ErrorCorrectionLevel.Q;
                        break;
                    default:
                        _QRCODEErrorLevel = ErrorCorrectionLevel.H;
                        break;
                }
            }
        }


        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {

                base.Zoom = value;
                isChangeed = true;
                LabelFont = LabelFont;//设置字体
            }
        }


        //如下的这个只是在序列化时用到。
        [Browsable(false)]//不在PropertyGrid上显示
        [XmlElement]
        public myFont SerializerFont
        {
            get
            {
                myFont font2 = new myFont();

                font2.Name = LabelFont.Name;
                font2.Size = LabelFont.Size;
                font2.Style = LabelFont.Style;

                return font2;

            }

            set
            {
                LabelFont = new Font(value.Name, value.Size, value.Style);
            }
        }
        public override ShapeEle DeepClone()
        {

            ShapeBarcode shapeEle = new ShapeBarcode();
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
            shapeEle.BarcodeNumber = BarcodeNumber;
            shapeEle.Encoding = Encoding;
            shapeEle.isIncludeLabel = isIncludeLabel;
            shapeEle.LabelFont = LabelFont;
            //shapeEle.LabelPosition = LabelPosition;//这个属性取消了。
            shapeEle.LanguageEncodingDisplayName = LanguageEncodingDisplayName;
            shapeEle.QrCodeErrorLevel = QrCodeErrorLevel;

            return shapeEle;
            //throw new NotImplementedException();
        }


        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isChange = base.updateVarValue(arrlistKeyValue);

            if (isChange)
            {
                _strBarcodeErrorMessage = "";//设置这个为空就能自动验证了。
                isChangeed = true;//这个是类中的变量，设置是否改变
            }

            return isChange;


            /**
            foreach (clsKeyValue item in arrlistKeyValue)
            {
                if (item.Key == _strVarName)
                {
                    //是否改变
                    bool isChange = false;
                    if (_strVarValue != item.Value)
                    {
                        isChange = true;

                        _strBarcodeErrorMessage = "";//设置这个为空就能自动验证了。
                    }
                    //更新变量值
                    _strVarValue = item.Value;
                    return isChange;//返回是否更新到不同的值

                }

            }
            **/
            //return false;
            //return base.updateVarValue(arrlistKeyValue);
        }


        public override void Draw(Graphics g, List<Matrix> listMatrix)
        {
            try
            {

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

                #region


                string strBarcodeNumber = "";

                //如果设置变量名，就用变量名的对应的变量值。
                if (_strVarName != "")
                {
                    strBarcodeNumber = _strVarValue;
                    //_strBarcodeNumber = "";

                }
                else//如果没有变量名就用默认的值
                {
                    strBarcodeNumber = _strBarcodeNumber;
                }

                if (strBarcodeNumber == "")
                    return;


                //条形码可能有异常，比如说位数不符等等
                try
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



                    PointF poingStr = new PointF(fltx, flty);//实际位置

                    int intBarCodeWidth = (int)(fltw * g.DpiX / 25.4);
                    int intBarCodeHeight = (int)(flth * g.DpiY / 25.4);
                    ;
                    string strEncoding = _BarcodeEncoding;
                    //BarcodeLib.TYPE myType = BarcodeLib.TYPE.EAN13;
                    //我需要更改这个条形码库到zxing，感觉这个比较稳定。
                    BarcodeFormat _barcodeFormat = BarcodeFormat.EAN_13;
                    //要将所有的编码都转为zxing的
                    switch (_BarcodeEncoding)
                    {
                        case "EAN13":
                            _barcodeFormat = BarcodeFormat.EAN_13;
                            break;
                        case "EAN8":
                            _barcodeFormat = BarcodeFormat.EAN_8;
                            break;
                        case "CODE_39":
                            _barcodeFormat = BarcodeFormat.CODE_39;
                            break;
                        case "QR_CODE":
                            _barcodeFormat = BarcodeFormat.QR_CODE;
                            break;

                    }

                    //如下得判断长度和宽度是否可以显示,我得茶皂他们最短需要多少。
                    if ((intBarCodeWidth < 21) || (intBarCodeHeight < 21))
                    {
                        g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                        g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                    }
                    else
                    {
                        //只有在这两个中有一个不相等的情况下才需要更新。
                        if ((_fltOldW != _Width) || (_fltOldh != _Height) || isChangeed)
                        {
                            isChangeed = false;//重新设置成没有更新。

                            _fltOldW = _Width;
                            _fltOldh = _Height;

                            Hashtable hints = new Hashtable();
                            //hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);//容错能力

                            hints.Add(EncodeHintType.ERROR_CORRECTION, _QRCODEErrorLevel);//设置容错率

                            //如下是读取编码，只有在这个不为空的时候才选择
                            if (_strLanguageEncodingName != "")
                            {
                                hints.Add(EncodeHintType.CHARACTER_SET, _strLanguageEncodingName);//字符集
                            }

                            //如下是打印条形码。
                            //设置条形码流
                            BarcodeWriter writer = new BarcodeWriter();
                            writer.Format = _barcodeFormat;//条形码格式
                            EncodingOptions options = new EncodingOptions()
                            {
                                Width = intBarCodeWidth,
                                Height=intBarCodeHeight,
                                Margin = 2
                            };
                            writer.Options = options;
                            _imageOld = writer.Write(strBarcodeNumber);

                            g.DrawImage(_imageOld, rect);

                        }
                        else//如果没有更新，就直接绘图就可以了。
                        {
                            g.DrawImage(_imageOld, rect);
                        }
                    }
                    return;

                    //这个是直接返回的。因为下边的是调用的一维码的程序

                    #region 如下的是原先一维码时候用的，现在全部取消，上边用一个return来返回，不会执行如下的这些。
                    /**
                    BarcodeLib.Barcode bar = new BarcodeLib.Barcode();
                    bar.IncludeLabel = _isIncludeLabel;
                    bar.LabelFont = _RealFont;
                    //bar.LabelPosition = LabelPosition;//条形码文字的位置，取消这个属性啦，2016/10/7

                    if ((intBarCodeWidth < 100) || (intBarCodeHeight < 30))
                    {
                        g.DrawString("图像太小显示不了", new Font("Arial", 6), new SolidBrush(Color.Black), poingStr);
                        g.DrawRectangle(new Pen(Color.Black, 0.5f), fltx, flty, fltw, flth);
                    }
                    else
                    {

                        Image myImage = bar.Encode(myType, strBarcodeNumber, intBarCodeWidth, intBarCodeHeight, g.DpiX, g.DpiY);
                        //将最新的宽度更新。好像不用更新。
                        g.DrawImage(myImage, rect);

                    }
                    bar.Dispose();
                     **/

                    #endregion


                }
                catch (Exception e)
                {
                    //因为这个Draw是持续刷新的，为了在条形码数字出错是只提示依次，在此需要这个，而我在条形码数字更改的时候，重新设置那个为空了
                    if (_strBarcodeErrorMessage != e.Message)
                    {
                        _strBarcodeErrorMessage = e.Message;
                        //MessageBox.Show(e.Message);
                        return;
                    }

                }

                #endregion

                //throw new NotImplementedException();

                g.ResetTransform();//恢复原先的坐标系。

                //base.Draw(g, arrlistMatrix);
            }
            catch (Exception ex)
            {
                //不发出错误了。
                //throw;
            }

        }

       



    }//end class ShapeBarcode
}
