using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.DTOs
{
    public class MemoInfoDTO
    {
        public int MemoId { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

        public int AccountId { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
