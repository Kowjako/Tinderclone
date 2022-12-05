namespace DatingAppAPI.Application.Interfaces.Pagination
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; } = string.Empty;
        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;

        public string OrderBy { get; set; } = "lastActive";
    }
}
