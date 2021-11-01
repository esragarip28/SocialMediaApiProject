using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities.Concretes;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class UserDal : IUserDal
    {

        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public UserDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");

        }

        public int FindNumberOfPeople()
        {
            int lastUserId = dbClient.GetDatabase("userdb").GetCollection<User>("User").AsQueryable().Count();
            return lastUserId + 1;
        }

        public User FindUser(string username, string password)
        {     
            var user = db.GetCollection<User>("User").Find(a => a.Username.Equals(username) & a.Password.Equals(password)).FirstOrDefault();
            return user;
        }

       
        public void SaveUser(User user)
        {
            var data = db.GetCollection<BsonDocument>("User");
            var doc = new BsonDocument
            {

                {"UserId",user.UserId},
                {"Name", user.Name},
                {"Surname", user.Surname},
                {"Username",user.Username },
                {"Password",user.Password },
                {"Email",user.Email }

            };
            data.InsertOne(doc);

        }
    }
}
