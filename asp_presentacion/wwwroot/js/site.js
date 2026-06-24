(function() {
    function initTheme() {
        const savedTheme = localStorage.getItem('bibliotechTheme');
        if (savedTheme === 'dark') {
            document.body.classList.add('dark-theme');
        } else if (savedTheme === 'light') {
            document.body.classList.remove('dark-theme');
        } else {
            if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                document.body.classList.add('dark-theme');
                localStorage.setItem('bibliotechTheme', 'dark');
            }
        }
        
        const themeIcon = document.querySelector('#themeToggle i');
        if (themeIcon) {
            if (document.body.classList.contains('dark-theme')) {
                themeIcon.classList.remove('bi-moon-stars');
                themeIcon.classList.add('bi-sun');
            } else {
                themeIcon.classList.remove('bi-sun');
                themeIcon.classList.add('bi-moon-stars');
            }
        }
    }
    
    function setupThemeToggle() {
        const themeToggle = document.getElementById('themeToggle');
        if (themeToggle) {
            const newToggle = themeToggle.cloneNode(true);
            themeToggle.parentNode.replaceChild(newToggle, themeToggle);
            
            newToggle.addEventListener('click', function() {
                const isDark = document.body.classList.contains('dark-theme');
                if (isDark) {
                    document.body.classList.remove('dark-theme');
                    localStorage.setItem('bibliotechTheme', 'light');
                } else {
                    document.body.classList.add('dark-theme');
                    localStorage.setItem('bibliotechTheme', 'dark');
                }
                
                const icon = this.querySelector('i');
                if (icon) {
                    if (document.body.classList.contains('dark-theme')) {
                        icon.classList.remove('bi-moon-stars');
                        icon.classList.add('bi-sun');
                    } else {
                        icon.classList.remove('bi-sun');
                        icon.classList.add('bi-moon-stars');
                    }
                }
            });
        }
    }
    
    initTheme();
    setupThemeToggle();
})();