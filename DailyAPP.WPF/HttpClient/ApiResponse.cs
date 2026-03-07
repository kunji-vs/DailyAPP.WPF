using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.HttpClient
{
    /// <summary>
    /// 接收模型
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 结果编码
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        /// 结果信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 数据结果
        /// </summary>
        public object ResultData { get; set; }
    }
}
