using System;

namespace Api.Parameters
{
    public class FilterMoodRequestParameter
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}