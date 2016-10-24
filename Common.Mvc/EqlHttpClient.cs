using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Exception;
using Common.Helper;
using log4net;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Common.Mvc
{
    public interface IEqlHttpClient
    {
        string ApiBaseUrl { get; }
        T GetAsyncContent<T>(string requestUri);
        TM CreateAsAsync<TM, TN>(string requestUri, TN data);
        void UpdateAsAsync<T>(string requestUri, T data);
        void RemoveAsAsync(string requestUri);
    }

    public class EqlHttpClient : IEqlHttpClient
    {
        private readonly ILog _log;
        private HttpClient _client;
        private readonly HttpRequestBase _request;
        private HttpClientHandler _clientHandler;
        private string _apiBaseUrl;

        public EqlHttpClient(ILog log, HttpRequestBase request)
        {
            _log = log;
            _request = request;
        }

        public string ApiBaseUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_apiBaseUrl)) return _apiBaseUrl;
                _apiBaseUrl = _request.GetDomain();
                _log.Debug(string.Format("Request Base Url: {0}", _apiBaseUrl));
                return _apiBaseUrl;
            }
        }

        private HttpClientHandler ClientHander
        {
            get
            {
                if (_clientHandler != null) return _clientHandler;

                var auth = "";
                if (_request.Url == null) return null;
                var token = _request.Cookies["EqualearningApp"];
                auth = HttpUtility.HtmlDecode(_request.Form["auth"]);
                var cookieContainer = new CookieContainer();
                if (auth == null && token == null)
                {
                    _clientHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                    };
                }
                else
                {
                    if (token != null)
                    {
                        var cookie = string.IsNullOrEmpty(auth)
                            ? new Cookie("EqualearningApp", token.Value)
                            : new Cookie("EqualearningApp", auth);
                        cookieContainer.Add(new Uri(ApiBaseUrl), cookie);
                    }
                    _clientHandler = new HttpClientHandler { CookieContainer = cookieContainer, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
                }
                return _clientHandler;
            }
        }

        private HttpClient Client
        {
            get
            {
                if (_client != null) return _client;
                _client = new HttpClient(ClientHander);
                //_client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
                if (_request.UserLanguages != null)
                    _client.DefaultRequestHeaders.Add("Accept-Language", string.Join(",", _request.UserLanguages));
                _client.BaseAddress = new Uri(ApiBaseUrl);
                return _client;
            }
        }

        Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return Client.GetAsync(requestUri);
        }

        public T GetAsyncContent<T>(string requestUri)
        {
            var ret = Client.GetAsync(requestUri);
            return RetrieveData<T>(ret);
        }

        private T RetrieveData<T>(Task<HttpResponseMessage> responseMessage)
        {
            if (responseMessage == null)
                throw new InternalServerErrorException("Unable to retrieve data from service.");
            var response = responseMessage.Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
                throw new System.Exception(content);
            var dataResponse = JsonConvert.DeserializeObject<T>(content);
            return dataResponse;
        }


        public TM CreateAsAsync<TM, TN>(string requestUri, TN data)
        {
            var responseMessage = Client.PostAsJsonAsync(requestUri, data);
            var content = HandlerResponseMessage(responseMessage);
            var dataResponse = JsonConvert.DeserializeObject<TM>(content);
            return dataResponse;
        }

        public void UpdateAsAsync<T>(string requestUri, T data)
        {
            var responseMessage = Client.PutAsJsonAsync(requestUri, data);
            HandlerResponseMessage(responseMessage);
        }

        public void RemoveAsAsync(string requestUri)
        {
            var responseMessage = Client.DeleteAsync(requestUri);
            HandlerResponseMessage(responseMessage);
        }

        private string HandlerResponseMessage(Task<HttpResponseMessage> responseMessage)
        {
            if (responseMessage == null)
                throw new InternalServerErrorException("Unable to retrieve data from service.");
            var response = responseMessage.Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode)
                throw new System.Exception(content);
            return content;
        }
    }
}
