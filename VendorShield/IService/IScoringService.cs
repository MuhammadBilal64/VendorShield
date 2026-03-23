using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IScoringService
    {
        Task<VendorScoreResult> CalculateVendorScoreAsync(int vendorId, DateTime from, DateTime to);
        Task<List<VendorScoreHistoryItem>> CalculateVendorScoreHistoryAsync(int vendorId, int periods, int monthsPerPeriod = 1);
    }
}

