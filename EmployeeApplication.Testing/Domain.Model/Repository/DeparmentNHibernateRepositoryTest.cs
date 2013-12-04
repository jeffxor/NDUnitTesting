using System.Collections.Generic;
using EmployeeApplication.Domain.Model;
using EmployeeApplication.Domain.Model.Repository;
using EmployeeApplication.Testing.NDbunit;
using NHibernate.Mapping;
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

        [Fact]
        public void TestGetAll()
        {
            List<Department> departments = DepartmentRepository.GetAll();

            Assert.NotNull(departments);
            Assert.Equal(2, departments.Count);
        }

        //[Fact]
        //public void TestSave()
        //{
        //    var department = new Department("John", "4015804958");
        //    Department savedDepartment = DepartmentRepository.Save(department);

        //    Assert.NotNull(savedDepartment);
        //    Assert.NotNull(savedDepartment.Id);
        //}
    
    }
}
