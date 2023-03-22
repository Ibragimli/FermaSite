using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class ProfileLoginServices : IProfileLoginServices
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ProfileLoginServices(IHttpContextAccessor httpContext, SignInManager<AppUser> signInManager, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _httpContext = httpContext;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<UserAuthentication> LoginAuthentication(string code, string phoneNumber, string token)
        {
            var authentication = await _unitOfWork.UserAuthenticationRepository.GetAsync(x => x.IsDisabled == false && x.Code == code && x.Token == token);
            var existAuthentication = await _unitOfWork.UserAuthenticationRepository.GetAsync(x => x.IsDisabled == false && x.Token == token);
            if (authentication == null)
            {
                if (existAuthentication != null)
                {
                    if (existAuthentication.Count > 1)
                        existAuthentication.Count -= 1;

                    else
                    {
                        existAuthentication.IsDisabled = true;

                    }
                    await _unitOfWork.CommitAsync();
                }
                throw new AuthenticationCodeException("Kod yanlışdır!");
            }
            return authentication;
        }

        public async Task UserCreate(string phoneNumber, string code)
        {
            AppUser newUser = new AppUser
            {
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                IsAdmin = false,
                Balance = 0,
            };
            var result = await _userManager.CreateAsync(newUser, code);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }
            await _userManager.AddToRoleAsync(newUser, "User");
            await _unitOfWork.CommitAsync();
        }

        public async Task UserLogin(string phoneNumber, string code, UserAuthentication authentication)
        {
            var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == phoneNumber && x.IsAdmin == false);
            if (UserExists == null)
                throw new NotFoundException("NotFound");

            var result = await _signInManager.PasswordSignInAsync(UserExists, code, false, false);
            if (!result.Succeeded)
                throw new NotFoundException("NotFound");

            authentication.IsDisabled = true;
            await _unitOfWork.CommitAsync();

            //return RedirectToAction("index", "anasehife");
        }

        public async Task UserResetPassword(string phoneNumber, string code)
        {
            var UserExists = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            var tokenResetPassword = await _userManager.GeneratePasswordResetTokenAsync(UserExists);

            if (UserExists == null || !(await _userManager.VerifyUserTokenAsync(UserExists, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", tokenResetPassword)))
            {
                throw new NotFoundException("NotFound");
            }

            var result = await _userManager.ResetPasswordAsync(UserExists, tokenResetPassword, code);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception(error.Description);
                }
            }

        }
    }
}
