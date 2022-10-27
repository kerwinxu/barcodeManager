using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Command
{
    public class CommandRecorder:ICommandRecorder
    {
        // 保存一堆的命令
        private List<ICommand> commands = new List<ICommand>();
        // 当前的下标
        private int current_index = -1; 

        public void addCommand(ICommand command)
        {
            // 这里要判断是否是最后一个
            if (current_index < commands.Count - 1)
            {
                // 说明后边还有，首先删除后边的
                commands.RemoveRange(current_index + 1, commands.Count - current_index-1);
            }
            // 添加进去
            commands.Add(command);
            // 重新设置
            current_index += 1;
        }

        public bool isRedoAble()
        {
            return current_index < this.commands.Count - 1;
            //throw new NotImplementedException();
        }

        public bool isUndoAble()
        {
            return current_index > 0;
            //throw new NotImplementedException();
        }

        public void Redo()
        {
            // 
            current_index += 1;// 往后退一步
            if (current_index >= commands.Count) current_index = commands.Count - 1;// 不能再前进了。
            // 这里表示有操作
            if (current_index >= 0 && current_index < commands.Count)
            {
                commands[current_index].Redo();
            }
            
        }

        public void Undo()
        {
            // 这里表示有操作
            if (current_index >= 0 && current_index < commands.Count)
            {
                commands[current_index].Undo();
            }
            current_index -= 1;// 往后退一步
            if (current_index < 0) current_index = -1;// 有最小的不能再退
        }
    }
}
