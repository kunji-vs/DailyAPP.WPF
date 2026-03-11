using AutoMapper;
using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
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

        IMapper mapper;

        HttpRestClient httpRestClient;

        WindowState windowState = WindowState.Normal;

        public MainWinViewModel(IEventAggregator _eventAggregator, IRegionManager _regionManager,IMapper _mapper, HttpRestClient httpRestClient)
        {
            _maximizeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/max.png", UriKind.Absolute));
            _userIcon = new BitmapImage(new Uri("pack://application:,,,/Images/user2.png", UriKind.Absolute));
            eventAggregator = _eventAggregator;
            regionManager = _regionManager;
            LeftMenuList = new List<LeftMenuInfo>();
            mapper = _mapper;
            this.httpRestClient = httpRestClient;
        }

        private List<LeftMenuInfo> _leftMenuList;

        private AccountInfoDTO AccountInfo;

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
        public void CreateMenuList(AccountInfoDTO accountInfo)
        {
            #region 手动创建菜单
            //LeftMenuList.Add(new LeftMenuInfo()
            //{
            //    MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/shouye.png", UriKind.Absolute)),
            //    MemuIconName= "Home",
            //    MenuName = "首页",
            //    ViewName = "HomeUC"
            //});
            //LeftMenuList.Add(new LeftMenuInfo()
            //{
            //    MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/daiban.png", UriKind.Absolute)),
            //    MemuIconName = "NotebookOutline",
            //    MenuName = "代办事项",
            //    ViewName = "WaitUC"
            //});
            //LeftMenuList.Add(new LeftMenuInfo()
            //{
            //    MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/beiwanglu.png", UriKind.Absolute)),
            //    MemuIconName= "NotebookPlus",
            //    MenuName = "备忘录",
            //    ViewName = "MemoUC"
            //});
            //LeftMenuList.Add(new LeftMenuInfo()
            //{
            //    MenuIcon = new BitmapImage(new Uri("pack://application:,,,/Images/shezhi.png", UriKind.Absolute)),
            //    MemuIconName= "Cog",
            //    MenuName = "设置",
            //    ViewName = "SettingsUC"
            //}); 
            #endregion

            ApiRequest apiRequest = new ApiRequest()
            {
                Route = "Data/GetMenuData",
                Method = RestSharp.Method.GET,
            };

            var res = httpRestClient.Excute(apiRequest);
            if(res.ResultCode == 200)
            {
                var menuInfoDTO = JsonConvert.DeserializeObject<List<MenuInfoDTO>>(res.ResultData.ToString());
                LeftMenuList = mapper.Map<List<LeftMenuInfo>>(menuInfoDTO);
                AccountInfo = accountInfo;
                NavigateCommand.Execute(LeftMenuList.First());
            }
            else
            {
                MessageBox.Show("菜单数据加载失败！");
                Environment.Exit(0);
            }

        }

        #region 区域导航
        private bool _isLeftDrawerOpen = false;
        public bool IsLeftDrawerOpen
        {
            get => _isLeftDrawerOpen;
            set
            {
                _isLeftDrawerOpen = value;
                RaisePropertyChanged();
            }
        }

        //导航管理器
        private readonly IRegionManager regionManager;

        private IRegionNavigationJournal journal;

        public DelegateCommand<LeftMenuInfo> NavigateCommand => new DelegateCommand<LeftMenuInfo>((menu) =>
        {
            if (menu != null)
            {
                NavigationParameters para = new NavigationParameters();
                para.Add("userInfo", AccountInfo);
                regionManager.Regions["ContentRegion"].RequestNavigate(menu.ViewName,callback=>
                {
                    journal = callback.Context.NavigationService.Journal;
                }, para);
            }
            IsLeftDrawerOpen = false;
        });

        public DelegateCommand MovePrevCommand => new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoBack)
            {
                journal.GoBack();
            }
        });

        public DelegateCommand MoveNextCommand => new DelegateCommand(() =>
        {
            if (journal != null && journal.CanGoForward)
            {
                journal.GoForward();
            }
        });

        #endregion

    }
}
