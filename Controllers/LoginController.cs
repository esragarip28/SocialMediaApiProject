using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.Models.Services.Abstracts;
using DemoApplication1.Models.SingletonServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DemoApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController: Controller {

        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

       

       

        [HttpGet]
        [Route("Login")]
        public bool Login(string username,string password) {

           var userId= _loginService.Login(username,password);

            if (userId!=null)
            {
                string val = userId.ToString();
                SessionSingleton.GetInstance()._session.SetString("userId", val);
                return true;
            }
            return false;
        }

        
       
        [HttpGet]
        [Route("CheckUserAccessToken")]
        public bool CheckUserAccessToken()
        {
            string idVal = SessionSingleton.GetInstance()._session.GetString("userId");
            if (idVal != null)
            {
                int id = int.Parse(idVal);
                bool result = _loginService.CheckUserAccessToken(id);
                return result;
            }
            return false;
        }

       
        [HttpGet]
        [Route("IsAccessTokenActive")]
        public bool IsAccessTokenActive()
        {
            string idVal = SessionSingleton.GetInstance()._session.GetString("userId");
            if (idVal != null)
            {
                int id = int.Parse(idVal);
                bool result = _loginService.IsAccessTokenActive(id);
                return result;

            }
            return false;
        }


        
        [HttpPost]
        [Route("Register")]
        public void Register(User user)
        {
            _loginService.Register(user);

        }

        
        [HttpGet]
        [Route("GetAccessToken")]
        public string GetAccessToken()
        {
    
            return _loginService.GetToken(int.Parse(SessionSingleton.GetInstance()._session.GetString("userId")));

        }

    }
}
