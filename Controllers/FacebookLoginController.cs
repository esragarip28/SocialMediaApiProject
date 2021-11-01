using DemoApplication1.Models.DataAccess;
using DemoApplication1.Models.Entities.Concretes;
using DemoApplication1.Models.Services.Abstracts;
using DemoApplication1.Models.SingletonServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DemoApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FacebookLoginController : Controller
    {
      
        private readonly IAccessTokenDal _accessTokenDal;
        private readonly ILoginService _loginService;

        public FacebookLoginController(IAccessTokenDal accessTokenDal,ILoginService loginService)
        {
            _accessTokenDal = accessTokenDal;
            _loginService = loginService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.userId = SessionSingleton.GetInstance()._session.GetString("userId");
            return View("Login");
        }

        [HttpPost]
        [Route("AddAccessTokenInfoToDb")]
        public string AddAccessTokenInfoToDb(AccessTokenData accessTokenData)
        {
            var tokenData= _loginService.MakeAccessTokenLongLife(accessTokenData.AccessToken);
            accessTokenData.AccessToken = tokenData.AccessToken.ToString();
            accessTokenData.ExpiresIn = tokenData.ExpiresIn.ToString();
            _accessTokenDal.SaveAccessToken(accessTokenData);
            return accessTokenData.AccessToken;

        }

       
    }
}
