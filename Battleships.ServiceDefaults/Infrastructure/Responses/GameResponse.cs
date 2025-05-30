using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.ServiceDefaults.Infrastructure.Responses
{
    public record GameResponse
    {
        public required int Id { get; init; }
    }
}
