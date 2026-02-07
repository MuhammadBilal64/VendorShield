using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class VendorService : IVendorService
    {
        private readonly IVendorDAL _vendorDAL;

        public VendorService(IVendorDAL vendorDAL)
        {
            _vendorDAL = vendorDAL;
        }

  
        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            return await _vendorDAL.GetAllAsync();
        }

       
        public async Task<Vendor?> GetVendorByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _vendorDAL.GetByIdAsync(id);
        }

        
        public async Task<bool> AddVendorAsync(Vendor vendor)
        {
            if (vendor == null)
                return false;

            if (string.IsNullOrWhiteSpace(vendor.Name))
                return false;

            vendor.CreatedDate = DateTime.Now;
            vendor.IsActive = true;

            await _vendorDAL.AddAsync(vendor);
            return true;
        }

    
        public async Task<bool> UpdateVendorAsync(Vendor vendor)
        {
            if (vendor == null || vendor.Id <= 0)
                return false;

            var existingVendor = await _vendorDAL.GetByIdAsync(vendor.Id);

            if (existingVendor == null || !existingVendor.IsActive)
                return false;

            existingVendor.Name = vendor.Name;
            existingVendor.Email = vendor.Email;
            existingVendor.Phone = vendor.Phone;
            existingVendor.Address = vendor.Address;
            existingVendor.ModifiedDate = DateTime.Now;

            await _vendorDAL.UpdateAsync(existingVendor);

            return true;
        }

        public async Task<bool> DeleteVendorAsync(int id)
        {
            if (id <= 0)
                return false;

            var existingVendor = await _vendorDAL.GetByIdAsync(id);

            if (existingVendor == null || !existingVendor.IsActive)
                return false;

            await _vendorDAL.RemoveAsync(id);

            return true;
        }
    }
}
