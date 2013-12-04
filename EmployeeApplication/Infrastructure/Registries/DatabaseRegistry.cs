using System.Configuration;
using EmployeeApplication.Domain.Model;
using EmployeeApplication.Infrastructure.Database;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using StructureMap.Configuration.DSL;

namespace EmployeeApplication.Infrastructure.Registries
{
    public class DatabaseRegistry : Registry
    {
        public DatabaseRegistry()
        {
            var nHibernateConfiguration = new EmployeeApplicationNHibernateConfiguration();
            //string connectionString = ConfigurationManager.ConnectionStrings["EmployeeApplicationContext"].ConnectionString;


            ISessionFactory sessionFactory = Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(@"sqlite.db"))
                //.Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
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
