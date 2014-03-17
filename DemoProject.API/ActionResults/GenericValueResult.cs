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

        public GenericValueResult(T metadataInfo, HttpRequestMessage request)
        {
            this.value = metadataInfo;
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