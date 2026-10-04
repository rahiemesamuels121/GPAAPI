using Google.Protobuf;
using GPACARICOMAPI.Helpers;
using GPACARICOMAPI.Models;
using GPACARICOMAPI.Models.DTO;
using GPACARICOMAPI.Services;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;




namespace GPACARICOMAPI.Controllers
{
[Authorize]
[Route("/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly AuthHelper _authHelper;

        private IConfiguration _config;
    private readonly IVerificationService _verificationService;
        private readonly IEmailService _emailService;

    public AuthController(IAuthService auth, IConfiguration config, IVerificationService verificationService, IEmailService emailService)
    {
        _auth = auth;
        _authHelper = new AuthHelper(config);
        _verificationService = verificationService;
        _emailService= emailService;
            _config = config;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        UserLoginResponse response = await _auth.Login(request);
        if (response == null)
            return Unauthorized( new ApiResponse<string>
        {
            success = false,
            message = "Invalid Username or Password",
        });

        response.Jwt = _authHelper.CreateToken(response.UserId.ToString());

        return Ok(new
            ApiResponse<UserLoginResponse>
        {
            success = true,
            message = "Login Successful",
            data = response
        });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(AppUserDTO user) 
    {
        var response = await _auth.Signup(user);

        if (!response.success) {
            return Conflict(
                new ApiResponse<string>
                {
                    success = false,
                    message = response.message,
                }
                );
        }
            return Ok(new ApiResponse<string> { 
        success =true,
        message="Verification Email sent",
        });
    }

    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        string userId = User.FindFirst("userId")?.Value + "";
        var response = await _auth.refreshToken(userId);
        if (response == null) {
            return NotFound(
                new ApiResponse<Dictionary<string, string>>
                {
                    success = false,
                    message = "This user does not exist",
                });
        }
        return Ok(
            new ApiResponse<Dictionary<string, string>>
            {
                success = true,
                message = "successfully refreshed Token",
                data = new Dictionary<string, string> {
                    {
                    "token", _authHelper.CreateToken(userId)
                    }
                }
            }
        );
    }

    //TODO:
    [AllowAnonymous]
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmailAddress(
        [FromQuery] string token,
        [FromQuery] string email) {

            string userId = User.FindFirst("userId")?.Value + "";
            var response = await _verificationService.VerifyToken(token, email);
            if (response)
            {
                var  frontend = _config.GetValue<String>("AppSettings:FrontEndUrl");

                return Redirect(frontend) ?? Redirect("Https://google.com");
            }
            else
            {
                return Unauthorized();
            }

            

        }



    }
}
