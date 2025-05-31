using Battleships.ServiceDefaults.Models;
using Battleships.Web.Models;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions.Canvas.WebGL;
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

        [Parameter]
        public bool IsOpponent { get; set; } = false;

        [Parameter]
        public EventCallback<Point> OnImpact { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; } = default!;

        private Size _size = new Size(300, 300);

        private BECanvasComponent? _canvas;
        private Canvas2DContext? _ctx;
        private Point? _mousePos;
        private ElementReference _div;

        private const string _waterColor = "#00ccff";
        private const string _fireColor = "#ffcc00";
        private const string _deadColor = "#ff0000";
        private const string _cursorColor = "#ffffff";
        private const string _shipColor = "#00ff00";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _ctx = await _canvas.CreateCanvas2DAsync();
                await RenderFrameAsync();
            }
        }

        private async Task<Point> GetMousePosAsync(MouseEventArgs e)
        {
            var rect = await JS.InvokeAsync<BoundingClientRect>("getBoundingClientRect", _div);

            return  new Point((int)(e.ClientX - rect.Left), (int)(e.ClientY - rect.Top));
        }

        private async void OnMouseMove(MouseEventArgs e)
        {
            _mousePos = await GetMousePosAsync(e);
            Invalidate();
        }

        private void OnMouseLeave()
        {
            _mousePos = null;
            Invalidate();
        }

        private async void OnMouseDown(MouseEventArgs e)
        {
            int w = GetW();
            _mousePos = await GetMousePosAsync(e);
            if (_mousePos.Value.X < 0 || _mousePos.Value.Y < 0 || _mousePos.Value.X >= Dimension.Width * w || _mousePos.Value.Y >= Dimension.Height * w)
            {
                return; // Out of bounds
            }
            await OnImpact.InvokeAsync(new Point((_mousePos.Value.X / w), (_mousePos.Value.Y / w)));
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
            if (!IsOpponent)
                await DrawShipsAsync();
            await DrawImpacts();
            await DrawCursor();

            await _ctx.EndBatchAsync();
        }

        private int GetW() => Math.Min(_size.Width / Dimension.Width, _size.Height / Dimension.Height);

        private async Task DrawImpacts()
        {
            if (PlayerImpacts.Count == 0 || _ctx == null)
                return;

            var w = GetW();
            var cursorSize = w / 4;            
            
            foreach (var item in PlayerImpacts)
            {
                switch (item.Type)
                {
                    case ServiceDefaults.ImpactType.Water:
                        await _ctx.SetFillStyleAsync(_waterColor);
                        break;
                    case ServiceDefaults.ImpactType.Ship:
                        await _ctx.SetFillStyleAsync(_fireColor);
                        break;
                    case ServiceDefaults.ImpactType.Destroyed:
                        await _ctx.SetFillStyleAsync(_deadColor);
                        break;
                    default:
                        continue;
                }

                await _ctx.BeginPathAsync();
                int x = item.Point.X * w + 1;
                int y = item.Point.Y * w + 1;
                await _ctx.ArcAsync(x + w / 2, y + w / 2, cursorSize, 0, MathF.PI * 2, false);                
                await _ctx.FillAsync();
            }            
        }

        private async Task DrawCursor()
        {
            if (!_mousePos.HasValue || _ctx == null || !IsOpponent)
                return;

            var w = GetW();
            var x = (_mousePos.Value.X / w) * w;
            var y = (_mousePos.Value.Y / w) * w;
            if (x < 0 || y < 0 || x >= Dimension.Width * w || y >= Dimension.Height * w)
            {
                return; // Out of bounds
            }

            await _ctx.BeginPathAsync();
            var cursorSize = w / 4;
            await _ctx.ArcAsync(x + w / 2, y + w / 2, cursorSize, 0, MathF.PI * 2, false);
            await _ctx.SetFillStyleAsync(_cursorColor);
            await _ctx.FillAsync();
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
            await _ctx.SetFillStyleAsync(_shipColor);
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
                    await _ctx.FillRectAsync(x, y, w - 2, w - 2);
                }
            }
        }
    }
}
