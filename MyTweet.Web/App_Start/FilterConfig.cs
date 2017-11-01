using Abp.Runtime.Security;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MyTweet.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new MvcAuthorizeAttribute());
        }
    }
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // IIS会从cookie解析出userId并生成一个principal赋值给Thread.CurrentPrincipal
            var userId = Thread.CurrentPrincipal?.Identity?.Name;

            if (!string.IsNullOrEmpty(userId))
            {
                // 创建identity
                var identity = new GenericIdentity(userId);
                // 添加Type为AbpClaimTypes.UserId使userId能注入到AbpSession
                identity.AddClaim(new Claim(AbpClaimTypes.UserId, userId));
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

            base.OnAuthorization(filterContext);
        }
    }
}
