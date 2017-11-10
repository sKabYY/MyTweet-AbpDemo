using Abp.Auditing;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyTweet.Web.App_Start
{
    public class MyTweetLogAuditingStore : IAuditingStore, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public MyTweetLogAuditingStore()
        {
            Logger = NullLogger.Instance;
        }

        public Task SaveAsync(AuditInfo auditInfo)
        {
            var userId = auditInfo.UserId;
            var userIp = auditInfo.ClientIpAddress;
            var browserInfo = auditInfo.BrowserInfo;
            var action = $"{auditInfo.ServiceName}.{auditInfo.MethodName}";
            var ms = auditInfo.ExecutionDuration;
            var msg = $"用户{userId}（坐标{userIp}）使用{browserInfo}访问了方法{action}，该方法在{ms}毫秒内进行了回击，回击结果：";
            if (auditInfo.Exception == null)
            {
                Logger.Info(msg + "成功！");
            }
            else
            {
                Logger.Warn(msg + "出错了：" + auditInfo.Exception.Message);
            }
            return Task.FromResult(0);
        }
    }

}