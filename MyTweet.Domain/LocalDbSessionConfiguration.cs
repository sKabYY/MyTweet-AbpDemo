using Abp.Dependency;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyTweet.Domain
{
    public interface ILocalDbSessionConfiguration : ISingletonDependency
    {
        ISessionFactory SessionFactory { get; }
    }

    public class LocalDbSessionConfiguration : ILocalDbSessionConfiguration, IDisposable
    {
        protected FluentConfiguration FluentConfiguration { get; private set; }

        public ISessionFactory SessionFactory { get; }

        public LocalDbSessionConfiguration()
        {
            FluentConfiguration = Fluently.Configure();
            // 数据库连接串
            var connString = "data source=|DataDirectory|MySQLite.db;";
            FluentConfiguration
                // 配置连接串
                .Database(SQLiteConfiguration.Standard.ConnectionString(connString))
                // 配置ORM
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
            // 生成session factory
            SessionFactory = FluentConfiguration.BuildSessionFactory();
        }

        public void Dispose()
        {
            SessionFactory.Dispose();
        }
    }
}
