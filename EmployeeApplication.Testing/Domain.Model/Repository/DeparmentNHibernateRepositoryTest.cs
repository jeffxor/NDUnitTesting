using EmployeeApplication.Domain.Model;
using EmployeeApplication.Domain.Model.Repository;
using EmployeeApplication.Testing.NDbunit;
using StructureMap;
using Xunit;

namespace EmployeeApplication.Testing.Domain.Model.Repository
{
    public class DeparmentNHibernateRepositoryTest : IUseFixture<NdbUnitFixture>
    {
        private IDepartmentRepository DepartmentRepository;

        public void SetFixture(NdbUnitFixture data)
        {
            DepartmentRepository = ObjectFactory.GetInstance<IDepartmentRepository>();

        }

        [Fact]
        public void TestGetById()
        {
            Department department = DepartmentRepository.GetById(1);
            Assert.NotNull(department);
            Assert.Equal(1, department.Id);
        }

    }
}
