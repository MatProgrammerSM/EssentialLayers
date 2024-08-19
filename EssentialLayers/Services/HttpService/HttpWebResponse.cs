using EssentialLayers.Helpers.Result;
using System.Net;

namespace EssentialLayers.Services.HttpService
{
    public class HttpWebResponse<T>(
        bool ok, string message, T data, HttpStatusCode httpStatusCode = HttpStatusCode.OK
    ) : ResultHelper<T>(ok, message, data)
    {
        public HttpStatusCode StatusCode { get; set; } = httpStatusCode;

        public static HttpWebResponse<T> Success(
            T data, HttpStatusCode httpStatusCode
        ) => new(true, string.Empty, data, httpStatusCode);

        public static HttpWebResponse<T> Fail(
            string message, HttpStatusCode httpStatusCode
        ) => new(false, message, default!, httpStatusCode);

        public static HttpWebResponse<T> Fail(Exception e, HttpStatusCode httpStatusCode)
        {
            ResultHelper<T> result = Fail(e);

            return Fail(result.Message, httpStatusCode);
        }
    }
}