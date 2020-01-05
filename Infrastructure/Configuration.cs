namespace Infrastructure
{
    public class Configuration
    {
        public string EmailRegex { get; set; }
        public string DefaultPassword { get; set; }
        public string Secret { get; set; }
        public int MaxPasswordExpiryDays { get; set; }
    }
}
