using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/wat-applications/docs")]
public class WATDocumentController
    : ControllerBase
{
    private readonly IWATDocumentService _documentService;

    public WATDocumentController(
        IWATDocumentService documentService)
    {
        _documentService = documentService;
    }

    [HttpPost(
        "{applicationId:long}/documents/{documentTypeId:int}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument(
        long applicationId,
        int documentTypeId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =
                await _documentService.UploadDocumentAsync(
                    applicationId,
                    documentTypeId,
                    documentTypeId.ToString(),
                    file,
                    cancellationToken);

            return Ok(new
            {
                success = true,
                message = "Document uploaded successfully.",
                data = result
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
    }
}