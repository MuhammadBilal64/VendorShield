namespace VendorShield.Model
{
    public class Delivery
    {
        public int Id { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public DateTime ActualDeliveryDate { get; set; }
        public int DeliveredQuantity { get; set; }
        public int DefectiveQuantity { get; set; }
        public string? Notes { get; set; }   

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
