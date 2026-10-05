using GPACARICOMAPI.Models;
using System.Data.Common;

namespace GPACARICOMAPI.Helpers
{
    public class WorkAndTravelHelper
    {
        public WATApplication MapApplication(DbDataReader reader)
        {
            return new WATApplication
            {
                Id = GetLong(reader, "Id"),

                ApplicantId =
                    GetLong(reader, "ApplicantId"),

                FirstName =
                    GetNullableString(reader, "FirstName"),

                MiddleName =
                    GetNullableString(reader, "MiddleName"),

                LastName =
                    GetNullableString(reader, "LastName"),

                DateOfBirth =
                    GetNullableDateOnly(reader, "DateOfBirth"),

                ParishOfBirth =
                    GetNullableString(reader, "ParishOfBirth"),

                HomeAddress =
                    GetNullableString(reader, "HomeAddress"),

                Email =
                    GetNullableString(reader, "Email"),

                CellPhone =
                    GetNullableString(reader, "CellPhone"),

                HomePhone =
                    GetNullableString(reader, "HomePhone"),

                Gender =
                    GetNullableString(reader, "Gender"),

                Instagram =
                    GetNullableString(reader, "Instagram"),

                Facebook =
                    GetNullableString(reader, "Facebook"),

                TikTok =
                    GetNullableString(reader, "TikTok"),

                OtherSocialMedia =
                    GetNullableString(reader, "OtherSocialMedia"),

                University =
                    GetNullableString(reader, "University"),

                SchoolAddress =
                    GetNullableString(reader, "SchoolAddress"),

                ProgrammeOfStudy =
                    GetNullableString(reader, "ProgrammeOfStudy"),

                AcademicYear =
                    GetNullableInt(reader, "AcademicYear"),

                DepartureDate =
                    GetNullableDateOnly(reader, "DepartureDate"),

                ReturnDate =
                    GetNullableDateOnly(reader, "ReturnDate"),

                Emergency1Name =
                    GetNullableString(reader, "Emergency1Name"),

                Emergency1Relation =
                    GetNullableString(reader, "Emergency1Relation"),

                Emergency1Address =
                    GetNullableString(reader, "Emergency1Address"),

                Emergency1Phone =
                    GetNullableString(reader, "Emergency1Phone"),

                Emergency1Email =
                    GetNullableString(reader, "Emergency1Email"),

                Emergency2Name =
                    GetNullableString(reader, "Emergency2Name"),

                Emergency2Relation =
                    GetNullableString(reader, "Emergency2Relation"),

                Emergency2Address =
                    GetNullableString(reader, "Emergency2Address"),

                Emergency2Phone =
                    GetNullableString(reader, "Emergency2Phone"),

                Emergency2Email =
                    GetNullableString(reader, "Emergency2Email"),

                PreviousJ1 =
                    GetNullableBool(reader, "PreviousJ1"),

                PreviousJ1Details =
                    GetNullableString(reader, "PreviousJ1Details"),

                PreviousJ1Count =
                    GetNullableInt(reader, "PreviousJ1Count"),

                OverseasSponsors =
                    GetNullableString(reader, "OverseasSponsors"),

                LocalAgency =
                    GetNullableString(reader, "LocalAgency"),

                PassportNumber =
                    GetNullableString(reader, "PassportNumber"),

                Ssn =
                    GetNullableString(reader, "Ssn"),

                VisaRefused =
                    GetNullableBool(reader, "VisaRefused"),

                VisaRefusedCategory =
                    GetNullableString(reader, "VisaRefusedCategory"),

                ParticipationSummary =
                    GetNullableString(reader, "ParticipationSummary"),

                AllAcknowledgementsApproved =
                    GetBool(reader, "AllAcknowledgementsApproved"),

                ReceivedBy =
                    GetNullableString(reader, "ReceivedBy"),

                ReceivedDate =
                    GetNullableDateOnly(reader, "ReceivedDate")
            };
        }

        // ==========================================
        // Required LONG
        // ==========================================
        private static long GetLong(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.GetInt64(ordinal);
        }

        // ==========================================
        // Nullable LONG
        // ==========================================
        private static long? GetNullableLong(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetInt64(ordinal);
        }

        // ==========================================
        // Nullable STRING
        // ==========================================
        private static string? GetNullableString(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetString(ordinal);
        }

        // ==========================================
        // Nullable INT
        // ==========================================
        private static int? GetNullableInt(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetInt32(ordinal);
        }

        // ==========================================
        // Required BOOL
        // ==========================================
        private static bool GetBool(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.GetBoolean(ordinal);
        }

        // ==========================================
        // Nullable BOOL
        // ==========================================
        private static bool? GetNullableBool(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            return reader.IsDBNull(ordinal)
                ? null
                : reader.GetBoolean(ordinal);
        }

        // ==========================================
        // Nullable DATEONLY
        // ==========================================
        private static DateOnly? GetNullableDateOnly(
            DbDataReader reader,
            string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return DateOnly.FromDateTime(
                reader.GetDateTime(ordinal));
        }
    }
}