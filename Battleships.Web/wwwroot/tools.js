document.addEventListener('mousemove', function (event) {
    DotNet.invokeMethodAsync('Battleships.Web.Components', 'OnMouseMove', event.pageX, event.pageY);
});

document.addEventListener('mouseup', function (event) {
    DotNet.invokeMethodAsync('Battleships.Web.Components', 'OnMouseUp');
});

document.addEventListener('keydown', function (event) {
    DotNet.invokeMethodAsync('Battleships.Web.Components', 'OnKeyDown', event.code);
});