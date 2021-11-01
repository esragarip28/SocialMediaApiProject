using DemoApplication1.Models.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess
{
    public interface IAccessTokenDal
    {

        bool CheckUserAccessToken(int id);
        bool IsAccessTokenActive(int id);
        void SaveAccessToken(AccessTokenData accessTokenData);
        string GetAccessTokenByUserId(int id);




    }
}
