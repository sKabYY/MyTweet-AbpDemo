using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;
using MyTweet.Application;
using MyTweet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTweet.Web.Controllers
{
    public class TweetController : AbpController
    {
        private IMyTweetAppService _myTweetAppService;

        public TweetController(IMyTweetAppService appSvc)
        {
            _myTweetAppService = appSvc;
        }

        [HttpPost]
        //[AbpMvcAuthorize(MyTweetPermission.CreateTweet)]
        public ActionResult Create(CreateTweetInput input)
        {
            var tweet = _myTweetAppService.CreateTweet(input);
            return Json(tweet);
        }
    }
}