namespace Battleships.ApiService.Storage
{
    public class GamesStorage : IGamesStorage
    {
        private List<Game> _games;

        public GamesStorage()
        {
            _games = new List<Game>();
        }

        public IList<Game> GetAllGames()
        {
            return _games;
        }

        public Game? GetGameById(int id)
        {
            return _games.FirstOrDefault(g => g.Id == id);
        }

        public Game AddGame(Game game)
        {
            game.Id = _games.Count > 0 ? _games.Max(g => g.Id) + 1 : 1;
            _games.Add(game);

            return game;
        }
    }
}
