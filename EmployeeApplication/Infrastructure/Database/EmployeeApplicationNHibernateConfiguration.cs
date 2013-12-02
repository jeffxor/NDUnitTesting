using System;
using FluentNHibernate.Automapping;

namespace EmployeeApplication.Infrastructure.Database
{
    public class EmployeeApplicationNHibernateConfiguration : DefaultAutomappingConfiguration
    {

        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "NDUnitTesting.Domain.Model";
        }
    }
}
