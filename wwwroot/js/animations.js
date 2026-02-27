// ============================
// PREMIUM ANIMATION CONTROLLER
// ============================

class AnimationController {
    constructor() {
        this.observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };
        
        this.observer = new IntersectionObserver(
            this.handleIntersection.bind(this),
            this.observerOptions
        );
        
        this.init();
    }

    init() {
        // Setup scroll reveal animations
        this.setupScrollReveal();
        
        // Setup parallax effect on desktop
        if (window.innerWidth > 768) {
            this.setupParallax();
        }
        
        // Setup button ripple effect
        this.setupButtonRipple();
        
        // Setup smooth scrolling
        this.setupSmoothScroll();
        
        // Add animation classes to hero elements
        this.animateHeroOnLoad();
    }

    // Handle intersection observer for scroll animations
    handleIntersection(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                // Add active class to trigger animation
                entry.target.classList.add('active');
                
                // Optional: Stop observing after animation
                if (entry.target.classList.contains('scroll-reveal')) {
                    this.observer.unobserve(entry.target);
                }
            }
        });
    }

    // Setup scroll reveal for elements
    setupScrollReveal() {
        const revealElements = document.querySelectorAll(
            '.fade-in-up, .fade-in-down, .fade-in-left, .fade-in-right, ' +
            '.stagger-item, .section, .timeline-item, .contact-item'
        );

        revealElements.forEach(element => {
            element.classList.add('scroll-reveal');
            this.observer.observe(element);
        });
    }

    // Setup parallax effect
    setupParallax() {
        const parallaxElements = document.querySelectorAll('[data-parallax]');
        
        if (parallaxElements.length === 0) return;

        window.addEventListener('scroll', () => {
            const scrollPos = window.pageYOffset;

            parallaxElements.forEach(element => {
                const speed = element.dataset.parallax || 0.5;
                const yPos = scrollPos * speed;
                element.style.transform = `translateY(${yPos}px)`;
            });
        }, { passive: true });
    }

    // Setup button ripple effect
    setupButtonRipple() {
        const buttons = document.querySelectorAll('.btn-animated');

        buttons.forEach(button => {
            button.addEventListener('click', (e) => {
                const ripple = document.createElement('span');
                const rect = button.getBoundingClientRect();
                const size = Math.max(rect.width, rect.height);
                const x = e.clientX - rect.left - size / 2;
                const y = e.clientY - rect.top - size / 2;

                ripple.style.width = ripple.style.height = size + 'px';
                ripple.style.left = x + 'px';
                ripple.style.top = y + 'px';
                ripple.classList.add('ripple');

                // Remove previous ripples
                const ripples = button.querySelectorAll('.ripple');
                ripples.forEach(r => r.remove());

                button.appendChild(ripple);

                setTimeout(() => ripple.remove(), 600);
            });
        });
    }

    // Setup smooth scroll behavior
    setupSmoothScroll() {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', (e) => {
                const href = anchor.getAttribute('href');
                if (href !== '#') {
                    e.preventDefault();
                    const target = document.querySelector(href);
                    if (target) {
                        target.scrollIntoView({
                            behavior: 'smooth',
                            block: 'start'
                        });
                    }
                }
            });
        });
    }

    // Animate hero elements on page load
    animateHeroOnLoad() {
        const heroElements = document.querySelectorAll(
            '.hero-title, .hero-subtitle, .hero-description, ' +
            '.hero-image, .hero-buttons'
        );

        heroElements.forEach((element, index) => {
            element.style.opacity = '0';
            element.style.animation = `fadeInUp 0.8s ease-out ${index * 0.2}s forwards`;
        });
    }

    // Add animation to card on hover
    addCardHoverAnimation() {
        const cards = document.querySelectorAll('.card-hover');

        cards.forEach(card => {
            card.addEventListener('mouseenter', () => {
                card.style.willChange = 'transform, box-shadow';
            });

            card.addEventListener('mouseleave', () => {
                card.style.willChange = 'auto';
            });
        });
    }

    // Add animation to skill bars on scroll
    animateSkillBars() {
        const skillBars = document.querySelectorAll('.skill-progress-bar');

        skillBars.forEach((bar, index) => {
            const progress = bar.style.width;
            bar.style.width = '0';
            
            setTimeout(() => {
                bar.style.transition = 'width 1s ease-out';
                bar.style.width = progress;
            }, index * 100);
        });
    }

    // Trigger animation when element becomes visible
    triggerWhenVisible(selector, animationClass) {
        const elements = document.querySelectorAll(selector);

        elements.forEach(element => {
            const observer = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add(animationClass);
                        observer.unobserve(entry.target);
                    }
                });
            });

            observer.observe(element);
        });
    }
}

// Initialize animations when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        window.animationController = new AnimationController();
    });
} else {
    window.animationController = new AnimationController();
}

// Handle window resize for responsive animations
window.addEventListener('resize', () => {
    if (window.innerWidth > 768) {
        // Enable parallax on larger screens
        if (window.animationController) {
            window.animationController.setupParallax();
        }
    }
}, { passive: true });

// Performance: Disable animations on low-power devices
if (navigator.deviceMemory && navigator.deviceMemory < 4) {
    document.documentElement.style.setProperty('--animation-duration', '0.1s');
}

// Accessibility: Respect user's motion preferences
const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
if (prefersReducedMotion) {
    document.documentElement.style.setProperty('--animation-duration', '0.01ms');
}
