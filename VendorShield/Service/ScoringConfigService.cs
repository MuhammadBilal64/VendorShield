using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class ScoringConfigService : IScoringConfigService
    {
        private readonly IScoringConfigDAL _scoringConfigDAL;

        public ScoringConfigService(IScoringConfigDAL scoringConfigDAL)
        {
            _scoringConfigDAL = scoringConfigDAL;
        }

        public Task<ScoringConfig?> GetActiveConfigAsync()
        {
            return _scoringConfigDAL.GetActiveAsync();
        }

        public async Task<bool> UpdateActiveConfigAsync(ScoringConfig config)
        {
            if (config == null) return false;

            if (config.OnTimeWeight < 0 || config.QualityWeight < 0 || config.IncidentWeight < 0)
                return false;

            if (config.ThresholdLowRisk < config.ThresholdMediumRisk)
                return false;

            return await _scoringConfigDAL.UpdateActiveAsync(config);
        }
    }
}

