
using DemoApplication1.Models.Entities;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.ResultJsonClass;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace DemoApplication1.Models.Services.Abstracts
{
    public interface IInstagramClientService
    {
        string GetUserPageId();
        string GetInstagramBusinessAccount();
        List<Post> GetUserMediaDetail(); //for now it wont save them
       
        List<Post> GetMediaByGivenNumber(int number);
        List<Post> SortMediaByLikesCount();
        List<Post> SortMediaByCommentsCount();
        List<Post> GetTopTenMedia();
        List<Post> GetLastTenMedia();
        string CreateContainer(string imageUrl);
        string PostContent(string creation_id);
        string PostComment(string media_id, string message);
        string ReplyComment(string comment_id, string message);
        string GetFollowerAndMediaCount();
        string GetFollowerAndMediaCountWithMedia();
        string GetAccountBasicMediaMetric();
        Post GetMediaInformation(string mediaId);
        InstagramUserProfile GetUserProfileInfo();
        List<string> GetUserStoryId();
        Story GetStoryData(string id);
        void DeleteComment(string id);
        string GetUserPageAccessToken();
        string GetInstagramBusinessAccountInboxObject();
        string GetHashtagId(string hashtagName);
        List<HashtagData> GetHashtagRecentMediaInfo(string hashtagId);
        List<HashtagData> GetHashtagTopMediaInfo(string hashtagId);
        List<MentionsData> GetMentionedData();
        List<MediaInsightsData> GetMediaInsights(string id);
        string GetStoryMetrics(string id);
        string GetNumberOfProfileViews();// for a day
        string GetNumberOfImpressionADay();
        string GetNumberOfReach(); // for a day
        string GetNumberOfImpressionForAWeek();
        string GetNumberOfImpressionForAMonth(); // for 28 days
        List<Post> GetPostFromDb();
     
        InstagramUserProfile GetUserProfileFromDb();

        void SaveProfileStatisticToDb();
        ProfileStatistic GetProfileStatisticFromDb();
        string EnablePageSubscriptions();

        Object DecideModelAndGetDataFromWebhook(Notification notification);
        string GetContainerIdToShareVideo(string videoUrl);
        string GetVideoUrl(IFormFile file);
        string ShareVideo(string creationId);

    } 
}
