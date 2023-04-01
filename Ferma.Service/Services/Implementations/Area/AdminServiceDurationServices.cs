using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminServiceDurationServices : IAdminServiceDurationServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminServiceDurationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ServiceDurationCreate(ServiceDuration ServiceDuration)
        {
            ServiceDuration newServiceDuration = new ServiceDuration();
            bool check = false;
            if (ServiceDuration.Duration != 0)
            {
                newServiceDuration.Duration = ServiceDuration.Duration;
                check = true;
            }
            else
                throw new ItemNullException("Vaxt boş ola bilməz");

            if (ServiceDuration.Amount != 0)
            {
                newServiceDuration.Amount = ServiceDuration.Amount;
                check = true;
            }
            else
                throw new ItemNullException("Pul hissəsi boş ola bilməz");


            if (ServiceDuration.ServiceType != 0)
            {
                newServiceDuration.ServiceType = ServiceDuration.ServiceType;
                check = true;
            }
            else
                throw new ItemNullException("Servis hissəsi boş ola bilməz");


            if (check)
            {
                await _unitOfWork.ServiceDurationRepository.InsertAsync(newServiceDuration);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task ServiceDurationEdit(ServiceDuration ServiceDuration)
        {
            bool check = false;
            var oldServiceDuration = await _unitOfWork.ServiceDurationRepository.GetAsync(x => x.Id == ServiceDuration.Id);

            if (ServiceDuration.Duration != 0)
            {
                oldServiceDuration.Duration = ServiceDuration.Duration;
                check = true;
            }
            else
                throw new ItemNullException("Vaxt boş ola bilməz");

            if (ServiceDuration.Amount != null)
            {
                oldServiceDuration.Amount = ServiceDuration.Amount;
                check = true;
            }
            else
                throw new ItemNullException("Pul hissəsi boş ola bilməz");


            if (ServiceDuration.ServiceType != 0)
            {
                oldServiceDuration.ServiceType = ServiceDuration.ServiceType;
                check = true;
            }
            else
                throw new ItemNullException("Servis hissəsi boş ola bilməz");

            if (check)
            {
                oldServiceDuration.ModifiedDate = DateTime.UtcNow.AddHours(4);
                await _unitOfWork.CommitAsync();
            }
        }


        public async Task<ServiceDuration> GetServiceDuration(int id)
        {
            var ServiceDuration = await _unitOfWork.ServiceDurationRepository.GetAsync(x => x.Id == id && !x.IsDelete);

            return ServiceDuration;
        }
        public IQueryable<ServiceDuration> GetServiceDurations()
        {
            var serviceDuration = _unitOfWork.ServiceDurationRepository.asQueryable();
            serviceDuration = serviceDuration.Where(x => !x.IsDelete);
            return serviceDuration;
        }


    }
}
