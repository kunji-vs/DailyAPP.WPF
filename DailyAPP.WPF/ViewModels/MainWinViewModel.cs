using DailyAPP.WPF.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DailyAPP.WPF.ViewModels
{
    public class MainWinViewModel : BindableBase
    {
        IEventAggregator eventAggregator;

        WindowState windowState = WindowState.Normal;

        public MainWinViewModel(IEventAggregator _eventAggregator,IRegionManager _regionManager)
        {
            _maximizeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/max.png", UriKind.Absolute));
            _userIcon = new BitmapImage(new Uri("pack://application:,,,/Images/user2.png", UriKind.Absolute));
            eventAggregator = _eventAggregator;
            regionManager = _regionManager;
            LeftMenuList = new List<LeftMenuInfo>();
            CreateMenuList();
        }

        private List<LeftMenuInfo> _leftMenuList;

        public List<LeftMenuInfo> LeftMenuList
        {
            get { return _leftMenuList; }
            set { _leftMenuList = value; RaisePropertyChanged(); }
        }


        private BitmapImage _userIcon;

        public BitmapImage UserIcon
        {
            get { return _userIcon; }
            set { _userIcon = value; }
        }


        private BitmapImage _maximizeIcon;

        public BitmapImage MaximizeIcon
        {
            get => _maximizeIcon;
            set
            {
                _maximizeIcon = value;
                RaisePropertyChanged();
            }
        }


        public DelegateCommand MaximizeCommand => new DelegateCommand(() =>
        {
            if (windowState == WindowState.Normal)
            {
                eventAggregator.GetEvent<Events.WindowStateChangedEvent>().Publish(WindowState.Maximized);
                MaximizeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/maxback.png", UriKind.Absolute));
                windowState = WindowState.Maximized;
            }
            else
            {
                eventAggregator.GetEvent<Events.WindowStateChangedEvent>().Publish(WindowState.Normal);
                MaximizeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/max.png", UriKind.Absolute));
                windowState = WindowState.Normal;
            }
        });

        public DelegateCommand MinimizeCommand => new DelegateCommand(() =>
        {
            eventAggregator.GetEvent<Events.WindowStateChangedEvent>().Publish(WindowState.Minimized);
        });

        public DelegateCommand CloseCommand => new DelegateCommand(() =>
        {
            eventAggregator.GetEvent<Events.WindowsCloseEvent>().Publish();
        });

        /// <summary>
        /// 创建菜单数据
        /// </summary>
        private void CreateMenuList()
        {
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/shouye.png", UriKind.Absolute)),
                MenuName = "首页",
                ViewName = "HomeUC"
            });
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/daiban.png", UriKind.Absolute)),
                MenuName = "代办事项",
                ViewName = "WaitUC"
            });
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/beiwanglu.png", UriKind.Absolute)),
                MenuName = "备忘录",
                ViewName = "MemoUC"
            });
            LeftMenuList.Add(new LeftMenuInfo()
            {
                MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/shezhi.png", UriKind.Absolute)),
                MenuName = "设置",
                ViewName = "SettingsUC"
            });
        }

        #region 区域导航
        //导航管理器
        private IRegionManager regionManager;

        public DelegateCommand<LeftMenuInfo> NavigateCommand => new DelegateCommand<LeftMenuInfo>((menu) =>
        {
            if (menu != null)
            {
                regionManager.Regions["ContentRegion"].RequestNavigate(menu.ViewName);
            }
        });

        #endregion

    }
}
