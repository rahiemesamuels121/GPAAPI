using GPACARICOMAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Services.Interfaces;

public interface IWorkAndTravelService
{
     Task<bool> CreateWorkAndTravelApplicationAsync(
     string userID,
     WATApplication application,
     CancellationToken cancellationToken);

    Task<WorkAndTravelApplicationProgress?> GetApplicationProgressAsync(
        long applicationId,
        CancellationToken cancellationToken = default);

    Task<bool> CompleteStageAsync(
    long applicationId,
    int stageId,
    CancellationToken cancellationToken = default);

    Task<bool> CheckForActiveApplicationAsync(int year,string userID,CancellationToken cancellationToken = default);
    Task<WATApplication> GetUserApplicationAsync(int season, string userId, CancellationToken cancellationToken = default);
}