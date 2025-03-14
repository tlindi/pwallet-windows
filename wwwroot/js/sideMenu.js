// Function to toggle the NavMenu
function toggleNavMenu() {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('active'); // Toggle the active class

    // Check if the sidebar is active
    if (sidebar.classList.contains('active')) {
        // Add a backdrop if the sidebar is active
        addBackdrop();
    } else {
        // Remove the backdrop if the sidebar is not active
        removeBackdrop();
    }
}

// Function to add a backdrop
function addBackdrop() {
    const backdrop = document.createElement('div');
    backdrop.className = 'sidebar-backdrop';
    document.body.appendChild(backdrop);

    // Add event listener to close the menu when clicking on the backdrop
    backdrop.addEventListener('click', () => {
        const sidebar = document.querySelector('.sidebar');
        sidebar.classList.remove('active'); // Close the menu
        removeBackdrop(); // Remove the backdrop
    });
}

// Function to remove the backdrop
function removeBackdrop() {
    const backdrop = document.querySelector('.sidebar-backdrop');
    if (backdrop) {
        backdrop.remove();
    }
}

// Close NavMenu when a NavLink is clicked
const navLinks = document.querySelectorAll('.nav-link');
navLinks.forEach(link => {
    link.addEventListener('click', () => {
        const sidebar = document.querySelector('.sidebar');
        sidebar.classList.remove('active'); // Close the menu when a link is clicked
        removeBackdrop(); // Remove the backdrop
    });
});

function addNavLinkClickListener() {
    const navLinks = document.querySelectorAll('.nav-link');
    navLinks.forEach(link => {
        link.addEventListener('click', () => {
            const sidebar = document.querySelector('.sidebar');
            sidebar.classList.remove('active'); // Close the menu when a link is clicked
            removeBackdrop(); // Remove the backdrop
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
        removeBackdrop(); // Remove the backdrop
    }
}
