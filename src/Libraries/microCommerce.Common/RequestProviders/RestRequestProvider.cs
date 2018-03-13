using RestSharp;
using System;
using System.Threading.Tasks;

namespace microCommerce.Common.RequestProviders
{
    public class RestRequestProvider : IRequestProvider
    {
        public virtual TResult Get<TResult>(string url, params object[] parameters) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("application/json", parameters, ParameterType.QueryString);

            return client.Execute<TResult>(request).Data;
        }

        public virtual async Task<TResult> GetAsync<TResult>(string url, params object[] parameters) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("application/json", parameters, ParameterType.QueryString);
            var response = await client.ExecuteTaskAsync<TResult>(request);

            return response.Data;
        }

        public virtual TResult Get<TResult>(string url, string token, params object[] parameters) where TResult : class, new()
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", token);
            request.AddParameter("application/json", parameters, ParameterType.QueryString);

            return client.Execute<TResult>(request).Data;
        }

        public virtual async Task<TResult> GetAsync<TResult>(string url, string token, params object[] parameters) where TResult : class, new()
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddParameter("Authorization", token);
            request.AddParameter("application/json", parameters, ParameterType.QueryString);
            var response = await client.ExecuteTaskAsync<TResult>(request);

            return response.Data;
        }

        public virtual TResult Post<TResult>(string url) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            return client.Execute<TResult>(request).Data;
        }

        public virtual void Post(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            client.Execute(request);
        }

        public virtual async Task<TResult> PostAsync<TResult>(string url) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            var response = await client.ExecuteTaskAsync<TResult>(request);

            return response.Data;
        }

        public virtual async Task PostAsync(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            await client.ExecuteTaskAsync(request);
        }

        public virtual TResult Post<TResult>(string url, object body) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddBody(body);

            return client.Execute<TResult>(request).Data;
        }

        public virtual async Task<TResult> PostAsync<TResult>(string url, object body) where TResult : class, new()
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddBody(body);
            var response = await client.ExecuteTaskAsync<TResult>(request);

            return response.Data;
        }

        public virtual TResult Post<TResult>(string url, object body, string token) where TResult : class, new()
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddBody(body);
            request.AddParameter("Authorization", token);

            return client.Execute<TResult>(request).Data;
        }

        public virtual async Task<TResult> PostAsync<TResult>(string url, object body, string token) where TResult : class, new()
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddBody(body);
            request.AddParameter("Authorization", token);
            var response = await client.ExecuteTaskAsync<TResult>(request);

            return response.Data;
        }
    }
}