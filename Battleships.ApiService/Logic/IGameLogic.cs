using Battleships.ApiService.Storage;
using Battleships.ServiceDefaults;
using Battleships.ServiceDefaults.Models;
using System.Drawing;

namespace Battleships.ApiService.Logic
{
    public interface IGameLogic
    {
        void CheckDestroyedShips(List<Ship> ships, List<Impact> impacts);
        ImpactType DecideImpactType(Point point, List<Ship> ships);
        void RandomizeShips(List<Ship> ships, Size dimension);
    }
}