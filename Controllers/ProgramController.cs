using GPACARICOMAPI.Models;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Controllers
{

    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramRepository _programRepository;

        public ProgramController( IProgramRepository programRepository) {
            _programRepository = programRepository;
        }

        [AllowAnonymous]
        [HttpGet("getAllPrograms")]
        public async Task<ActionResult<List<ProgramModel>>> GetPrograms()
        {
            var programs = await _programRepository.GetProgramsAsync();

            if (programs == null) {
                return NotFound(
                    new ApiResponse<string> { 
                    success = false,
                    message = "No Pograms found"
                    }
                    );
            }

            return Ok(new ApiResponse<List<ProgramModel>>
            {
                success = true,
                message = "No Pograms found",
                data = programs
            });
        }

        [AllowAnonymous]
        [HttpGet("getProgram/{id}")]
        public async Task<ActionResult<ProgramModel>> GetProgram(int id)
        {
            var program = await _programRepository.GetProgram(id);

            if (program == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    success = false,
                    message = "Program Not Found"
                });
            }

            return Ok(new ApiResponse<ProgramModel>
            {
                success = true,
                message = "No Pograms found",
                data = program
            });
        }

        [HttpPost("addProgram")]
        public async Task<IActionResult> AddProgram([FromBody] ProgramModel program)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = await _programRepository.AddNewProgram(program);

            if (!success)
            {
                return BadRequest(new ApiResponse<string>
                {
                    success = false,
                    message = "Unable to add program"
                });
            }

            return Ok(new ApiResponse<string>
            {
                success = true,
                message = "Program added Successfully"
            });
        }

        [HttpPut("updateProgram/{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromBody] ProgramModel program)
        {
            if (id != program.programid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    success = false,
                    message = "The id provided does not match the program provided"
                });
            }

            bool success = await _programRepository.UpdateProgram(program);

            if (!success)
            {
                return NotFound(new ApiResponse<string>
                {
                    success = false,
                    message = "Program Not Found update Failed"
                });
            }

            return Ok(new ApiResponse<string>
            {
                success = true,
                message = "Program updated Successfully"
            });
        }
        [HttpDelete("deleteProgram/{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            bool success = await _programRepository.DeleteProgram(id);

            if (!success)
            {
                return NotFound(new ApiResponse<string>
                {
                    success = false,
                    message = "Program Not Found or is already deleted"
                });
            }

            return Ok(new ApiResponse<string>
            {
                success = true,
                message = "Program deleted successfully"
            });
        }
    }
}
