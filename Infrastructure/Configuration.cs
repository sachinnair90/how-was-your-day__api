using System.Collections.Generic;

namespace Infrastructure
{
    public class Configuration
    {
        public string EmailRegex { get; set; }
        public Security Security { get; set; }
        public int TokenExpiryDays { get; set; }
        public List<Mood> Moods { get; set; }
    }

    public class Security
    {
        public string JwtSecret { get; set; }
        public string DefaultPassword { get; set; }
    }

    public class Mood
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}