namespace GPACARICOMAPI.Models
{
    public class EmailVerificationRequest
    {
        public string email { get; set; } = string.Empty;
        public string token{ get; set; } = string.Empty;
    }
}
