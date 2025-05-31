using Battleships.ServiceDefaults.Infrastructure;
using Battleships.ServiceDefaults.Models;

namespace Battleships.Web.Services
{
    public class ApiClient(HttpClient httpClient) : IApiClient
    {

        public async Task<GameResponse> AddGameAsync(AddGameRequest request, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PostAsJsonAsync("/api/games/add", request, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<GameResponse>(cancellationToken: cancellationToken) ??
                throw new InvalidOperationException("Failed to deserialize GameResponse.");
        }

        public async Task<GetRandomShipPositionsResponse> GetRandomShipPositionsAsync(GetRandomShipPositionsRequest request, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetFromJsonAsync<GetRandomShipPositionsResponse>($"/api/games/{request.GameId}/randomshippositions?playerName={request.PlayerName}", cancellationToken) ?? throw new InvalidOperationException("Failed to get random ship positions or game not found.");

            return response;
        }

        public async Task<GetGameResponse> JoinGame(GetGameRequest request, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetFromJsonAsync<GetGameResponse>($"/api/games/{request.GameId}?playerName={request.PlayerName}", cancellationToken) ??
                throw new InvalidOperationException("Failed to get game or game not found.");

            return response;
        }

        public async Task<GameStatus> SendPlayerReady(int gameId, string playerName, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PutAsync($"/api/games/{gameId}/ReadyPlayerStatus?playerName={playerName}", null, cancellationToken);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<GameStatusResponse>(cancellationToken: cancellationToken))?.GameStatus ??
                throw new InvalidOperationException("Failed to deserialize GameResponse.");
        }

        public async Task<GameStatus> GetGameStatus(int gameId, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync($"/api/games/{gameId}/GameStatus", cancellationToken);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<GameStatusResponse>(cancellationToken: cancellationToken))?.GameStatus ??
                throw new InvalidOperationException("Failed to deserialize GameResponse.");
        }

        public async Task<UpdateImpactsResponse> UpdateImpacts(int gameId, UpdateImpactsRequest request, CancellationToken cancellationToken = default)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/games/{gameId}/", request, cancellationToken) ??
                throw new InvalidOperationException("Failed to get game or game not found.");

            return (await response.Content.ReadFromJsonAsync<UpdateImpactsResponse>(cancellationToken: cancellationToken)) ??
                throw new InvalidOperationException("Failed to deserialize UpdateImpactsResponse.");
        }

    }
}
