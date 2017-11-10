using Abp.Dependency;
using Abp.Runtime.Security;
using Abp.Web;
using Abp.Castle.Logging.Log4Net;
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
using Castle.Facilities.Logging;

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

            IocManager.Instance.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config"));

            base.Application_Start(sender, e);
        }
    }
}
