using Battleships.ServiceDefaults.Models;
using Battleships.Web.Viewmodels;
using System.Data;
using System.Drawing;

namespace Battleships.Web.Components.Pages
{
    public partial class Home
    {
        private GameViewModel? _model;

        protected override async Task OnInitializedAsync()
        {
            //_model = await Http.GetFromJsonAsync<GameViewModel>("api/game/1");
            //if (_model == null)
            //{
            //    throw new DataException("Game not found");
            //}

            _model = new GameViewModel(new Size(10, 10));
        }
    }
}
