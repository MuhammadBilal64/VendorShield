using VendorShield.Utility;

namespace VendorShield.Model
{
    public class Incident
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public DateTime IncidentDate { get; set; }

        public IncidentType Type { get; set; }        
        public IncidentSeverity Severity { get; set; } = IncidentSeverity.Low;
        public IncidentStatus Status { get; set; } = IncidentStatus.Open;

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
