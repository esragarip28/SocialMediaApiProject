using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
namespace DemoApplication1.Models.Entities.Concretes
{
    public class InstagramUserProfile
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string ObjectId { get; set; }

        [JsonProperty("UserId")]// this is foreign key that we simplified before
        public string UserId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        [JsonProperty("follows_count")]
        public int FollowsCount { get; set; }

        [JsonProperty("media_count")]
        public int MediaCount { get; set; }

        [JsonProperty("profile_picture_url")]
        public Uri ProfilePictureUrl { get; set; }

        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("website")]
        public Uri Website { get; set; }

       
    }
}
