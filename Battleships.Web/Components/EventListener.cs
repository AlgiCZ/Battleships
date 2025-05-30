using Microsoft.JSInterop;

namespace Battleships.Web.Components
{
    public static class EventListener
    {
        public static event EventHandler<double>? OnMouseMoveEvent;
        public static event EventHandler? OnMouseUpEvent;
        public static event EventHandler<string>? OnKeyDownEvent;

        [JSInvokable]
        public static Task OnMouseMove(double pageX, double pageY)
        {
            OnMouseMoveEvent?.Invoke(null, pageX);
            return Task.CompletedTask;
        }

        [JSInvokable]
        public static Task OnMouseUp()
        {
            OnMouseUpEvent?.Invoke(null, EventArgs.Empty);
            return Task.CompletedTask;
        }

        [JSInvokable]
        public static Task OnKeyDown(string code)
        {
            OnKeyDownEvent?.Invoke(null, code);
            return Task.CompletedTask;
        }
    }
}
