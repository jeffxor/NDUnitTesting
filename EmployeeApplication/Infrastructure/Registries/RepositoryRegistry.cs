using EmployeeApplication.Domain.Model.Repository;
using EmployeeApplication.Domain.Model.Repository.NHibernate;
using StructureMap.Configuration.DSL;

namespace EmployeeApplication.Infrastructure.Registries
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IEmployeeRepository>().Use<EmployeeNHibernateRespository>();
            For<IDepartmentRepository>().Use<DepartmentNHibernateRespository>();
        }

    }
}
