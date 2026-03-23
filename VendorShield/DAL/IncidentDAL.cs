using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;

namespace VendorShield.DAL
{
    public class IncidentDAL : IIncidentDAL
    {
        private readonly AppDbContext _context;

        public IncidentDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Incident>> GetAllAsync()
        {
            return await _context.Incidents
                .Where(i => i.IsActive)
                .Include(i => i.Vendor)
                .Include(i => i.PurchaseOrder)
                .OrderByDescending(i => i.IncidentDate)
                .ToListAsync();
        }

        public async Task<Incident?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await _context.Incidents
                .Where(i => i.IsActive && i.Id == id)
                .Include(i => i.Vendor)
                .Include(i => i.PurchaseOrder)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Incident incident)
        {
            await _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Incident incident)
        {
            if (incident == null || incident.Id <= 0) return;

            var existing = await _context.Incidents
                .FirstOrDefaultAsync(i => i.IsActive && i.Id == incident.Id);

            if (existing == null) return;

            existing.VendorId = incident.VendorId;
            existing.PurchaseOrderId = incident.PurchaseOrderId;
            existing.IncidentDate = incident.IncidentDate;
            existing.Type = incident.Type;
            existing.Severity = incident.Severity;
            existing.Status = incident.Status;
            existing.Description = incident.Description;
            existing.ModifiedDate = incident.ModifiedDate;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            if (id <= 0) return;

            var existing = await _context.Incidents.FindAsync(id);
            if (existing == null || !existing.IsActive) return;

            existing.IsActive = false;
            existing.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}

