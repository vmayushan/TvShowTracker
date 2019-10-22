using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccess.OMDb.Models;
using Newtonsoft.Json.Linq;

namespace TvShowTracker.DataAccess.OMDb
{
    public class OMDbShowClient : IOMDbShowClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IOMDbShowClient> _logger;

        public OMDbShowClient(HttpClient httpClient, 
            ILogger<IOMDbShowClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<OMDbShowAutocomplete>> SearchShowsByTitle(string query)
        {
            var queryString = QueryHelpers
                .AddQueryString("/", new Dictionary<string, string>()
                {
                    {"s", query},
                    {"type", "series"}
                });
            var response = await _httpClient.GetStringAsync(queryString);
            _logger.LogInformation("OMDb request: query {query}, response: {response}", 
                queryString, response);
            
            var json = JObject.Parse(response);
            return IsResponseSuccess(json)
                ? json.SelectToken("Search").ToObject<List<OMDbShowAutocomplete>>()
                : null;
        }

        public async Task<OMDbShow> GetShowByImdbId(string imdbId)
        {
            var queryString = QueryHelpers
                .AddQueryString("/", "i", imdbId);
            var response = await _httpClient.GetStringAsync(queryString);
            
            _logger.LogInformation("OMDb request: query {query}, response: {response}", 
                queryString, response);
            var json = JObject.Parse(response);
            
            return  IsResponseSuccess(json)
                ? json.ToObject<OMDbShow>()
                : null;
        }
        
        public async Task<OMDbSeason> GetSeason(string imdbId, int seasonNumber)
        {
            var queryString = QueryHelpers
                .AddQueryString("/", new Dictionary<string, string>()
                {
                    {"i", imdbId},
                    {"season", seasonNumber.ToString()}
                });
            var response = await _httpClient.GetStringAsync(queryString);
            
            _logger.LogInformation("OMDb request: query {query}, response: {response}", 
                queryString, response);
            
            var json = JObject.Parse(response);
            
            return IsResponseSuccess(json)
                ? json.ToObject<OMDbSeason>()
                : null;
        }

        private static bool IsResponseSuccess(JObject json)
        {
            return json.SelectToken("Response").Value<string>() == "True";
        }
    }
}