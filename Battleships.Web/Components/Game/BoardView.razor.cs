using AvnCanvasHelper;
using Battleships.ServiceDefaults.Models;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Drawing;

namespace Battleships.Web.Components.Game
{
    public partial class BoardView
    {
        [Parameter]
        public required Size Dimension { get; set; }

        [Parameter]
        public required Impact[] PlayerImpacts { get; set; }

        [Parameter]
        public required Ship[] Ships { get; set; }

        private Size _size = new Size(300, 300);

        private BECanvasComponent? _ctx;
        private Canvas2DContext? _context;
        private CanvasHelper? _canvasHelper;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _ctx.CreateCanvas2DAsync();
                await _canvasHelper.Initialize();
            }
        }

        public async Task RenderFrame(double fps)
        {
            await _context.BeginBatchAsync();
            await _context.ClearRectAsync(0, 0, _size.Width, _size.Height);

            await DrawGridAsync();
            await DrawShipsAsync();

            await _context.EndBatchAsync();
        }

        private int GetW() => Math.Min(_size.Width / Dimension.Width, _size.Height / Dimension.Height);

        private async Task DrawGridAsync()
        {
            await _context.SetStrokeStyleAsync("#003366");
            var w = GetW();
            for (int x = 0; x < Dimension.Width; x++)
            {
                for (int y = 0; y < Dimension.Height; y++)
                {
                    await _context.StrokeRectAsync(x * w, y * w, w, w);
                }
            }
        }

        private async Task DrawShipsAsync()
        {
            var w = GetW();
            await _context.SetStrokeStyleAsync("#ff3366");
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
                    await _context.MoveToAsync(x, y);
                    await _context.LineToAsync(x + w - 2, y + w - 2);
                    await _context.StrokeAsync();

                    await _context.MoveToAsync(x + w - 2, y);
                    await _context.LineToAsync(x, y + w - 2);
                    await _context.StrokeAsync();
                }
            }
        }
    }
}
