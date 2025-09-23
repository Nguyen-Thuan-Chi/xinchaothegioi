using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace xinchaothegioi
{
    public class MovieSummary
    {
        public int id { get; set; }
        public string title { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string poster_path { get; set; }
        public double vote_average { get; set; }
    }

    class PagedMovieResult
    {
        public int page { get; set; }
        public List<MovieSummary> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    public class MovieApiClient
    {
        private static readonly HttpClient _http;
        private readonly string _apiKey;

        static MovieApiClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _http = new HttpClient { BaseAddress = new Uri("https://api.themoviedb.org/3/") };
            _http.Timeout = TimeSpan.FromSeconds(15);
        }

        private static string ReadApiKeyFromConfig()
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                var node = doc.SelectSingleNode("/configuration/appSettings/add[@key='TMDB_API_KEY']");
                if (node?.Attributes?["value"] != null)
                    return node.Attributes["value"].Value;
            }
            catch { }
            return null;
        }

        public MovieApiClient(string apiKey = null)
        {
            _apiKey = apiKey
                ?? Environment.GetEnvironmentVariable("TMDB_API_KEY")
                ?? ReadApiKeyFromConfig();

            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("TMDB API key not found. Set environment variable TMDB_API_KEY or appSettings key.");
        }

        public async Task<List<MovieSummary>> GetMoviesAsync(string relativePath, IDictionary<string, string> query, CancellationToken ct)
        {
            var qp = $"api_key={_apiKey}&language=vi-VN";
            if (query != null)
            {
                foreach (var kv in query)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                        qp += "&" + Uri.EscapeDataString(kv.Key) + "=" + Uri.EscapeDataString(kv.Value);
                }
            }

            var requestUri = relativePath;
            if (!requestUri.EndsWith("?") && !requestUri.Contains("?"))
                requestUri += "?";
            requestUri += qp;

            using (var req = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false))
            {
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<PagedMovieResult>(json);
                return data?.results ?? new List<MovieSummary>();
            }
        }

        public Task<List<MovieSummary>> GetTrendingAsync(string timeWindow, CancellationToken ct) => GetMoviesAsync($"trending/movie/{timeWindow}", null, ct);
        public Task<List<MovieSummary>> SearchAsync(string query, CancellationToken ct) => GetMoviesAsync("search/movie", new Dictionary<string, string> { { "query", query } }, ct);
        public Task<List<MovieSummary>> GetCategoryAsync(string category, CancellationToken ct) => GetMoviesAsync("movie/" + category, null, ct);
    }
}
