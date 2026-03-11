using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.DTOs
{
    public class StatWaitDTO
    {
        public int TotalCount { get; set; }

        public int FinishCount { get; set; }

        public string FinishPercent
        {
            get; set;
        }
    }
}
