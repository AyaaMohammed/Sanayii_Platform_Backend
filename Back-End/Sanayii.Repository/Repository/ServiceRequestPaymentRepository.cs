using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Enums;
using Sanayii.Repository;
using Sanayii.Repository.Data;
namespace Sanayii.Core.Repository
{
    public class ServiceRequestPaymentRepo : GenericRepository<ServiceRequestPayment>
    {
        public ServiceRequestPaymentRepo(SanayiiContext db) : base(db)
        {
        }
        //public ServiceRequestPayment GetByIDS(string customerId, int paymentId, int serviceId)
        //{
        //    return db.ServiceRequestPayments.FirstOrDefault(x => x.CustomerId == customerId && x.PaymentId == paymentId && x.ServiceId == serviceId);
        //}
        //public List<ServiceRequestPayment> GetByCustomerId(string customerId)
        //{
        //    return db.ServiceRequestPayments.Where(x => x.CustomerId == customerId).ToList();
        //}
        public async Task<List<ServiceRequestPayment>> GetAllAsync()
        {
            return await db.ServiceRequestPayments.ToListAsync();
        }

        public async Task<ServiceRequestPayment> GetByIdsAsync(string customerId, int paymentId, int serviceId)
        {
            return await db.ServiceRequestPayments
                .FirstOrDefaultAsync(x => x.CustomerId == customerId &&
                                          x.PaymentId == paymentId &&
                                          x.ServiceId == serviceId);
        }

        public async Task<List<ServiceRequestPayment>> GetByCustomerAsync(string customerId)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<ServiceRequestPayment>> GetByArtisanAsync(string artisanId)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.ArtisanId == artisanId)
                .ToListAsync();
        }

        public async Task<List<ServiceRequestPayment>> GetByServiceAsync(int serviceId)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<List<ServiceRequestPayment>> GetByPaymentAsync(int paymentId)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.PaymentId == paymentId)
                .ToListAsync();
        }

        public async Task<List<ServiceRequestPayment>> GetByStatusAsync(ServiceStatus status)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.Status == status)
                .ToListAsync();
        }

        public async Task AddAsync(ServiceRequestPayment entity)
        {
            await db.ServiceRequestPayments.AddAsync(entity);
        }

        public void Update(ServiceRequestPayment entity)
        {
            db.ServiceRequestPayments.Update(entity);
        }


        public void Delete(ServiceRequestPayment entity)
        {
            db.ServiceRequestPayments.Remove(entity);
        }

        public async Task<List<ServiceRequestPayment>> GetByCustomerAndStatusAsync(string customerId, ServiceStatus status)
        {
            return await db.ServiceRequestPayments
                .Where(x => x.CustomerId == customerId && x.Status == status)
                .ToListAsync();
        }



     
    }
}