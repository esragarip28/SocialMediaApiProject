using DemoApplication1.Models.DataAccess.Abstracts;
using DemoApplication1.Models.Entities.Concretes;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Concretes
{
    public class ProfileStatisticDal : IProfileStatisticDal
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoClient dbClient;
        private readonly IMongoDatabase db;

        public ProfileStatisticDal(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("DemoAppCon"));
            db = dbClient.GetDatabase("userdb");
        }

        public void Delete(string id)
        {
            var filter = Builders<ProfileStatistic>.Filter.Eq(a => a.UserId, id);
            db.GetCollection<ProfileStatistic>("ProfileStatistic").DeleteOne(filter);
        }

        public ProfileStatistic Get(string id)
        {
            var filter = Builders<ProfileStatistic>.Filter.Eq(a => a.UserId, id);
            return db.GetCollection<ProfileStatistic>("ProfileStatistic").Find(filter).FirstOrDefault();
        }

        public bool IsUpdated(ProfileStatistic profileStatistic, string id)
        {
                ProfileStatistic statistic = Get(id);
                if (statistic!= null)
                {
                    if(statistic.DailyImpression.Equals(profileStatistic.DailyImpression)&statistic.MonthlyImpression.Equals(profileStatistic.MonthlyImpression)&
                        statistic.WeeklyImpression.Equals(profileStatistic.WeeklyImpression)&statistic.NumberOfReach.Equals(profileStatistic.NumberOfReach)&
                        statistic.NumberOfProfileView.Equals(profileStatistic.NumberOfProfileView))
                    {
                        return false;
                    }
                    return true;
                }
                else { return true; }
        }

        public void Save(ProfileStatistic profileStatistic)
        {
            db.GetCollection<ProfileStatistic>("ProfileStatistic").InsertOne(profileStatistic);
        }
    }
}
