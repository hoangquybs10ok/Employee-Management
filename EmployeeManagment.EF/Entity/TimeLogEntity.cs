namespace EmployeeManagement.EF.Entity;
public class TimeLogEntity : EntityBase
{
    public int UserId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public string? Task { get; set; }
    public string? Description { get; set; }
    public UserEntity? User { get; set; }
}
