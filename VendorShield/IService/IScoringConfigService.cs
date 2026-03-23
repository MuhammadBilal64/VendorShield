using VendorShield.Model;

namespace VendorShield.IService
{
    public interface IScoringConfigService
    {
        Task<ScoringConfig?> GetActiveConfigAsync();
        Task<bool> UpdateActiveConfigAsync(ScoringConfig config);
    }
}

