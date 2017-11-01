using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;
using MyTweet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyTweet.Web.Controllers
{

    public class HomeController : AbpController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AbpMvcAuthorize(MyTweetPermission.CreateTweet)]
        public ActionResult CreateTweet()
        {
            return View();
        }
    }

}