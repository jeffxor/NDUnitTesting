namespace NDUnitTesting.Domain.Model
{
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Position { get; set; }
        public virtual Department EmployeeDepartment { get; set; }
    }
}
