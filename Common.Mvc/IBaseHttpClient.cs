using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Mvc
{
    public interface IBaseHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

        T GetAsyncContent<T>(string requestUri);

        TM CreateAsAsync<TM, TN>(string requestUri, TN data);

        void UpdateAsAsync<T>(string requestUri, T data);

        void RemoveAsAsync(string requestUri);
        void SetAuth(string value);
    }
}
