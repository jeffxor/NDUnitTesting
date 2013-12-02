﻿using System.Collections.Generic;

namespace NDUnitTesting.Domain.Model.Repository
{
    public interface IDepartmentRepository
    {
        Department GetById(int Id);
        List<Department> GetAll(); 
        Department Save(Department department);
        void Delete(Department department);

    }
}
