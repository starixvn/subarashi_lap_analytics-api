namespace Fanex.Athena.AnalyticsAPI.Common.Models
{
    public class ResponseDataModel<T> : ResponseDataModel
    {
        public ResponseDataModel(T data)
        {
            Data = data;

            //to do
            IsSuccess = true;
            ErrorMsg = string.Empty;
        }

        public T Data { get; set; }
    }

    public class ResponseDataModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }
}