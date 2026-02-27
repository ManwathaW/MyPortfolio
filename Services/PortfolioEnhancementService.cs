using System.Text.Json.Serialization;

namespace Portfolio.Services
{
    /// <summary>
    /// Service for portfolio analytics, performance metrics, and SEO enhancements
    /// TIER 3: Technical Enhancements
    /// </summary>
    public class PortfolioEnhancementService
    {
        private readonly ILogger<PortfolioEnhancementService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public PortfolioEnhancementService(
            ILogger<PortfolioEnhancementService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get page analytics data (would integrate with Google Analytics API)
        /// </summary>
        public class PageAnalytics
        {
            public string PageName { get; set; }
            public int PageViews { get; set; }
            public int UniqueVisitors { get; set; }
            public double AverageSessionDuration { get; set; }
            public double BounceRate { get; set; }
            public List<string> TopTraffic { get; set; } = new();
        }

        /// <summary>
        /// Get performance metrics
        /// </summary>
        public class PerformanceMetrics
        {
            public double PageLoadTime { get; set; }
            public double FirstContentfulPaint { get; set; }
            public double LargestContentfulPaint { get; set; }
            public double CumulativeLayoutShift { get; set; }
            public int PageSize { get; set; }
            public int RequestCount { get; set; }
            public int LighthouseScore { get; set; }
        }

        /// <summary>
        /// Get SEO metrics
        /// </summary>
        public class SEOMetrics
        {
            public int IndexedPages { get; set; }
            public int BacklinksCount { get; set; }
            public List<string> KeywordRankings { get; set; } = new();
            public double DomainAuthority { get; set; }
            public string MetaTitle { get; set; }
            public string MetaDescription { get; set; }
            public List<string> StructuredData { get; set; } = new();
        }

        /// <summary>
        /// Get SEO metrics for a page
        /// </summary>
        public SEOMetrics GetSEOMetrics(string pageName)
        {
            return new SEOMetrics
            {
                MetaTitle = GetMetaTitle(pageName),
                MetaDescription = GetMetaDescription(pageName),
                StructuredData = GetStructuredData(pageName),
                IndexedPages = 15,
                BacklinksCount = 25,
                DomainAuthority = 35.0,
                KeywordRankings = GetKeywordRankings(pageName)
            };
        }

        /// <summary>
        /// Get structured data (Schema.org) for a page
        /// </summary>
        public List<string> GetStructuredData(string pageName)
        {
            return pageName.ToLower() switch
            {
                "index" => new List<string>
                {
                    "Person",
                    "ProfessionalService",
                    "LocalBusiness"
                },
                "about" => new List<string>
                {
                    "Person",
                    "BiomedicalEntity"
                },
                "projects" => new List<string>
                {
                    "SoftwareApplication",
                    "CreativeWork"
                },
                "contact" => new List<string>
                {
                    "ContactPoint",
                    "LocalBusiness"
                },
                _ => new List<string> { "WebPage" }
            };
        }

        /// <summary>
        /// Get meta description for SEO
        /// </summary>
        private string GetMetaDescription(string pageName)
        {
            return pageName.ToLower() switch
            {
                "index" => "Full Stack Developer specializing in .NET, PHP, and Mobile Development. Explore my projects and expertise.",
                "about" => "Learn about my journey as a full stack developer, skills, and passion for creating impactful software solutions.",
                "projects" => "Browse my portfolio of projects including COMMUNIZEN mental health app and more enterprise solutions.",
                "skills" => "Technical skills and expertise in ASP.NET Core, .NET MAUI, PHP, Laravel, and cloud technologies.",
                "contact" => "Get in touch for freelance work, collaboration, or just a chat about tech.",
                _ => "Manwatha Wanga - Full Stack Developer Portfolio"
            };
        }

        /// <summary>
        /// Get meta title for SEO
        /// </summary>
        private string GetMetaTitle(string pageName)
        {
            return pageName.ToLower() switch
            {
                "index" => "Manwatha Wanga | Full Stack Developer & .NET Expert",
                "about" => "About Me | Manwatha Wanga - Full Stack Developer",
                "projects" => "My Projects | Manwatha Wanga Portfolio",
                "skills" => "Technical Skills | Manwatha Wanga",
                "contact" => "Contact | Manwatha Wanga",
                _ => "Manwatha Wanga - Full Stack Developer"
            };
        }

        /// <summary>
        /// Get keyword rankings for a page
        /// </summary>
        private List<string> GetKeywordRankings(string pageName)
        {
            return pageName.ToLower() switch
            {
                "index" => new List<string>
                {
                    ".NET Developer",
                    "Full Stack Developer",
                    "ASP.NET Core Developer",
                    "C# Developer"
                },
                "projects" => new List<string>
                {
                    ".NET Projects",
                    "Mobile App Development",
                    "Web Development Portfolio"
                },
                _ => new List<string>()
            };
        }

        /// <summary>
        /// Log page view for analytics
        /// </summary>
        public void LogPageView(string pageName, string userAgent, string referrer)
        {
            _logger.LogInformation(
                "Page View: {PageName} | UserAgent: {UserAgent} | Referrer: {Referrer}",
                pageName, userAgent, referrer);
        }

        /// <summary>
        /// Log user interaction for analytics
        /// </summary>
        public void LogUserInteraction(string pageName, string action, string details)
        {
            _logger.LogInformation(
                "User Interaction: {PageName} | Action: {Action} | Details: {Details}",
                pageName, action, details);
        }

        /// <summary>
        /// Get performance summary
        /// </summary>
        public PerformanceMetrics GetPerformanceMetrics()
        {
            return new PerformanceMetrics
            {
                PageLoadTime = 1.8,
                FirstContentfulPaint = 0.9,
                LargestContentfulPaint = 2.1,
                CumulativeLayoutShift = 0.05,
                PageSize = 450, // KB
                RequestCount = 28,
                LighthouseScore = 85
            };
        }
    }

