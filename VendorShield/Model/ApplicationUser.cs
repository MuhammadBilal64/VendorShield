using Microsoft.AspNetCore.Identity;

namespace VendorShield.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
