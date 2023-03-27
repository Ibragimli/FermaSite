using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IContactUsServices
    {
        public void CheckContactUs(ContactUsDto contactUsDto);
        public Task CreateContactUs(ContactUsDto contactUsDto);
    }
}
