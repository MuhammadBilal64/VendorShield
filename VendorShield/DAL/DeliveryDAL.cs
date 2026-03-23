using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;

namespace VendorShield.DAL
{
    public class DeliveryDAL : IDeliveryDAL
    {
        private readonly AppDbContext _context;

        public DeliveryDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task<Delivery?> GetByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await _context.Deliveries
                .Where(d => d.IsActive)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Delivery>> GetByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            if (purchaseOrderId <= 0) return new List<Delivery>();

            return await _context.Deliveries
                .Where(d => d.IsActive && d.PurchaseOrderId == purchaseOrderId)
                .OrderByDescending(d => d.ActualDeliveryDate)
                .ToListAsync();
        }

        public async Task<List<Delivery>> GetAllAsync()
        {
            return await _context.Deliveries
                .Where(d => d.IsActive)
                .Include(d => d.PurchaseOrder)
                    .ThenInclude(po => po.Vendor)
                .Include(d => d.PurchaseOrder)
                    .ThenInclude(po => po.Lines)
                .OrderByDescending(d => d.ActualDeliveryDate)
                .ToListAsync();
        }
    }
}

