using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EmployeeManagement.Entity
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
