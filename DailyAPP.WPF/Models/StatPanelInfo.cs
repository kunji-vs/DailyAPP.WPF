using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.Models
{
    public class StatPanelInfo
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        /// 跳转界面名称
        /// </summary>
        public string ViewName { get; set; }
    }
}
