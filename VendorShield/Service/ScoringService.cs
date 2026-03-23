using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class ScoringService : IScoringService
    {
        private readonly IScoringDAL _scoringDAL;

        public ScoringService(IScoringDAL scoringDAL)
        {
            _scoringDAL = scoringDAL;
        }

        public async Task<VendorScoreResult> CalculateVendorScoreAsync(int vendorId, DateTime from, DateTime to)
        {
            var config = await _scoringDAL.GetActiveScoringConfigAsync() ?? new ScoringConfig();
            var raw = await _scoringDAL.GetVendorRawScoringDataAsync(vendorId, from, to);

            double onTimeScore = raw.TotalPoCount > 0
                ? (raw.OnTimePoCount * 100.0) / raw.TotalPoCount
                : 0.0;

            double qualityScore = raw.DeliveredQty > 0
                ? (1.0 - ((double)raw.DefectiveQty / (double)raw.DeliveredQty)) * 100.0
                : 0.0;

            // Incident penalty model: High = -15, Medium = -8, Low = -3; capped at 0.
            int penalty =
                raw.HighIncidentCount * 15 +
                raw.MediumIncidentCount * 8 +
                raw.LowIncidentCount * 3;

            double incidentScore = Math.Max(0.0, 100.0 - penalty);

            double overallScore =
                onTimeScore * config.OnTimeWeight +
                qualityScore * config.QualityWeight +
                incidentScore * config.IncidentWeight;

            overallScore = Math.Max(0.0, Math.Min(100.0, overallScore));

            double riskScore;
            string riskLevel;

            if (overallScore >= config.ThresholdLowRisk)
            {
                riskScore = 20;
                riskLevel = "Low";
            }
            else if (overallScore >= config.ThresholdMediumRisk)
            {
                riskScore = 50;
                riskLevel = "Medium";
            }
            else
            {
                riskScore = 80;
                riskLevel = "High";
            }

            return new VendorScoreResult
            {
                OnTimeScore = onTimeScore,
                QualityScore = qualityScore,
                IncidentScore = incidentScore,
                OverallScore = overallScore,
                RiskScore = riskScore,
                RiskLevel = riskLevel,
                IncidentCount = raw.LowIncidentCount + raw.MediumIncidentCount + raw.HighIncidentCount
            };
        }

        public async Task<List<VendorScoreHistoryItem>> CalculateVendorScoreHistoryAsync(
            int vendorId,
            int periods,
            int monthsPerPeriod = 1)
        {
            if (vendorId <= 0) return new List<VendorScoreHistoryItem>();
            if (periods <= 0) return new List<VendorScoreHistoryItem>();
            if (monthsPerPeriod <= 0) monthsPerPeriod = 1;

            // Periods are contiguous going backwards from "to".
            var to = DateTime.Now;
            var start = to.AddMonths(-(periods * monthsPerPeriod));

            var items = new List<VendorScoreHistoryItem>();
            for (var i = 0; i < periods; i++)
            {
                var periodFrom = start.AddMonths(i * monthsPerPeriod);
                var periodTo = periodFrom.AddMonths(monthsPerPeriod);

                var score = await CalculateVendorScoreAsync(vendorId, periodFrom, periodTo);

                items.Add(new VendorScoreHistoryItem
                {
                    From = periodFrom,
                    To = periodTo,
                    OverallScore = score.OverallScore,
                    OnTimeScore = score.OnTimeScore,
                    QualityScore = score.QualityScore,
                    IncidentScore = score.IncidentScore,
                    RiskScore = score.RiskScore,
                    RiskLevel = score.RiskLevel,
                    IncidentCount = score.IncidentCount
                });
            }

            return items;
        }
    }
}

