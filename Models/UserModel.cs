using GPACARICOMAPI.Models.Interface;
using Microsoft.AspNetCore.Identity;

namespace GPACARICOMAPI.Models
{
    public class AppUser : IAuditable
    {
        private int userId { get; set; }
        private string userFirstname { get; set; } = string.Empty;
        private string userLastname { get; set; } = string.Empty;
        private string userEmail { get; set; } = string.Empty;
        private int userRoleId { get; set; }
        private DateTime LastLoginAt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
