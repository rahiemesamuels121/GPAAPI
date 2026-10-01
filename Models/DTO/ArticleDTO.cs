namespace GPACARICOMAPI.Models.DTO
{
    using global::GPACARICOMAPI.Models.Interface;
    namespace GPACARICOMAPI.Models
    {
        public class ArticleDTO : IAuditable
        {
            // BASE CLASS 
            public string name { get; set; } = string.Empty;
            public string title { get; set; } = string.Empty;
            public string shortDescription { get; set; } = string.Empty;
            public string fullDescription { get; set; } = string.Empty;
            public string hyperlink { get; set; } = string.Empty;

            // AUDIT AUDIT AUDIT AUDIT AUDIT 

            // AUDIT AUDIT AUDIT AUDIT AUDIT 

            // AUDIT AUDIT AUDIT AUDIT AUDIT 

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
