using System.Collections.Generic;

namespace EmployeeApplication.Domain.Model.Repository
{
    public interface IEmployeeRepository
    {
        Employee GetById(int id);
        List<Employee> GetAll(); 
        Employee Save(Employee employee);
        void Delete(Employee employee);

    }
}
