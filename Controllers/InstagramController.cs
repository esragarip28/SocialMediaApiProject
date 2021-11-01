using DemoApplication1.ExternalServices.Cloudinary;
using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.Models.Services.Abstracts;
using DemoApplication1.ResultJsonClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Timers;

namespace DemoApplication1.Controllers
{

    [Route("api/[controller]")]
    public class InstagramController : ControllerBase
    {

        private readonly IInstagramClientService _instagramClientService;



        public InstagramController(IInstagramClientService instagramClientService)
        {
            _instagramClientService = instagramClientService;


        }


        [Route("GetUserPageId")]
        [HttpGet]
        public string GetUserPageId()
        {
            return _instagramClientService.GetUserPageId();
        }


        [Route("InstagramBusinessAccount")]
        [HttpGet]
        public string GetInstagramBusinessAccountId()
        {
            return _instagramClientService.GetInstagramBusinessAccount();

        }


        [HttpPost]
        [Route("GetContainerId")]
        public string GetContainerId(string imageUrl)
        {
            return _instagramClientService.CreateContainer(imageUrl);
        }


        [HttpPost]
        [Route("PostContent")]
        public string PostContent(string creationId)
        {
            return _instagramClientService.PostContent(creationId);
        }

        [HttpPost]
        [Route("PostComment")]
        public string PostComment(string id, string message)
        {
            return _instagramClientService.PostComment(id, message);
        }

        [HttpPost]
        [Route("ReplyComment")]
        public string ReplyComment(string id, string message) //comment_id
        {
            return _instagramClientService.ReplyComment(id, message);
        }

        [HttpGet]
        [Route("GetFollowerAndMediaCount")]
        public string GetFollowerAndMediaCount()
        {
            return _instagramClientService.GetFollowerAndMediaCount();
        }


        [HttpGet]
        [Route("GetFollowerAndMediaCountWithMedia")]
        public string GetFollowerAndMediaCountWithMedia()
        {
            return _instagramClientService.GetFollowerAndMediaCountWithMedia();
        }


        [HttpGet]
        [Route("GetAccountBasicMediaMetric")]
        public string GetAccountBasicMediaMetric()
        {
            return _instagramClientService.GetAccountBasicMediaMetric();
        }


        //firstly,it will upload image to cloudinary after then it will return image url
        [HttpPost]
        [Route("UploadImageCloudinaryAndGetImageUrl")]
        public string UploadImageToCloudianaryAndGetUrl(IFormFile file)
        {
            CloudinaryService cloudinaryService = new CloudinaryService();
            return cloudinaryService.GetUploadedImageUrl(file);
        }


        [HttpGet]
        [Route("GetInstagramMediaInfo")]
        public List<Post> GetUserMediaDetail()
        {
            return _instagramClientService.GetUserMediaDetail();
        }

        [HttpGet]
        [Route("GetUserProfileInfo")]
        public InstagramUserProfile GetUserProfileInfo()
        {
            return _instagramClientService.GetUserProfileInfo();
        }

        [HttpGet]
        [Route("storiesId")]
        public List<string> GetUserStoryId()
        {
            return _instagramClientService.GetUserStoryId();
        }

        [HttpGet]
        [Route("storiesData")]
        public Story GetUserStoryData(string id)
        {

            return _instagramClientService.GetStoryData(id);
        }

        [HttpDelete]
        [Route("delete/comments")]
        public IActionResult DeleteComment(string id)
        {
            _instagramClientService.DeleteComment(id);
            return Ok("Comment has been deleted");
        }

        [HttpGet]
        [Route("mediaByLikesCount")]
        public List<Post> GetMostLikedMedia()
        {
            return _instagramClientService.SortMediaByLikesCount();

        }

        [HttpGet]
        [Route("media/by/given/number")]
        public List<Post> GetMediaByGivenNumber(int number = 5)
        {
            return _instagramClientService.GetMediaByGivenNumber(number);
        }

        [HttpGet]
        [Route("mediaByCommentsCount")]
        public List<Post> GetMediaByCommentCount()
        {
            return _instagramClientService.SortMediaByCommentsCount();
        }

        [HttpGet]
        [Route("last/ten/media")]
        public List<Post> GetLastTenMedia()
        {
            return _instagramClientService.GetLastTenMedia();
        }

