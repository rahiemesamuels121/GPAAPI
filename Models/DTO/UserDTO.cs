using GPACARICOMAPI.Models.Interface;

namespace GPACARICOMAPI.Models.DTO
{
    public class AppUserDTO : IAuditable
    {
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }

        public string Telephone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Organization { get; set; } = string.Empty;
        public DateTime LastLoginAt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get ; set ; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
