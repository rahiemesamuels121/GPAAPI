using GPACARICOMAPI.Models.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GPACARICOMAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        public AuthHelper(IConfiguration config) { 
            _config = config;
        }


        public string CreateToken(string userId) 
        {
            Claim[] claims = new Claim[] {
             new Claim("userId", userId),
            };

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config.GetSection("AppSettings:TokenKey").Value)
                );

            SigningCredentials credentials = new SigningCredentials(
                tokenKey, 
                SecurityAlgorithms.HmacSha512
                );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityToken token  = handler.CreateToken(descriptor);

            return handler.WriteToken(token);



        }

        public HashDTO GeneratePasswordhash(string password)
        {
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) { 
                rng.GetNonZeroBytes(passwordSalt);
            }

            string passwordSaltPlusString = _config.GetSection("AppSettings: PasswordKey").Value +
                Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt:Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount:100000,
                numBytesRequested: 256/8

                );

            return new HashDTO
            {
                passwordHash = passwordHash,
                passwordSalt = passwordSalt,
            };

            
        }
    }
}
