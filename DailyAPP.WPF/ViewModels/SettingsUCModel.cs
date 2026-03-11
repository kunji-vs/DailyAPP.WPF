using DailyAPP.WPF.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DailyAPP.WPF.ViewModels
{
    internal class SettingsUCModel:BindableBase
    {

        private readonly IRegionManager regionManager;

        public SettingsUCModel(IRegionManager _regionManager)
        {
            CreateMenuList();
            regionManager = _regionManager;
        }

        private List<LeftMenuInfo> _leftMenuList;

        public List<LeftMenuInfo> LeftMenuList
        {
            get { return _leftMenuList; }
            set { _leftMenuList = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 创建菜单数据
        /// </summary>
        private void CreateMenuList()
        {
            LeftMenuList = new List<LeftMenuInfo>();
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIconName = "Palette",
                MenuName = "个性化",
                ViewName = "PersonalUC"
            });
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIconName = "Cog",
                MenuName = "系统设置",
                ViewName = "SysSetUC"
            });
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIconName = "Information",
                MenuName = "关于更多",
                ViewName = "AboutUs"
            });
        }

        public DelegateCommand<LeftMenuInfo> NavigateCommand => new DelegateCommand<LeftMenuInfo>((menu) =>
        {
            //导航到对应的页面
            if (menu != null)
            {
                regionManager.Regions["SettingRegion"].RequestNavigate(menu.ViewName);
            }
        });

    }
}
