using Microsoft.EntityFrameworkCore;

namespace VendorShield.Model
{
    public class PurchaseOrderLine
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
