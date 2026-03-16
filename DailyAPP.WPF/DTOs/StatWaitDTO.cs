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

        public int MemoCount { get; set; }
        public string FinishPercent
        {
            get
            {
                if (FinishCount == 0)
                    return "0.00%";
                return $"{(FinishCount * 100.00 / TotalCount).ToString("f2")}%";
            }
        }
    }
}
