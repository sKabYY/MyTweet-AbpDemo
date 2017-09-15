using Abp.Domain.Entities;
using Abp.NHibernate.Repositories;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.NHibernate;
using Abp.Dependency;
using Abp.Domain.Repositories;
using MyTweet.Infrastructure;

namespace MyTweet.Domain
{
    public class Tweet : Entity<string>
    {
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class TweetMapper : ClassMap<Tweet>
    {
        public TweetMapper()
        {
            // 禁用惰性加载
            Not.LazyLoad();
            // 映射到表tweet
            Table("tweet");
            // 主键映射
            Id(x => x.Id).Column("pk_id");
            // 字段映射
            Map(x => x.Content).Column("content");
            Map(x => x.CreateTime).Column("create_time");
        }
    }

    public interface ITweetRepository : IRepository<Tweet, string> { }

    public class TweetRepository : NhRepositoryBase<Tweet, string>, ITweetRepository
    {
        public TweetRepository()
            : base(IocManager.Instance.Resolve<LocalDbSessionProvider>())
        { }
    }

    public interface ITweetQueryService
    {
        IList<Tweet> SearchTweets(string keyword);
    }

    public class TweetQueryService : BaseQueryService, ITweetQueryService
    {
        public TweetQueryService() : base(IocManager.Instance.Resolve<LocalDbSessionProvider>())
        { }

        public IList<Tweet> SearchTweets(string keyword)
        {
            var sql = @"select
                            pk_id Id,
                            content Content,
                            create_time CreateTime
                        from tweet
                        where content like '%' || @Keyword || '%'";
            return Query<Tweet>(sql, new { Keyword = keyword ?? "" }).ToList();
        }
    }
}
