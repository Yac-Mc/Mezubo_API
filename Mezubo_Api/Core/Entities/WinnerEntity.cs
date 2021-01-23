using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mezubo_Api.Core.Entities
{
    public class WinnerEntity
    {
        [BsonElement("isNumber")]
        public bool IsWinningNumber { get; set; }

        [BsonElement("isColor")]
        public bool IsWinningColor { get; set; }

        [BsonElement("earnedValue")]
        public decimal EarnedValue { get; set; }
    }
}
