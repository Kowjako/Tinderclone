﻿namespace DatingAppAPI.Application.Interfaces.Pagination
{
    public class UserParams
    {
        private const int MAX_PAGE_SIZE = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value;
        }


        public string CurrentUsername { get; set; } = string.Empty;
        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
    }
}
