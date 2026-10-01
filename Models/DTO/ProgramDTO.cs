namespace GPACARICOMAPI.Models.DTO
{
    using global::GPACARICOMAPI.Models.Interface;

    namespace GPACARICOMAPI.Models
    {
        public class ProgramDTO : IAuditable
        {
            public int programid { get; set; }
            public string programName { get; set; } = string.Empty;
            public string programDescription { get; set; } = string.Empty;
            public string? ImagePath { get; set; }
            public string programDate { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime? UpdatedDate { get; set; }
            public string? UpdatedBy { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? DeletedDate { get; set; }
            public string? DeletedBy { get; set; }
        }
    }

}
