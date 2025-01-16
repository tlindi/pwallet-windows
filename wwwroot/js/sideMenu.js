function toggleNavMenu() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('active'); // Toggle the active class
}


// Close NavMenu when a NavLink is clicked
const navLinks = document.querySelectorAll('.nav-link');
navLinks.forEach(link => {
    link.addEventListener('click', () => {
        const sidebar = document.querySelector('.sidebar');
        sidebar.classList.remove('active'); // Close the menu when a link is clicked
    });
});

function addNavLinkClickListener() {
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            const sidebar = document.querySelector('.sidebar');
            sidebar.classList.remove('active'); // Close the menu when a link is clicked
        });
    });
}

// Close NavMenu when clicking outside of it
function closeNavMenu(event) {
    const sidebar = document.querySelector('.sidebar');
    const hamburgerMenu = document.getElementById('hamburger-menu');

    // Check if the click is outside the sidebar or hamburger button
    if (!sidebar.contains(event.target) && !hamburgerMenu.contains(event.target)) {
        sidebar.classList.remove('active'); // Remove active class to close the menu
    }
}