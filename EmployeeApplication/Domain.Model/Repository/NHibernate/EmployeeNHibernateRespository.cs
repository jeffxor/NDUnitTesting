using System;
using System.Collections.Generic;
using NHibernate;

namespace EmployeeApplication.Domain.Model.Repository.NHibernate
{
    public class EmployeeNHibernateRespository : IEmployeeRepository
    {
        private readonly ISession _session;

        public EmployeeNHibernateRespository(ISession session)
        {
            _session = session;
        }

        public Employee GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetAll()
        {
            throw new NotImplementedException();
        }

        public Employee Save(Employee employee)
        {
            throw new NotImplementedException();
        }

        public void Delete(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
