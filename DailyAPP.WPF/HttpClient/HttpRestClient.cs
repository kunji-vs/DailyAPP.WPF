using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailyAPP.WPF.HttpClient
{
    public class HttpRestClient
    {
        private RestClient Client;

        private readonly string baseUri = "http://localhost:5011/api/";

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpRestClient()
        {
            Client = new RestClient();

        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="apiRequest">发送数据</param>
        /// <returns>接收数据</returns>
        public ApiResponse Excute(ApiRequest apiRequest)
        {
            var request = new RestRequest(apiRequest.Method);
            request.AddHeader("Content-Type", apiRequest.ContentType);
            if(apiRequest.Parameters != null)
            {
                request.AddParameter("param", JsonConvert.SerializeObject(apiRequest.Parameters),ParameterType.RequestBody);
            }
            //http://localhost:5011/api/Account/RegiestAccount
            string uri = $"{baseUri}{apiRequest.Route}";
            Client.BaseUrl = new Uri(uri);
            try
            {
                var res = Client.Execute(request);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<ApiResponse>(res.Content);
                }
                else
                {
                    return new ApiResponse()
                    {
                        ResultCode = -1,
                        msg = "请求失败"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    ResultCode = -1,
                    msg = ex.Message
                };
            }
        }
    }
}