    /// <summary>
    /// TIER 4: UI/UX Enhancement Service
    /// Handles breadcrumbs, notifications, pagination, etc.
    /// </summary>
    public class UIUXEnhancementService
    {
        /// <summary>
        /// Breadcrumb item model
        /// </summary>
        public class BreadcrumbItem
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public bool IsActive { get; set; }
        }

        /// <summary>
        /// Toast notification model
        /// </summary>
        public class ToastNotification
        {
            public string Message { get; set; }
            public string Type { get; set; } // success, error, warning, info
            public int DurationMs { get; set; } = 3000;
            public string Icon { get; set; }
        }

        /// <summary>
        /// Pagination model
        /// </summary>
        public class PaginationInfo
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int TotalItems { get; set; }
            public int ItemsPerPage { get; set; }
            public bool HasPreviousPage => CurrentPage > 1;
            public bool HasNextPage => CurrentPage < TotalPages;
            public int StartItem => (CurrentPage - 1) * ItemsPerPage + 1;
            public int EndItem => Math.Min(CurrentPage * ItemsPerPage, TotalItems);
        }

        private readonly ILogger<UIUXEnhancementService> _logger;

        public UIUXEnhancementService(ILogger<UIUXEnhancementService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generate breadcrumb navigation
        /// </summary>
        public List<BreadcrumbItem> GetBreadcrumbs(string currentPage, string currentTitle)
        {
            var breadcrumbs = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { Title = "Home", Url = "/", IsActive = false }
            };

            if (!string.IsNullOrEmpty(currentPage) && currentPage.ToLower() != "index")
            {
                breadcrumbs.Add(new BreadcrumbItem
                {
                    Title = currentTitle,
                    Url = $"/{currentPage}",
                    IsActive = true
                });
            }
            else
            {
                breadcrumbs[0].IsActive = true;
            }

            return breadcrumbs;
        }

        /// <summary>
        /// Create success toast notification
        /// </summary>
        public ToastNotification CreateSuccessToast(string message, int durationMs = 3000)
        {
            return new ToastNotification
            {
                Message = message,
                Type = "success",
                DurationMs = durationMs,
                Icon = "bi-check-circle"
            };
        }

        /// <summary>
        /// Create error toast notification
        /// </summary>
        public ToastNotification CreateErrorToast(string message, int durationMs = 5000)
        {
            return new ToastNotification
            {
                Message = message,
                Type = "error",
                DurationMs = durationMs,
                Icon = "bi-exclamation-circle"
            };
        }

        /// <summary>
        /// Create warning toast notification
        /// </summary>
        public ToastNotification CreateWarningToast(string message, int durationMs = 4000)
        {
            return new ToastNotification
            {
                Message = message,
                Type = "warning",
                DurationMs = durationMs,
                Icon = "bi-exclamation-triangle"
            };
        }

        /// <summary>
        /// Create info toast notification
        /// </summary>
        public ToastNotification CreateInfoToast(string message, int durationMs = 3000)
        {
            return new ToastNotification
            {
                Message = message,
                Type = "info",
                DurationMs = durationMs,
                Icon = "bi-info-circle"
            };
        }

        /// <summary>
        /// Calculate pagination info
        /// </summary>
        public PaginationInfo CalculatePagination(int totalItems, int currentPage, int itemsPerPage = 10)
        {
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            
            if (currentPage < 1) currentPage = 1;
            if (currentPage > totalPages) currentPage = totalPages;

            return new PaginationInfo
            {
                CurrentPage = currentPage,
                TotalPages = totalPages,
                TotalItems = totalItems,
                ItemsPerPage = itemsPerPage
            };
        }

        /// <summary>
        /// Get page numbers for pagination display
        /// </summary>
        public List<int> GetPageNumbers(int currentPage, int totalPages, int maxDisplay = 5)
        {
            var pageNumbers = new List<int>();
            int startPage = Math.Max(1, currentPage - maxDisplay / 2);
            int endPage = Math.Min(totalPages, startPage + maxDisplay - 1);

            if (endPage - startPage < maxDisplay - 1)
            {
                startPage = Math.Max(1, endPage - maxDisplay + 1);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                pageNumbers.Add(i);
            }

            return pageNumbers;
        }
    }
}
