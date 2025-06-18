// Navigation Menu script
window.setupNavMenu = function () {
    const navToggle = document.getElementById('navToggle');
    const navMenu = document.getElementById('navMenu');
    
    if (navToggle && navMenu) {
        // Clear any existing event listeners
        const newNavToggle = navToggle.cloneNode(true);
        navToggle.parentNode.replaceChild(newNavToggle, navToggle);
        
        // Add click event listener to toggle menu visibility
        newNavToggle.addEventListener('click', function() {
            navMenu.classList.toggle('hidden');
        });
    }
    
    // Handle responsive behavior
    function handleResize() {
        if (window.innerWidth >= 768) { // 768px is the md breakpoint in Tailwind
            navMenu.classList.remove('hidden');
        } else {
            navMenu.classList.add('hidden');
        }
    }
    
    // Set initial state based on screen size
    handleResize();
    
    // Handle window resize events
    window.removeEventListener('resize', handleResize);
    window.addEventListener('resize', handleResize);
    
    // Make menu links close the mobile menu when clicked
    const menuLinks = navMenu.querySelectorAll('a');
    menuLinks.forEach(link => {
        link.addEventListener('click', () => {
            if (window.innerWidth < 768) {
                navMenu.classList.add('hidden');
            }
        });
    });
    
    return true;
};
