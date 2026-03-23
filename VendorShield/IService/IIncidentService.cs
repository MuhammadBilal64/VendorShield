using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IIncidentService
    {
        Task<List<Incident>> GetAllIncidentsAsync();
        Task<Incident?> GetIncidentByIdAsync(int id);

        Task<bool> AddIncidentAsync(Incident incident);
        Task<bool> UpdateIncidentAsync(Incident incident);
        Task<bool> DeleteIncidentAsync(int id);
    }
}

