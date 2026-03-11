using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.Models;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.ViewModels
{
    internal class HomeUCModel : BindableBase,INavigationAware
    {

        private string _loginInfo;

        public string LoginInfo
        {
            get { return _loginInfo; }
            set { _loginInfo = value; RaisePropertyChanged(); }
        }


        private List<StatPanelInfo> _statPanelList;

        public List<StatPanelInfo> StatPanelList
        {
            get { return _statPanelList; }
            set { _statPanelList = value; RaisePropertyChanged(); }
        }

        private List<WaitInfoDTO> _waitList;

        public List<WaitInfoDTO> WaitList
        {
            get { return _waitList; }
            set { _waitList = value; RaisePropertyChanged(); }
        }

        private List<MemoInfoDTO> _memoList;

        public List<MemoInfoDTO> MemoList
        {
            get { return _memoList; }
            set { _memoList = value; }
        }

        private AccountInfoDTO AccountInfo;

        private readonly HttpRestClient httpClient;
        public HomeUCModel(HttpRestClient httpClient)
        {
            CreateData1();
            CreateData2();
            this.httpClient = httpClient;
        }


        void CreateCardData()
        {

            StatPanelList = new List<StatPanelInfo>
            {
                new StatPanelInfo { Icon = "ClockFast", Name = "汇总", BackgroundColor = "#0BA0FD",ViewName="WaitUC",Result="9" },
                new StatPanelInfo { Icon = "ClockCheckOutline", Name = "已完成", BackgroundColor = "#1DCA38",ViewName="WaitUC",Result="12" },
                new StatPanelInfo { Icon = "ChartLineVariant", Name = "完成比例", BackgroundColor = "#01C7DC",Result="90%" },
                new StatPanelInfo { Icon = "PlaylistStar", Name = "备忘录", BackgroundColor = "#FDA100",ViewName="MemoUC",Result="20" },

            };
            ApiRequest req = new ApiRequest()
            {
                Route = $"Data/GetStatWaitData?accountId={AccountInfo.AccountId}",
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode == 200)
            {
                var statWaitInfo = JsonConvert.DeserializeObject<StatWaitDTO>(res.ResultData.ToString());
                StatPanelList[0].Result = statWaitInfo.TotalCount.ToString();
                StatPanelList[1].Result = statWaitInfo.FinishCount.ToString();
                StatPanelList[2].Result = statWaitInfo.FinishPercent;
            }
        }

        void CreateData1()
        {

            WaitList = new List<WaitInfoDTO>
            {
                new WaitInfoDTO { WaitId = 1, Title = "待办事项1", Content = "这是待办事项1的内容", Status = 0 },
                new WaitInfoDTO { WaitId = 2, Title = "待办事项2", Content = "这是待办事项2的内容", Status = 0 },
                new WaitInfoDTO { WaitId = 3, Title = "待办事项3", Content = "这是待办事项3的内容", Status = 1 },
                new WaitInfoDTO { WaitId = 4, Title = "待办事项4", Content = "这是待办事项4的内容", Status = 0 },
                new WaitInfoDTO { WaitId = 5, Title = "待办事项5", Content = "这是待办事项5的内容", Status = 1 },
            };
        }

        void CreateData2()
        {
            MemoList = new List<MemoInfoDTO>
            {
                new MemoInfoDTO { MemoId = 1, Title = "备忘录1", Content = "这是备忘录1的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 2, Title = "备忘录2", Content = "这是备忘录2的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 3, Title = "备忘录3", Content = "这是备忘录3的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 4, Title = "备忘录4", Content = "这是备忘录4的内容", Status = 0 },
            };
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //用来接收
            if(navigationContext != null && navigationContext.Parameters.ContainsKey("userInfo"))
            {
                AccountInfo = navigationContext.Parameters.GetValue<AccountInfoDTO>("userInfo");
                LoginInfo = $"欢迎您，{AccountInfo.Name}！ 今天是：{DateTime.Now.ToString("yyyy年MM月dd日 ddd")}";
                CreateCardData();
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //处理拦截
        }



    }
}
