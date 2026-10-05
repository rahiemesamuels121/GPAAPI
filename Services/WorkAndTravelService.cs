using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GPACARICOMAPI.Services;

public class WorkAndTravelService
    : IWorkAndTravelService
{
    private readonly IWorkAndTravelRepository
        _repository;

    public WorkAndTravelService(
        IWorkAndTravelRepository progressRepository)
    {
        _repository = progressRepository;
    }

    public async Task<bool> CreateWorkAndTravelApplicationAsync(string userID, WATApplication application, CancellationToken cancellationToken)
    {
        var id = await _repository.CreateApplicationAsync(
              userID,
              application,
              cancellationToken);

        if (id == null || id/2 ==0) {
            return false;
        }
        else
        {
            return true;
        }
    }




    public async Task<WorkAndTravelApplicationProgress?>
        GetApplicationProgressAsync(
            long applicationId,
            CancellationToken cancellationToken = default)
    {
        if (applicationId <= 0)
        {
            throw new ArgumentException(
                "Application ID must be greater than zero.",
                nameof(applicationId));
        }

        var stages =
            await _repository
                .GetApplicationProgressAsync(
                    applicationId,
                    cancellationToken);

        if (stages.Count == 0)
        {
            return null;
        }

        var totalStages = stages.Count;

        var completedStages =
            stages.Count(x =>
                x.StatusCode == "COMPLETED");

        var percentage =
            totalStages == 0
                ? 0
                : Math.Round(
                    (decimal)completedStages /
                    totalStages * 100,
                    2);

        var currentStage =
            stages
                .OrderBy(x => x.DisplayOrder)
                .FirstOrDefault(x =>
                    x.StatusCode != "COMPLETED");

        // If every stage is completed,
        // use the final stage.
        currentStage ??=
            stages
                .OrderByDescending(x =>
                    x.DisplayOrder)
                .First();

        return new WorkAndTravelApplicationProgress
        {
            ApplicationId = applicationId,

            TotalStages = totalStages,

            CompletedStages = completedStages,

            ProgressPercentage = percentage,

            CurrentStage =
                currentStage.StageName,

            CurrentStatus =
                currentStage.StatusName,

            Stages = stages
        };
    }

    public async Task<bool> CompleteStageAsync(
    long applicationId,
    int stageId,
    CancellationToken cancellationToken = default)
    {
        if (applicationId <= 0)
        {
            throw new ArgumentException(
                "Invalid application ID.");
        }

        if (stageId <= 0)
        {
            throw new ArgumentException(
                "Invalid stage ID.");
        }

        return await _repository.CompleteStageAsync(
            applicationId,
            stageId,
            cancellationToken);
    }

    public async Task<bool> CheckForActiveApplicationAsync(int year , string userID, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userID))
            throw new ArgumentException("User ID is required.", nameof(userID));

        return await _repository.CheckForActiveApplicationAsync(year, userID, cancellationToken);
    }

    public async Task<WATApplication> GetUserApplicationAsync(int season, string userId, CancellationToken cancellationToken = default)
    {
        if (season == default)
            throw new ArgumentException("Season is required.", nameof(season));
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID is required.", nameof(userId));

        return await _repository.GetUserApplicationAsync(season, userId, cancellationToken);
    }

    public async Task<IEnumerable<WATApplication>> GetAllApplicationsAsync(
    string? ApplicantId,
    DateTime? startDate,
    DateTime? endDate,
    CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ApplicantId) && startDate == null && endDate == null) {
            throw new ArgumentException("No Filters Provided");
        }

        if (startDate != null && endDate == null)
        {
            throw new ArgumentException("An End Date is Required");
        }

        return await _repository.GetAllApplicationsAsync(ApplicantId, startDate, endDate);
    }

}