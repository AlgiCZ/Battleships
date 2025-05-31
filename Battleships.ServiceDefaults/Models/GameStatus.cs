namespace Battleships.ServiceDefaults.Models
{
    public enum GameStatus
    {
        Unknown,
        Created,
        Planning,
        WaitingForPlayer1,
        WaitingForPlayer2,
        Player1Turn,
        Player2Turn,
        Player1Won,
        Player2Won
    }
}
