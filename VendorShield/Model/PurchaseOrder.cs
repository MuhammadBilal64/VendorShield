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

        public DateTime PoTime { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        [Precision(18, 2)]

        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Open;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }


    }
}
