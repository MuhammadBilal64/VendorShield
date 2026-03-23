using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IScoringDAL
    {
        Task<ScoringConfig?> GetActiveScoringConfigAsync();
        Task<VendorScoringRawData> GetVendorRawScoringDataAsync(int vendorId, DateTime from, DateTime to);
    }
}

