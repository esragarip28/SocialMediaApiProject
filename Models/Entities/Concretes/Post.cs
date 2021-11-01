using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace DemoApplication1.Models.Entities
{
        public  class Post
        {

            [BsonId]
            [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
            [JsonProperty("_id")]
            public string ObjectId { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("UserId")]
            public string UserId { get; set; }

             [JsonProperty("comments_count")]
            public long CommentsCount { get; set; }

            [JsonProperty("ig_id")]
            public string IgId { get; set; }

            [JsonProperty("is_comment_enabled")]
            public bool IsCommentEnabled { get; set; }

            [JsonProperty("like_count")]
            public int LikeCount { get; set; }
           
            [JsonProperty("is_active")]
            public bool IsActive { get; set; }

            [JsonProperty("media_product_type")]
            public string MediaProductType { get; set; }

            [JsonProperty("media_type")]
            public string MediaType { get; set; }

            [JsonProperty("media_url")]
            public Uri MediaUrl { get; set; }

            [JsonProperty("owner")]
            public Owner Owner { get; set; }

            [JsonProperty("permalink")]
            public Uri Permalink { get; set; }

            [JsonProperty("shortcode")]
            public string Shortcode { get; set; }

            [JsonProperty("timestamp")]
            public string Timestamp { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("comments")]
            public Comments Comments { get; set; }

            
        }

        public  class Comments
        {
            [JsonProperty("data")]
            public CommentData[] Data { get; set; }
        }

        public  class CommentData
        {
            [JsonProperty("timestamp")]
            public string Timestamp { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }
        }

        public  class Owner
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
    }


