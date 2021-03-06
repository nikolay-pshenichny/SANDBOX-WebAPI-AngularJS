﻿using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DemoProject.API.ActionResults
{
    /// <summary>
    /// Custom implementation of the IHttpActionResult, that creates a Http Response with a file attachment
    /// </summary>
    public class Attachment : IHttpActionResult
    {
        private readonly Stream stream;
        private readonly string fileName;
        private readonly HttpRequestMessage request;

        /// <summary>
        /// Initializes a new instance of the <see cref="Attachment" /> class.
        /// </summary>
        /// <param name="stream">File's data</param>
        /// <param name="fileName">Name of a file to set in ContentDisposition header</param>
        /// <param name="request">Request, from which Response should be created</param>
        public Attachment(Stream stream, string fileName, HttpRequestMessage request)
        {
            this.stream = stream;
            this.fileName = fileName;
            this.request = request;
        }

        /// <summary>
        /// Gets the stream associated with the current instance
        /// </summary>
        public Stream Stream
        {
            get
            {
                return this.stream;
            }
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = this.request.CreateResponse();

            response.Content = new StreamContent(this.stream);

            response.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = this.fileName
                };

            return Task.FromResult(response);
        }
    }
}