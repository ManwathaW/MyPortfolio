/**
 * ===========================
 * TIER 3 & TIER 4 ENHANCEMENTS
 * ===========================
 * Analytics, Performance Metrics, UI/UX Polish
 */

class PortfolioEnhancementManager {
    constructor() {
        this.initBackToTop();
        this.initToastContainer();
        this.initLazyLoading();
        this.initBreadcrumbs();
        this.initAnalytics();
        this.initPerformanceMetrics();
    }

    // ========== BACK TO TOP BUTTON ==========

    initBackToTop() {
        const backToTopBtn = document.getElementById('backToTop');
        if (!backToTopBtn) {
            this.createBackToTopButton();
        }

        const btn = document.getElementById('backToTop');
        
        // Show button when scrolled down
        window.addEventListener('scroll', () => {
            if (window.pageYOffset > 300) {
                btn.style.display = 'flex';
            } else {
                btn.style.display = 'none';
            }
        }, { passive: true });

        // Smooth scroll to top
        btn.addEventListener('click', () => {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    createBackToTopButton() {
        const btn = document.createElement('button');
        btn.id = 'backToTop';
        btn.innerHTML = '<i class="bi bi-arrow-up"></i>';
        btn.setAttribute('title', 'Back to top');
        document.body.appendChild(btn);
    }

    // ========== TOAST NOTIFICATIONS ==========

    initToastContainer() {
        if (!document.querySelector('.toast-container')) {
            const container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
        }
    }

    showToast(message, type = 'info', durationMs = 3000) {
        const container = document.querySelector('.toast-container');
        
        const toast = document.createElement('div');
        toast.className = `toast toast-${type} fade-in-right`;
        
        const icons = {
            success: 'bi-check-circle',
            error: 'bi-exclamation-circle',
            warning: 'bi-exclamation-triangle',
            info: 'bi-info-circle'
        };

        toast.innerHTML = `
            <i class="bi ${icons[type] || icons.info}"></i>
            <span>${message}</span>
        `;

        container.appendChild(toast);

        // Auto-remove after duration
        setTimeout(() => {
            toast.classList.add('removing');
            setTimeout(() => toast.remove(), 300);
        }, durationMs);
    }

    // ========== LAZY LOADING IMAGES ==========

    initLazyLoading() {
        const images = document.querySelectorAll('img[data-src]');
        
        if ('IntersectionObserver' in window) {
            const imageObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const img = entry.target;
                        img.src = img.dataset.src;
                        img.removeAttribute('data-src');
                        img.classList.add('loaded');
                        observer.unobserve(img);
                    }
                });
            });

