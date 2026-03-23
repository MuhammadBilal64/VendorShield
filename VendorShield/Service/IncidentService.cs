using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentDAL _incidentDAL;
        private readonly IVendorDAL _vendorDAL;
        private readonly IPurchaseOrderDAL _purchaseOrderDAL;

        public IncidentService(IIncidentDAL incidentDAL, IVendorDAL vendorDAL, IPurchaseOrderDAL purchaseOrderDAL)
        {
            _incidentDAL = incidentDAL;
            _vendorDAL = vendorDAL;
            _purchaseOrderDAL = purchaseOrderDAL;
        }

        public Task<List<Incident>> GetAllIncidentsAsync()
        {
            return _incidentDAL.GetAllAsync();
        }

        public Task<Incident?> GetIncidentByIdAsync(int id)
        {
            return _incidentDAL.GetByIdAsync(id);
        }

        public async Task<bool> AddIncidentAsync(Incident incident)
        {
            if (incident == null) return false;
            if (incident.VendorId <= 0) return false;
            if (incident.PurchaseOrderId <= 0) return false;

            if (incident.IncidentDate == default) return false;
            if (string.IsNullOrWhiteSpace(incident.Description)) return false;

            var vendor = await _vendorDAL.GetByIdAsync(incident.VendorId);
            if (vendor == null) return false;

            var po = await _purchaseOrderDAL.GetByIdAsync(incident.PurchaseOrderId);
            if (po == null) return false;

            if (po.VendorId != incident.VendorId) return false;

            incident.CreatedDate = DateTime.Now;
            incident.ModifiedDate = null;
            incident.IsActive = true;

            await _incidentDAL.AddAsync(incident);
            return true;
        }

        public async Task<bool> UpdateIncidentAsync(Incident incident)
        {
            if (incident == null) return false;
            if (incident.Id <= 0) return false;
            if (incident.VendorId <= 0) return false;
            if (incident.PurchaseOrderId <= 0) return false;

            if (incident.IncidentDate == default) return false;
            if (string.IsNullOrWhiteSpace(incident.Description)) return false;

            var vendor = await _vendorDAL.GetByIdAsync(incident.VendorId);
            if (vendor == null) return false;

            var po = await _purchaseOrderDAL.GetByIdAsync(incident.PurchaseOrderId);
            if (po == null) return false;
            if (po.VendorId != incident.VendorId) return false;

            incident.ModifiedDate = DateTime.Now;
            incident.IsActive = true;

            await _incidentDAL.UpdateAsync(incident);
            return true;
        }

        public async Task<bool> DeleteIncidentAsync(int id)
        {
            if (id <= 0) return false;

            var existing = await _incidentDAL.GetByIdAsync(id);
            if (existing == null) return false;

            await _incidentDAL.RemoveAsync(id);
            return true;
        }
    }
}

