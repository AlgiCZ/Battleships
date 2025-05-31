using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ServiceDefaults.Infrastructure
{
    public record AddGameRequest(string Player1Name, string Player2Name, Size Dimension);
    public record GetRandomShipPositionsRequest(int GameId, string PlayerName);
    public record GetGameRequest(int GameId, string PlayerName);
    public record UpdateImpactsRequest(int GameId, string PlayerName, Point Point);
}
