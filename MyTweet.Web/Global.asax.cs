using Abp.Web;
using MyTweet.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyTweet.Web
{
    public class MvcApplication : AbpWebApplication<MyTweetWebModule>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            base.Application_Start(sender, e);
        }

        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            string userId = null;
            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null)
            {
                // IIS会从cookie解析出userId并生成一个principal赋值给Thread.CurrentPrincipal
                userId = Thread.CurrentPrincipal.Identity.Name;
            }
            if (!string.IsNullOrEmpty(userId))
            {
                // 创建identity
                var identity = new GenericIdentity(userId);
                // 添加Type为ClaimTypes.NameIdentifier使userId能注入到AbpSession
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                // 创建principal
                var principal = new GenericPrincipal(identity, null);
                // 同步Thread.CurrentPrincipal
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    // 将principal赋值给HttpContext.Current.User，用户就登录进去了。
                    HttpContext.Current.User = principal;
                }
            }

            base.Application_BeginRequest(sender, e);
        }
    }
}
