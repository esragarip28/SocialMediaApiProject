using DemoApplication1.Models.Entities.Concretes;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class AccessTokenDal : IAccessTokenDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public AccessTokenDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        public void SaveAccessToken(AccessTokenData accessTokenData)
        {
            var data = db.GetCollection<BsonDocument>("AccessTokenData");
            
            var doc = new BsonDocument
            {
                {"UserId",accessTokenData.UserId},
                {"AccessToken",accessTokenData.AccessToken},
                {"ExpiresIn",accessTokenData.ExpiresIn},
                {"DataAccessExpirationTime",accessTokenData.DataAccessExpirationTime},
                {"IsActive",accessTokenData.IsActive},
                {"ForeignKeyId",accessTokenData.ForeignKeyId}

            };
            data.InsertOne(doc);
        }

        public bool CheckUserAccessToken(int id)
        {
            var user = db.GetCollection<AccessTokenData>("AccessTokenData").Find(a => a.ForeignKeyId == id).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }


        public bool IsAccessTokenActive(int id)
        {
            var user = db.GetCollection<AccessTokenData>("AccessTokenData").Find(a => a.ForeignKeyId == id).FirstOrDefault();
            if (user.IsActive.Equals("active")) return true;
            return false;
           
        }

        public string GetAccessTokenByUserId(int id)
        {
            string idVal = id.ToString();
            var data = db.GetCollection<AccessTokenData>("AccessTokenData").Find(a => a.ForeignKeyId.Equals(idVal)).FirstOrDefault();
            if(data!=null) return data.AccessToken;
            return "null error";

        }
    }
}
