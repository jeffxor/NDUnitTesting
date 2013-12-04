using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace EmployeeApplication.Domain.Model.Repository.NHibernate
{
    public class DepartmentNHibernateRespository : IDepartmentRepository
    {
        private readonly ISession _session;

        public DepartmentNHibernateRespository(ISession session)
        {
            _session = session;
        }

        public Department GetById(int id)
        {
            return _session.Get<Department>(id);
        }

        public List<Department> GetAll()
        {
            return _session.Query<Department>().ToList();
        }

        public Department Save(Department department)
        {
            _session.Save(department);
            return department;
        }

        public void Delete(Department department)
        {
            throw new NotImplementedException();
        }
    }
}