        [HttpGet]
        [Route("top/ten/media")]
        public List<Post> GetTopTenMedia()
        {
            return _instagramClientService.GetTopTenMedia();
        }

        [HttpGet]
        [Route("hashtag/id")]
        public string GetHashtagId(string hashtagName)
        {
            return _instagramClientService.GetHashtagId(hashtagName);
        }

        [HttpGet]
        [Route("hashtag/recent/media/data")]
        public List<HashtagData> GetHashtagRecentMediaInfo(string hashtagId)
        {
            return _instagramClientService.GetHashtagRecentMediaInfo(hashtagId);

        }

        [HttpGet]
        [Route("hashtag/top/media/data")]
        public List<HashtagData> GetHashtagTopMediaInfo(string hashtagId)
        {
            return _instagramClientService.GetHashtagTopMediaInfo(hashtagId);

        }

        [HttpGet]
        [Route("mentions")]
        public List<MentionsData> GetMentionedData()
        {
            return _instagramClientService.GetMentionedData();
        }

        [HttpGet]
        [Route("webhooks")]
        public int Get([FromQuery(Name = "hub.challenge")] int challenge,
            [FromQuery(Name = "hub.verify_token")] string verifyToken = "esragarip",
            [FromQuery(Name = "hub.mode")] string mode = "subscribe"
            )
        {
            if (verifyToken.Equals("esragarip"))
            {
                return challenge;
            }
            else return 0;
        }

        [HttpPost]
        [Route("webhooks")]
        public object Post([FromBody] Notification element)
        {

            _instagramClientService.DecideModelAndGetDataFromWebhook(element);
            return element;
        }

        [HttpGet]
        [Route("media/insights")]
        public List<MediaInsightsData> GetMediaInsights(string id)
        {
            return _instagramClientService.GetMediaInsights(id);
        }

        [HttpGet]
        [Route("number/of/profile/views")]
        public string GetNumberOfProfileViews()
        {
            return _instagramClientService.GetNumberOfProfileViews();
        }

        [HttpGet]
        [Route("number/of/impressions")]
        public string GetNumberOfImpressionADay()
        {
            return _instagramClientService.GetNumberOfImpressionADay();
        }

        [HttpGet]
        [Route("number/of/reach")]
        public string GetNumberOfReach()
        {
            return _instagramClientService.GetNumberOfReach();

        }

        [HttpGet]
        [Route("number/of/impressions/for/week")]
        public string GetNumberOfImpressionForAWeek()
        {
            return _instagramClientService.GetNumberOfImpressionForAWeek();
        }

        [HttpGet]
        [Route("number/of/impressions/for/month")]
        public string GetNumberOfImpressionForAMonth()
        {
            return _instagramClientService.GetNumberOfImpressionForAMonth();
        }

        [HttpGet]
        [Route("media/from/db")]
        public List<Post> GetPostFromDb()
        {
            return _instagramClientService.GetPostFromDb();
        }

        /// <summary>
        /// there are 2 method in controller that are for getting profile information one of them from server and other one(this method) from mongo db.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("user/profile/information")]
        public InstagramUserProfile GetUserProfileFromDb()
        {
            return _instagramClientService.GetUserProfileFromDb();
        }


        [HttpGet]
        [Route("user/profile/statistics")] //from db
        public ProfileStatistic GetProfileStatistic()
        {
            return _instagramClientService.GetProfileStatisticFromDb();
        }
        [HttpGet]
        [Route("profile/statistics")] //saving statistic to db
        public void deneme3()
        {
            _instagramClientService.SaveProfileStatisticToDb();
        }

        [HttpPost]
        [Route("enable/page/subscriptions")]
        public string EnablePageSubscription()
        {
            return _instagramClientService.EnablePageSubscriptions();
        }

        [HttpPost]
        [Route("video/url")]
        public string GetVideoUrlFromCloudinary(IFormFile file)
        {
            return _instagramClientService.GetVideoUrl(file);
        }

        [HttpPost]
        [Route("share/video")]
        public string ShareVideo(IFormFile file)
        {
            string url = _instagramClientService.GetVideoUrl(file);
            string creationId = _instagramClientService.GetContainerIdToShareVideo(url);
            return creationId;
        }
    }
}