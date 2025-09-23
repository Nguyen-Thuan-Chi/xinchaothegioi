using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinchaothegioi
{
    public partial class frmMovie : Form
    {
        private MovieApiClient _client;
        private CancellationTokenSource _cts;
        private static readonly HttpClient _img = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        private readonly Dictionary<string, Image> _posterCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private bool _autoRefresh;
        private bool _isLoading;
        private Panel _currentSelectedPanel;
        
        // Smart polling variables
        private System.Windows.Forms.Timer _smartPollingTimer;
        private readonly object _pollingLock = new object();
        private string _currentEndpoint;

        public MovieSummary SelectedMovie { get; private set; }

        public frmMovie()
        {
            InitializeComponent();
            KhoiTao();
            // Dùng sự kiện Shown để tải dữ liệu ban đầu thay vì BeginInvoke async void gây nuốt exception
            this.Shown += async (s, e) => await InitialLoadAsync();
        }

        private void KhoiTao()
        {
            try
            {
                _client = new MovieApiClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không khởi tạo được API: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSearch.Enabled = false;
            }

            if (cboMode.Items.Count == 0)
            {
                cboMode.Items.AddRange(new object[]
                {
                    "Trending Day","Trending Week","Now Playing","Popular","Top Rated","Upcoming"
                });
                cboMode.SelectedIndex = 0;
            }

            // Gắn sự kiện (designer đã gắn 1 số sự kiện Tick -> tránh gắn trùng)
            btnSearch.Click += btnSearch_Click;
            cboMode.SelectedIndexChanged += cboMode_SelectedIndexChanged;
            txtQuery.KeyDown += txtQuery_KeyDown;
            txtQuery.TextChanged += txtQuery_TextChanged;
            nudSeconds.ValueChanged += nudSeconds_ValueChanged;
            btnAutoRefreshToggle.Click += btnAutoRefreshToggle_Click;
            btnSelectMovie.Click += btnSelectMovie_Click;
            btnCancelMovie.Click += btnCancelMovie_Click;
            this.FormClosing += frmMovie_FormClosing;
            flowMovies.Click += flowMovies_Click;

            // Khởi tạo smart polling timer (thay thế timer cũ)
            InitializeSmartPolling();
        }

        private void InitializeSmartPolling()
        {
            // Timer này sẽ kiểm tra định kỳ nhưng chỉ gọi API khi cần thiết
            _smartPollingTimer = new System.Windows.Forms.Timer();
            _smartPollingTimer.Interval = 30000; // Kiểm tra mỗi 30 giây
            _smartPollingTimer.Tick += SmartPollingTimer_Tick;
        }

        private async void SmartPollingTimer_Tick(object sender, EventArgs e)
        {
            if (!_autoRefresh || _isLoading || string.IsNullOrEmpty(_currentEndpoint))
                return;

            lock (_pollingLock)
            {
                if (_isLoading) return;
            }

            // Kiểm tra xem có nên polling không dựa trên smart logic
            if (_client != null && _client.ShouldPoll(_currentEndpoint))
            {
                _ = SafeSmartReloadAsync();
            }
            else
            {
                // Cập nhật status để user biết đang dùng cache
                var cachedData = _client?.GetCachedDataIfValid(_currentEndpoint);
                if (cachedData != null)
                {
                    CapNhatStatus($"Dùng cache - {cachedData.Count} phim (tiết kiệm request)");
                }
            }
        }

        private async Task SafeSmartReloadAsync()
        {
            try
            {
                await TaiPhimSmartAsync(false); // false = không force refresh
            }
            catch (Exception ex)
            {
                CapNhatStatus("Lỗi smart refresh: " + ex.Message);
                Debug.WriteLine("[SmartRefresh] Exception: " + ex);
            }
        }

        // Tải lần đầu
        private async Task InitialLoadAsync()
        {
            try
            {
                await TaiPhimSmartAsync(true); // true = force refresh lần đầu
            }
            catch (Exception ex)
            {
                ShowError("Lỗi tải phim ban đầu: " + ex.Message);
            }
        }

        // Event Timer cũ (giữ lại để tương thích với Designer)
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Chuyển sang smart polling
            SmartPollingTimer_Tick(sender, e);
        }

        // Nút tìm
        private async void btnSearch_Click(object sender, EventArgs e) => await TaiPhimSmartAsync(true);

        // Mode đổi
        private async void cboMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuery.Text))
                await TaiPhimSmartAsync(true);
        }

        // Enter trong ô tìm
        private async void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await TaiPhimSmartAsync(true);
            }
        }

        // Đổi số giây (giữ lại cho tương thích)
        private void nudSeconds_ValueChanged(object sender, EventArgs e)
        {
            if (timerRefresh != null)
                timerRefresh.Interval = (int)nudSeconds.Value * 1000;
            
            // Cập nhật smart polling timer cũng
            if (_smartPollingTimer != null)
            {
                var newInterval = Math.Max((int)nudSeconds.Value * 1000, 30000); // Tối thiểu 30 giây
                _smartPollingTimer.Interval = newInterval;
            }
        }

        // Bật / tắt auto refresh với smart polling
        private void btnAutoRefreshToggle_Click(object sender, EventArgs e)
        {
            _autoRefresh = !_autoRefresh;
            if (_autoRefresh)
            {
                var interval = Math.Max((int)nudSeconds.Value * 1000, 30000);
                
                // Dùng smart polling timer thay vì timer cũ
                _smartPollingTimer.Interval = interval;
                _smartPollingTimer.Start();
                
                // Vẫn giữ timer cũ cho tương thích
                timerRefresh.Interval = (int)nudSeconds.Value * 1000;
                timerRefresh.Start();
                
                btnAutoRefreshToggle.Text = "Stop Smart Auto";
                CapNhatStatus("Smart auto refresh ON - chỉ gọi API khi cần thiết");
            }
            else
            {
                _smartPollingTimer.Stop();
                timerRefresh.Stop();
                btnAutoRefreshToggle.Text = "Start Smart Auto";
                CapNhatStatus("Smart auto refresh OFF");
            }
        }

        // Chọn phim (OK)
        private void btnSelectMovie_Click(object sender, EventArgs e)
        {
            if (SelectedMovie == null)
            {
                MessageBox.Show("Chưa chọn phim.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        // Hủy
        private void btnCancelMovie_Click(object sender, EventArgs e)
        {
            SelectedMovie = null;
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmMovie_FormClosing(object sender, FormClosingEventArgs e)
        {
            HuyDangChay();
            _smartPollingTimer?.Stop();
            _smartPollingTimer?.Dispose();
        }

        // --------- Logic tải phim thông minh ---------
        private async Task TaiPhimSmartAsync(bool forceRefresh = false)
        {
            if (_client == null) return;
            
            lock (_pollingLock)
            {
                if (_isLoading) return; // chống gọi chồng chéo
                _isLoading = true;
            }

            HuyDangChay();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            var query = txtQuery.Text.Trim();
            var mode = cboMode.SelectedItem?.ToString() ?? "Trending Day";

            // Tạo endpoint identifier
            _currentEndpoint = !string.IsNullOrEmpty(query) ? $"search:{query}" : $"mode:{mode}";

            var statusPrefix = forceRefresh ? "Đang tải (force)" : "Đang kiểm tra";
            CapNhatStatus($"{statusPrefix}...");
            btnSearch.Enabled = false;
            flowMovies.Enabled = false;

            try
            {
                List<MovieSummary> list;
                if (!string.IsNullOrEmpty(query))
                    list = await _client.SearchSmartAsync(query, token, forceRefresh);
                else
                    list = await LayTheoModeSmartAsync(mode, token, forceRefresh);

                // Chỉ cập nhật UI nếu có data hoặc force refresh
                if (list?.Count > 0 || forceRefresh)
                {
                    ClearMovies();
                    DoMovies(list);
                }

                var statusSuffix = forceRefresh ? "" : " (smart cache)";
                CapNhatStatus($"Đã tải {list?.Count ?? 0} phim{statusSuffix}");
                
                // Debug info
                if (_client != null)
                {
                    var stats = _client.GetPollingStats();
                    if (stats.ContainsKey(_currentEndpoint))
                    {
                        Debug.WriteLine($"[SmartPolling] Endpoint: {_currentEndpoint}, Stats: {stats[_currentEndpoint]}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                CapNhatStatus("Đã huỷ");
            }
            catch (HttpRequestException httpEx)
            {
                // Thêm gợi ý nếu lỗi 401
                string hint = httpEx.Message.Contains("401") ? " (Kiểm tra API Key TMDB trong App.config)" : "";
                CapNhatStatus("Lỗi HTTP: " + httpEx.Message + hint);
                ShowError("Không tải được dữ liệu TMDB: " + httpEx.Message + hint);
            }
            catch (Exception ex)
            {
                CapNhatStatus("Lỗi: " + ex.Message);
                ShowError("Lỗi không xác định: " + ex);
            }
            finally
            {
                btnSearch.Enabled = true;
                flowMovies.Enabled = true;
                lock (_pollingLock)
                {
                    _isLoading = false;
                }
            }
        }

        private Task<List<MovieSummary>> LayTheoModeSmartAsync(string mode, CancellationToken ct, bool forceRefresh = false)
        {
            switch (mode)
            {
                case "Trending Day": return _client.GetTrendingSmartAsync("day", ct, forceRefresh);
                case "Trending Week": return _client.GetTrendingSmartAsync("week", ct, forceRefresh);
                case "Now Playing": return _client.GetCategorySmartAsync("now_playing", ct, forceRefresh);
                case "Popular": return _client.GetCategorySmartAsync("popular", ct, forceRefresh);
                case "Top Rated": return _client.GetCategorySmartAsync("top_rated", ct, forceRefresh);
                case "Upcoming": return _client.GetCategorySmartAsync("upcoming", ct, forceRefresh);
                default: return _client.GetTrendingSmartAsync("day", ct, forceRefresh);
            }
        }

        // Giữ lại phương thức cũ cho backward compatibility
        private async Task TaiPhimAsync()
        {
            await TaiPhimSmartAsync(true);
        }

        private Task<List<MovieSummary>> LayTheoModeAsync(string mode, CancellationToken ct)
        {
            switch (mode)
            {
                case "Trending Day": return _client.GetTrendingAsync("day", ct);
                case "Trending Week": return _client.GetTrendingAsync("week", ct);
                case "Now Playing": return _client.GetCategoryAsync("now_playing", ct);
                case "Popular": return _client.GetCategoryAsync("popular", ct);
                case "Top Rated": return _client.GetCategoryAsync("top_rated", ct);
                case "Upcoming": return _client.GetCategoryAsync("upcoming", ct);
                default: return _client.GetTrendingAsync("day", ct);
            }
        }

        private void DoMovies(List<MovieSummary> movies)
        {
            if (movies == null) return;
            
            flowMovies.SuspendLayout();
            try
            {
                foreach (var m in movies)
                {
                    var card = TaoCard(m);
                    flowMovies.Controls.Add(card);
                }
            }
            finally
            {
                flowMovies.ResumeLayout();
            }
        }

        private void ClearMovies()
        {
            flowMovies.Controls.Clear();
            SelectedMovie = null;
            _currentSelectedPanel = null;
        }

        private Panel TaoCard(MovieSummary m)
        {
            var panel = new Panel
            {
                Width = 180,
                Height = 320,
                Margin = new Padding(6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = m
            };

            var pic = new PictureBox
            {
                Width = 160,
                Height = 250,
                Left = 10,
                Top = 10,
                SizeMode = PictureBoxSizeMode.Zoom,
                Tag = m,
                Cursor = Cursors.Hand
            };

            if (!string.IsNullOrEmpty(m.poster_path))
                LoadPosterAsync(pic, m.poster_path);

            var lbl = new Label
            {
                Left = 10,
                Top = 265,
                Width = 160,
                Height = 34,
                Text = m.title,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoEllipsis = true
            };

            var tip = new ToolTip();
            tip.SetToolTip(pic, TooltipText(m));
            tip.SetToolTip(lbl, TooltipText(m));

            void clickHandler(object s, EventArgs e) => ChonPanel(panel);

            panel.Click += clickHandler;
            pic.Click += clickHandler;
            lbl.Click += clickHandler;

            panel.Controls.Add(pic);
            panel.Controls.Add(lbl);
            return panel;
        }

        private void ChonPanel(Panel p)
        {
            if (_currentSelectedPanel != null)
            {
                _currentSelectedPanel.BackColor = Color.White;
                _currentSelectedPanel.Padding = new Padding(0);
            }

            _currentSelectedPanel = p;
            _currentSelectedPanel.BackColor = Color.LightGoldenrodYellow;
            _currentSelectedPanel.Padding = new Padding(2);

            SelectedMovie = (MovieSummary)p.Tag;
            CapNhatStatus("Đã chọn: " + SelectedMovie.title);
        }

        private string TooltipText(MovieSummary m)
            => $"{m.title}\nĐiểm: {m.vote_average}/10\nPhát hành: {(string.IsNullOrEmpty(m.release_date) ? "N/A" : m.release_date)}";

        private async void LoadPosterAsync(PictureBox pic, string posterPath)
        {
            if (_posterCache.ContainsKey(posterPath))
            {
                pic.Image = _posterCache[posterPath];
                return;
            }

            string url = "https://image.tmdb.org/t/p/w185" + posterPath;
            try
            {
                using (var resp = await _img.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!resp.IsSuccessStatusCode) return;
                    var bytes = await resp.Content.ReadAsByteArrayAsync();
                    using (var ms = new MemoryStream(bytes))
                    {
                        var img = Image.FromStream(ms);
                        _posterCache[posterPath] = img;
                        pic.Image = img;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[Poster] Lỗi tải ảnh: " + ex.Message);
            }
        }

        private void CapNhatStatus(string text)
        {
            var lbl = this.Controls.Find("lblStatus", true);
            if (lbl.Length > 0 && lbl[0] is Label statusLbl)
                statusLbl.Text = text;
        }

        private void HuyDangChay()
        {
            if (_cts != null)
            {
                try { _cts.Cancel(); } catch { }
                _cts.Dispose();
                _cts = null;
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Thêm button để xem thống kê polling (debugging)
        private void ShowPollingStats()
        {
            if (_client == null) return;
            
            var stats = _client.GetPollingStats();
            var message = "Smart Polling Statistics:\n\n";
            
            foreach (var kvp in stats)
            {
                message += $"Endpoint: {kvp.Key}\n";
                message += $"Stats: {kvp.Value}\n\n";
            }
            
            MessageBox.Show(message, "Polling Stats", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== Handlers thêm (Designer tham chiếu) =====
        private void txtQuery_TextChanged(object sender, EventArgs e) { }
        private void flowMovies_Click(object sender, EventArgs e) { }
    }
}
