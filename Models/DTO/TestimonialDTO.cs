namespace GPACARICOMAPI.Models.DTO
{


    using global::GPACARICOMAPI.Models.Interface;
    using GPACARICOMAPI.Models;
    namespace GPACARICOMAPI.Models

    {
        public class TestimonialDTO : IAuditable
        {
            public int testimonialUserid { get; set; }
            public string username { get; set; } = string.Empty;
            public string testimonialTitle { get; set; } = string.Empty;
            public string testimonialbody { get; set; } = string.Empty;


            //AUDIT  ADUIT AUDIT
            //AUDIT  ADUIT AUDIT 
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string? UpdatedBy { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime? DeletedDate { get; set; }
            public string? DeletedBy { get; set; }
        }
    }

}
