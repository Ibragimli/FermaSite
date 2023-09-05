using Ferma.Mvc.Areas.manage.ViewModels;
using Ferma.Core.Entites;
using Ferma.Mvc.Areas.manage.ViewModels;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area.RoleManagers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]

    public class RoleManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAdminRoleManagerIndexServices _adminRoleManagerIndexServices;
        private readonly IAdminRoleManagerDeleteServices _adminRoleManagerDeleteServices;
        private readonly IAdminRoleManagerEditServices _adminRoleManagerEditServices;
        private readonly IAdminRoleManagerCreateServices _adminRoleManagerCreateServices;

        public RoleManagerController(UserManager<AppUser> userManager, IAdminRoleManagerIndexServices adminRoleManagerIndexServices, IAdminRoleManagerDeleteServices adminRoleManagerDeleteServices, IAdminRoleManagerEditServices adminRoleManagerEditServices, IAdminRoleManagerCreateServices adminRoleManagerCreateServices)
        {
            _userManager = userManager;
            _adminRoleManagerIndexServices = adminRoleManagerIndexServices;
            _adminRoleManagerDeleteServices = adminRoleManagerDeleteServices;
            _adminRoleManagerEditServices = adminRoleManagerEditServices;
            _adminRoleManagerCreateServices = adminRoleManagerCreateServices;
        }
        public IActionResult Index(int page = 1, string name = null)
        {
            RoleManagerIndexViewModel RoleManagerIndexVM = new RoleManagerIndexViewModel();
            try
            {
                var roleManager = _adminRoleManagerIndexServices.GetRoleManager(name);
                RoleManagerIndexVM = new RoleManagerIndexViewModel
                {
                    RoleManagers = PagenetedList<IdentityRole>.Create(roleManager, page, 5),
                };
            }
            catch (NotFoundException)
            {
                return RedirectToAction("index", "notfound");
            }

            catch (Exception)
            {
                return RedirectToAction("index", "notfound");
            }
            return View(RoleManagerIndexVM);
        }
        public IActionResult Create()
        {
            RoleManagerCreateDto roleManagerCreateDto = new RoleManagerCreateDto();

            return View(roleManagerCreateDto);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(RoleManagerCreateDto roleManagerCreateDto)
        {
            try
            {
                await _adminRoleManagerCreateServices.CreateRoleManager(roleManagerCreateDto);

            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(roleManagerCreateDto);
            }
            catch (ItemNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(roleManagerCreateDto);
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(roleManagerCreateDto);
            }
            catch (ValueFormatExpception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(roleManagerCreateDto);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(roleManagerCreateDto);
            }


            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //TempData["Error"] = ("Proses uğursuz oldu!");
                return View(roleManagerCreateDto);
            }

            return RedirectToAction("index", "RoleManager");

        }


        public async Task<IActionResult> Edit(string Id)
        {
            var roleManagerExist = new RoleManagerEditDto();
            try
            {
                var role = await _adminRoleManagerEditServices.GetRoleManager(Id);
                roleManagerExist = new RoleManagerEditDto
                {
                    Id = role.Id,
                    RoleName = role.Name
                };
            }
            catch (NotFoundException)
            {
                return RedirectToAction("Index", "notfound");
            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", roleManagerExist);
            }
            catch (ItemNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", roleManagerExist);
            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", roleManagerExist);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "notfound");
            }
            return View(roleManagerExist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleManagerEditDto roleManagerEditDto)
        {
            var roleManagerExist = new RoleManagerEditDto();

            try
            {
                var role = await _adminRoleManagerEditServices.GetRoleManager(roleManagerEditDto.Id);
                roleManagerExist = new RoleManagerEditDto
                {
                    Id = role.Id,
                    RoleName = role.Name
                };
                await _adminRoleManagerEditServices.EditRoleManager(roleManagerEditDto);


            }

            catch (NotFoundException)
            {

                return RedirectToAction("Index", "notfound");
            }
            catch (ItemNullException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }
            catch (ValueFormatExpception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }
            catch (ItemNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);

            }
            catch (ItemAlreadyException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }
            catch (ItemUseException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }

            catch (ValueAlreadyExpception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Edit", roleManagerEditDto);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "notfound");
            }
            TempData["Success"] = ("Proses uğurlu oldu");
            return RedirectToAction("Index", "RoleManager");
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _adminRoleManagerDeleteServices.DeleteRoleManager(id);
            }
            catch (ItemNotFoundException ex)
            {
                TempData["Error"] = (ex.Message);
                return Ok();
            }
            catch (UserNotFoundException ex)
            {
                TempData["Error"] = (ex.Message);
                return Ok();
            }
            catch (ItemUseException ex)
            {
                TempData["Error"] = (ex.Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //return RedirectToAction("index", "notfound");
            }
            TempData["Success"] = ("Sənəd silindi!");
            return Ok();
        }
    }

}
