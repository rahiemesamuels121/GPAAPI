namespace GPACARICOMAPI.Models
{
    public class UserLoginResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Jwt { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}


