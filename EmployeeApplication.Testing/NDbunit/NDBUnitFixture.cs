using System;
using EmployeeApplication.Domain.Model;
using EmployeeApplication.Infrastructure.Registries;
using NDbUnit.Core;
using NDbUnit.Core.SqlLite;
using NHibernate;
using StructureMap;

namespace EmployeeApplication.Testing.NDbunit
{
    public class NdbUnitFixture : IDisposable
    {

        public NdbUnitFixture()
        {
            ObjectFactory.Initialize(x =>
                            x.Scan(scan =>
                                    {
                                        scan.AssemblyContainingType<Employee>();
                                        scan.WithDefaultConventions();
                                        scan.LookForRegistries();
                                    })
                
                );

            ObjectFactory.AssertConfigurationIsValid();

            var session = ObjectFactory.GetInstance<ISession>();



            var dbunitTest = new SqlLiteDbUnitTest(session.Connection);

            dbunitTest.ReadXmlSchema(@"NDbunit\EmployeeApplication.xsd");
            dbunitTest.ReadXml(@"NDbunit\EmployeeApplication.xml");
            dbunitTest.PerformDbOperation(DbOperationFlag.CleanInsert); 
       
        }

        public void Dispose()
        {
            //ObjectFactory.ResetDefaults();
        }
    }
}
