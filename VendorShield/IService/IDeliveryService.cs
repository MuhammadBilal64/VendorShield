using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IDeliveryService
    {
        Task<bool> AddDeliveryAsync(Delivery delivery);
        Task<List<Delivery>> GetAllDeliveriesAsync();
    }
}

