using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.ViewModels
{
    internal class HomeUCModel : BindableBase
    {
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


        void CreateCardData()
        {
            StatPanelList = new List<StatPanelInfo>
            {
                new StatPanelInfo { Icon = "ClockFast", Name = "汇总", BackgroundColor = "#0BA0FD",ViewName="WaitUC",Result="9" },
                new StatPanelInfo { Icon = "ClockCheckOutline", Name = "已完成", BackgroundColor = "#1DCA38",ViewName="WaitUC",Result="12" },
                new StatPanelInfo { Icon = "ChartLineVariant", Name = "完成比例", BackgroundColor = "#01C7DC",Result="90%" },
                new StatPanelInfo { Icon = "PlaylistStar", Name = "备忘录", BackgroundColor = "#FDA100",ViewName="MemoUC",Result="20" },

            };
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

        public HomeUCModel()
        {
            CreateCardData();
            CreateData1();
            CreateData2();
        }
    }
}
