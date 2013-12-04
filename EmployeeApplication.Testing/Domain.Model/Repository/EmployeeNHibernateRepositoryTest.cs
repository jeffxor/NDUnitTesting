using System.Collections.Generic;
using EmployeeApplication.Domain.Model;
using EmployeeApplication.Domain.Model.Repository;
using EmployeeApplication.Testing.NDbunit;
using StructureMap;
using Xunit;

namespace EmployeeApplication.Testing.Domain.Model.Repository
{
    public class EmployeeNHibernateRepositoryTest : IUseFixture<NdbUnitFixture>
    {
        private IEmployeeRepository EmployeeRepository;

        public void SetFixture(NdbUnitFixture data)
        {
            EmployeeRepository = ObjectFactory.GetInstance<IEmployeeRepository>();
        }

        [Fact]
        public void TestGetById()
        {
            Employee employee = EmployeeRepository.GetById(1);
            Assert.NotNull(employee);
            Assert.Equal(1, employee.Id);
        }

        [Fact]
        public void TestGetAll()
        {
            List<Employee> employees = EmployeeRepository.GetAll();

            Assert.NotNull(employees);
            Assert.Equal(2, employees.Count);
        }
    
    }
}
