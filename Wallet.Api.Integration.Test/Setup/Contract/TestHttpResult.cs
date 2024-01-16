using System.Net;

namespace Wallet.Api.Integration.Test.Setup.Contract
{
    public class TestHttpResult<T>
    {
        public TestHttpResult(T result, HttpStatusCode httpStatusCode)
        {
            this.Response = result;
            this.HttpStatusCode = httpStatusCode;
        }

        public T Response { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
