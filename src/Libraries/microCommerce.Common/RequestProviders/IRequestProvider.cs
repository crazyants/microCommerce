using System.Threading.Tasks;

namespace microCommerce.Common.RequestProviders
{
    public interface IRequestProvider
    {
        TResult Get<TResult>(string url, params object[] parameters) where TResult : class, new();
        Task<TResult> GetAsync<TResult>(string url, params object[] parameters) where TResult : class, new();
        TResult Get<TResult>(string url, string token, params object[] parameters) where TResult : class, new();
        Task<TResult> GetAsync<TResult>(string url, string token, params object[] parameters) where TResult : class, new();
        TResult Post<TResult>(string url) where TResult : class, new();
        Task<TResult> PostAsync<TResult>(string url) where TResult : class, new();
        TResult Post<TResult>(string url, object body) where TResult : class, new();
        Task<TResult> PostAsync<TResult>(string url, object body) where TResult : class, new();
        TResult Post<TResult>(string url, object body, string token) where TResult : class, new();
        Task<TResult> PostAsync<TResult>(string url, object body, string token) where TResult : class, new();
    }
}