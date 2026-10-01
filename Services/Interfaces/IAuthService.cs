using GPACARICOMAPI.Models;
using GPACARICOMAPI.Models.DTO;
using Microsoft.AspNetCore.Identity.Data;


namespace GPACARICOMAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginResponse> Login(UserLoginRequest request);
        Task<SignUpResponse> Signup(AppUserDTO user);
        public Task<int?> refreshToken(string userID);
    }
}
