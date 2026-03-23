using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;

namespace VendorShield.DAL
{
    public class ScoringConfigDAL : IScoringConfigDAL
    {
        private readonly AppDbContext _context;

        public ScoringConfigDAL(AppDbContext context)
        {
            _context = context;
        }

        public Task<ScoringConfig?> GetActiveAsync()
        {
            return _context.ScoringConfigs
                .FirstOrDefaultAsync(c => c.IsActive);
        }

        public async Task<bool> UpdateActiveAsync(ScoringConfig config)
        {
            if (config == null) return false;

            var existing = await _context.ScoringConfigs
                .FirstOrDefaultAsync(c => c.IsActive);

            if (existing == null)
            {
                config.IsActive = true;
                config.CreatedDate = DateTime.Now;
                config.ModifiedDate = DateTime.Now;
                await _context.ScoringConfigs.AddAsync(config);
                await _context.SaveChangesAsync();
                return true;
            }

            existing.OnTimeWeight = config.OnTimeWeight;
            existing.QualityWeight = config.QualityWeight;
            existing.IncidentWeight = config.IncidentWeight;
            existing.ThresholdLowRisk = config.ThresholdLowRisk;
            existing.ThresholdMediumRisk = config.ThresholdMediumRisk;

            existing.ModifiedDate = DateTime.Now;
            existing.IsActive = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

