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


