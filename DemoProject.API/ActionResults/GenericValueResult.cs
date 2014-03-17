using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoProject.API.ActionResults
{
    /// <summary>
    /// Custom implementation of the IHttpActionResult, that allows to have custom types in Http Response.
    /// Created to ease the unit testing.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public class GenericValueResult<T> : IHttpActionResult
    {
        private readonly T value;
        private readonly HttpRequestMessage request;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericValueResult"/> class.
        /// </summary>
        /// <param name="value">Value, to return in Response</param>
        /// <param name="request">Request, from which Response should be created</param>
        public GenericValueResult(T value, HttpRequestMessage request)
        {
            this.value = value;
            this.request = request;
        }

        /// <summary>
        /// Gets the value associated with current instance
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.request.CreateResponse(this.value);
            return Task.FromResult(response);
        }
    }
}