using System;
using System.Collections.Generic;
using NHibernate;

namespace NDUnitTesting.Domain.Model.Repository.NHibernate
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
            throw new NotImplementedException();
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
