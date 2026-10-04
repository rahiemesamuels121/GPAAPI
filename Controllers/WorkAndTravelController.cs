using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

[Authorize]
[ApiController]
[Route("Work-And-Travel/applications")]
public class WATApplicationsController : ControllerBase
{
    private readonly IWorkAndTravelRepository _service;

    public WATApplicationsController(
        IWorkAndTravelRepository service)
    {
        _service = service;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] WATApplication application,
        CancellationToken cancellationToken)
    {
        try
        {
            string userId = User.FindFirst("userId")?.Value + "";
            var didCreateApplication = await _service.CreateApplicationAsync(
                userId,
                application,
                cancellationToken);

            return Ok(new
            {
                success = true,
                message = "Application submitted successfully."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    success = false,
                    message = "An error occurred while submitting the application."
                });
        }
    }

    [HttpGet("{applicationId:long}/progress")]
    public async Task<IActionResult> GetProgress(
     long applicationId,
     CancellationToken cancellationToken)
    {
        var progress =
            await _service
                .GetApplicationProgressAsync(
                    applicationId,
                    cancellationToken);
        if (progress is null)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Application progress was not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = progress
        });
    }

    [HttpPut(
    "{applicationId:long}/stages/{stageId:int}/complete")]
    public async Task<IActionResult> CompleteStage(
    long applicationId,
    int stageId,
    CancellationToken cancellationToken)
    {
        var result =
            await _service
                .CompleteStageAsync(
                    applicationId,
                    stageId,
                    cancellationToken);

        if (!result)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Application or stage was not found."
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Stage completed successfully. " +
                "The next stage has been activated."
        });



    }

    [HttpGet("/HasExistingApplication")]
    public async Task<IActionResult> CheckForExistingApplications(
    int year,
    CancellationToken cancellationToken)
    {
        string userId = User.FindFirst("userId")?.Value + "";
        try
        {

            var hasApplication =
                await _service
                    .CheckForActiveApplicationAsync(
                    year,
                    userId,
                    cancellationToken

                    );
            if (hasApplication)
            {
                return Ok(new
                {
                    success = false,
                    message =
                        "Application Exists."
                });
            }

            return Ok(new
            {
                success = true,
                data = hasApplication
            });

        }
        catch
        {
            return BadRequest(
                new
                {
                    success = false,
                    message = "Server Error Occured"
                }
                );

        }




    }


    [HttpGet("/ExistingApplication/Get")]
    public async Task<IActionResult> GetUserApplicationAsync(
   int year,
   CancellationToken cancellationToken)
    {
        string userId = User.FindFirst("userId")?.Value + "";
        try
        {

            var hasApplication =
                await _service
                    .GetUserApplicationAsync(
                    year,
                    userId,
                    cancellationToken
                    );

            if (hasApplication == null)
            {
                return Ok(new
                {
                    success = false,
                    message =
                        "Application Exists."
                });
            }

            return Ok(new
            {
                success = true,
                data = hasApplication
            });

        }
        catch
        {
            return BadRequest(
                new
                {
                    success = false,
                    message = "Server Error Occured"
                }
                );

        }










    }
}