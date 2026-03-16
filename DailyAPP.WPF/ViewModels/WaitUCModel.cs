using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.Models;
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

        private string _drawerButtonTitle = "添加";
        public string DrawerButtonTitle
        {
            get { return _drawerButtonTitle; }
            set { _drawerButtonTitle = value; RaisePropertyChanged(); }
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

        private List<WaitInfoDTO> _allWaitList;

        public List<WaitInfoDTO> AllWaitList
        {
            get { return _allWaitList; }
            set { _allWaitList = value; }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set { _filterText = value; RaisePropertyChanged(); }
        }

        private int _selectedFilterIndex = 0;
        public int SelectedFilterIndex
        {
            get => _selectedFilterIndex;
            set { _selectedFilterIndex = value; RaisePropertyChanged(); }
        }

        public DelegateCommand FilterCommand => new DelegateCommand(() =>
        {
            if (AllWaitList == null)
                return;

            IEnumerable<WaitInfoDTO> result = AllWaitList;

            // filter by status based on SelectedFilterIndex: 0=All,1=待办(0),2=已完成(1)
            if (SelectedFilterIndex == 1)
            {
                result = result.Where(x => x.Status == 0);
            }
            else if (SelectedFilterIndex == 2)
            {
                result = result.Where(x => x.Status == 1);
            }

            // filter by title if provided (case-insensitive contains)
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                var key = FilterText.Trim();
                result = result.Where(x => !string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            WaitList = result.ToList();
        });

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
                AllWaitList = waitinfo.ToList();
                // 默认展示全部
                WaitList = AllWaitList.ToList();
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
            DrawerButtonTitle = "添加";
            OperateWaitDataDTO = new WaitInfoDTO();
            OperateType = WaitOperateType.Add;
            IsRightDrawerOpen = true;
        });

        public DelegateCommand<WaitInfoDTO> ShowEditWaitCmm => new DelegateCommand<WaitInfoDTO> ((waitInfo) =>
        {
            OperateWaitDataDTO = waitInfo;
            DrawerHostTitle = "编辑待办";
            DrawerButtonTitle = "保存";
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
