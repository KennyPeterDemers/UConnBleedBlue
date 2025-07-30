window.enterFullscreen = (element) => {
    if (element.requestFullscreen) {
        console.warn("Fullscreen 1");
        element.requestFullscreen();
    } else if (element.webkitRequestFullscreen) {
        console.warn("Fullscreen 2");
        element.webkitRequestFullscreen(); // Safari
    } else if (element.msRequestFullscreen) {
        console.warn("Fullscreen 3");
        element.msRequestFullscreen(); // IE11
    } else {
        console.warn("Fullscreen API not supported.");
    }
};
