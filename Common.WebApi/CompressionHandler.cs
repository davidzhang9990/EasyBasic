using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Common.WebApi
{
    public class CompressionHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith((responseToCompleteTask) =>
            {
                var response = responseToCompleteTask.Result;

                if (response.RequestMessage.Headers.AcceptEncoding != null
                    && (response.Content != null && response.Content.Headers.ContentType != null && response.Content.Headers.ContentType.MediaType != null && !response.Content.Headers.ContentType.MediaType.Contains("text/html")))
                {
                    if (response.RequestMessage.Headers.AcceptEncoding.Count == 0) return response;
                    var encodingType = response.RequestMessage.Headers.AcceptEncoding.First().Value;

                    response.Content = new CompressedContent(response.Content, encodingType);
                }

                return response;
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }

    public class CompressedContent : HttpContent
    {
        private readonly HttpContent _originalContent;
        private readonly string _encodingType;

        public CompressedContent(HttpContent content, string encodingType)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encodingType == null)
            {
                throw new ArgumentNullException("encodingType");
            }

            _originalContent = content;
            _encodingType = encodingType.ToLowerInvariant();

            if (_encodingType != "gzip" && _encodingType != "deflate")
            {
                throw new InvalidOperationException(string.Format("Encoding '{0}' is not supported. Only supports gzip or deflate encoding.", _encodingType));
            }

            // copy the headers from the original content
            foreach (var header in _originalContent.Headers)
            {
                Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            Headers.ContentEncoding.Add(encodingType);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;

            return false;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            Stream compressedStream = null;

            if (_encodingType == "gzip")
            {
                compressedStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true);
            }
            else if (_encodingType == "deflate")
            {
                compressedStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true);
            }

            return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
            {
                if (compressedStream != null)
                {
                    compressedStream.Dispose();
                }
            });
        }
    }
}
