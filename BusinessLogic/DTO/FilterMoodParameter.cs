using System;

namespace BusinessLogic.DTO
{
    public class FilterMoodParameter
    {
        public int UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}