
namespace Battleships.ApiService.Storage
{
    public interface IGamesStorage
    {
        Game AddGame(Game game);
        IList<Game> GetAllGames();
        Game? GetGameById(int id);
    }
}