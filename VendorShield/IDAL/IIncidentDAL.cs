using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IIncidentDAL
    {
        Task<List<Incident>> GetAllAsync();
        Task<Incident?> GetByIdAsync(int id);

        Task AddAsync(Incident incident);
        Task UpdateAsync(Incident incident);
        Task RemoveAsync(int id);
    }
}

