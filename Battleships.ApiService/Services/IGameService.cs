using Battleships.ServiceDefaults.Infrastructure;

namespace Battleships.ApiService.Services
{
    public interface IGameService
    {
        GameResponse CreateGame(AddGameRequest request);
        GetGameResponse GetGame(int gameId, string playerName);
        GameStatusResponse? GetGameStatus(int gameId);
        GetRandomShipPositionsResponse GetRandomShipPositions(int gameId, string playerName);
        UpdateImpactsResponse? UpdateImpacts(int gameId, UpdateImpactsRequest request);
        GameStatusResponse? UpdateReadyPlayerStatus(int gameId, string playerName);
    }
}
