using DemoApplication1.Models.DataAccess.Concretes;
using DemoApplication1.Models.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface IProfileStatisticDal
    {
        ProfileStatistic Get(string id);
        void Delete(string id);
        void Save(ProfileStatistic profileStatistic);

        bool IsUpdated(ProfileStatistic profileStatistic, string id);
    }
}
