using DemoApplication1.Models.Constants;
using DemoApplication1.Models.DataAccess;
using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.Models.Services.Abstracts;
using DemoApplication1.ResultJsonClass;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace DemoApplication1.Models.Services.Concretes
{
    [BsonIgnoreExtraElements]
    public class LoginService : ILoginService
    {

        private readonly IUserDal _userDal;
        private readonly IAccessTokenDal _accessTokenDal;
        public LoginService(IUserDal userDal,IAccessTokenDal accessTokenDal)
        {
            _userDal = userDal;
            _accessTokenDal = accessTokenDal;
        }

        /// <summary>
        /// In the method, when user make login ,if there is user information in db ,the system will hold session information
        /// (unique id value for every single user)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>boolean</returns>
        public string Login(string username, string password)
        {
            //string password2 = DecodePassword(password);
            var user=_userDal.FindUser(username,password);
            if (user != null) return user.UserId.ToString();         
            return null;

        }


        public string Register(User user)
        {
            user.UserId = _userDal.FindNumberOfPeople();
            _userDal.SaveUser(user);
            return "Başarılı";
        }

        /// <summary>
        /// We neeed to access token to make process. So after login process we have to check whether user has access token or not.
        /// If user hasnt token this method returns false.
        /// </summary>
        /// <returns></returns>

        public bool CheckUserAccessToken(int id)
        {
            return _accessTokenDal.CheckUserAccessToken(id);
        }

        /// <summary>
        /// By using this method we will check that token is active or not. To make this process we can add "isActive" property to hold this value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAccessTokenActive(int id)
        {
            return _accessTokenDal.IsAccessTokenActive(id);
        }

        /// <summary>
        /// After we got access token we saved it db. To get access token we will use this method. You should examine data access layer to see details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetToken(int id)
        {
            return _accessTokenDal.GetAccessTokenByUserId(id);
        }

        /// <summary>
        /// When we get access token,it is not long-lived access token so we need to make it longer by using this method.
        /// By the way, we will get the token from interface to see js code you need to examine views(Login.cshtml)
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public LongLivedTokenResult MakeAccessTokenLongLife(string accessToken)
        {
            using (var client = new HttpClient())
            {
        
                client.BaseAddress = new Uri($"{Constant.BaseAddressWithVersion}");
                HttpResponseMessage response = client.GetAsync($"oauth/access_token?grant_type=fb_exchange_token&client_id={Constant.AppId}&" +
                    $"client_secret={Constant.AppSecret}&fb_exchange_token={accessToken}").Result;
                string result = response.Content.ReadAsStringAsync().Result;
                LongLivedTokenResult  longLivedToken = JsonConvert.DeserializeObject<LongLivedTokenResult>(result);
                
                return longLivedToken;
            }

        }

       
    }
}
