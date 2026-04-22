// ===== NEON-FLORA CINEMA — JS =====

// Navbar dropdown user menu
const btnUserToggle = document.getElementById('btn-user-toggle');
const userDropdown  = document.getElementById('user-dropdown');
if (btnUserToggle && userDropdown) {
    btnUserToggle.addEventListener('click', (e) => {
        e.stopPropagation();
        userDropdown.classList.toggle('open');
    });
    document.addEventListener('click', () => userDropdown.classList.remove('open'));
}

// Mobile navbar toggle
const mobileToggle = document.getElementById('mobile-toggle');
const navLinks     = document.getElementById('nav-links');
if (mobileToggle && navLinks) {
    mobileToggle.addEventListener('click', () => {
        navLinks.classList.toggle('mobile-open');
        mobileToggle.classList.toggle('open');
    });
}

// Navbar scroll effect
window.addEventListener('scroll', () => {
    const navbar = document.getElementById('navbar');
    if (navbar) {
        if (window.scrollY > 50) {
            navbar.style.boxShadow = '0 4px 30px rgba(0,245,255,0.1)';
        } else {
            navbar.style.boxShadow = 'none';
        }
    }
}, { passive: true });

// Auto-dismiss alerts
document.querySelectorAll('.alert').forEach(alert => {
    setTimeout(() => {
        alert.style.opacity = '0';
        alert.style.transform = 'translateX(20px)';
        setTimeout(() => alert.remove(), 400);
    }, 5000);
});
