using DemoApplication1.Models.DataAccess.Concretes;
using DemoApplication1.ResultJsonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.DataAccess.Abstracts
{
    public interface ICommentNotificationDal
    {
        void SaveNotification(Notification comment);

    }
}
