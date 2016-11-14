中文版：
	这个类库的主要类是如下：
		UserControlCanvas ： 这个是当画布类
		UserControlToolBox： 这个是工具栏、显示修改属性的
		Shapes ： 这个是当形状类，保存的是这个矢量绘图的所以信息的，是可以以XML文件保存的，主要包含的类有
			ClsPageSettings ： 这个是保存的是页面的设置，包括纸张长宽之类的信息
			ShapeEle ： 这个是形状元素的基类，包括如下的子类
				ShapeLine ：直线形状
				...
	运行流程是
		1、 UserControlCanvas 有个print事件（UserControl1_Paint），这个事件做的事情有如下价格
			创建双缓冲区等其他设置

	这个库杂谈
		1、这个库绘制是靠的是DrawPath()来绘制边界的，以FillPath()来填充的，我这里是以 getGraphicsPath()
		来取得各个图形的GraphicsPath。这个方法是有几个重载方法，
			getGraphicsPath();//这个是取得没有偏移的路径的，很简单，作为这个ShapeEle，是没必要知道横向偏移或者纵向偏移之类的信息的，这个是由外部控制的。
			getGraphicsPath(List<Matrix> listMatrix);//这个的参数是变换参数，外部的使用者可以选择偏移等数据
			getGraphicsPath(float fltKongX, float fltKongY);//这个参数是特定的2个偏移。

		2、这个画布包含几个特殊的属性，一个是横向偏移，一个是纵向偏移，一个是放大倍数。
			其中横向偏移和纵向偏移是一对，这个就好比你移动画布，图形也跟着移动一样
			而放大倍数必须放在ShapeEle中，原因是线条的粗细、字体的大小等，都跟这个严重相关的。比如说如下的
				//这个是画笔的宽度，你可以看到，是跟放大倍数非常相关的。
		        [DescriptionAttribute("画笔宽度"), DisplayName("画笔宽度"), CategoryAttribute("设计")]
				[XmlElement]
				public float PenWidth
					{
						get
							{
								return _penWidth / Zoom;
							}
						set
							{
								_penWidth = value * Zoom;
							}
					}


