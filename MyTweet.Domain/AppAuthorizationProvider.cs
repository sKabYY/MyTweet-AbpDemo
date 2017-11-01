using Abp.Authorization;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Runtime.Session;

namespace MyTweet.Domain
{
    public static class MyTweetPermission
    {
        public const string CreateTweet = "CreateTweet";
    }

    public class MyTweetAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(MyTweetPermission.CreateTweet);
        }
    }

    public class MyTweetPermissionChecker : IPermissionChecker, ITransientDependency
    {
        public IAbpSession AbpSession { get; set; }

        public Task<bool> IsGrantedAsync(string permissionName)
        {
            var userId = AbpSession.GetUserId();
            return IsGrantedAsync(new UserIdentifier(null, userId), permissionName);
        }

        public Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            var userId = user.UserId;
            var t = new Task<bool>(() =>
            {
                if (permissionName == MyTweetPermission.CreateTweet)
                {
                    return user.UserId == 1988;
                }
                return true;
            });
            t.Start();
            return t;
        }
    }

}
