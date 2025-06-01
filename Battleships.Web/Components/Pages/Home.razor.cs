using Battleships.Web.Components.Game;
using Battleships.Web.Viewmodels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace Battleships.Web.Components.Pages
{
    public partial class Home
    {
        [Inject]
        private GameViewModel Model { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter]
        public string? PlayerName { get; set; }

        [Parameter]
        public int? GameId { get; set; }

        private BoardView? _p1Board;
        private BoardView? _p2Board;

        protected override async Task OnParametersSetAsync()
        {
            if (GameId != null && PlayerName != null && (Model.Id != GameId || Model.PlayerName != PlayerName))
            {
                await Model.JoinGame(GameId.Value, PlayerName);
                StateHasChanged();
            }
            base.OnParametersSet();
        }

        private async void NewGameClick()
        {
            await Model.NewGameAsync();
            NavigationManager.NavigateTo($"/{Model.Id}/{Model.Player1Name}");
            var uri = NavigationManager.BaseUri + $"{Model.Id}/{Model.Player2Name}";
            await JSRuntime.InvokeVoidAsync("open", uri, "_blank");
        }

        private async void ShuffleClick()
        {
            await Model.ShufflePlayerShipsAsync();
            StateHasChanged();
            _p1Board?.Invalidate();
        }

        private async void StartGameClick()
        {
            await Model.PlayerReady();
            StateHasChanged();

            await Model.WaitForPlayerTurn();
            StateHasChanged();
        }

        private async void OnImpact(Point point)
        {
            await Model.Shot(point);
            StateHasChanged();
            _p1Board?.Invalidate();
            _p2Board?.Invalidate();

            await Model.WaitForPlayerTurn();
            StateHasChanged();
        }
    }
}
