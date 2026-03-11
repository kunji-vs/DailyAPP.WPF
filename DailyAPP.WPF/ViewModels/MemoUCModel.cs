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
    internal class MemoUCModel:BindableBase
    {
        private List<MemoInfoDTO> _memoList;

        public List<MemoInfoDTO> MemoList
        {
            get { return _memoList; }
            set { _memoList = value; RaisePropertyChanged(); }
        }

        public MemoUCModel()
        {
            CreateData1();
        }

        void CreateData1()
        {
            MemoList = new List<MemoInfoDTO>
            {
                new MemoInfoDTO { MemoId = 1 , Title = "备忘录1 ", Content = "这是待备忘录1 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 2 , Title = "备忘录2 ", Content = "这是待备忘录2 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 3 , Title = "备忘录3 ", Content = "这是待备忘录3 的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 4 , Title = "备忘录4 ", Content = "这是待备忘录4 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 5 , Title = "备忘录5 ", Content = "这是待备忘录5 的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 6 , Title = "备忘录6 ", Content = "这是待备忘录6 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 7 , Title = "备忘录7 ", Content = "这是待备忘录7 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 8 , Title = "备忘录8 ", Content = "这是待备忘录8 的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 9 , Title = "备忘录9 ", Content = "这是待备忘录9 的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 10, Title = "备忘录10", Content = "这是待备忘录10的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 11, Title = "备忘录11", Content = "这是待备忘录11的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 12, Title = "备忘录12", Content = "这是待备忘录12的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 13, Title = "备忘录13", Content = "这是待备忘录13的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 14, Title = "备忘录14", Content = "这是待备忘录14的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 15, Title = "备忘录15", Content = "这是待备忘录15的内容", Status = 1 },
                new MemoInfoDTO { MemoId = 16, Title = "备忘录16", Content = "这是待备忘录16的内容", Status = 0 },
                new MemoInfoDTO { MemoId = 17, Title = "备忘录17", Content = "这是待备忘录17的内容", Status = 0 },
            };
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
            IsRightDrawerOpen = true;
        });

        #endregion
    }
}
