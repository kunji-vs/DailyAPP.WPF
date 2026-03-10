using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF.Events
{
    public class WindowStateChangedEvent : PubSubEvent<WindowState>
    {
    }
    public class WindowsCloseEvent : PubSubEvent
    {
    }
}
