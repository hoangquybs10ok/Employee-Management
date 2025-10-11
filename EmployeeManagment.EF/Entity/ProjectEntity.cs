using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.EF.Entity
{
    public class ProjectEntity : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<UserProjectEntity> UserProjects { get; set; } = new List<UserProjectEntity>();
    }
}

