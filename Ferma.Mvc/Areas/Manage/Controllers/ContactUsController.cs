using Ferma.Core.Entites;
using Ferma.Mvc.Areas.Manage.ViewModels;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Ferma.Service.Services.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]

    public class ContactUsController : Controller
    {
        private readonly IEmailServices _emailServices;
        private readonly IContactUsIndexServices _ContactUsIndexServices;
        private readonly IContactUsDeleteServices _ContactUsDelete;
        private readonly IContactRespondServices _СontactRespondServices;
        public ContactUsController(IEmailServices emailServices, IContactUsIndexServices ContactUsIndexServices, IContactUsDeleteServices ContactUsDelete, IContactRespondServices ContactRespondServices)
        {
            _emailServices = emailServices;
            _ContactUsIndexServices = ContactUsIndexServices;
            _ContactUsDelete = ContactUsDelete;
            _СontactRespondServices = ContactRespondServices;
        }
        public IActionResult Index(int page = 1, string search = null)
        {
            ViewBag.Page = page;
            ViewBag.ContactUsSubject = search;

            var ContactUss = _ContactUsIndexServices.SearchCheck(search);

            ContactUsIndexViewModel ContactUsIndexVM = new ContactUsIndexViewModel
            {
                PagenatedItems = PagenetedList<ContactUs>.Create(ContactUss, page, 6),
            };

            return View(ContactUsIndexVM);
        }

        public async Task<IActionResult> ReplyContactUs(int contactUsId)
        {
            ContactUsReplyViewDto contactUs;
            try
            {
                contactUs = await _СontactRespondServices.RespondView(contactUsId);
            }
            catch (Exception ex)
            {

                contactUs = await _СontactRespondServices.RespondView(contactUsId);
                ModelState.AddModelError("ReplyText", ex.Message);
                return View(contactUs);
            }
            return View(contactUs);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ReplyContactUs(int contactUsId, ReplyContactPostDto replyContactPostDto)
        {
            ContactUsReplyViewDto contactUs;
            ReplyContactPostDto contactUsPost;
            try
            {
                contactUsPost = await _СontactRespondServices.RespondAnswer(contactUsId, replyContactPostDto.ReplyText);
                contactUs = await _СontactRespondServices.RespondView(contactUsId);
            }
            catch (Exception ex)
            {

                contactUs = await _СontactRespondServices.RespondView(contactUsId);
                ModelState.AddModelError("ReplyText", ex.Message);
                return View(contactUs);
            }
            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/contactEmail.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{fullname}}", contactUs.Fullname);
            body = body.Replace("{{replyText}}", replyContactPostDto.ReplyText);
            _emailServices.Send(contactUsPost.Email, "no-reply", body);
            TempData["Success"] = "Mesaj göndərildi";

            return RedirectToAction("index", "contactus");
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _ContactUsDelete.ContactUsDelete(id);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    TempData["Error"] = ("ContactUs məhsulda istifade olunur deye silmek mümkün olmadı!");
                    return Ok();

                }
                TempData["Error"] = ("Proses uğursuz oldu!");
                return Ok();

            }
            TempData["Success"] = ("Proses uğurlu oldu!");
            return Ok();
        }
    }
}
