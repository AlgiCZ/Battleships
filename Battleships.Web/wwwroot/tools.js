window.getBoundingClientRect = (element) => {
    if (!element) return null;
    const rect = element.getBoundingClientRect();
    return {
        top: rect.top,
        left: rect.left,
        width: rect.width,
        height: rect.height,
        right: rect.right,
        bottom: rect.bottom
    };
};

window.openInNewTab = (url) => {
    window.open(url, "_blank");
};



