using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using DemoProject.API.Repositories;
using DemoProject.Common.Windsor;

namespace DemoProject.API.Controllers
{
    /// <summary>
    /// This API Controller is responsible for Downloads
    /// </summary>
    public class DownloadController : ApiController
    {
        [Inject]
        public IMetadataRepository MetadataRepository { private get; set; }

        [Inject]
        public IStorageRepository StorageRepository { private get; set; }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            // Search for the requested metadata Id
            var metadata = this.MetadataRepository.GetAll().FirstOrDefault(x => x.Id == id);

            // If found, attach the file associated with metadata to the Response
            if (metadata != null)
            {
                Stream stream;
                if (this.StorageRepository.Get(metadata.FileStorageUid, out stream))
                {
                    var response = this.Request.CreateResponse();
                    response.Content = new StreamContent(stream);
                    response.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = metadata.FileName
                        };

                    return response;
                }
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}