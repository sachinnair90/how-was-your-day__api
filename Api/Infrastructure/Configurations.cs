namespace Api.Infrastructure
{
    public class Configurations
    {
        public string EmailRegex { get; set; }
        public Security Security { get; set; }
    }

    public class Security
    {
        public string JwtSecret { get; set; }
        public string DefaultPassword { get; set; }
    }
}
