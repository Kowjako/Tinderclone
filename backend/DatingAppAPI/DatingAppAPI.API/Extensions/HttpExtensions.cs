using DatingAppAPI.Application.Interfaces.Pagination;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DatingAppAPI.API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse resp, PaginationHeader header)
        {
            var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            resp.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOption));
            resp.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
