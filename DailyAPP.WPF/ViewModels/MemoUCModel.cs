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
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF.ViewModels
{
    internal class MemoUCModel:BindableBase, INavigationAware
    {
        private string _drawerHostTitle = "添加备忘录";
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

        private WaitOperateType OperateType = WaitOperateType.Add;


        private AccountInfoDTO AccountInfo;

        HttpRestClient httpClient;


        private MemoInfoDTO _operateMemoDataDTO = new MemoInfoDTO();

        public MemoInfoDTO OperateMemoDataDTO
        {
            get { return _operateMemoDataDTO; }
            set { _operateMemoDataDTO = value; RaisePropertyChanged(); }
        }

        private List<MemoInfoDTO> _allMemoList;

        public List<MemoInfoDTO> AllMemoList
        {
            get { return _allMemoList; }
            set { _allMemoList = value; }
        }

        private List<MemoInfoDTO> _memoList;


        public List<MemoInfoDTO> MemoList
        {
            get { return _memoList; }
            set { _memoList = value; RaisePropertyChanged(); }
        }

        public MemoUCModel(HttpRestClient httpClient)
        {
            this.httpClient = httpClient;
            OperateMemoDataDTO = new MemoInfoDTO();
        }

        #region 添加备忘
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

        public DelegateCommand ShowAddMemoCmm => new DelegateCommand(() =>
        {
            DrawerHostTitle = "添加备忘录";
            DrawerButtonTitle = "添加";
            OperateMemoDataDTO = new MemoInfoDTO();
            OperateType = WaitOperateType.Add;
            IsRightDrawerOpen = true;
        });

        public DelegateCommand<MemoInfoDTO> ShowEditMemoCmm => new DelegateCommand<MemoInfoDTO>((memoInfo) =>
        {
            OperateMemoDataDTO = memoInfo;
            DrawerHostTitle = "编辑备忘录";
            DrawerButtonTitle = "保存";
            OperateType = WaitOperateType.Edit;
            IsRightDrawerOpen = true;
        });

        public DelegateCommand AddMemoCmm => new DelegateCommand(() =>
        {
            OperateMemoDataDTO.AccountId = AccountInfo.AccountId;
            OperateMemoDataDTO.CreateTime = DateTime.Now;
            var res = false;
            switch (OperateType)
            {
                case WaitOperateType.Add:
                    res = AddMemoData(OperateMemoDataDTO);
                    break;
                case WaitOperateType.Edit:
                    res = UpdateMemoData(OperateMemoDataDTO);
                    break;
                default:
                    break;
            }
            if (res)
            {
                GetMemoData();
                IsRightDrawerOpen = false;
                OperateMemoDataDTO = new MemoInfoDTO();
            }
        });

        public DelegateCommand<MemoInfoDTO> DeleteWaitDataCmm => new DelegateCommand<MemoInfoDTO>((waitInfo) =>
        {
            var res = DeleteMemoData(waitInfo);
            if (res)
            {
                GetMemoData();
            }
        });

        public DelegateCommand FilterCommand => new DelegateCommand(() =>
        {
            if (AllMemoList == null)
                return;

            IEnumerable<MemoInfoDTO> result = AllMemoList;

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

            MemoList = result.ToList();
        });

        void GetMemoData()
        {
            var req = new ApiRequest()
            {
                Route = "Data/GetMemoData?accountId=" + AccountInfo.AccountId,
                Method = RestSharp.Method.GET,
            };
            var res = httpClient.Excute(req);
            if (res.ResultCode == 200)
            {
                var memoinfo = JsonConvert.DeserializeObject<List<MemoInfoDTO>>(res.ResultData.ToString());
                AllMemoList = memoinfo.ToList();
                // 默认展示全部
                MemoList = AllMemoList.ToList();
                return;
            }

        }

        /// <summary>
        /// 添加备忘录数据
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
            return true;
        }

        /// <summary>
        /// 更新备忘录数据
        /// </summary>
        /// <param name="MemoInfo"></param>
        /// <returns></returns>
        bool UpdateMemoData(MemoInfoDTO MemoInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/UpdateMemoData",
                Method = RestSharp.Method.POST,
                Parameters = MemoInfo,
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
        /// 删除备忘录数据
        /// </summary>
        /// <param name="memoInfo"></param>
        /// <returns></returns>
        bool DeleteMemoData(MemoInfoDTO memoInfo)
        {
            var req = new ApiRequest()
            {
                Route = "Data/DeleteMemoData?memoId=" + memoInfo.MemoId,
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null) return;
            if (navigationContext.Parameters.ContainsKey("userInfo"))
            {
                var userInfo = navigationContext.Parameters["userInfo"] as AccountInfoDTO;
                if (userInfo != null)
                {
                    AccountInfo = userInfo;
                    GetMemoData();
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

        #endregion
    }
}
