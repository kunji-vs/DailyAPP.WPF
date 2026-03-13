using DailyAPP.WPF.DTOs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.ViewModels
{
    internal class WaitUCModel:BindableBase
    {


        private List<WaitInfoDTO> _waitList;

        public List<WaitInfoDTO> WaitList
        {
            get { return _waitList; }
            set { _waitList = value; RaisePropertyChanged(); }
        }

        public WaitUCModel()
        {
            CreateData1();
        }

        void CreateData1()
        {
            WaitList = new List<WaitInfoDTO>
            {
                new WaitInfoDTO { Title = "待办事项1 ", Content = "这是待办事项1 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项2 ", Content = "这是待办事项2 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项3 ", Content = "这是待办事项3 的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项4 ", Content = "这是待办事项4 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项5 ", Content = "这是待办事项5 的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项6 ", Content = "这是待办事项6 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项7 ", Content = "这是待办事项7 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项8 ", Content = "这是待办事项8 的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项9 ", Content = "这是待办事项9 的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项10", Content = "这是待办事项10的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项11", Content = "这是待办事项11的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项12", Content = "这是待办事项12的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项13", Content = "这是待办事项13的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项14", Content = "这是待办事项14的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项15", Content = "这是待办事项15的内容", Status = 1 },
                new WaitInfoDTO { Title = "待办事项16", Content = "这是待办事项16的内容", Status = 0 },
                new WaitInfoDTO { Title = "待办事项17", Content = "这是待办事项17的内容", Status = 0 },
            };
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
            IsRightDrawerOpen = true;
        });

        #endregion

    }
}
