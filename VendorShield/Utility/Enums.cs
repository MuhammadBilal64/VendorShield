namespace VendorShield.Utility
{
  public enum VendorStatus
    {
        Active=1,
        OnHold=2,
        Blacklisted=3,
    }


    public enum PurchaseOrderStatus
    {
        Open = 1,
        PartiallyDelivered = 2,
        Completed = 3,
        Cancelled = 4
    }

    public enum IncidentStatus
    {
        Open = 1,
        InProgress = 2,
        Resolved = 3
    }

    public enum IncidentSeverity
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public enum IncidentType
    {
        QualityIssue = 1,
        DeliveryDelay = 2,
        CommunicationProblem = 3,
        Other = 4
    }
}
