namespace Fanex.Athena.AnalyticsAPI.Core.Models.DataDomain
{
    public class ResponseDataVM<T> : ResponseDataVM
    {
        public ResponseDataVM(T data, bool isSuccess, string msg)
            : base(isSuccess, msg)
        {
            Data = data;
        }

        public T Data { get; set; }
    }

    public class ResponseDataVM
    {
        public ResponseDataVM(bool isSuccess, string msg)
        {
            IsSuccess = isSuccess;
            Msg = msg;
        }

        public bool IsSuccess { get; set; }

        public string Msg { get; set; }
    }
}