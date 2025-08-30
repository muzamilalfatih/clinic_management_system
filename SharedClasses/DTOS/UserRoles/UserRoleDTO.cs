namespace SharedClasses
{
    public class UserRoleDTO
    {
        public int id { get; set; }
        public int roleId { get; set; }
        public int userId { get; set; }
        public bool isActive { get; set; }
        public DateTime createdDate { get; set; }
        public UserRoleDTO(int id, int roleId, int userId, bool isActive, DateTime createdDate)
        {
            this.id = id;
            this.roleId = roleId;
            this.userId = userId;
            this.isActive = isActive;
            this.createdDate = createdDate;
        }
    }
}
