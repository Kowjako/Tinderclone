namespace DatingAppAPI.Application.DTO
{
    public class UserWithRoleDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
