namespace EmployeeManagement.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate {  get; set; }
        public string Status { get; set; } = "Pending";

        public ICollection<UserModel> Users = new List<UserModel>();
    }
}
