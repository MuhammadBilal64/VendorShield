using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync();
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id);

        Task<bool> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder);
        Task<bool> DeletePurchaseOrderAsync(int id);
    }
}

