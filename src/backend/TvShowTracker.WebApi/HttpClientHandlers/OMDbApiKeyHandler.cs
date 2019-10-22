using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using TvShowTracker.WebApi.Configuration;

namespace TvShowTracker.WebApi.HttpClientHandlers
{
    public class OMDbApiKeyHandler : DelegatingHandler
    {
        private readonly IOptions<OMDbApiSettings> _apiSettings;

        public OMDbApiKeyHandler(IOptions<OMDbApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.RequestUri = AddParameter(
                request.RequestUri, 
                "apiKey", 
                _apiSettings.Value.ApiKey
            );

            return base.SendAsync(request, cancellationToken);
        }
            
        private static Uri AddParameter(Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}