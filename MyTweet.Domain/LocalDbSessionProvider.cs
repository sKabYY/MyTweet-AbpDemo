using Abp.Dependency;
using Abp.NHibernate;
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

    public class LocalDbSessionProvider : ISessionProvider, ISingletonDependency, IDisposable
    {
        protected FluentConfiguration FluentConfiguration { get; private set; }

        private ISessionFactory _sessionFactory;

        public LocalDbSessionProvider()
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
            _sessionFactory = FluentConfiguration.BuildSessionFactory();
        }

        private ISession _session;

        public ISession Session
        {
            get
            {
                if (_session != null)
                {
                    // 每次访问都flush上一个session。这里有效率和多线程问题，暂且这样用，后面会改。
                    _session.Flush();
                    _session.Dispose();
                }
                _session = _sessionFactory.OpenSession();
                return _session;
            }
        }

        public void Dispose()
        {
            _sessionFactory.Dispose();
        }
    }
}
