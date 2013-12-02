using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NDUnitTesting.Domain.Model;
using NHibernate.Tool.hbm2ddl;
using StructureMap.Configuration.DSL;
using NDUnitTesting.Infrastructure.Database;
using NHibernate;

namespace NDUnitTesting.Infrastructure.Registries
{
    public class DatabaseRegistry : Registry
    {
        public DatabaseRegistry()
        {
            var nHibernateConfiguration = new EmployeeApplicationNHibernateConfiguration();

            ISessionFactory sessionFactory = Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.InMemory)
                .Mappings(m =>
                          m.AutoMappings
                           .Add(AutoMap.AssemblyOf<Employee>(nHibernateConfiguration))
                )
                .ExposeConfiguration(cfg =>
                                     {
                                         var schemaExport = new SchemaExport(cfg);
                                         schemaExport.Drop(true, true);
                                         schemaExport.Create(true, true);
                                     })
                .BuildSessionFactory();


            For<ISessionFactory>().Singleton().Use(sessionFactory);
            For<ISession>().HybridHttpOrThreadLocalScoped().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
            //TODO: Handle Tansactions For Application
            //For<IUnitOfWork>().CacheBy(new HybridLifecycle()).Use<UnitOfWork>();
        }
    }
}
