using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IPurchaseOrderDAL
    {
        Task<List<PurchaseOrder>> GetAllAsync();
        Task<PurchaseOrder?> GetByIdAsync(int id);

        Task AddAsync(PurchaseOrder purchaseOrder);
        Task UpdateAsync(PurchaseOrder purchaseOrder);
        Task UpdateDeliveryFlagsAsync(int purchaseOrderId, bool isOnTime, bool isHighQuality);
        Task RemoveAsync(int id);
    }
}

