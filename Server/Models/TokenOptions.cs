namespace Server.Models
{
    public class TokenOptions
    {
        public const string SectionName = "Token";

        public string SecretKey { get; set; }
    }
}