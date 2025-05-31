using Battleships.ServiceDefaults.Infrastructure;
using Battleships.ServiceDefaults.Models;

namespace Battleships.Web.Services
{
    public interface IApiClient
    {
        Task<GameResponse> AddGameAsync(AddGameRequest request, CancellationToken cancellationToken = default);
        Task<GameStatus> GetGameStatus(int gameId, CancellationToken cancellationToken = default);
        Task<GetRandomShipPositionsResponse> GetRandomShipPositionsAsync(GetRandomShipPositionsRequest request, CancellationToken cancellationToken = default);
        Task<GetGameResponse> JoinGame(GetGameRequest request, CancellationToken cancellationToken = default);
        Task<GameStatus> SendPlayerReady(int gameId, string playerName, CancellationToken cancellationToken = default);
        Task<UpdateImpactsResponse> UpdateImpacts(int gameId, UpdateImpactsRequest request, CancellationToken cancellationToken = default);
    }
}