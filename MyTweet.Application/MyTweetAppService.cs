using Abp.Application.Services;
using Abp.Authorization;
using MyTweet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTweet.Application
{
    public class CreateTweetInput
    {
        public string Content { get; set; }
    }

    public interface IMyTweetAppService : IApplicationService
    {
        object GetTweets(string msg);
        object CreateTweet(CreateTweetInput input);
        object GetWillFail();
        object GetTweetsFromQS(string keyword);
        object DeleteAll();
    }

    public class MyTweetAppService : ApplicationService, IMyTweetAppService
    {
        public ITweetRepository TweetRepository { get; set; }

        public object GetTweets(string msg)
        {
            return TweetRepository.GetAll().OrderByDescending(x => x.CreateTime).ToList();
        }

        [AbpAuthorize(MyTweetPermission.CreateTweet)]
        public object CreateTweet(CreateTweetInput input)
        {
            var tweet = new Tweet
            {
                Id = Guid.NewGuid().ToString("N"),
                Content = input.Content,
                CreateTime = DateTime.Now
            };
            var o = TweetRepository.Insert(tweet);
            return o;
        }

        public object GetWillFail()
        {
            var tweet = new Tweet
            {
                Id = Guid.NewGuid().ToString("N"),
                Content = "Spiderman.",
                CreateTime = DateTime.Now
            };
            TweetRepository.Insert(tweet);
            throw new Exception("注定失败");
        }

        public ITweetQueryService TweetQueryService { get; set; }

        public object GetTweetsFromQS(string keyword)
        {
            return TweetQueryService.SearchTweets(keyword);
        }

        public object DeleteAll()
        {
            var entities = TweetRepository.GetAll().ToList();
            foreach (var entity in entities)
            {
                TweetRepository.Delete(entity);
            }
            return entities.Count;
        }
    }
}
