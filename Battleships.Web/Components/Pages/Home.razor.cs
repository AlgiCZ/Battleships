using Battleships.ServiceDefaults.Models;
using Battleships.Web.Components.Game;
using Battleships.Web.Viewmodels;
using System.Data;
using System.Drawing;

namespace Battleships.Web.Components.Pages
{
    public partial class Home
    {
        private GameViewModel _model = new GameViewModel(new Size(10, 10));

        private BoardView? _p1Board;
        private BoardView? _p2Board;

        protected override async Task OnInitializedAsync()
        {
            //_model = await Http.GetFromJsonAsync<GameViewModel>("api/game/1");
            //if (_model == null)
            //{
            //    throw new DataException("Game not found");
            //}

            await Task.CompletedTask;
        }

        private void NewGameClick()
        {
            _model.NewGame();
        }

        private void ShuffleClick()
        {
            _model.ShufflePlayer1Ships();
            _p1Board?.Invalidate();
        }

        private void StartGameClick()
        {
            _model.StartGame();
        }
    }
}
