using Fanex.Athena.AnalyticsAPI.Common.Extensions;
using Fanex.Athena.AnalyticsAPI.Common.Services;

namespace Fanex.Athena.AnalyticsAPI.Common.Models.DataDomain
{
    public class ResponseDataModel<T> : ResponseDataModel
    {
        public ResponseDataModel(T data)
        {
            Data = data;

            //to do
            var service = ServiceProviderExtensions.GetRequiredService<IContextService>();
            ErrorMsg = service.GetError();
            IsSuccess = string.IsNullOrEmpty(ErrorMsg);
        }

        public T Data { get; set; }
    }

    public class ResponseDataModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }
}