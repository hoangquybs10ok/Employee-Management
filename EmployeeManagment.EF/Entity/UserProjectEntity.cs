namespace EmployeeManagement.EF.Entity
{
    public class UserProjectEntity :EntityBase
    {
        public int UserId {  get; set; }
        public UserEntity? User { get; set; }
        public int ProjectId { get; set; }
        public ProjectEntity? Project { get; set; }
    }
}
