using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.ResultJsonClass;
using System.Collections.Generic;

namespace DemoApplication1.Models.Services.Abstracts
{
    public interface ILoginService
    {

        string Login(string username,string password);
        //string Register(User user,string repassword);
        string Register(User user);

        bool CheckUserAccessToken(int id);

        bool IsAccessTokenActive(int id);
        string GetToken(int id);

        LongLivedTokenResult MakeAccessTokenLongLife(string accessToken);

        //string DecodePassword(string password);
        //string EncodePassword(string password);






    }
}
