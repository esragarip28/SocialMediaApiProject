using DemoApplication1.Models.Entities.Concretes;


namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface IInstagramBusinessInfoDal
    {
        //void SaveInstagramBusinessInfo(InstagramBusinessInfo instagramBusinessInfo);
        string GetPageIdById(); //user id
        string GetIGUserId(); //instagram business id
        void Save(string id,string pageId,string igBusinessId);

    }
}
