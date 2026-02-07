using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;

namespace VendorShield.DAL
{
    public class VendorDAL : IVendorDAL
    {
        private readonly AppDbContext _context;

        public VendorDAL(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Vendor>> GetAllAsync()
        {
            return await _context.Vendors
                .Include(v => v.PurchaseOrders)
                .Include(v => v.Incidents)
                .Where(v => v.IsActive)
                .ToListAsync();
        }

        public async Task<Vendor?> GetByIdAsync(int id)
        {
            return await _context.Vendors
                .Include(v => v.PurchaseOrders)
                .Include(v => v.Incidents)
                .FirstOrDefaultAsync(v => v.Id == id && v.IsActive);
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return;

            vendor.IsActive = false;
            vendor.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
