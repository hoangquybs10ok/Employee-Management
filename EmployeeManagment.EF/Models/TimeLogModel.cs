namespace EmployeeManagement.Models
{
    public class TimeLogModel
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? Task { get; set; }
        public string? Description { get; set; }
        public string? UserName { get; set; }
    }
}
