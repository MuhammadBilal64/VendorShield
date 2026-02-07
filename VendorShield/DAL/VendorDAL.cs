using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;


namespace VendorShield.DAL
{
    public class VendorDAL:IVendorDAL
    {
        private readonly AppDbContext _context;
        public VendorDAL(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Model.Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Model.Vendor>> GetAllAsync()
        {
            return await _context.Vendors.Where(v=>v.IsActive).ToListAsync();
        }
        public async Task UpdateAsync(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task<Model.Vendor> GetByIdAsync(int id)
        {
            return await _context.Vendors.FindAsync(id);
        }
        public async Task RemoveAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
                return;

            vendor.IsActive = false;
            vendor.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