            images.forEach(img => {
                // Add loading class
                img.classList.add('lazy-image');
                imageObserver.observe(img);
            });
        } else {
            // Fallback for browsers without IntersectionObserver
            images.forEach(img => {
                img.src = img.dataset.src;
                img.removeAttribute('data-src');
            });
        }
    }

    // ========== BREADCRUMB NAVIGATION ==========

    initBreadcrumbs() {
        const breadcrumb = document.querySelector('.breadcrumb');
        if (breadcrumb) {
            // Add animation to breadcrumb items
            const items = breadcrumb.querySelectorAll('.breadcrumb-item');
            items.forEach((item, index) => {
                item.style.animation = `fadeInLeft 0.6s ease-out ${index * 0.1}s backwards`;
            });
        }
    }

    // ========== PAGINATION ==========

    initPagination() {
        const pageLinks = document.querySelectorAll('.page-link');
        pageLinks.forEach(link => {
            link.addEventListener('click', (e) => {
                if (!link.closest('.page-item.disabled')) {
                    const pageNumber = link.textContent.trim();
                    this.showToast(`Loading page ${pageNumber}...`, 'info', 2000);
                }
            });
        });
    }

    // ========== ANALYTICS ==========

    initAnalytics() {
        // Log page view
        const pageName = document.title;
        const userAgent = navigator.userAgent;
        const referrer = document.referrer || 'direct';

        this.logPageView(pageName, userAgent, referrer);

        // Track user interactions
        this.trackUserInteractions();
    }

    logPageView(pageName, userAgent, referrer) {
        const payload = {
            pageName,
            userAgent,
            referrer,
            timestamp: new Date().toISOString(),
            screenResolution: `${window.innerWidth}x${window.innerHeight}`
        };

        // Log to console (in production, send to analytics service)
        console.log('?? Page View Analytics:', payload);

        // Optional: Send to server
        // fetch('/api/analytics/pageview', {
        //     method: 'POST',
        //     headers: { 'Content-Type': 'application/json' },
        //     body: JSON.stringify(payload)
        // }).catch(err => console.error('Analytics error:', err));
    }

    trackUserInteractions() {
        // Track button clicks
        document.addEventListener('click', (e) => {
            const target = e.target.closest('button, a[href^="#"], .link-animated');
            if (target) {
                const action = target.textContent?.trim() || target.className;
                console.log('?? User Interaction:', {
                    action,
                    type: target.tagName,
                    timestamp: new Date().toISOString()
                });
            }
        }, true);

        // Track form submissions
        document.addEventListener('submit', (e) => {
            const form = e.target;
            console.log('?? Form Submission:', {
                formId: form.id,
                timestamp: new Date().toISOString()
            });
        }, true);

        // Track scroll depth
        let maxScrollDepth = 0;
        window.addEventListener('scroll', () => {
            const scrollPercent = (window.scrollY / (document.documentElement.scrollHeight - window.innerHeight)) * 100;
            maxScrollDepth = Math.max(maxScrollDepth, scrollPercent);

            if (Math.floor(scrollPercent) % 25 === 0 && scrollPercent > 0) {
                console.log(`?? Scroll Depth: ${Math.floor(scrollPercent)}%`);
            }
        }, { passive: true });
    }

    // ========== PERFORMANCE METRICS ==========

    initPerformanceMetrics() {
        if (window.performance && window.performance.timing) {
            window.addEventListener('load', () => {
                setTimeout(() => {
                    this.reportPerformanceMetrics();
                }, 0);
            });
        }
    }

    reportPerformanceMetrics() {
        const perfData = window.performance.timing;
        const pageLoadTime = perfData.loadEventEnd - perfData.navigationStart;
        const connectTime = perfData.responseEnd - perfData.requestStart;
        const renderTime = perfData.domComplete - perfData.domLoading;
        const domReadyTime = perfData.domContentLoadedEventEnd - perfData.navigationStart;

        const metrics = {
            pageLoadTime: `${pageLoadTime}ms`,
            connectTime: `${connectTime}ms`,
            renderTime: `${renderTime}ms`,
            domReadyTime: `${domReadyTime}ms`,
            timestamp: new Date().toISOString()
        };

        console.log('? Performance Metrics:', metrics);

        // Optional: Send to analytics service
        // fetch('/api/analytics/performance', {
        //     method: 'POST',
        //     headers: { 'Content-Type': 'application/json' },
        //     body: JSON.stringify(metrics)
        // }).catch(err => console.error('Performance tracking error:', err));
    }

    // ========== SEO SCHEMA MARKUP ==========

    addSchemaMarkup(schemaType = 'Person') {
        const schemas = {
            Person: {
                '@context': 'https://schema.org',
                '@type': 'Person',
                name: 'Manwatha Wanga',
                url: 'https://manwathawanga.com',
                image: '/Images/profile.webp',
                jobTitle: 'Full Stack Developer',
                sameAs: [
                    'https://github.com/ManwathaW',
                    'https://linkedin.com/in/wanga-manwatha-112wc'
                ],
                email: 'Manwathawanga312@gmail.com'
            },
            SoftwareApplication: {
                '@context': 'https://schema.org',
                '@type': 'SoftwareApplication',
                name: 'COMMUNIZEN',
                description: 'Mental Health Support Platform',
                applicationCategory: 'HealthApplication',
                operatingSystem: 'Android, iOS'
            }
        };

        const schema = schemas[schemaType] || schemas.Person;
        const script = document.createElement('script');
        script.type = 'application/ld+json';
        script.innerHTML = JSON.stringify(schema);
        document.head.appendChild(script);
    }

    // ========== COPY TO CLIPBOARD ==========

    setupCopyToClipboard() {
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('copy-to-clipboard')) {
                const text = e.target.dataset.text || e.target.textContent;
                navigator.clipboard.writeText(text).then(() => {
                    this.showToast('Copied to clipboard!', 'success', 2000);
                }).catch(() => {
                    this.showToast('Failed to copy', 'error', 2000);
                });
            }
        });
    }
}

// Initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.portfolioEnhancement = new PortfolioEnhancementManager();
    });
} else {
    window.portfolioEnhancement = new PortfolioEnhancementManager();
}

// ========== UTILITY FUNCTIONS ==========

/**
 * Show a toast notification
 * Usage: showToastNotification('Message', 'success')
 */
function showToastNotification(message, type = 'info', durationMs = 3000) {
    if (window.portfolioEnhancement) {
        window.portfolioEnhancement.showToast(message, type, durationMs);
    }
}

/**
 * Format performance metrics
 */
function formatPerformanceMetrics(metrics) {
    return {
        pageLoadTime: `${metrics.pageLoadTime}ms`,
        firstContentfulPaint: `${metrics.firstContentfulPaint}ms`,
        largestContentfulPaint: `${metrics.largestContentfulPaint}ms`,
        cumulativeLayoutShift: metrics.cumulativeLayoutShift.toFixed(3),
        timeToInteractive: `${metrics.timeToInteractive}ms`
    };
}

/**
 * Get performance summary
 */
function getPerformanceSummary() {
    const perfData = window.performance.timing;
    return {
        pageLoadTime: perfData.loadEventEnd - perfData.navigationStart,
        domContentLoaded: perfData.domContentLoadedEventEnd - perfData.navigationStart,
        firstPaint: perfData.responseStart - perfData.navigationStart
    };
}

/**
 * Track custom event
 */
function trackCustomEvent(eventName, eventData = {}) {
    const event = {
        name: eventName,
        data: eventData,
        timestamp: new Date().toISOString(),
        url: window.location.href
    };

    console.log(`?? Custom Event: ${eventName}`, event);

    // Optional: Send to analytics service
    // fetch('/api/analytics/event', {
    //     method: 'POST',
    //     headers: { 'Content-Type': 'application/json' },
    //     body: JSON.stringify(event)
    // }).catch(err => console.error('Event tracking error:', err));
}

/**
 * Debounce function for performance
 */
function debounce(func, delay = 250) {
    let timeoutId;
    return function (...args) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => func.apply(this, args), delay);
    };
}

/**
 * Throttle function for performance
 */
function throttle(func, limit = 250) {
    let inThrottle;
    return function (...args) {
        if (!inThrottle) {
            func.apply(this, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}
