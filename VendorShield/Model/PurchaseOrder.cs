using Microsoft.EntityFrameworkCore;
using VendorShield.Utility;

namespace VendorShield.Model
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public string PoNumber { get; set; } = string.Empty;
        // These are currently used by VendorService scoring.
        // They are typically derived later from deliveries/incidents.
        public bool IsOnTime { get; set; } = false;
        public bool IsHighQuality { get; set; } = false;


        public DateTime PoTime { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        [Precision(18, 2)]

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Open;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        // Navigation properties (needed for PO CRUD UI and EF includes).
        // Use List<> so the UI can safely index/remove lines.
        public List<PurchaseOrderLine> Lines { get; set; } = new();
        public List<Delivery> Deliveries { get; set; } = new();
        public List<Incident> Incidents { get; set; } = new();


    }
}
