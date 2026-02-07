using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class VendorService : IVendorService
    {
        private readonly IVendorDAL _vendorDAL;
        private readonly AppDbContext _context; // To fetch ScoringConfig

        public VendorService(IVendorDAL vendorDAL, AppDbContext context)
        {
            _vendorDAL = vendorDAL;
            _context = context;
        }

        // Fetch all vendors and calculate scores
        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            var vendors = await _vendorDAL.GetAllAsync();
            var config = await GetActiveScoringConfigAsync();

            foreach (var v in vendors)
            {
                CalculateScores(v, config);
            }

            return vendors;
        }

        public async Task<Vendor?> GetVendorByIdAsync(int id)
        {
            if (id <= 0) return null;

            var vendor = await _vendorDAL.GetByIdAsync(id);
            if (vendor == null) return null;

            var config = await GetActiveScoringConfigAsync();
            CalculateScores(vendor, config);

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

            var config = await GetActiveScoringConfigAsync();
            CalculateScores(existingVendor, config);

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


        private async Task<ScoringConfig> GetActiveScoringConfigAsync()
        {
            var config = await _context.ScoringConfigs.FirstOrDefaultAsync(c => c.IsActive);
            if (config == null)
            {
                config = new ScoringConfig();
            }
            return config;
        }

        private void CalculateScores(Vendor v, ScoringConfig config)
        {
            double onTimePct = v.PurchaseOrders.Any() ?
                v.PurchaseOrders.Count(po => po.IsOnTime) * 100.0 / v.PurchaseOrders.Count : 0;

            double qualityPct = v.PurchaseOrders.Any() ?
                v.PurchaseOrders.Count(po => po.IsHighQuality) * 100.0 / v.PurchaseOrders.Count : 0;

            double incidentImpact = v.Incidents.Count > 0 ? 100.0 : 0.0;

            v.PerformanceScore =
                onTimePct * config.OnTimeWeight +
                qualityPct * config.QualityWeight -
                incidentImpact * config.IncidentWeight;

            v.PerformanceScore = Math.Max(0, Math.Min(100, v.PerformanceScore)); // Clamp 0-100

            // === RiskScore Calculation ===
            if (v.PerformanceScore >= config.ThresholdLowRisk)
                v.RiskScore = 20; 
            else if (v.PerformanceScore >= config.ThresholdMediumRisk)
                v.RiskScore = 50;
            else
                v.RiskScore = 80; 
        }
    }
}
