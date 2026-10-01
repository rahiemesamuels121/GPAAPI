using GPACARICOMAPI.Models;

namespace GPACARICOMAPI.Repositories.Interface
{
    public interface IWorkAndTravelRepository
    {
       Task<long> CreateApplicationAsync(
       string userID,
       WATApplication application,
       CancellationToken cancellationToken = default
           );

        Task<List<WorkAndTravelApplicationStageProgress>> GetApplicationProgressAsync(
       long applicationId,
       CancellationToken cancellationToken = default);

        Task<bool> CompleteStageAsync(
    long applicationId,
    int stageId,
    CancellationToken cancellationToken = default);
    }
}
