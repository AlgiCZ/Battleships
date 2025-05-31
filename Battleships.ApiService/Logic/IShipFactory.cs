using Battleships.ServiceDefaults.Models;

namespace Battleships.ApiService.Logic
{
    public interface IShipFactory
    {
        List<Ship> CreateDefaulShips();
    }
}