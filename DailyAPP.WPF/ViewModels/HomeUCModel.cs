using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyAPP.WPF.ViewModels
{
    internal class HomeUCModel : BindableBase,INavigationAware
    {

        private readonly IDialogService dialogService;


        private string _loginInfo;

        private StatWaitDTO _statWaitInfo;

        public StatWaitDTO StatWaitInfo
        {
            get { return _statWaitInfo; }
            set { _statWaitInfo = value; UpdateCardData(); }
        }

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
            set { _memoList = value; RaisePropertyChanged(); }
        }

        private AccountInfoDTO AccountInfo;

        private readonly HttpRestClient httpClient;

        private readonly IRegionManager regionManager;

        public HomeUCModel(HttpRestClient httpClient, IDialogService _dialogService,IRegionManager _regionManager)
        {
            CreateData2();
            this.httpClient = httpClient;
            dialogService = _dialogService;
            regionManager = _regionManager;
        }


        public DelegateCommand ShowAddWaitUC => new DelegateCommand(() =>
        {
            var param = new DialogParameters();
            param.Add("userInfo", AccountInfo);
            param.Add("title", "添加待办");
            dialogService.ShowDialog("AddWaitUC", param, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    if (callback.Parameters.ContainsKey("waitInfo"))
                    {
                        var waitInfo = callback.Parameters.GetValue<WaitInfoDTO>("waitInfo");
                        waitInfo.CreateTime = DateTime.Now;
                        var success = AddWaitData(waitInfo);
                        if(success)
                        {
                            GetWaitData();
                        }
                    }
                }
            });
        });

        public DelegateCommand ShowAddMemoUC => new DelegateCommand(() =>
        {
            var param = new DialogParameters();
            param.Add("userInfo", AccountInfo);
            param.Add("title", "添加备忘录");
            dialogService.ShowDialog("AddMemoUC", param, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    if (callback.Parameters.ContainsKey("memoInfo"))
                    {
                        var waitInfo = callback.Parameters.GetValue<MemoInfoDTO>("memoInfo");
                        waitInfo.CreateTime = DateTime.Now;
                        var success = AddMemoData(waitInfo);
                        if (success)
                        {
                            GetMemoData();
                        }
                    }
                }
            });
        });

        public DelegateCommand<WaitInfoDTO> ShowEditWaitUC => new DelegateCommand<WaitInfoDTO>((waitInfo) =>
        {
            var param = new DialogParameters();
            param.Add("userInfo", AccountInfo);
            param.Add("waitInfo", waitInfo);
            param.Add("title", "编辑待办");
            dialogService.ShowDialog("AddWaitUC", param, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    if (callback.Parameters.ContainsKey("waitInfo"))
                    {
                        var waitInfo = callback.Parameters.GetValue<WaitInfoDTO>("waitInfo");
                        var success = UpdateWaitData(waitInfo);
                        if (success)
                        {
                            GetWaitData();
                        }
                    }
                }
            });
        });

        public DelegateCommand<StatPanelInfo> ShowDetailCommand => new DelegateCommand<StatPanelInfo>((panelInfo) =>
        {
            if(string.IsNullOrEmpty(panelInfo.ViewName))
            {
                return;
            }
            NavigationParameters para = new NavigationParameters();
            para.Add("userInfo", AccountInfo);
            regionManager.Regions["ContentRegion"].RequestNavigate(panelInfo.ViewName, para);
        });

        /// <summary>
        /// 创建主页卡片数据
        /// </summary>
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
                StatWaitInfo = JsonConvert.DeserializeObject<StatWaitDTO>(res.ResultData.ToString());
            }
        }

        void UpdateCardData()
        {
            StatPanelList[0].Result = StatWaitInfo.TotalCount.ToString();
            StatPanelList[1].Result = StatWaitInfo.FinishCount.ToString();
            StatPanelList[2].Result = StatWaitInfo.FinishPercent;
            StatPanelList[3].Result = StatWaitInfo.MemoCount.ToString();
        }

        /// <summary>
        /// 获取待办事项数据
        /// </summary>
        void GetWaitData()
        {
            var req = new ApiRequest()
            {
                Route = "Data/GetWaitData?accountId=" + AccountInfo.AccountId,
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if(res.ResultCode == 200)
            {
                var waitinfo = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(res.ResultData.ToString());
                WaitList = waitinfo.Where(x=>x.Status==0).ToList();
                return;
            }
        }

        void GetMemoData()
        {
            var req = new ApiRequest()
            {
                Route = "Data/GetMemoData?accountId=" + AccountInfo.AccountId,
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if (res?.ResultCode == 200)
            {
                var memoinfo = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(res.ResultData.ToString());
                MemoList = memoinfo.Where(x => x.Status == 0).ToList();
            }
        }
        /// <summary>
        /// 添加待办数据
        /// </summary>
        /// <param name="waitInfo"></param>
        bool AddWaitData(WaitInfoDTO waitInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/AddWaitData",
                Method = RestSharp.Method.POST,
                Parameters = waitInfo,
            };
            var res = httpClient.Excute(req);
            if(res.ResultCode != 200)
            {
               MessageBox.Show("添加失败！"+res.msg);
                return false;
            }
            StatWaitInfo.TotalCount++;
            UpdateCardData();
            return true;
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="memoInfo"></param>
        bool AddMemoData(MemoInfoDTO memoInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/AddMemoData",
                Method = RestSharp.Method.POST,
                Parameters = memoInfo,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode != 200)
            {
                MessageBox.Show("添加失败！" + res.msg);
                return false;
            }
            StatWaitInfo.MemoCount++;
            UpdateCardData();
            return true;
        }

        /// <summary>
        /// 更新待办数据
        /// </summary>
        /// <param name="waitInfo"></param>
        /// <returns></returns>
        bool UpdateWaitData(WaitInfoDTO waitInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/UpdateWaitData",
                Method = RestSharp.Method.POST,
                Parameters = waitInfo,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode != 200)
            {
                MessageBox.Show("操作失败！" + res.msg);
                return false;
            }
            return true;
        }


        public DelegateCommand<WaitInfoDTO> CompleteWaitCom => new DelegateCommand<WaitInfoDTO>((waitInfo) =>
        {
            var para = new DialogParameters();
            para.Add("message", $"确定要完成待办事项：{waitInfo.Title}吗？");
            var dialogResult = ButtonResult.OK;
            dialogService.ShowDialog("CustomMessageBox", para, callback =>
            {
                dialogResult = callback.Result;

            });
            if (dialogResult != ButtonResult.OK)
            {
                return;
            }
            var req = new ApiRequest()
            {
                Route = "Data/UpdateWaitData",
                Method = RestSharp.Method.POST,
                Parameters = waitInfo,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode != 200)
            {
                MessageBox.Show("操作失败！" + res.msg);
                return;
            }
            GetWaitData();
            StatWaitInfo.FinishCount++;
            UpdateCardData();
        });

        

        public DelegateCommand<MemoInfoDTO> CompleteMemoCom => new DelegateCommand<MemoInfoDTO>((memoInfo) =>
        {
            var para = new DialogParameters();
            para.Add("message", $"确定要完成备忘录：{memoInfo.Title}吗？");
            var dialogResult = ButtonResult.OK;
            dialogService.ShowDialog("CustomMessageBox", para, callback =>
            {
                dialogResult = callback.Result;
            });
            if (dialogResult != ButtonResult.OK)
            {
                return;
            }
            var req = new ApiRequest()
            {
                Route = "Data/UpdateMemoData",
                Method = RestSharp.Method.POST,
                Parameters = memoInfo,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode != 200)
            {
                MessageBox.Show("操作失败！" + res.msg);
                return;
            }
            GetMemoData();
            StatWaitInfo.MemoCount--;
            UpdateCardData();

        });

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
                GetWaitData();
                GetMemoData();
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
