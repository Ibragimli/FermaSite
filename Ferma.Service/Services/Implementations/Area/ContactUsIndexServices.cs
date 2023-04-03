using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class ContactUsIndexServices : IContactUsIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;


        public ContactUsIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<ContactUs> SearchCheck(string search)
        {
            var contactUsLast = _unitOfWork.ContactUsRepository.asQueryable();

            var ContactUs = _unitOfWork.ContactUsRepository;
            if (search != null)
            {
                search = search.ToLower();
                //categorySearch
                if (search != null)
                    contactUsLast = contactUsLast.Where(i => EF.Functions.Like(i.Subject, $"%{search}%"));
            }
            return contactUsLast;
        }

    }

}
