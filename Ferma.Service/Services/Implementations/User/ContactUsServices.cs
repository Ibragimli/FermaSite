using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.User;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class ContactUsServices : IContactUsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactUsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CheckContactUs(ContactUsDto contactUsDto)
        {
            if (contactUsDto == null)
                throw new ItemNotFoundException("Error404");
        }

        public async Task CreateContactUs(ContactUsDto contactUsDto)
        {
            ContactUs contact = new ContactUs
            {
                Email = contactUsDto.Email,
                FullName = contactUsDto.FullName,
                Subject = contactUsDto.Subject,
                Message = contactUsDto.Message,
            };
            await _unitOfWork.ContactUsRepository.InsertAsync(contact);
            await _unitOfWork.CommitAsync();
        }
    }
}
