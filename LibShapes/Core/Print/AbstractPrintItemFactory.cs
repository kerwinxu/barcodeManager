using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    public abstract class AbstractPrintItemFactory : IPrintItemFactory
    {
        /// <summary>
        ///  用这个来保存PrintItem
        /// </summary>
        protected Dictionary<string, PrintItem> dict_printItem = new Dictionary<string, PrintItem>();


        public PrintItem GetPrintItem(string name)
        {
            if (dict_printItem.ContainsKey(name))
            {
                return dict_printItem[name];
            }
            return null;
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 删除，受到保护的，外部不能随便调用的。
        /// </summary>
        /// <param name="name"></param>
        protected void deletePrintItem(string name)
        {
            dict_printItem.Remove(name);
            //throw new NotImplementedException();
        }

        /// <summary>
        ///  外部不能随便的增加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="printItem"></param>
        protected void registerPrintItem(string name, PrintItem printItem)
        {
            if (! dict_printItem.ContainsKey(name))
            {
                // 没有才添加
                dict_printItem[name] = printItem;
            }
            //throw new NotImplementedException();
        }

        public string registerPrintItem(PrintItem printItem)
        {
            string id = getNoRepeatId();       // 先取得一个不重复的id
            registerPrintItem(id, printItem);  // 注册
            return id;                         // 返回这个id
        }

        public abstract string getNoRepeatId();

    }
}
