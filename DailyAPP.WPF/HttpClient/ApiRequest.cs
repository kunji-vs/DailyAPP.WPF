using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.HttpClient
{
    public class ApiRequest
    {
        /// <summary>
        /// 请求地址/API路由地址
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string ContentType { get; set; } = "application/json";


    }
}
