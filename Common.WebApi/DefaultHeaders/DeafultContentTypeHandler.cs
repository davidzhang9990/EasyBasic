using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Common.WebApi.DefaultHeaders
{
    public class DeafultContentTypeHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {

            IEnumerable<string> contentTypes;
            request.Content.Headers.TryGetValues("Content-Type", out contentTypes);
            if (contentTypes == null)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return base.SendAsync(request, cancellationToken);

        }
    }
}
