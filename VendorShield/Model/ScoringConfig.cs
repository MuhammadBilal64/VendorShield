namespace VendorShield.Model
{
    public class ScoringConfig
    {
        public int Id { get; set; }

        public double OnTimeWeight { get; set; } = 0.5;
        public double QualityWeight { get; set; } = 0.3;
        public double IncidentWeight { get; set; } = 0.2;

        public int ThresholdLowRisk { get; set; } = 80;
        public int ThresholdMediumRisk { get; set; } = 60;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ?ModifiedDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
