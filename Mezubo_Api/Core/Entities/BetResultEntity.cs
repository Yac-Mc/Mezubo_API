using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mezubo_Api.Core.Entities
{
    [BsonCollection("BetResult")]
    public class BetResultEntity : Document
    {
        [BsonElement("winningNumber")]
        public int WinningNumber { get; set; }

        [BsonElement("winningColor")]
        public string WinningColor { get; set; }

        [BsonElement("bets")]
        public List<BetEntity> ListBets { get; set; } = new List<BetEntity>();

        [BsonElement("winners")]
        public List<WinnerEntity> ListWinners { get; set; } = new List<WinnerEntity>();
    }
}
