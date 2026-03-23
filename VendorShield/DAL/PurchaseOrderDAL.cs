using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;

namespace VendorShield.DAL
{
    public class PurchaseOrderDAL : IPurchaseOrderDAL
    {
        private readonly AppDbContext _context;

        public PurchaseOrderDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseOrder>> GetAllAsync()
        {
            return await _context.PurchaseOrders
                .Where(po => po.IsActive)
                .Include(po => po.Vendor)
                .Include(po => po.Lines.Where(l => l.IsActive))
                .ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await _context.PurchaseOrders
                .Where(po => po.IsActive && po.Id == id)
                .Include(po => po.Vendor)
                .Include(po => po.Lines.Where(l => l.IsActive))
                .Include(po => po.Deliveries.Where(d => d.IsActive))
                .Include(po => po.Incidents.Where(i => i.IsActive))
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrders.AddAsync(purchaseOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null || purchaseOrder.Id <= 0) return;

            var existing = await _context.PurchaseOrders
                .Where(po => po.IsActive && po.Id == purchaseOrder.Id)
                .Include(po => po.Lines)
                .FirstOrDefaultAsync();

            if (existing == null) return;

            existing.VendorId = purchaseOrder.VendorId;
            existing.PoNumber = purchaseOrder.PoNumber;
            existing.PoTime = purchaseOrder.PoTime;
            existing.ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate;
            existing.TotalAmount = purchaseOrder.TotalAmount;
            existing.Status = purchaseOrder.Status;
            existing.IsOnTime = purchaseOrder.IsOnTime;
            existing.IsHighQuality = purchaseOrder.IsHighQuality;
            existing.ModifiedDate = purchaseOrder.ModifiedDate;

            // Replace all lines for now (simplifies edit UX).
            if (existing.Lines.Any())
            {
                _context.PurchaseOrderLines.RemoveRange(existing.Lines);
            }

            if (purchaseOrder.Lines != null && purchaseOrder.Lines.Any())
            {
                foreach (var line in purchaseOrder.Lines)
                {
                    line.Id = 0; // ensure EF treats them as new rows
                    line.PurchaseOrderId = existing.Id;
                    line.PurchaseOrder = existing;
                }
                await _context.PurchaseOrderLines.AddRangeAsync(purchaseOrder.Lines);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryFlagsAsync(int purchaseOrderId, bool isOnTime, bool isHighQuality)
        {
            if (purchaseOrderId <= 0) return;

            var existing = await _context.PurchaseOrders
                .FirstOrDefaultAsync(po => po.IsActive && po.Id == purchaseOrderId);

            if (existing == null) return;

            existing.IsOnTime = isOnTime;
            existing.IsHighQuality = isHighQuality;
            existing.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            if (id <= 0) return;

            var existing = await _context.PurchaseOrders.FindAsync(id);
            if (existing == null || !existing.IsActive) return;

            existing.IsActive = false;
            existing.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}

