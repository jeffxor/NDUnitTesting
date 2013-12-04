using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace EmployeeApplication.Domain.Model.Repository.NHibernate
{
    public class EmployeeNHibernateRespository : IEmployeeRepository
    {
        private readonly ISession _session;

        public EmployeeNHibernateRespository(ISession session)
        {
            _session = session;
        }

        public Employee GetById(int id)
        {
            return _session.Get<Employee>(id);
        }

        public List<Employee> GetAll()
        {
            return _session.Query<Employee>().ToList();
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
