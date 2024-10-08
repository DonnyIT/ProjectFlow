document.addEventListener('DOMContentLoaded', function () {
    const menuIcon = document.querySelector('.menu-icon');
    const menuContent = document.querySelector('.menu-content');

    menuIcon.addEventListener('mouseover', () => {
        menuContent.style.display = 'block';
    });

    menuIcon.addEventListener('mouseout', () => {
        menuContent.style.display = 'none';
    });

    menuContent.addEventListener('mouseover', () => {
        menuContent.style.display = 'block';
    });

    menuContent.addEventListener('mouseout', () => {
        menuContent.style.display = 'none';
    });
});