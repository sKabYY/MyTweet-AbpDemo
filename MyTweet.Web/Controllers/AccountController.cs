using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyTweet.Web.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public class LoginInput
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LoginAjax(LoginInput input)
        {
            // 这里应该放验证用户名密码是否正确的代码。
            // 为了测试方便，这里跳过验证，使用任意用户名任意密码都能登录。
            var username = input.Username;
            var ticket = new FormsAuthenticationTicket(
                1 /* version */,
                username,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                false /* persistCookie */,
                "" /* userData */);
            var userCookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                FormsAuthentication.Encrypt(ticket));
            HttpContext.Response.Cookies.Add(userCookie);

            return Json("OK");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}