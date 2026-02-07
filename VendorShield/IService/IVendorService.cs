using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IVendorService
    {
        Task<List<Vendor>> GetAllVendorsAsync();
        Task<Vendor?> GetVendorByIdAsync(int id);

        Task<bool> AddVendorAsync(Vendor vendor);
        Task<bool> UpdateVendorAsync(Vendor vendor);

        Task<bool> DeleteVendorAsync(int id);
    }
}
