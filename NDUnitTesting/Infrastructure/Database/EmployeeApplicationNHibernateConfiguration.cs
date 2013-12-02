using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;

namespace NDUnitTesting.Infrastructure.Database
{
    public class EmployeeApplicationNHibernateConfiguration : DefaultAutomappingConfiguration
    {

        public override bool ShouldMap(Type type)
        {
            return type.Namespace == "NDUnitTesting.Domain.Model";
        }
    }
}
