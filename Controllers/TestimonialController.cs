using GPACARICOMAPI.Models;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class TestimonialController : ControllerBase
    {
        private readonly ITestimonialRepository _testimonialRepository;

        public TestimonialController(ITestimonialRepository testimonialRepository)
        {
            _testimonialRepository = testimonialRepository;
        }

        [AllowAnonymous]
        [HttpGet("getTestimonial/{id}")]
        public async Task<ActionResult<TestimonialModel>> GetTestimonial(int id)
        {
            var testimonial = await _testimonialRepository.GetTestimonialbyId(id);

            if (testimonial == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    success = false,
                    message = "Testimonial not found."
                });
            }

            return Ok(new ApiResponse<string>
            {
                success = true,
                message = "ssuccesfully fetched Testimonial.",
          
            });
        }

        [AllowAnonymous]
        [HttpGet("getAllTestimonials")]
        public async Task<ActionResult<TestimonialModel>> GetllATestimonials()
        {
            var testimonial = await _testimonialRepository.GetTestimonialsAsync();

            if (testimonial == null)
            {
                return NotFound(new ApiResponse<List<TestimonialModel>>
                {
                    success = false,
                    message = "Testimonial not found.",
                    data = testimonial
                });
            }

            return Ok(new ApiResponse<List<TestimonialModel>>
            {
                success = false,
                message = "Testimonial not found.",
                data = testimonial
            });
        }

        [HttpPost("AddTestimonial")]
        public async Task<IActionResult>  AddNewTestimonial([FromBody] TestimonialModel testimonial)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = await _testimonialRepository.AddNewTestimonial(testimonial);

            if (!success)
                return BadRequest(new ApiResponse<string>
                {
                    success = false,
                    message = "unable to create Testimonial"
                });

            return Ok(
            new ApiResponse<string>
            {
                success = true,
                message = "Testimonial created successfully."
   
            });
        }

       
        [HttpPut("updateTestimonial/{id}")]
        public async Task<IActionResult> UpdateTestimonial(int id, [FromBody] TestimonialModel testimonial)
        {
            if (id != testimonial.testimonialid)
            {
                return BadRequest(
                    new ApiResponse<string>
                    {
                        success = false,
                        message = "The testimonial ID in the URL does not match the request body."
                    });
            }

            bool success = await _testimonialRepository.UpdateTestimonial(testimonial);

            if (!success)
            {
                return NotFound(new ApiResponse<string>
                {
                    success = false,
                    message = "Tedtimonial Not found or Update Failed"
                });
            }

            return Ok(
            new ApiResponse<string>
            {
                success = true,
                message = "Testimonial updated Successfully"
            });
        }

    }
}
