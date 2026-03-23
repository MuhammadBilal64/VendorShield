namespace VendorShield.Model
{
    public class VendorScoreHistoryItem
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public double OverallScore { get; set; }
        public double OnTimeScore { get; set; }
        public double QualityScore { get; set; }
        public double IncidentScore { get; set; }

        public double RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public int IncidentCount { get; set; }
    }
}

