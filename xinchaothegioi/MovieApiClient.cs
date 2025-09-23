using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

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

    // Lớp để tracking thay đổi và tối ưu hóa polling
    public class ContentChangeTracker
    {
        public string ContentHash { get; set; }
        public DateTime LastChecked { get; set; }
        public DateTime LastChanged { get; set; }
        public int ConsecutiveNoChanges { get; set; }
        public List<MovieSummary> CachedData { get; set; } = new List<MovieSummary>();
    }

    public class MovieApiClient
    {
        private static readonly HttpClient _http;
        private readonly string _apiKey;
        
        // Dictionary để lưu trữ thông tin tracking cho từng endpoint
        private readonly Dictionary<string, ContentChangeTracker> _contentTrackers = new Dictionary<string, ContentChangeTracker>();
        
        // Cấu hình polling thông minh
        private readonly TimeSpan _minimumInterval = TimeSpan.FromMinutes(1); // Tối thiểu 1 phút
        private readonly TimeSpan _maximumInterval = TimeSpan.FromMinutes(30); // Tối đa 30 phút
        private readonly int _maxConsecutiveNoChanges = 5; // Sau 5 lần không đổi thì tăng interval

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

        // Tính hash của dữ liệu để phát hiện thay đổi
        private string ComputeContentHash(List<MovieSummary> movies)
        {
            if (movies == null || movies.Count == 0)
                return string.Empty;

            // Tạo signature dựa trên id và title của các phim
            var content = string.Join("|", movies.OrderBy(m => m.id).Select(m => $"{m.id}:{m.title}:{m.vote_average}"));
            
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
                return Convert.ToBase64String(hash);
            }
        }

        // Tính toán interval polling thông minh dựa trên lịch sử thay đổi
        private TimeSpan CalculatePollingInterval(ContentChangeTracker tracker)
        {
            if (tracker == null || tracker.ConsecutiveNoChanges == 0)
                return _minimumInterval;

            // Exponential backoff: càng nhiều lần không đổi thì càng tăng interval
            var multiplier = Math.Min(tracker.ConsecutiveNoChanges, _maxConsecutiveNoChanges);
            var interval = TimeSpan.FromTicks(_minimumInterval.Ticks * (long)Math.Pow(2, multiplier - 1));
            
            return interval > _maximumInterval ? _maximumInterval : interval;
        }

        // Kiểm tra xem có nên polling hay không dựa trên lịch sử
        public bool ShouldPoll(string endpoint)
        {
            if (!_contentTrackers.TryGetValue(endpoint, out var tracker))
                return true; // Lần đầu tiên luôn poll

            var requiredInterval = CalculatePollingInterval(tracker);
            return DateTime.Now - tracker.LastChecked >= requiredInterval;
        }

        // Lấy data từ cache nếu vẫn còn valid
        public List<MovieSummary> GetCachedDataIfValid(string endpoint)
        {
            if (_contentTrackers.TryGetValue(endpoint, out var tracker))
            {
                if (!ShouldPoll(endpoint) && tracker.CachedData?.Count > 0)
                {
                    return new List<MovieSummary>(tracker.CachedData); // Return copy
                }
            }
            return null;
        }

        // Phiên bản thông minh của GetMoviesAsync với change detection
        public async Task<List<MovieSummary>> GetMoviesSmartAsync(string relativePath, IDictionary<string, string> query, CancellationToken ct, bool forceRefresh = false)
        {
            var endpoint = $"{relativePath}:{JsonConvert.SerializeObject(query ?? new Dictionary<string, string>())}";
            
            // Kiểm tra cache trước nếu không bắt buộc refresh
            if (!forceRefresh)
            {
                var cachedData = GetCachedDataIfValid(endpoint);
                if (cachedData != null)
                {
                    return cachedData;
                }
            }

            // Nếu không force refresh và chưa đến lúc poll, trả về cache cũ
            if (!forceRefresh && !ShouldPoll(endpoint))
            {
                if (_contentTrackers.TryGetValue(endpoint, out var tracker) && tracker.CachedData?.Count > 0)
                {
                    return new List<MovieSummary>(tracker.CachedData);
                }
            }

            // Thực hiện API call
            var movies = await GetMoviesAsync(relativePath, query, ct);
            var newHash = ComputeContentHash(movies);

            // Cập nhật tracking info
            if (!_contentTrackers.TryGetValue(endpoint, out var currentTracker))
            {
                currentTracker = new ContentChangeTracker();
                _contentTrackers[endpoint] = currentTracker;
            }

            currentTracker.LastChecked = DateTime.Now;

            // Kiểm tra xem có thay đổi không
            if (currentTracker.ContentHash != newHash)
            {
                // Có thay đổi
                currentTracker.ContentHash = newHash;
                currentTracker.LastChanged = DateTime.Now;
                currentTracker.ConsecutiveNoChanges = 0;
                currentTracker.CachedData = new List<MovieSummary>(movies);
            }
            else
            {
                // Không có thay đổi
                currentTracker.ConsecutiveNoChanges++;
            }

            return movies;
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

        // Các phương thức thông minh mới thay thế cho các phương thức cũ
        public Task<List<MovieSummary>> GetTrendingSmartAsync(string timeWindow, CancellationToken ct, bool forceRefresh = false) 
            => GetMoviesSmartAsync($"trending/movie/{timeWindow}", null, ct, forceRefresh);
            
        public Task<List<MovieSummary>> SearchSmartAsync(string query, CancellationToken ct, bool forceRefresh = false) 
            => GetMoviesSmartAsync("search/movie", new Dictionary<string, string> { { "query", query } }, ct, forceRefresh);
            
        public Task<List<MovieSummary>> GetCategorySmartAsync(string category, CancellationToken ct, bool forceRefresh = false) 
            => GetMoviesSmartAsync("movie/" + category, null, ct, forceRefresh);

        // Giữ lại các phương thức cũ để backward compatibility
        public Task<List<MovieSummary>> GetTrendingAsync(string timeWindow, CancellationToken ct) => GetMoviesAsync($"trending/movie/{timeWindow}", null, ct);
        public Task<List<MovieSummary>> SearchAsync(string query, CancellationToken ct) => GetMoviesAsync("search/movie", new Dictionary<string, string> { { "query", query } }, ct);
        public Task<List<MovieSummary>> GetCategoryAsync(string category, CancellationToken ct) => GetMoviesAsync("movie/" + category, null, ct);

        // Thông tin debugging
        public Dictionary<string, object> GetPollingStats()
        {
            var stats = new Dictionary<string, object>();
            foreach (var kvp in _contentTrackers)
            {
                stats[kvp.Key] = new
                {
                    LastChecked = kvp.Value.LastChecked,
                    LastChanged = kvp.Value.LastChanged,
                    ConsecutiveNoChanges = kvp.Value.ConsecutiveNoChanges,
                    NextPollTime = kvp.Value.LastChecked.Add(CalculatePollingInterval(kvp.Value)),
                    CachedMovieCount = kvp.Value.CachedData?.Count ?? 0
                };
            }
            return stats;
        }

        // Reset tracking cho một endpoint cụ thể
        public void ResetTracking(string endpoint)
        {
            _contentTrackers.Remove(endpoint);
        }

        // Clear tất cả tracking data
        public void ClearAllTracking()
        {
            _contentTrackers.Clear();
        }
    }
}
