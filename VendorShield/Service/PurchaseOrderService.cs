using VendorShield.IDAL;
using VendorShield.IService;
using VendorShield.Model;

namespace VendorShield.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderDAL _purchaseOrderDAL;
        private readonly IVendorDAL _vendorDAL;

        public PurchaseOrderService(IPurchaseOrderDAL purchaseOrderDAL, IVendorDAL vendorDAL)
        {
            _purchaseOrderDAL = purchaseOrderDAL;
            _vendorDAL = vendorDAL;
        }

        public Task<List<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            return _purchaseOrderDAL.GetAllAsync();
        }

        public Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int id)
        {
            return _purchaseOrderDAL.GetByIdAsync(id);
        }

        public async Task<bool> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null) return false;
            if (purchaseOrder.VendorId <= 0) return false;

            var vendor = await _vendorDAL.GetByIdAsync(purchaseOrder.VendorId);
            if (vendor == null) return false;

            if (purchaseOrder.Lines == null || !purchaseOrder.Lines.Any())
                return false;

            foreach (var line in purchaseOrder.Lines)
            {
                if (string.IsNullOrWhiteSpace(line.ItemName)) return false;
                if (line.Quantity <= 0) return false;
                if (line.UnitPrice < 0) return false;
            }

            purchaseOrder.TotalAmount = purchaseOrder.Lines.Sum(l => l.LineTotal);
            purchaseOrder.CreatedDate = DateTime.Now;
            purchaseOrder.IsActive = true;
            purchaseOrder.ModifiedDate = null;

            var now = DateTime.Now;
            foreach (var line in purchaseOrder.Lines)
            {
                line.IsActive = true;
                line.CreatedDate = now;
                line.ModifiedDate = now;
            }

            await _purchaseOrderDAL.AddAsync(purchaseOrder);
            return true;
        }

        public async Task<bool> UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            if (purchaseOrder == null) return false;
            if (purchaseOrder.Id <= 0) return false;
            if (purchaseOrder.VendorId <= 0) return false;

            var vendor = await _vendorDAL.GetByIdAsync(purchaseOrder.VendorId);
            if (vendor == null) return false;

            if (purchaseOrder.Lines == null || !purchaseOrder.Lines.Any())
                return false;

            foreach (var line in purchaseOrder.Lines)
            {
                if (string.IsNullOrWhiteSpace(line.ItemName)) return false;
                if (line.Quantity <= 0) return false;
                if (line.UnitPrice < 0) return false;
            }

            purchaseOrder.TotalAmount = purchaseOrder.Lines.Sum(l => l.LineTotal);
            purchaseOrder.ModifiedDate = DateTime.Now;

            var now = DateTime.Now;
            foreach (var line in purchaseOrder.Lines)
            {
                line.IsActive = true;
                line.CreatedDate = now; // since DAL replaces lines
                line.ModifiedDate = now;
            }

            await _purchaseOrderDAL.UpdateAsync(purchaseOrder);
            return true;
        }

        public async Task<bool> DeletePurchaseOrderAsync(int id)
        {
            if (id <= 0) return false;

            var existing = await _purchaseOrderDAL.GetByIdAsync(id);
            if (existing == null) return false;

            await _purchaseOrderDAL.RemoveAsync(id);
            return true;
        }
    }
}

