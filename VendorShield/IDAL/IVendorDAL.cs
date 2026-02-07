using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IVendorDAL
    {
        Task AddAsync(Vendor vendor);
        Task RemoveAsync(int id);
        Task UpdateAsync(Vendor vendor);
        Task<List<Vendor>> GetAllAsync();
        Task<Vendor> GetByIdAsync(int id);

    }

}
