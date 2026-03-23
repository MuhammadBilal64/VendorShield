namespace VendorShield.Model
{
    public class VendorScoreResult
    {
        public double OnTimeScore { get; set; }
        public double QualityScore { get; set; }
        public double IncidentScore { get; set; }
        public double OverallScore { get; set; }

        // Kept as a numeric value because the existing UI uses RiskScore to color/size badges.
        // Mapping:
        // Low risk -> 20
        // Medium risk -> 50
        // High risk -> 80
        public double RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;

        // Total incidents in the scoring date range.
        public int IncidentCount { get; set; }
    }
}

