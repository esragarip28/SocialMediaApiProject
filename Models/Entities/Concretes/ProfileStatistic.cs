using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.Entities.Concretes
{
    public class ProfileStatistic
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [JsonProperty("_id")]
        public string ObjectId { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("daily_impression")]
        public string DailyImpression { get; set; }

        [JsonProperty("weekly_impression")]
        public string WeeklyImpression { get; set; }

        [JsonProperty("monthly_impression")]
        public string MonthlyImpression { get; set; }

        [JsonProperty("number_of_reach")]
        public string NumberOfReach { get; set; }
       
        [JsonProperty("number_of_profile_view")]
        public string NumberOfProfileView { get; set; }
    }
}
