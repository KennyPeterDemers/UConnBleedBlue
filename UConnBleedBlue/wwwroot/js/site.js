window.setFocus = (usernameLoginId) => {
    const el = document.getElementById(usernameLoginId);
    if (el) {
        el.focus();
    }
};