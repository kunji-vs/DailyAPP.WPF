using DailyAPP.WPF.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DailyAPP.WPF.Views
{
    /// <summary>
    /// MainWin.xaml 的交互逻辑
    /// </summary>
    public partial class MainWin : Window
    {
        private readonly IEventAggregator _eventAggregator;

        public MainWin(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<WindowStateChangedEvent>().Subscribe(OnWindowStateChangedEvent);

            _eventAggregator.GetEvent<WindowsCloseEvent>().Subscribe(OnWindowsCloseEvent);
        }

        void OnWindowStateChangedEvent(WindowState state)
        {
            this.WindowState = state;
        }

        void OnWindowsCloseEvent()
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _eventAggregator.GetEvent<WindowStateChangedEvent>().Unsubscribe(OnWindowStateChangedEvent);
            _eventAggregator.GetEvent<WindowsCloseEvent>().Unsubscribe(OnWindowsCloseEvent);
        }
    }
}
