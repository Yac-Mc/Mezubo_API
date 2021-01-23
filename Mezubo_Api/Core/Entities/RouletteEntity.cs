using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Globalization;

namespace Mezubo_Api.Core.Entities
{
    [BsonCollection("Roulette")]
    public class RouletteEntity : Document
    {

        [BsonElement("open")]
        public bool Open { get; set; } = false;

        [BsonElement("creationDate")]
        public string CreationDate { get; } = DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);

        [BsonElement("openDate")]
        public DateTime? OpenDate { get; set; }

        [BsonElement("closeDate")]
        public DateTime? ClosedDate { get; set; }
    }
}
