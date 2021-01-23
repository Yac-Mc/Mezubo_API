using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Mezubo_Api.Core.Entities
{
    [BsonCollection("RouletteBet")]
    public class BetEntity : Document
    {
        [Required]
        [BsonElement("rouletteId")]
        public string RouletteId { get; set; }

        [Required]
        [BsonElement("number")]
        public int Number { get; set; }

        [Required]
        [BsonElement("color")]
        public string Color { get; set; }

        [Required]
        [BsonElement("value")]
        public decimal Value { get; set; }
    }
}
