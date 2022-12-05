namespace DatingAppAPI.Application.Interfaces.Pagination
{
    public class LikeParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}
