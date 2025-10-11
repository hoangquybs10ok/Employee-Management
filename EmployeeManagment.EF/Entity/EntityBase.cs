namespace EmployeeManagement.EF.Entity
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
