namespace VendorShield.Model
{
    public class VendorScoringRawData
    {
        public int TotalPoCount { get; set; }
        public int OnTimePoCount { get; set; }

        public decimal OrderedQty { get; set; }
        public decimal DeliveredQty { get; set; }
        public decimal DefectiveQty { get; set; }

        public int LowIncidentCount { get; set; }
        public int MediumIncidentCount { get; set; }
        public int HighIncidentCount { get; set; }
    }
}

