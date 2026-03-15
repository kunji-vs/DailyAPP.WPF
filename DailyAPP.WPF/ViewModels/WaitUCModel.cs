using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF.ViewModels
{
    internal class WaitUCModel:BindableBase,INavigationAware
    {

        private string _drawerHostTitle = "添加待办";

        public string DrawerHostTitle
        {
            get { return _drawerHostTitle; }
            set { _drawerHostTitle = value; RaisePropertyChanged(); }
        }

        private enum WaitOperateType
        {
            Add,
            Edit,
        }

        private WaitOperateType OperateType = WaitOperateType.Add;

        private AccountInfoDTO AccountInfo;

        private WaitInfoDTO _operateWaitDataDTO = new WaitInfoDTO();

        public WaitInfoDTO OperateWaitDataDTO
        {
            get { return _operateWaitDataDTO; }
            set { _operateWaitDataDTO = value; RaisePropertyChanged(); }
        }


        HttpRestClient httpClient;

        private List<WaitInfoDTO> _waitList;

        public List<WaitInfoDTO> WaitList
        {
            get { return _waitList; }
            set { _waitList = value; RaisePropertyChanged(); }
        }

        public WaitUCModel(HttpRestClient httpRestClient )
        {
            httpClient = httpRestClient;
            OperateWaitDataDTO = new WaitInfoDTO();
        }

        void GetWaitData()
        {
            var req = new ApiRequest()
            {
                Route = "Data/GetWaitData?accountId=" + AccountInfo.AccountId,
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode == 200)
            {
                var waitinfo = JsonConvert.DeserializeObject<List<WaitInfoDTO>>(res.ResultData.ToString());
                WaitList = waitinfo.Where(x => x.Status == 0).ToList();
                return;
            }
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null) return;
            if(navigationContext.Parameters.ContainsKey("userInfo"))
            {
                var userInfo = navigationContext.Parameters["userInfo"] as AccountInfoDTO;
                if (userInfo != null)
                {
                    AccountInfo = userInfo;
                    GetWaitData();
                }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #region 添加待办
        private bool _isRightDrawerOpen = false;
        public bool IsRightDrawerOpen
        {
            get => _isRightDrawerOpen;
            set
            {
                _isRightDrawerOpen = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand ShowAddWaitCmm => new DelegateCommand(() =>
        {
            DrawerHostTitle = "添加待办";
            OperateWaitDataDTO = new WaitInfoDTO();
            OperateType = WaitOperateType.Add;
            IsRightDrawerOpen = true;
        });

        public DelegateCommand<WaitInfoDTO> ShowEditWaitCmm => new DelegateCommand<WaitInfoDTO> ((waitInfo) =>
        {
            OperateWaitDataDTO = waitInfo;
            DrawerHostTitle = "编辑待办";
            OperateType = WaitOperateType.Edit;
            IsRightDrawerOpen = true;
        });

        public DelegateCommand AddWaitCmm => new DelegateCommand(() =>
        {
            OperateWaitDataDTO.AccountId = AccountInfo.AccountId;
            OperateWaitDataDTO.CreateTime = DateTime.Now;
            var res = false;
            switch (OperateType)
            {
                case WaitOperateType.Add:
                    res = AddWaitData(OperateWaitDataDTO);
                    break;
                case WaitOperateType.Edit:
                    res = UpdateWaitData(OperateWaitDataDTO);
                    break;
                default:
                    break;
            }
            if(res)
            {
                GetWaitData();
                IsRightDrawerOpen = false;
                OperateWaitDataDTO = new WaitInfoDTO();
            }
        });

        public DelegateCommand<WaitInfoDTO> DeleteWaitDataCmm => new DelegateCommand<WaitInfoDTO>((waitInfo) =>
        {
            var res = DeleteWaitData(waitInfo);
            if(res)
            {
                GetWaitData();
            }
        });


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
            if (res.ResultCode != 200)
            {
                MessageBox.Show("添加失败！" + res.msg);
                return false;
            }
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
        /// <summary>
        /// 删除待办数据
        /// </summary>
        /// <param name="waitInfo"></param>
        /// <returns></returns>
        bool DeleteWaitData(WaitInfoDTO waitInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/DeleteWaitData?waitId="+waitInfo.WaitId,
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode != 200)
            {
                MessageBox.Show("操作失败！" + res.msg);
                return false;
            }
            return true;
        }

        #endregion

    }
}
