using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace DemoApplication1.Models.Entities.Concretes
{
    public class AccessTokenData
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string ObjectId { get; set; }

        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("ExpiresIn")]
        public string ExpiresIn { get; set; }

        [JsonProperty("DataAccessExpirationTime")]
        public string DataAccessExpirationTime { get; set; }

        [JsonProperty("IsActive")]
        public string IsActive { get; set; }

        [JsonProperty("ForeignKeyId")]
        public int ForeignKeyId { get; set; }





    }
}
