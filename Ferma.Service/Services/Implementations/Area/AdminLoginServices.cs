using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class AdminLoginServices : IAdminLoginServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminLoginServices(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<bool> Login(AdminLoginPostDto adminLoginPostDto)
        {
            CheckValues(adminLoginPostDto);

            AppUser adminExist = await _unitOfWork.UserRepository.GetAsync(x => x.UserName == adminLoginPostDto.Username);

            if (adminExist != null && adminExist.IsAdmin == true)
            {
                var result = await _signInManager.PasswordSignInAsync(adminExist, adminLoginPostDto.Password, false, false);
                if (!result.Succeeded) throw new UserNotFoundException("Username və ya Passoword yanlışdır!");

                return true;
            }
            throw new UserNotFoundException("Username və ya Passoword yanlışdır!");
        }

        private void CheckValues(AdminLoginPostDto adminLoginPostDto)
        {
            if (adminLoginPostDto.Username == null)
                throw new ItemNullException("Username-i daxil edin");
            if (adminLoginPostDto.Password == null)
                throw new ItemNullException("Password-u daxil edin");
        }
        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
