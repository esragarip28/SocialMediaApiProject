using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.Models.SingletonServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class InstagramBusinessInfoDal : IInstagramBusinessInfoDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public InstagramBusinessInfoDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        public string GetIGUserId()
        {
            var id = SessionSingleton.GetInstance()._session.GetString("userId");
            var user = db.GetCollection<InstagramBusinessInfo>("InstagramBusinessInfo").Find(a => a.UserId.Equals(id.ToString())).FirstOrDefault();
            return user.IGUserId;
        }

        public string GetPageIdById()
        {
            var id = SessionSingleton.GetInstance()._session.GetString("userId");
            var user = db.GetCollection<InstagramBusinessInfo>("InstagramBusinessInfo").Find(a => a.UserId.Equals(id.ToString())).FirstOrDefault();
            return user.PageId;
        }

        public void Save(string id, string pageId, string igBusinessId)
        {
            var data = db.GetCollection<BsonDocument>("InstagramBusinessInfo");
            var doc = new BsonDocument
            {
                {"UserId",id},
                {"PageId",pageId},
                {"IGUserId",igBusinessId},

            };
            data.InsertOne(doc);
        }

      
    }
}
