using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Common.Helper;

namespace Common.WebApi
{
    public class LanguageMessageHandler : DelegatingHandler
    {
        private void SetCulture(HttpRequestMessage request, string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var lang = LangHelper.GetLanguage();

            SetCulture(request, lang);

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
