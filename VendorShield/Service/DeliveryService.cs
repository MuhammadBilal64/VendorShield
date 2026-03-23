using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryDAL _deliveryDAL;
        private readonly IPurchaseOrderDAL _purchaseOrderDAL;

        public DeliveryService(IDeliveryDAL deliveryDAL, IPurchaseOrderDAL purchaseOrderDAL)
        {
            _deliveryDAL = deliveryDAL;
            _purchaseOrderDAL = purchaseOrderDAL;
        }

        public async Task<bool> AddDeliveryAsync(Delivery delivery)
        {
            if (delivery == null) return false;
            if (delivery.PurchaseOrderId <= 0) return false;

            if (delivery.ActualDeliveryDate == default) return false;
            if (delivery.DeliveredQuantity < 0) return false;
            if (delivery.DefectiveQuantity < 0) return false;
            if (delivery.DefectiveQuantity > delivery.DeliveredQuantity) return false;

            var po = await _purchaseOrderDAL.GetByIdAsync(delivery.PurchaseOrderId);
            if (po == null) return false;

            var isOnTime = po.ExpectedDeliveryDate.HasValue
                ? delivery.ActualDeliveryDate <= po.ExpectedDeliveryDate.Value
                : false;

            // Temporary rule until ScoringService is implemented:
            // treat any defect as not high-quality.
            var isHighQuality = delivery.DefectiveQuantity == 0;

            delivery.CreatedDate = DateTime.Now;
            delivery.ModifiedDate = null;
            delivery.IsActive = true;

            await _deliveryDAL.AddAsync(delivery);
            await _purchaseOrderDAL.UpdateDeliveryFlagsAsync(delivery.PurchaseOrderId, isOnTime, isHighQuality);

            return true;
        }

        public Task<List<Delivery>> GetAllDeliveriesAsync()
        {
            return _deliveryDAL.GetAllAsync();
        }
    }
}

