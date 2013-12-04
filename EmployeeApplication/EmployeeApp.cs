using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeApplication.Domain.Model;
using StructureMap;

namespace EmployeeApplication
{
    class EmployeeApp
    {
        static void Main(string[] args)
        {

            ObjectFactory.Initialize(x =>
                x.Scan(scan =>
                {
                    scan.AssemblyContainingType<Employee>();
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                }));

            ObjectFactory.AssertConfigurationIsValid();
        }
    }
}
