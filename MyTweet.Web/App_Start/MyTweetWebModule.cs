﻿using Abp.Application.Services;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.Web.Mvc;
using Abp.WebApi;
using MyTweet.Application;
using MyTweet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyTweet.Web.App_Start
{
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpWebApiModule),
        typeof(MyTweetApplicationModule))]
    public class MyTweetWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;
            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                // 注入Application层的public方法生成相应的API接口
                .ForAll<IApplicationService>(typeof(MyTweetApplicationModule).Assembly, "MyTweet")
                // 根据方法名称绑定相应的Http method动词
                .WithConventionalVerbs()
                .Build();
        }
    }
}