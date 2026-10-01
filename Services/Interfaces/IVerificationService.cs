namespace GPACARICOMAPI.Services.Interfaces
{
    public interface IVerificationService
    {
        public  Task<bool> VerifyToken(string token, string userId);
        public Task<bool> SaveToken(string token, string userid);
        public string CreateToken();
        public string GenerateConfirmationLink(string token, string email);
    }
}
