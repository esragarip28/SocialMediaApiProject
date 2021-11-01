using DemoApplication1.Models.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using DemoApplication1.ResultJsonClass;
using System.Net;
using DemoApplication1.Models.Services.Abstracts;
using System.Linq;
using DemoApplication1.Models.DataAccess;
using Microsoft.AspNetCore.Http;
using DemoApplication1.Models.SingletonServices;
using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.ResultJsonClass.WebhookModels;
using DemoApplication1.ExternalServices.Cloudinary;

namespace DemoApplication1.Models.Services.Concretes
{
    public class InstagramClientService:IInstagramClientService
    {
        private readonly IAccessTokenDal _accessTokenDal;
        private readonly IInstagramBusinessInfoDal _instagramBusinessInfoDal;
        private readonly IPostDal _postDal;
        private readonly string accessToken;
        private static string pageId;
        private readonly string igUserId;
        private readonly string igUsername;
        private readonly ILog _logger;
        private readonly IInstagramUserProfileDal _instagramUserProfileDal;
        private readonly IProfileStatisticDal _profileStatistic;
        private readonly ICommentNotificationDal _commentNotificationDal;
        public  static ProfileStatistic profileStatistic=new ProfileStatistic();

        public InstagramClientService(IAccessTokenDal accessTokenDal, IInstagramBusinessInfoDal instagramBusinessInfoDal,IPostDal postDal,ILog logger,
            IInstagramUserProfileDal instagramUserProfileDal,IProfileStatisticDal profileStatistic,ICommentNotificationDal commentNotificationDal)
        {
            _accessTokenDal = accessTokenDal;
            _postDal = postDal;
            _instagramUserProfileDal = instagramUserProfileDal;
            _instagramBusinessInfoDal = instagramBusinessInfoDal;
            _profileStatistic = profileStatistic;
             accessToken = _accessTokenDal.GetAccessTokenByUserId(int.Parse(SessionSingleton.GetInstance()._session.GetString("userId")));
             igUserId= _instagramBusinessInfoDal.GetIGUserId();
             igUsername = GetUserProfileInfo().Username;
            _logger = logger;
            _commentNotificationDal = commentNotificationDal;
           
        }

