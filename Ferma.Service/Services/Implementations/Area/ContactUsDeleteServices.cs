using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class ContactUsDeleteServices : IContactUsDeleteServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsDeleteServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ContactUsDelete(int id)
        {
            if (!await _unitOfWork.ContactUsRepository.IsExistAsync(x => x.Id == id)) throw new ItemNotFoundException("Əlaqə məlumatları  tapılmadı!");

            var ContactUs = await _unitOfWork.ContactUsRepository.GetAsync(x => x.Id == id);

            _unitOfWork.ContactUsRepository.Remove(ContactUs);
            await _unitOfWork.CommitAsync();
        }
    }
}
