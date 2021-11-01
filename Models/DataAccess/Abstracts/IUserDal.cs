using DemoApplication1.Models.Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface IUserDal
    {
        void SaveUser(User user);
        User FindUser(string username,string password);
        int FindNumberOfPeople();

        
    }
}
