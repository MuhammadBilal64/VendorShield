using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace VendorShield.Model
{
    [Index(nameof(Email), IsUnique = true)]

    public class AdminUser
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }  

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
