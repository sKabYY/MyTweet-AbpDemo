using Abp.Dependency;
using Abp.Domain.Uow;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTweet.Domain
{
    public class UnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        public ILocalDbSessionConfiguration DbSessionConfiguration { get; }

        private ISession _session;

        public UnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter,
            ILocalDbSessionConfiguration localDbSessionConfiguration)
            : base(connectionStringResolver, defaultOptions, filterExecuter)
        {
            DbSessionConfiguration = localDbSessionConfiguration;
        }

        public ISession GetOrCreateSession()
        {
            if (_session == null)
            {
                _session = DbSessionConfiguration.SessionFactory.OpenSession();
                _session.BeginTransaction();
            }
            return _session;
        }

        public override void SaveChanges()
        {
            _session?.Flush();
        }

        public override Task SaveChangesAsync()
        {
            // 我们不用异步Action，就不实现这个方法了。
            throw new NotImplementedException();
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            _session?.Transaction?.Commit();
        }

        protected override Task CompleteUowAsync()
        {
            // 我们不用异步Action，就不实现这个方法了。
            throw new NotImplementedException();
        }

        protected override void DisposeUow()
        {
            _session?.Transaction?.Dispose();
            _session?.Dispose();
        }
    }

    internal static class UnitOfWorkExtensions
    {
        public static ISession GetSession(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is UnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(UnitOfWork).FullName, nameof(unitOfWork));
            }

            return (unitOfWork as UnitOfWork).GetOrCreateSession();
        }
    }

}
