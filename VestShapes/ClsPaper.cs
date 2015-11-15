using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace VestShapes
{
    /// <summary>
    /// 纸张类，因为我做的这个矢量绘图主要用来一比一打印的，所以需要这个纸张类。
    /// </summary>
    [Serializable]
    public class ClsPaper
    {
        private float _fltPaperWidth = 40, _fltPaperHeight = 40;//纸张的宽度和高度
        //private bool _isContinuousMedias = false;//是否是连续纸张
        private int _NumOfLables = 2;//每行的条形码纸个数
        private float _fltHorizontalRepeatDistance = 2f;//水平间距
        //private float _fltVarticalRepeatDistance=2f;//垂直间距

        //只有设置成如下的属性才能被属性选择器认识
        [DescriptionAttribute("纸张宽度"), CategoryAttribute("布局")]
        public float Width
        {
            get
            {
                return _fltPaperWidth;
            }
            set
            {
                _fltPaperWidth = value;
            }
        }
        [DescriptionAttribute("纸张高度"), CategoryAttribute("布局")]
        public float Height
        {
            get
            {
                return _fltPaperHeight;
            }
            set
            {
                _fltPaperHeight = value;
            }
        }

        [DescriptionAttribute("每行条形码纸个数"), CategoryAttribute("布局")]
        public int NumOfLables
        {
            get
            {
                return _NumOfLables;
            }
            set
            {
                _NumOfLables = value;
            }
        }

        [DescriptionAttribute("条形码纸之间的水平间距"), CategoryAttribute("布局")]
        public float HorizontalRepeatDistance
        {
            get
            {
                return _fltHorizontalRepeatDistance;
            }
            set
            {
                _fltHorizontalRepeatDistance = value;
            }
        }

        /**我暂时不想搞这个连续纸张，因为连续纸张只是高度加上垂直间隔，让用户直接设置高度增加就可以了。
        [DescriptionAttribute("条形码纸之间的垂直间距"), CategoryAttribute("布局")]
        public float VarticalRepeatDistance
        {
            get
            {
                return _fltVarticalRepeatDistance;
            }
            set
            {
                _fltVarticalRepeatDistance = value;
            }
        }

        [DescriptionAttribute("是否是连续纸张"), CategoryAttribute("布局")]
        public bool IsContinuousMedias
        {
            get
            {
                return _isContinuousMedias;
            }
            set
            {
                _isContinuousMedias = value;
            }

        }
         * */

    }
}