        /// <summary>
        /// by using this method we will get user's page id (if user has more than one page we can get all id values
        /// but the method returned first page id value because my account has only one page). We need to this value to have some information 
        /// about user page ,instagram user account and so on.
        /// </summary>
        /// <returns></returns>
        public string GetUserPageId()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}me/");
                HttpResponseMessage response = client.GetAsync($"accounts?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                UserPageResult userPageResult = JsonConvert.DeserializeObject<UserPageResult>(result);
                pageId = userPageResult.Data.First().Id;
                return userPageResult.Data.First().Id;
                
            }
        }
        /// <summary>
        /// with this method we will get user page(or pages)s access token value(or values). 
        /// when we check our webhook connection of page we can use this value.
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetUserPageAccessToken()
        {
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}");
                string pageId = GetUserPageId();
                HttpResponseMessage response = client.GetAsync($"pageId?fields=access_token&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;

            }
        }


        public string GetInstagramBusinessAccountInboxObject()
        {
            using (var client = new HttpClient())
            {
                string pageId = GetUserPageId();
                client.BaseAddress = new Uri($"{Constant.BaseAddress}/v9.0/{pageId}/");
                HttpResponseMessage response = client.GetAsync($"conversations?platform=instagram&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;

            }
        }


        /// <summary>
        /// by using this method we will get instagram businesss or account id that is connected to the facebook page.
        /// the method returns instagram business or creator account id
        /// </summary>
        /// <returns></returns>
        public string GetInstagramBusinessAccount()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.BaseAddressWithVersion);
                HttpResponseMessage response = client.GetAsync($"{pageId}?fields=instagram_business_account&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessPageResult instagramBusinessAccountResult = JsonConvert.DeserializeObject<IGBusinessPageResult>(result);
                string igBusinessId = instagramBusinessAccountResult.InstagramBusinessAccount.Id;
                _instagramBusinessInfoDal.Save(SessionSingleton.GetInstance()._session.GetString("userId"),pageId,igBusinessId);
                return igBusinessId;
            }
        }

        /// <summary>
        /// this method return post details such as comments count,likes count and so on.
        /// In the method first I got json and then deserialize it. after that I added media detail and return media list from db.
        /// But you can return media details directly.
        /// Also,mediaResult list will return id of media. After then, with GetMediaInformation method we will access details of media.
        /// </summary>
        /// <returns> instagram media details of user</returns>
        //
        public List<Post> GetUserMediaDetail()
        {
            string id = SessionSingleton.GetInstance()._session.GetString("userId");
            using (var client = new HttpClient())
            {
               // List<Post> postInfo = new List<Post>();
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client. GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content. ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                if (_postDal.GetPostData(id) != null)
                {
                    _postDal.DeleteAllPostData(id);
                }
                foreach (var item in mediaResult.Data)
                {
                     Post data=GetMediaInformation(item.Id.ToString());
                    //postInfo.Add(data); //If you dont want to save media db you can return postInfo list
                     data.UserId = SessionSingleton.GetInstance()._session.GetString("userId");
                    _postDal.SavePostData(data);
                }
                return _postDal.GetPostData(SessionSingleton.GetInstance()._session.GetString("userId"));
            }

        }


        /// <summary>
        /// To share image, firstly we need to get container id. Also there is limit to content sharing for example, format of image must be jpeg
        ///multi-image posts are not supported so we need to make control rules before trying to share content.
        ///In addition, with this method I used webClient instead of HttpClient but you can use HttpClient if you want.
        ///Finally,to get cretion(container) id, we need to have url of image so I use cloudinary service and included it in my project.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns>container id</returns>
        public string CreateContainer(string imageUrl)
        {
             
            using (WebClient client = new WebClient())
            {
                string url = $"{Constant.BaseAddress}{igUserId}/media?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {

                    {"image_url",imageUrl},
                    {"access_token",accessToken},
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                ItemId item = JsonConvert.DeserializeObject<ItemId>(result);
                return item.id.ToString();
            }
        }


        /// <summary>
        /// After get creation id ,by using this method we can share our media.
        /// </summary>
        /// <param name="creation_id"></param>
        /// <returns>posted media id</returns>
        public string PostContent(string creation_id)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{Constant.BaseAddress}{igUserId}/media_publish?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {
                    {"creation_id",creation_id },
                    {"access_token",accessToken },
                });
                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                ItemId item = JsonConvert.DeserializeObject<ItemId>(result);
                GetUserMediaDetail();

                return item.id.ToString();
            }
        }

        /// <summary>
        /// We can make comment media we want. Our method has two parameters one of them is media id which media we want to add commet it will simplify
        /// that and second one is our comment. 
        /// </summary>
        /// <param name="media_id"></param>
        /// <param name="message"></param>
        /// <returns>comment id </returns>
        public string PostComment(string media_id, string message)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{Constant.BaseAddress}{ media_id}/comments?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {
                    {"message",message },
                    {"access_token",accessToken },
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                return result;
            }
        }

        /// <summary>
        /// to share video firstly we need to get url(from public server) so we have to use any external service so I used cloudinary.
        /// When I upload video, I saved the video as large video(because before I got error maybe it is because of the video size)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetVideoUrl(IFormFile file)
        {
            CloudinaryService cloudinaryService = new CloudinaryService();
            return cloudinaryService.GetUploadedVideoUrl(file);
        }

        /// <summary>
        /// with this method we will get container or creation id for video.
        /// If you want to share video with caption you only add new parameter for caption and add caption in the query like this( &caption="#Heyyyyyyyy!")
        /// </summary>
        /// <param name="videoUrl"></param>
        /// <returns></returns>
        public string GetContainerIdToShareVideo(string videoUrl)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{Constant.BaseAddress}{igUserId}/media?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {
                    {"media_type","VIDEO"},
                    {"video_url",videoUrl},
                    {"access_token",accessToken},
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                ItemId item = JsonConvert.DeserializeObject<ItemId>(result);
                return item.id.ToString();
            }
        }

       
        /// <summary>
        /// This is some critical point to share video because of some limits such as file size must be max 100mb,type of video must be mp4 or mov ...
        /// so we should be careful When we share video.
        /// </summary>
        /// <param name="creationId"></param>
        /// <returns></returns>
        public string ShareVideo(string creationId)
        {
            using (WebClient client = new WebClient())
            {
                string url = $"{Constant.BaseAddress}{igUserId}/media_publish?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {

                    {"creation_id",creationId},
                    {"access_token",accessToken},
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                ItemId item = JsonConvert.DeserializeObject<ItemId>(result);
                return item.id.ToString();
            }
        }

        /// <summary>
        ///by taking advantage of this method we can reply commet we want. This method has 2 parameters. First one will be simplified which
        ///comment we want to reply.
        /// </summary>
        /// <param name="comment_id"></param>
        /// <param name="message"></param>
        /// <returns>reply id</returns>
        public string ReplyComment(string comment_id, string message)
        {
            using (WebClient client = new WebClient())
            {

                string url = $"{Constant.BaseAddress}{comment_id}/replies?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {
                    {"message",message },
                    {"access_token",accessToken },
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                return result;
            }
        }

        /// <summary>
        /// by taking advantage of this method we can have user's business discovery for example followers count, media count.
        /// </summary>
        /// <returns></returns>
        public string GetFollowerAndMediaCount()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}");
                HttpResponseMessage response = client.GetAsync($"{igUserId}?fields=business_discovery.username({igUsername})" 
                    + "{followers_count,media_count}" + "&access_token=" + accessToken).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }

        /// <summary>
        /// this method same as one above method
        /// </summary>
        /// <returns></returns>
        public string GetFollowerAndMediaCountWithMedia()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.BaseAddressWithVersion);
                HttpResponseMessage response = client.GetAsync($"{igUserId}?fields=business_discovery.username({igUsername})"
                    + "{followers_count,media_count,media}" + "&access_token=" + accessToken).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }

        /// <summary>
        /// this method more comprensive than one and two above methods so we can use only this method to  get user's business discovery information.
        /// </summary>
        /// <returns></returns>
      public string GetAccountBasicMediaMetric()
       {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constant.BaseAddressWithVersion);
                HttpResponseMessage response = client.GetAsync($"{igUserId}?fields=business_discovery.username({igUsername})"
                    + "{followers_count,media_count,media{comments_count,like_count,media_url}}" + "&access_token=" + accessToken).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
           }
        }
     
        /// <summary>
        /// with this method we will get all media details according to media id.
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns>post information</returns>
        public Post GetMediaInformation(string mediaId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}");
                HttpResponseMessage response = client.GetAsync($"{mediaId}?fields=id,caption,comments_count,ig_id,is_comment_enabled,like_count,media_product_type," +
                    $"media_type,media_url,owner,permalink,shortcode,thumbnail_url,timestamp,username,children,comments&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                Post post = JsonConvert.DeserializeObject<Post>(result);
                return post;
               
            }
        }

        /// <summary>
        /// by using this method, we can get user instagram profile information such as username,email,website,biograph,profile picture url and so on.
        /// </summary>
        /// <returns>user profile information</returns>
      
        public InstagramUserProfile GetUserProfileInfo()
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}");
                HttpResponseMessage response = client.GetAsync($"{igUserId}?fields=name,username,followers_count," +
                    $"follows_count,media_count,profile_picture_url,biography,website&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                InstagramUserProfile profile = JsonConvert.DeserializeObject<InstagramUserProfile>(result);
                string userId = SessionSingleton.GetInstance()._session.GetString("userId");
                if (_instagramUserProfileDal.Get(userId) != null)
                {
                    if (_instagramUserProfileDal.IsUpdated(profile, userId)==true)
                    {
                        _instagramUserProfileDal.Delete(userId);
                         profile.UserId = userId;
                        _instagramUserProfileDal.Save(profile);                    
                    }
                    
                }
                else
                {
                    profile.UserId = userId;
                    _instagramUserProfileDal.Save(profile);
                }
                return profile;
           }
        }

        /// <summary>
        /// we can get user's story id by using the method and then we add story/stories id to list after that will return the list.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUserStoryId()
        {
           
                List<string> idList = new List<string>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{Constant.BaseAddress}");
                    HttpResponseMessage response = client.GetAsync($"{igUserId}?fields=stories&access_token={accessToken}").Result;
                    string result = response.Content.ReadAsStringAsync().Result;
                    var storyData = JsonConvert.DeserializeObject<StoryDataResult>(result);
                    if (storyData.Stories != null)
                    {
                        foreach (var item in storyData.Stories.Data)
                        {
                            idList.Add(item.Id);
                        }
                    }
                }
                 return idList;                  
        }

        /// <summary>
        /// To get story information,we will use this method. We will have media url,permalik ,media product,media type...
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Story GetStoryData(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}");
                HttpResponseMessage response = client.GetAsync($"{id}?fields=media_url,permalink,media_product_type,timestamp,media_type," +
                    $"username&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                Story story = JsonConvert.DeserializeObject<Story>(result);
                return story;
            }

        }

        /// <summary>
        /// If we want to delete any comment we shared we can delete it with the following method. we only need to indicate id of comment.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteComment(string id)
        {
            using (var client =new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}");
                HttpResponseMessage response = client.DeleteAsync($"{id}?fields=comments&access_token={accessToken}").Result;
                
            }
        }

        /// <summary>
        /// If we want to show exact number media to user, we can make filter to do it. 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<Post> GetMediaByGivenNumber(int number)
        {

            using (var client = new HttpClient())
            {
                List<Post> postInfo = new List<Post>();
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                if (mediaResult != null)
                {
                    for (int i = 0; i < number; i++)
                    {
                        Post data = GetMediaInformation(mediaResult.Data[i].Id.ToString());
                        postInfo.Add(data);
                    }
                }
                return postInfo;
            }

        }

        /// <summary>
        /// If user want to see media which has most likes count we can use this method.
        /// </summary>
        /// <returns></returns>
        public List<Post> SortMediaByLikesCount()
        {
            using (var client = new HttpClient())
            {
                List<Post> postInfo = new List<Post>();

                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                if (mediaResult != null)
                {
                    foreach (var item in mediaResult.Data)
                    {
                        Post data = GetMediaInformation(item.Id.ToString());
                        postInfo.Add(data);
                    }
                    postInfo = postInfo.OrderByDescending(x => x.LikeCount).ToList();
                }
                return postInfo;
            }
        }

        //this will sort media according to comments count
        public List<Post> SortMediaByCommentsCount()
        {
              using (var client = new HttpClient())
            {
                List<Post> postInfo = new List<Post>();

                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                if (mediaResult != null)
                {
                    foreach (var item in mediaResult.Data)
                    {
                        Post data = GetMediaInformation(item.Id.ToString());
                        postInfo.Add(data);
                    }
                    postInfo = postInfo.OrderByDescending(x => x.CommentsCount).ToList();
                }
                return postInfo;
            }
        }

        public List<Post> GetTopTenMedia()
        {
            using (var client = new HttpClient())
            {
                List<Post> postInfo = new List<Post>();
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                if (mediaResult.Data.Length >= 10)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        Post data = GetMediaInformation(mediaResult.Data[i].Id.ToString());
                        postInfo.Add(data);
                    }

                    return postInfo;
                }
                else
                {
                    foreach (var item in mediaResult.Data)
                    {
                        Post data = GetMediaInformation(item.Id.ToString());
                        postInfo.Add(data);
                    }
                    return postInfo;
                }
            }
        }

        public List<Post> GetLastTenMedia()
        {
            using (var client = new HttpClient())
            {
                List<Post> postInfo = new List<Post>();
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"media?access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                IGBusinessMediaObjectResult mediaResult = JsonConvert.DeserializeObject<IGBusinessMediaObjectResult>(result);
                int index = mediaResult.Data.Length;
                if (mediaResult.Data.Length >= 10)
                {
                    for (int i=index-1; i <index-11; i++)
                    {
                        Post data = GetMediaInformation(mediaResult.Data[i].Id.ToString());
                        postInfo.Add(data);
                    }
                    return postInfo;
                }
                else
                {
                    foreach (var item in mediaResult.Data)
                    {
                        Post data = GetMediaInformation(item.Id.ToString());
                        postInfo.Add(data);
                    }
                    postInfo = postInfo.OrderByDescending(x => x.CommentsCount).ToList();
                    return postInfo;
                }
            }

        }

        /// <summary>
        /// by using this method we will get hastag id, after that with the id we can access recent media and top media which have this hashtag.
        /// </summary>
        /// <param name="hashtagName"></param>
        /// <returns></returns>
        public string GetHashtagId(string hashtagName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}");
                HttpResponseMessage response = client.GetAsync($"ig_hashtag_search?user_id={igUserId}&q={hashtagName}&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                HashtagIdJsonData hashtagData = JsonConvert.DeserializeObject<HashtagIdJsonData>(result);
                return hashtagData.Data.First().Id;
            }
        }

        public List<HashtagData> GetHashtagRecentMediaInfo(string hashtagId)
        {
            List<HashtagData> list = new List<HashtagData>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}{hashtagId}/");
                HttpResponseMessage response = client.GetAsync($"recent_media?user_id={igUserId}& &fields=id,media_type,comments_count,like_count," +
                    $"permalink,media_url&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HashtagMediaJsonData>(result);
                if (data.Data != null)
                {
                    foreach (var item in data.Data)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }

        public List<HashtagData> GetHashtagTopMediaInfo(string hashtagId)
        {
            List<HashtagData> list = new List<HashtagData>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}{hashtagId}/");
                HttpResponseMessage response = client.GetAsync($"top_media?user_id={igUserId}& &fields=id,media_type,comments_count,like_count,permalink," +
                    $"media_url&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HashtagMediaJsonData>(result);
                if (data.Data != null)
                {
                    foreach (var item in data.Data)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }


        public List<MentionsData> GetMentionedData()
        {
            List<MentionsData> list = new List<MentionsData>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddress}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"tags?user_id={igUserId}& &fields=id,caption,media_url," +
                    $"comments_count,like_count,permalink,media_product_type" +
                    $"&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<MentionJsonData>(result);
                if (data.Data!=null)
                {
                    foreach (var item in data.Data)
                    {
                        list.Add(item);
                    }
                }              
                return list;
            }
        }

        /// <summary>
        /// this will give us impression number,saved number and etc about feed(video or image not carousel)
        /// </summary>
        /// <returns></returns>
        public List<MediaInsightsData> GetMediaInsights(string id)
        {
            List<MediaInsightsData> list = new List<MediaInsightsData>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{id}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=saved,impressions,reach,engagement&access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<MediaInsights>(result);
                if (data.Data != null)
                {
                    foreach (var item in data.Data)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }

        /// <summary>
        /// this will give information us about stories' impression,reach,replies number.
        /// for this part we need to integrate webhook
        /// </summary>
        /// <returns></returns>
        public string GetStoryMetrics(string id)
        {
            throw new NotImplementedException();
        }

        public string GetNumberOfImpressionADay()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=impressions&period=day&" +
                    $"access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                AccountInsights data = JsonConvert.DeserializeObject<AccountInsights>(result);
                string numberOfImpressions = data.Data.First().Values.First().Val.ToString();
                profileStatistic.DailyImpression = numberOfImpressions;
                return numberOfImpressions;
            }
        }

        public string GetNumberOfReach()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=reach&period=day&" +
                    $"access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                AccountInsights data = JsonConvert.DeserializeObject<AccountInsights>(result);
                string numberOfReach = data.Data.First().Values.First().Val.ToString();
                profileStatistic.NumberOfReach = numberOfReach;
                return numberOfReach;
            }
        }

        public string GetNumberOfImpressionForAWeek()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=impressions&period=week&" +
                    $"access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                AccountInsights data = JsonConvert.DeserializeObject<AccountInsights>(result);
                string numberOfImpressions = data.Data.First().Values.First().Val.ToString();
                profileStatistic.WeeklyImpression = numberOfImpressions;
                return numberOfImpressions;
            }
        }

        public string GetNumberOfImpressionForAMonth()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=impressions&period=days_28&" +
                    $"access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                AccountInsights data = JsonConvert.DeserializeObject<AccountInsights>(result);
                string numberOfImpressions = data.Data.First().Values.First().Val.ToString();
                profileStatistic.MonthlyImpression = numberOfImpressions;
                return numberOfImpressions;
            }
        }

        public string GetNumberOfProfileViews()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}{igUserId}/");
                HttpResponseMessage response = client.GetAsync($"insights?metric=profile_views&period=day&" +
                    $"access_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                AccountInsights data = JsonConvert.DeserializeObject<AccountInsights>(result);
                string numberofview = data.Data.First().Values.First().Val.ToString();
                profileStatistic.NumberOfProfileView = numberofview; //in this part we started to set profileStatistic instance.
                return numberofview;
            }
        }
        /// <summary>
        /// this method will get post from mongodb
        /// </summary>
        /// <returns></returns>
        public List<Post> GetPostFromDb()
        {
            string id = SessionSingleton.GetInstance()._session.GetString("userId");
            return _postDal.GetPostData(id);
        }

       
        public InstagramUserProfile GetUserProfileFromDb()
        {
            string userId = SessionSingleton.GetInstance()._session.GetString("userId");
            return _instagramUserProfileDal.Get(userId);

        }

       
        public void SaveProfileStatisticToDb()
        {
            string id = SessionSingleton.GetInstance()._session.GetString("userId");
            GetNumberOfImpressionADay();
            GetNumberOfImpressionForAMonth();
            GetNumberOfImpressionForAWeek();
            GetNumberOfReach();
            GetNumberOfProfileViews();
            profileStatistic.UserId = id;
            bool isUpdated=_profileStatistic.IsUpdated(profileStatistic, id);
            if (isUpdated != false)
            {
                _profileStatistic.Delete(id);
                _profileStatistic.Save(profileStatistic);
            }
        }

        public ProfileStatistic GetProfileStatisticFromDb()
        {
            string id = SessionSingleton.GetInstance()._session.GetString("userId");
            return _profileStatistic.Get(id);
        }

        /// <summary>
        /// in this method I will update that instead of access_token I will enter page's access token value.
        /// </summary>
        /// <returns></returns>
        public string EnablePageSubscriptions()
        {
            using (WebClient client = new WebClient())
            {

                string url = $"{Constant.BaseAddressWithVersion}{pageId}/subscribed_apps?";
                byte[] responseVal = client.UploadValues(url, new System.Collections.Specialized.NameValueCollection()
                {
                    {"subscribed_fields","feed"},
                    {"access_token",accessToken },
                });

                string result = System.Text.Encoding.UTF8.GetString(responseVal);
                return result;
            }
        }

        /// <summary>
        ///To get webhook information and decide which model we will use, first, I created general class(Notification class)
        ///After deserializetion process, to decide which model, we will get field attribute.(field will give us information of model such as mention,comment..)
        ///Moreover,with switch case we will deserialize again but if you dont want to deserialize them, you can make cast process.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public object DecideModelAndGetDataFromWebhook(Notification element)
        {
            var data = JsonConvert.SerializeObject(element);
            Notification notification= JsonConvert.DeserializeObject<Notification>(data);
            string field = notification.Entry[0].Changes[0].Field;
            switch (field)
            {
                case "comments":
                    var comment = JsonConvert.DeserializeObject<CommentNotification>(data);
                    break;
                case "mentions":
                    var mention = JsonConvert.DeserializeObject<MentionNotification>(data);
                    break;
                case "story_insights":
                    var storyInsight = JsonConvert.DeserializeObject<StoryNotification>(data);
                    break;
                default:
                    break;
            }
            return notification;
        }


    }

}

