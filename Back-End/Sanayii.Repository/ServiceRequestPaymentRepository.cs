using Microsoft.EntityFrameworkCore;
using Sanayii.Core.Entities;
using Sanayii.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Repository
{
    public class ServiceRequestPaymentRepository
    {
        private readonly SanayiiContext _dbContext;

        public ServiceRequestPaymentRepository(SanayiiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ServiceRequestPayment>> GetAllAsync()
        {
            return await _dbContext.ServiceRequestPayments.ToListAsync();
        }

        public async Task<ServiceRequestPayment> GetByIdsAsync(string customerId, int paymentId, int serviceId)
        {
            return await _dbContext.ServiceRequestPayments
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.PaymentId == paymentId && x.ServiceId == serviceId);
        }

        public async Task AddAsync(ServiceRequestPayment entity)
        {
            await _dbContext.ServiceRequestPayments.AddAsync(entity);
        }

        public void Update(ServiceRequestPayment entity)
        {
            _dbContext.ServiceRequestPayments.Update(entity);
        }

        public void Delete(ServiceRequestPayment entity)
        {
            _dbContext.ServiceRequestPayments.Remove(entity);
        }

        public void cancelRequest(string? customerId, string artisanId, int serviceId, int paymentId)
        {
            // Find the payment record
            var serviceRequestPayment = _dbContext.ServiceRequestPayments
                .FirstOrDefault(p => p.CustomerId == customerId &&
                                     p.ArtisanId == artisanId &&
                                     p.ServiceId == serviceId);

           
            var service = _dbContext.Service
                .FirstOrDefault(s => s.Id == serviceId);

            var payment = _dbContext.Payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment != null) {

                _dbContext.Payments.Remove(payment);
            }

            
            if (serviceRequestPayment != null)
            {
                _dbContext.ServiceRequestPayments.Remove(serviceRequestPayment);
            }

            if (service != null)
            {
                _dbContext.Service.Remove(service);
            }

            _dbContext.SaveChanges();
        }

    }
}
