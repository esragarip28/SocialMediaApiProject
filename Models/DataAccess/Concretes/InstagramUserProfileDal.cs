using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities.Concretes;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class InstagramUserProfileDal : IInstagramUserProfileDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public InstagramUserProfileDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        public void CheckForUpdate(InstagramUserProfile profile, string id)
        {
            InstagramUserProfile userProfile = Get(id);
            var filter = Builders<InstagramUserProfile>.Filter.Eq(a => a.UserId, id);
            if (!userProfile.Name.Equals(profile.Name))
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.Name, profile.Name);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter,update);
            }
            if (!userProfile.Username.Equals(profile.Username))
            {
               var update = Builders<InstagramUserProfile>.Update.Set(a=>a.Username,profile.Username);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter,update);
            }

            if (!userProfile.Biography.Equals(profile.Biography))
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.Biography, profile.Biography);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter,update);
            }

            if (!userProfile.ProfilePictureUrl.ToString().Equals(profile.ProfilePictureUrl.ToString()))
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.ProfilePictureUrl, profile.ProfilePictureUrl);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter, update);
            }
            if (!userProfile.Website.ToString().Equals(profile.Website.ToString()))
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.Website, profile.Website);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter, update);
            }
            if (userProfile.MediaCount!=profile.MediaCount)
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.MediaCount, profile.MediaCount);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter, update);
            }
            if (userProfile.FollowersCount != profile.FollowersCount)
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.FollowersCount, profile.FollowersCount);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter, update);
            }
            if (userProfile.FollowsCount!=profile.FollowsCount)
            {
                var update = Builders<InstagramUserProfile>.Update.Set(a => a.FollowsCount, profile.FollowsCount);
                db.GetCollection<InstagramUserProfile>("InstagramUserProfile").UpdateOne(filter, update);
            }
        }



        public void Delete(string id)
        {
            var filter = Builders<InstagramUserProfile>.Filter.Eq(a => a.UserId, id);
            db.GetCollection<InstagramUserProfile>("InstagramUserProfile").DeleteOne(filter);
        }

        public InstagramUserProfile Get(string id)
        {
            try
            {
                var filter = Builders<InstagramUserProfile>.Filter.Eq(a => a.UserId, id);
                return db.GetCollection<InstagramUserProfile>("InstagramUserProfile").Find(filter).FirstOrDefault();
                
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
    
        }

        public bool IsUpdated(InstagramUserProfile userProfile,string id)
        {

            InstagramUserProfile userProfile2 = Get(id);

            if (userProfile2 != null)
            {
                //if (userProfile.Name.Equals(userProfile2.Name) & userProfile.Username.Equals(userProfile2.Username) & userProfile.FollowersCount == userProfile2.FollowersCount &
                // userProfile.FollowsCount == userProfile2.FollowsCount & userProfile.Biography.Equals(userProfile2.Biography) & userProfile.Website.ToString().Equals(userProfile2.Website.ToString())
                // & userProfile.MediaCount == userProfile2.MediaCount & userProfile.ProfilePictureUrl.ToString().Equals(userProfile2.ProfilePictureUrl.ToString()))
                //{
                //    return false;
                //}

                return true;
            }
            return true;
        }

        public void Save(InstagramUserProfile userProfile)
        {
            db.GetCollection<InstagramUserProfile>("InstagramUserProfile").InsertOne(userProfile);
        }
         

    }
}
