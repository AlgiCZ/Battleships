using Battleships.ServiceDefaults.Models;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Drawing;

namespace Battleships.Web.Components.Game
{
    public partial class BoardView
    {
        [Parameter]
        public required Size Dimension { get; set; }

        [Parameter]
        public required List<Impact> PlayerImpacts { get; set; }

        [Parameter]
        public required List<Ship> Ships { get; set; }

        private Size _size = new Size(300, 300);

        private BECanvasComponent? _canvas;
        private Canvas2DContext? _ctx;
        private double? _mouseX;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _ctx = await _canvas.CreateCanvas2DAsync();
                //EventListener.OnMouseMoveEvent += OnMouseMove;
                //EventListener.OnMouseUpEvent += OnMouseUp;
                await RenderFrameAsync();
            }
        }

        private async void OnMouseMove(MouseEventArgs args)
        {
            //_mouseX = pageX;
            Invalidate();
        }

        private async void OnMouseUp(object? sender, EventArgs e)
        {

        }

        public async void Invalidate()
        {
            await RenderFrameAsync();
        }

        private async Task RenderFrameAsync()
        {
            if (_ctx == null)
            {
                return;
            }

            await _ctx.BeginBatchAsync();
            await _ctx.ClearRectAsync(0, 0, _size.Width, _size.Height);

            await DrawGridAsync();
            await DrawShipsAsync();

            await _ctx.EndBatchAsync();
        }

        private int GetW() => Math.Min(_size.Width / Dimension.Width, _size.Height / Dimension.Height);

        private async Task DrawCursor()
        {
            if (!_mouseX.HasValue || _ctx == null)
                return;

            var x = (int)_mouseX;
            var y = 10;
            var w = GetW();

            await _ctx.MoveToAsync(x, y);
            await _ctx.LineToAsync(x + w - 2, y + w - 2);
            await _ctx.StrokeAsync();
            await _ctx.MoveToAsync(x + w - 2, y);
            await _ctx.LineToAsync(x, y + w - 2);
            await _ctx.StrokeAsync();
        }

        private async Task DrawGridAsync()
        {
            if (_ctx == null)
            {
                return;
            }

            await _ctx.SetStrokeStyleAsync("#a0a0a0");
            var w = GetW();
            for (int x = 0; x < Dimension.Width; x++)
            {
                for (int y = 0; y < Dimension.Height; y++)
                {
                    await _ctx.StrokeRectAsync(x * w, y * w, w, w);
                }
            }
        }

        private async Task DrawShipsAsync()
        {
            if (_ctx == null || Ships == null || Ships.Count == 0)
            {
                return;
            }

            var w = GetW();
            //await _context.SetStrokeStyleAsync("#ff3366");
            await _ctx.SetFillStyleAsync("#00ccff");
            foreach (var ship in Ships)
            {
                if (!ship.Position.HasValue)
                {
                    throw new InvalidOperationException("Ship position is not set.");
                }

                foreach (var hull in ship.Hull)
                {
                    var x = (hull.X + ship.Position.Value.X) * w + 1;
                    var y = (hull.Y + ship.Position.Value.Y) * w + 1;
                    //await _context.MoveToAsync(x, y);
                    //await _context.LineToAsync(x + w - 2, y + w - 2);
                    //await _context.StrokeAsync();

                    //await _context.MoveToAsync(x + w - 2, y);
                    //await _context.LineToAsync(x, y + w - 2);
                    //await _context.StrokeAsync();

                    await _ctx.FillRectAsync(x, y, w - 2, w - 2);
                }
            }
        }
    }
}
