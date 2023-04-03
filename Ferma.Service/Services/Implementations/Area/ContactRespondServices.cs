using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Services.Interfaces.Area;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class ContactRespondServices : IContactRespondServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactRespondServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReplyContactPostDto> RespondAnswer(int contactUsId, string RespondText)
        {
            var contactUs = await _unitOfWork.ContactUsRepository.GetAsync(x => x.Id == contactUsId);
            if (contactUs == null) throw new ItemNotFoundException("Xəta baş verdi");

            ReplyContactPostDto replyCommentPostDto = new ReplyContactPostDto
            {
                ContactUsId = contactUsId,
                ReplyText = RespondText,
                Email = contactUs.Email
            };
            return replyCommentPostDto;
        }

        public async Task<ContactUsReplyViewDto> RespondView(int contactUsId)
        {
            var contactUs = await _unitOfWork.ContactUsRepository.GetAsync(x => x.Id == contactUsId);
            if (contactUs == null) throw new ItemNotFoundException("Xəta baş verdi");
            ContactUsReplyViewDto contactUsReplyViewDto = new ContactUsReplyViewDto
            {
                ContactUsId = contactUsId,
                Fullname = contactUs.FullName,
                ContactText = contactUs.Message,
                ReplyContactPostDto = new ReplyContactPostDto(),
            };
            return contactUsReplyViewDto;
        }
    }
}
