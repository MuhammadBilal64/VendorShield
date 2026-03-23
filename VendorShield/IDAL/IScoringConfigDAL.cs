using VendorShield.Model;

namespace VendorShield.IDAL
{
    public interface IScoringConfigDAL
    {
        Task<ScoringConfig?> GetActiveAsync();
        Task<bool> UpdateActiveAsync(ScoringConfig config);
    }
}

