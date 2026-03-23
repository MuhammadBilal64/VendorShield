using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IDeliveryDAL
    {
        Task AddAsync(Delivery delivery);
        Task<Delivery?> GetByIdAsync(int id);
        Task<List<Delivery>> GetByPurchaseOrderIdAsync(int purchaseOrderId);
        Task<List<Delivery>> GetAllAsync();
    }
}

