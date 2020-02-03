using System;

namespace BusinessLogic.DTO
{
    public class UserMoodDetails
    {
        public string Name { get; set; }
        public string Comments { get; set; }
        public DateTime RecordedOn { get; set; }
        public long Id { get; set; }
        public int MoodId { get; set; }
    }
}