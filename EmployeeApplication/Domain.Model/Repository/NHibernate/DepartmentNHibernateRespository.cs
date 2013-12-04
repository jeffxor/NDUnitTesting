using System;
using System.Collections.Generic;
using NHibernate;

namespace EmployeeApplication.Domain.Model.Repository.NHibernate
{
    public class DepartmentNHibernateRespository : IDepartmentRepository
    {
        private readonly ISession _session;

        public DepartmentNHibernateRespository(ISession session)
        {
            _session = session;
        }

        public Department GetById(int Id)
        {
            return _session.Get<Department>(Id);
        }

        public List<Department> GetAll()
        {
            throw new NotImplementedException();
        }

        public Department Save(Department department)
        {
            throw new NotImplementedException();
        }

        public void Delete(Department department)
        {
            throw new NotImplementedException();
        }
    }
}
