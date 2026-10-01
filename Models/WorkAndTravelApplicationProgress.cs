namespace GPACARICOMAPI.Models;

public class WorkAndTravelApplicationProgress
{
    public long ApplicationId { get; set; }

    public int TotalStages { get; set; }

    public int CompletedStages { get; set; }

    public decimal ProgressPercentage { get; set; }

    public string CurrentStage { get; set; } = string.Empty;

    public string CurrentStatus { get; set; } = string.Empty;

    public List<WorkAndTravelApplicationStageProgress> Stages { get; set; } = [];
}

public class WorkAndTravelApplicationStageProgress
{
    public int StageId { get; set; }

    public string StageCode { get; set; } = string.Empty;

    public string StageName { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public int StatusId { get; set; }

    public string StatusCode { get; set; } = string.Empty;

    public string StatusName { get; set; } = string.Empty;

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Notes { get; set; }
}