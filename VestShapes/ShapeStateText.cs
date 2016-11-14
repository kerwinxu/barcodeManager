using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{
    public class ShapeStateText : shapeSingleText
    {
        [DescriptionAttribute("文字"), DisplayName("文字"), CategoryAttribute("文字")]
        [XmlElement]
        public string Text
        {
            get
            {
                return DefaultText;
            }
            set
            {
                DefaultText = value;
                PreFix = "";
                Suffix = "";
                UpdateWidthHeight();

            }
        }

    }
}
