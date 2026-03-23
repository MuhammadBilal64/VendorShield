using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class VendorService : IVendorService
    {
        private readonly IVendorDAL _vendorDAL;
        private readonly IScoringService _scoringService;

        public VendorService(IVendorDAL vendorDAL, IScoringService scoringService)
        {
            _vendorDAL = vendorDAL;
            _scoringService = scoringService;
        }

        private static (DateTime From, DateTime To) GetDefaultScoringRange()
        {
            // Default scoring window for now; dashboard/time filters can extend this later.
            var to = DateTime.Now;
            var from = to.AddMonths(-6);
            return (from, to);
        }

        // Fetch all vendors and calculate scores
        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            var vendors = await _vendorDAL.GetAllAsync();
            var (from, to) = GetDefaultScoringRange();

            foreach (var v in vendors)
            {
                var score = await _scoringService.CalculateVendorScoreAsync(v.Id, from, to);
                v.PerformanceScore = score.OverallScore;
                v.RiskScore = score.RiskScore;
            }

            return vendors;
        }

        public async Task<Vendor?> GetVendorByIdAsync(int id)
        {
            if (id <= 0) return null;

            var vendor = await _vendorDAL.GetByIdAsync(id);
            if (vendor == null) return null;

            var (from, to) = GetDefaultScoringRange();
            var score = await _scoringService.CalculateVendorScoreAsync(vendor.Id, from, to);
            vendor.PerformanceScore = score.OverallScore;
            vendor.RiskScore = score.RiskScore;

            return vendor;
        }

        public async Task<bool> AddVendorAsync(Vendor vendor)
        {
            if (vendor == null || string.IsNullOrWhiteSpace(vendor.Name))
                return false;

            vendor.CreatedDate = DateTime.Now;
            vendor.IsActive = true;

            vendor.PerformanceScore = 0;
            vendor.RiskScore = 0;

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

            var (from, to) = GetDefaultScoringRange();
            var score = await _scoringService.CalculateVendorScoreAsync(existingVendor.Id, from, to);
            existingVendor.PerformanceScore = score.OverallScore;
            existingVendor.RiskScore = score.RiskScore;

            await _vendorDAL.UpdateAsync(existingVendor);
            return true;
        }

        public async Task<bool> DeleteVendorAsync(int id)
        {
            if (id <= 0) return false;

            var existingVendor = await _vendorDAL.GetByIdAsync(id);
            if (existingVendor == null || !existingVendor.IsActive) return false;

            await _vendorDAL.RemoveAsync(id);
            return true;
        }
    }
}
