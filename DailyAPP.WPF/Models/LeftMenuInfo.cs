using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DailyAPP.WPF.Models
{
    public class LeftMenuInfo
    {
        public string Id { get; set; }

        ///// <summary>
        ///// 菜单头像
        ///// </summary>
        //public BitmapImage MenuIcon { get; set; }

        public string MenuIconName { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 视图名称
        /// </summary>
        public string ViewName { get; set; }
    }
}
