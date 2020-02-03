using System;

namespace DataAccess.DTO
{
    public class FilterMood
    {
        public int UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}