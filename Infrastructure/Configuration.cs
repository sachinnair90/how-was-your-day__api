namespace Infrastructure
{
    public class Configuration
    {
        public string EmailRegex { get; set; }
        public Security Security { get; set; }
        public int MaxPasswordExpiryDays { get; set; }
    }

    public class Security
    {
        public string JwtSecret { get; set; }
        public string DefaultPassword { get; set; }
    }
}
