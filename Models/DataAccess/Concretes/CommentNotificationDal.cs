using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.ResultJsonClass;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class CommentNotificationDal : ICommentNotificationDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public CommentNotificationDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        public void SaveNotification(Notification comment)
        {
            var data = db.GetCollection<Notification>("CommentNotification");
            data.InsertOne(comment);
        }

      
    }
}
