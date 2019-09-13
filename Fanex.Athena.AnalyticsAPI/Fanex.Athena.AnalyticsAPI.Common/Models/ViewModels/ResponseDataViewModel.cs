using Newtonsoft.Json;

namespace Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain
{
    public class ResponseDataViewModel<T> : ResponseDataViewModel
    {
        public ResponseDataViewModel(T data, bool isSuccess, string msg)
            : base(isSuccess, msg)
        {
            Data = data;
        }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ResponseDataViewModel
    {
        public ResponseDataViewModel(bool isSuccess, string msg)
        {
            IsSuccess = isSuccess;
            Msg = msg;
        }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
}