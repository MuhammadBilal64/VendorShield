using System.ComponentModel.DataAnnotations;
using VendorShield.Utility;

namespace VendorShield.Model
{
    public class Vendor
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public VendorStatus Status { get; set; } = VendorStatus.Active;

        public DateTime ContractStartDate { get; set; } = DateTime.Now;
        public DateTime? ContractEndDate { get; set; }

        public double PerformanceScore { get; set; }
        public double RiskScore { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();



    }
}
