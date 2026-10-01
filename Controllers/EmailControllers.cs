//using GPACARICOMAPI.Services.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Runtime.ConstrainedExecution;

//namespace GPACARICOMAPI.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class EmailController : ControllerBase
//    {
//        private readonly IEmailService _emailService;
//        private readonly IVerificationService _verificationService;
//        public EmailController(IEmailService emailservice, IVerificationService verificationService) { 
//          _emailService = emailservice;
//          _verificationService = verificationService;
//        }

//        [HttpPost("send-verification-email")]
//        public async Task<IActionResult> SendEmail([FromBody]string receptor) {

//            var confirmationLink = _verificationService.GenerateConfirmationLink();
//            var body  = $"<h1>Welcome to GPACARICOM</h1><p>Please click the link below to verify your email address:</p><a href='{confirmationLink}?email={receptor}'>Verify Email</a>";

//          await _emailService.sendEmail(receptor, "Email Verification Required",body);
//            return Ok(
                
//                );
//        }

//    }
//}
