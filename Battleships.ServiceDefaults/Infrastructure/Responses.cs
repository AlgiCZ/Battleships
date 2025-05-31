using Battleships.ServiceDefaults.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ServiceDefaults.Infrastructure
{
    public record GameResponse(int GameId);
    public record GameStatusResponse(GameStatus GameStatus);
    public record GetRandomShipPositionsResponse(IList<Ship> Ships);
    public record GetGameResponse(Size Dimension, IList<Ship> Ships, IList<Impact> PlayerImpacts,
        IList<Impact> OpponentImpacts, GameStatus GameStatus, string Player1Name, string Player2Name);
    public record UpdateImpactsResponse(GameStatus GameStatus, List<Impact> Impacts);
}
