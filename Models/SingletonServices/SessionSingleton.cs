using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.Models.SingletonServices
{
    public class SessionSingleton
    {
        public static SessionSingleton instance;
        public IHttpContextAccessor _httpContextAccessor;
        public  ISession _session;
     
        private SessionSingleton()
        {

            _httpContextAccessor = new HttpContextAccessor();
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public static SessionSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new SessionSingleton();
            }
            return instance;
        }
    }
}
