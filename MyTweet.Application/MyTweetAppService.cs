using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTweet.Application
{
    public class CreateTweetInput
    {
        public string S { get; set; }
        public int I { get; set; }
    }

    public interface IMyTweetAppService : IApplicationService
    {
        object GetTweets(string msg);
        object CreateTweet(CreateTweetInput input);
    }

    public class MyTweetAppService : ApplicationService, IMyTweetAppService
    {
        public object GetTweets(string msg)
        {
            return new List<string> { "Hello", msg };
        }

        public object CreateTweet(CreateTweetInput input)
        {
            return new
            {
                Msg = input.S,
                Ret = input.I
            };
        }
    }
}
