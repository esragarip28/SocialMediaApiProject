using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace DemoApplication1.Models.Entities.Concretes
{
    public class InstagramBusinessInfo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string ObjectId { get; set; }


        [JsonProperty("UserId")]
        public string UserId { get; set; }


        [JsonProperty("PageId")]
        public string PageId { get; set; }


        [JsonProperty("IGUserId")]
        public string IGUserId { get; set; } //instagram user id



    }
}
