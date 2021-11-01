using DemoApplication1.Models.Entities.Concretes;

namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface IInstagramUserProfileDal
    {
        void Save(InstagramUserProfile userProfile);
        void Delete(string id); //user id
        InstagramUserProfile Get(string id);
        bool IsUpdated(InstagramUserProfile userProfile,string id);
        void CheckForUpdate(InstagramUserProfile profile,string id);

    }
}
