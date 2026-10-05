using GPACARICOMAPI.Helpers;
using GPACARICOMAPI.Models;
using GPACARICOMAPI.Repositories.Interface;
using GPACARICOMAPI.Services.Interfaces;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace GPACARICOMAPI.Repositories;

public class WorkAndTravelRepository : IWorkAndTravelRepository
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly WorkAndTravelHelper _helper;

    public WorkAndTravelRepository(
        IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _helper = new WorkAndTravelHelper();
    }

    public async Task<long> CreateApplicationAsync(
        string userID,
        WATApplication application,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        await using var transaction =
            await connection.BeginTransactionAsync(
                cancellationToken);

        try
        {
            const string sql = """
                INSERT INTO WATApplications
                (
                    FirstName,
                    MiddleName,
                    LastName,
                    ApplicantId,
                    DateOfBirth,
                    ParishOfBirth,
                    HomeAddress,
                    Email,
                    CellPhone,
                    HomePhone,
                    Gender,
                    Instagram,
                    Facebook,
                    TikTok,
                    OtherSocialMedia,

                    University,
                    SchoolAddress,
                    ProgrammeOfStudy,
                    AcademicYear,
                    DepartureDate,
                    ReturnDate,

                    Emergency1Name,
                    Emergency1Relation,
                    Emergency1Address,
                    Emergency1Phone,
                    Emergency1Email,

                    Emergency2Name,
                    Emergency2Relation,
                    Emergency2Address,
                    Emergency2Phone,
                    Emergency2Email,

                    PreviousJ1,
                    PreviousJ1Details,
                    PreviousJ1Count,
                    OverseasSponsors,
                    LocalAgency,
                    PassportNumber,
                    Ssn,
                    VisaRefused,
                    VisaRefusedCategory,
                    ParticipationSummary,

                    AllAcknowledgementsApproved,

                    ReceivedBy,
                    ReceivedDate
                )
                VALUES
                (
                    @FirstName,
                    @MiddleName,
                    @LastName,
                    @ApplicantId,
                    @DateOfBirth,
                    @ParishOfBirth,
                    @HomeAddress,
                    @Email,
                    @CellPhone,
                    @HomePhone,
                    @Gender,
                    @Instagram,
                    @Facebook,
                    @TikTok,
                    @OtherSocialMedia,

                    @University,
                    @SchoolAddress,
                    @ProgrammeOfStudy,
                    @AcademicYear,
                    @DepartureDate,
                    @ReturnDate,

                    @Emergency1Name,
                    @Emergency1Relation,
                    @Emergency1Address,
                    @Emergency1Phone,
                    @Emergency1Email,

                    @Emergency2Name,
                    @Emergency2Relation,
                    @Emergency2Address,
                    @Emergency2Phone,
                    @Emergency2Email,

                    @PreviousJ1,
                    @PreviousJ1Details,
                    @PreviousJ1Count,
                    @OverseasSponsors,
                    @LocalAgency,
                    @PassportNumber,
                    @Ssn,
                    @VisaRefused,
                    @VisaRefusedCategory,
                    @ParticipationSummary,

                    @AllAcknowledgementsApproved,

                    @ReceivedBy,
                    @ReceivedDate
                );

                SELECT LAST_INSERT_ID();
                """;

            await using var command =
                new MySqlCommand(
                    sql,
                    connection,
                    (MySqlTransaction)transaction);

            // Personal information
            command.Parameters.AddWithValue(
                "@FirstName",
                DbValue(application.FirstName));

            command.Parameters.AddWithValue(
                "@MiddleName",
                DbValue(application.MiddleName));

            command.Parameters.AddWithValue(
                "@LastName",
                DbValue(application.LastName));
            command.Parameters.AddWithValue(
              "@ApplicantId",
              DbValue(userID));

            command.Parameters.AddWithValue(
                "@DateOfBirth",
                ToDatabaseDate(application.DateOfBirth));

            command.Parameters.AddWithValue(
                "@ParishOfBirth",
                DbValue(application.ParishOfBirth));

            command.Parameters.AddWithValue(
                "@HomeAddress",
                DbValue(application.HomeAddress));

            command.Parameters.AddWithValue(
                "@Email",
                DbValue(application.Email));

            command.Parameters.AddWithValue(
                "@CellPhone",
                DbValue(application.CellPhone));

            command.Parameters.AddWithValue(
                "@HomePhone",
                DbValue(application.HomePhone));

            command.Parameters.AddWithValue(
                "@Gender",
                DbValue(application.Gender));

            command.Parameters.AddWithValue(
                "@Instagram",
                DbValue(application.Instagram));

            command.Parameters.AddWithValue(
                "@Facebook",
                DbValue(application.Facebook));

            command.Parameters.AddWithValue(
                "@TikTok",
                DbValue(application.TikTok));

            command.Parameters.AddWithValue(
                "@OtherSocialMedia",
                DbValue(application.OtherSocialMedia));

            // University information
            command.Parameters.AddWithValue(
                "@University",
                DbValue(application.University));

            command.Parameters.AddWithValue(
                "@SchoolAddress",
                DbValue(application.SchoolAddress));

            command.Parameters.AddWithValue(
                "@ProgrammeOfStudy",
                DbValue(application.ProgrammeOfStudy));

            command.Parameters.AddWithValue(
                "@AcademicYear",
                DbValue(application.AcademicYear));

            command.Parameters.AddWithValue(
                "@DepartureDate",
                ToDatabaseDate(application.DepartureDate));

            command.Parameters.AddWithValue(
                "@ReturnDate",
                ToDatabaseDate(application.ReturnDate));

            // Emergency Contact 1
            command.Parameters.AddWithValue(
                "@Emergency1Name",
                DbValue(application.Emergency1Name));

            command.Parameters.AddWithValue(
                "@Emergency1Relation",
                DbValue(application.Emergency1Relation));

            command.Parameters.AddWithValue(
                "@Emergency1Address",
                DbValue(application.Emergency1Address));

            command.Parameters.AddWithValue(
                "@Emergency1Phone",
                DbValue(application.Emergency1Phone));

            command.Parameters.AddWithValue(
                "@Emergency1Email",
                DbValue(application.Emergency1Email));

            // Emergency Contact 2
            command.Parameters.AddWithValue(
                "@Emergency2Name",
                DbValue(application.Emergency2Name));

            command.Parameters.AddWithValue(
                "@Emergency2Relation",
                DbValue(application.Emergency2Relation));

            command.Parameters.AddWithValue(
                "@Emergency2Address",
                DbValue(application.Emergency2Address));

            command.Parameters.AddWithValue(
                "@Emergency2Phone",
                DbValue(application.Emergency2Phone));

            command.Parameters.AddWithValue(
                "@Emergency2Email",
                DbValue(application.Emergency2Email));

            // Programme history
            command.Parameters.AddWithValue(
                "@PreviousJ1",
                DbValue(application.PreviousJ1));

            command.Parameters.AddWithValue(
                "@PreviousJ1Details",
                DbValue(application.PreviousJ1Details));

            command.Parameters.AddWithValue(
                "@PreviousJ1Count",
                DbValue(application.PreviousJ1Count));

            command.Parameters.AddWithValue(
                "@OverseasSponsors",
                DbValue(application.OverseasSponsors));

            command.Parameters.AddWithValue(
                "@LocalAgency",
                DbValue(application.LocalAgency));

            command.Parameters.AddWithValue(
                "@PassportNumber",
                DbValue(application.PassportNumber));

            command.Parameters.AddWithValue(
                "@Ssn",
                DbValue(application.Ssn));

            command.Parameters.AddWithValue(
                "@VisaRefused",
                DbValue(application.VisaRefused));

            command.Parameters.AddWithValue(
                "@VisaRefusedCategory",
                DbValue(application.VisaRefusedCategory));

            command.Parameters.AddWithValue(
                "@ParticipationSummary",
                DbValue(application.ParticipationSummary));

            // Acknowledgements
            command.Parameters.AddWithValue(
                "@AllAcknowledgementsApproved",
                application.AllAcknowledgementsApproved);

            // Office use
            command.Parameters.AddWithValue(
                "@ReceivedBy",
                DbValue(application.ReceivedBy));

            command.Parameters.AddWithValue(
                "@ReceivedDate",
                ToDatabaseDate(application.ReceivedDate));

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            if (result is null ||
                result == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "The application was inserted but no application ID was returned.");
            }

            var applicationId =
                Convert.ToInt64(result);

            // Automatically create the application workflow.
            await InitializeProgressAsync(
                connection,
                (MySqlTransaction)transaction,
                applicationId,
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);

            return applicationId;
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }

    public async Task<List<WorkAndTravelApplicationStageProgress>>
        GetApplicationProgressAsync(
            long applicationId,
            CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(
            cancellationToken);

        const string sql = """
            SELECT
                p.stage_id,
                s.stage_code,
                s.stage_name,
                s.display_order,

                p.status_id,
                st.status_code,
                st.status_name,

                p.started_at,
                p.completed_at,
                p.notes

            FROM wat_application_stage_progress p

            INNER JOIN wat_application_stages s
                ON p.stage_id = s.stage_id

            INNER JOIN wat_stage_statuses st
                ON p.status_id = st.status_id

            WHERE p.application_id = @ApplicationId

            ORDER BY s.display_order;
            """;

        await using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@ApplicationId",
            applicationId);

        var results =
            new List<WorkAndTravelApplicationStageProgress>();

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        while (await reader.ReadAsync(
                   cancellationToken))
        {
            results.Add(
                new WorkAndTravelApplicationStageProgress
                {
                    StageId =
                        reader.GetInt32("stage_id"),

                    StageCode =
                        reader.GetString("stage_code"),

                    StageName =
                        reader.GetString("stage_name"),

                    DisplayOrder =
                        reader.GetInt32("display_order"),

                    StatusId =
                        reader.GetInt32("status_id"),

                    StatusCode =
                        reader.GetString("status_code"),

                    StatusName =
                        reader.GetString("status_name"),

                    StartedAt =
                        reader.IsDBNull(
                            reader.GetOrdinal("started_at"))
                            ? null
                            : reader.GetDateTime(
                                "started_at"),

                    CompletedAt =
                        reader.IsDBNull(
                            reader.GetOrdinal("completed_at"))
                            ? null
                            : reader.GetDateTime(
                                "completed_at"),

                    Notes =
                        reader.IsDBNull(
                            reader.GetOrdinal("notes"))
                            ? null
                            : reader.GetString(
                                "notes")
                });
        }

        return results;
    }

    private static async Task InitializeProgressAsync(
        MySqlConnection connection,
        MySqlTransaction transaction,
        long applicationId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO wat_application_stage_progress
            (
                application_id,
                stage_id,
                status_id,
                started_at
            )
            VALUES
                (@ApplicationId, 1, 2, CURRENT_TIMESTAMP),
                (@ApplicationId, 2, 1, NULL),
                (@ApplicationId, 3, 1, NULL),
                (@ApplicationId, 4, 1, NULL),
                (@ApplicationId, 5, 1, NULL),
                (@ApplicationId, 6, 1, NULL);
            """;

        await using var command =
            new MySqlCommand(
                sql,
                connection,
                transaction);

        command.Parameters.AddWithValue(
            "@ApplicationId",
            applicationId);

        await command.ExecuteNonQueryAsync(
            cancellationToken);
    }

    private static object ToDatabaseDate(
        DateOnly? date)
    {
        return date.HasValue
            ? date.Value.ToDateTime(
                TimeOnly.MinValue)
            : DBNull.Value;
    }

    private static object DbValue(
        object? value)
    {
        return value ?? DBNull.Value;
    }

    public async Task<bool> CompleteStageAsync(
    long applicationId,
    int stageId,
    CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        await using var transaction =
            await connection.BeginTransactionAsync(
                cancellationToken);

        try
        {
            // -----------------------------------------------------
            // 1. Find the current stage
            // -----------------------------------------------------

            const string stageSql = """
            SELECT
                p.stage_id,
                p.status_id,
                s.display_order
            FROM wat_application_stage_progress p
            INNER JOIN wat_application_stages s
                ON p.stage_id = s.stage_id
            WHERE p.application_id = @ApplicationId
              AND p.stage_id = @StageId;
            """;

            int currentDisplayOrder;

            await using (var stageCommand =
                new MySqlCommand(
                    stageSql,
                    connection,
                    (MySqlTransaction)transaction))
            {
                stageCommand.Parameters.AddWithValue(
                    "@ApplicationId",
                    applicationId);

                stageCommand.Parameters.AddWithValue(
                    "@StageId",
                    stageId);

                await using var reader =
                    await stageCommand.ExecuteReaderAsync(
                        cancellationToken);

                if (!await reader.ReadAsync(
                        cancellationToken))
                {
                    await transaction.RollbackAsync(
                        cancellationToken);

                    return false;
                }

                currentDisplayOrder =
                    reader.GetInt32("display_order");
            }

            // -----------------------------------------------------
            // 2. Mark current stage as COMPLETED
            // -----------------------------------------------------

            const string completeSql = """
            UPDATE wat_application_stage_progress
            SET
                status_id = (
                    SELECT status_id
                    FROM wat_stage_statuses
                    WHERE status_code = 'COMPLETED'
                    LIMIT 1
                ),
                completed_at = CURRENT_TIMESTAMP,
                updated_at = CURRENT_TIMESTAMP
            WHERE application_id = @ApplicationId
              AND stage_id = @StageId;
            """;

            await using (var completeCommand =
                new MySqlCommand(
                    completeSql,
                    connection,
                    (MySqlTransaction)transaction))
            {
                completeCommand.Parameters.AddWithValue(
                    "@ApplicationId",
                    applicationId);

                completeCommand.Parameters.AddWithValue(
                    "@StageId",
                    stageId);

                await completeCommand.ExecuteNonQueryAsync(
                    cancellationToken);
            }

            // -----------------------------------------------------
            // 3. Find the next stage
            // -----------------------------------------------------

            const string nextStageSql = """
            SELECT stage_id
            FROM wat_application_stages
            WHERE display_order > @CurrentDisplayOrder
              AND is_active = TRUE
            ORDER BY display_order
            LIMIT 1;
            """;

            int? nextStageId = null;

            await using (var nextStageCommand =
                new MySqlCommand(
                    nextStageSql,
                    connection,
                    (MySqlTransaction)transaction))
            {
                nextStageCommand.Parameters.AddWithValue(
                    "@CurrentDisplayOrder",
                    currentDisplayOrder);

                var result =
                    await nextStageCommand.ExecuteScalarAsync(
                        cancellationToken);

                if (result is not null &&
                    result != DBNull.Value)
                {
                    nextStageId =
                        Convert.ToInt32(result);
                }
            }

            // -----------------------------------------------------
            // 4. If another stage exists, make it IN PROGRESS
            // -----------------------------------------------------

            if (nextStageId.HasValue)
            {
                const string startNextSql = """
                UPDATE wat_application_stage_progress
                SET
                    status_id = (
                        SELECT status_id
                        FROM wat_stage_statuses
                        WHERE status_code = 'IN_PROGRESS'
                        LIMIT 1
                    ),
                    started_at =
                        COALESCE(
                            started_at,
                            CURRENT_TIMESTAMP
                        ),
                    completed_at = NULL,
                    updated_at = CURRENT_TIMESTAMP
                WHERE application_id = @ApplicationId
                  AND stage_id = @NextStageId;
                """;

                await using var nextCommand =
                    new MySqlCommand(
                        startNextSql,
                        connection,
                        (MySqlTransaction)transaction);

                nextCommand.Parameters.AddWithValue(
                    "@ApplicationId",
                    applicationId);

                nextCommand.Parameters.AddWithValue(
                    "@NextStageId",
                    nextStageId.Value);

                await nextCommand.ExecuteNonQueryAsync(
                    cancellationToken);
            }

            // -----------------------------------------------------
            // 5. Commit both changes together
            // -----------------------------------------------------

            await transaction.CommitAsync(
                cancellationToken);

            return true;
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }

    public async Task<bool> CheckForActiveApplicationAsync(
     int year,
     string userId,
     CancellationToken cancellationToken = default)
    {
        await using var connection =
            _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        const string sql = """
        SELECT EXISTS
        (
            SELECT 1
            FROM WATApplications
            WHERE ApplicantId = @UserId
              AND ReceivedDate >= STR_TO_DATE(
                    CONCAT(@Year, '-07-01'),
                    '%Y-%m-%d'
                  )
              AND ReceivedDate < STR_TO_DATE(
                    CONCAT(@Year + 1, '-06-30'),
                    '%Y-%m-%d'
                  )
        ) AS HasApplication;
        """;

        await using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Year",
            year);

        command.Parameters.AddWithValue(
            "@UserId",
            userId);

        var result =
            await command.ExecuteScalarAsync(
                cancellationToken);

        return Convert.ToBoolean(result);
    }

    public async Task<WATApplication> GetUserApplicationAsync(
        int season,
        string userId,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
        _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        const string sql = """
        SELECT *
        FROM WATApplications
        WHERE ApplicantId = @UserId
          AND ReceivedDate >= STR_TO_DATE(
                CONCAT(@Year, '-07-01'),
                '%Y-%m-%d'
              )
          AND ReceivedDate < STR_TO_DATE(
                CONCAT(@Year + 1, '-06-30'),
                '%Y-%m-%d'
              )
        ORDER BY ReceivedDate DESC
        LIMIT 1;
        """;

        await using var command =
            new MySqlCommand(
                sql,
                connection);

        command.Parameters.AddWithValue(
            "@Year",
            season);

        command.Parameters.AddWithValue(
            "@UserId",
            userId);

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return _helper.MapApplication(reader);
       


    }

    

    public async Task<IEnumerable<WATApplication>> GetAllApplicationsAsync(
    string? ApplicantId,
    DateTime? startDate,
    DateTime? endDate,
    CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        const string query = """
        SELECT *
        FROM WATApplications
        WHERE (@ApplicantId IS NULL OR ApplicantID = @ApplicantId)
          AND (@StartDate IS NULL OR ReceivedDate >= @StartDate)
          AND (@EndDate IS NULL OR ReceivedDate < DATE_ADD(@EndDate, INTERVAL 1 DAY))
        ORDER BY ReceivedDate DESC;
        """;

        using var command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue(
            "@ApplicantId",
            string.IsNullOrWhiteSpace(ApplicantId)
                ? DBNull.Value
                : ApplicantId);

        command.Parameters.AddWithValue(
            "@StartDate",
            startDate.HasValue
                ? startDate.Value
                : DBNull.Value);

        command.Parameters.AddWithValue(
            "@EndDate",
            endDate.HasValue
                ? endDate.Value
                : DBNull.Value);

        var applications = new List<WATApplication>();

        using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            applications.Add(
                _helper.MapApplication(reader));
        }

        return applications;
    }
}