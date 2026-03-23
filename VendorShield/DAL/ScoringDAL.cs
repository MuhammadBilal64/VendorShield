using Microsoft.EntityFrameworkCore;
using VendorShield.Database;
using VendorShield.IDAL;
using VendorShield.Model;
using VendorShield.Utility;

namespace VendorShield.DAL
{
    public class ScoringDAL : IScoringDAL
    {
        private readonly AppDbContext _context;

        public ScoringDAL(AppDbContext context)
        {
            _context = context;
        }

        public Task<ScoringConfig?> GetActiveScoringConfigAsync()
        {
            return _context.ScoringConfigs
                .FirstOrDefaultAsync(c => c.IsActive);
        }

        public async Task<VendorScoringRawData> GetVendorRawScoringDataAsync(int vendorId, DateTime from, DateTime to)
        {
            if (vendorId <= 0) return new VendorScoringRawData();

            if (from > to)
            {
                var tmp = from;
                from = to;
                to = tmp;
            }

            // Consider only POs that have at least one delivery in the date range.
            // On-time is measured against the PO's ExpectedDeliveryDate.
            var poIds = await _context.PurchaseOrders
                .Where(po =>
                    po.IsActive &&
                    po.VendorId == vendorId &&
                    po.ExpectedDeliveryDate != null &&
                    po.Deliveries.Any(d =>
                        d.IsActive &&
                        d.ActualDeliveryDate >= from &&
                        d.ActualDeliveryDate <= to))
                .Select(po => po.Id)
                .ToListAsync();

            var result = new VendorScoringRawData
            {
                TotalPoCount = poIds.Count,
                OnTimePoCount = 0,
                OrderedQty = 0,
                DeliveredQty = 0,
                DefectiveQty = 0,
                LowIncidentCount = 0,
                MediumIncidentCount = 0,
                HighIncidentCount = 0
            };

            if (poIds.Count > 0)
            {
                result.OnTimePoCount = await _context.PurchaseOrders
                    .Where(po => po.IsActive && po.VendorId == vendorId && poIds.Contains(po.Id))
                    .CountAsync(po =>
                        po.ExpectedDeliveryDate != null &&
                        po.Deliveries.Any(d =>
                            d.IsActive &&
                            d.ActualDeliveryDate >= from &&
                            d.ActualDeliveryDate <= to &&
                            d.ActualDeliveryDate <= po.ExpectedDeliveryDate.Value));

                result.OrderedQty = await _context.PurchaseOrderLines
                    .Where(l => l.IsActive && poIds.Contains(l.PurchaseOrderId))
                    .SumAsync(l => (decimal)l.Quantity);

                result.DeliveredQty = await _context.Deliveries
                    .Where(d =>
                        d.IsActive &&
                        poIds.Contains(d.PurchaseOrderId) &&
                        d.ActualDeliveryDate >= from &&
                        d.ActualDeliveryDate <= to)
                    .SumAsync(d => (decimal)d.DeliveredQuantity);

                result.DefectiveQty = await _context.Deliveries
                    .Where(d =>
                        d.IsActive &&
                        poIds.Contains(d.PurchaseOrderId) &&
                        d.ActualDeliveryDate >= from &&
                        d.ActualDeliveryDate <= to)
                    .SumAsync(d => (decimal)d.DefectiveQuantity);
            }

            // Incident score uses incidents in the date range.
            result.LowIncidentCount = await _context.Incidents
                .Where(i => i.IsActive && i.VendorId == vendorId && i.IncidentDate >= from && i.IncidentDate <= to && i.Severity == IncidentSeverity.Low)
                .CountAsync();

            result.MediumIncidentCount = await _context.Incidents
                .Where(i => i.IsActive && i.VendorId == vendorId && i.IncidentDate >= from && i.IncidentDate <= to && i.Severity == IncidentSeverity.Medium)
                .CountAsync();

            result.HighIncidentCount = await _context.Incidents
                .Where(i => i.IsActive && i.VendorId == vendorId && i.IncidentDate >= from && i.IncidentDate <= to && i.Severity == IncidentSeverity.High)
                .CountAsync();

            return result;
        }
    }
}

