﻿using Ferma.Core.Entites;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class PosterCreateDto
    {

        public int SubCategoryId { get; set; }
        public string PosterName { get; set; }
        public decimal Price { get; set; }
        public bool PriceCurrency { get; set; }
        public int CityId { get; set; }
        public string Describe { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> ImageFilesStr { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
    public class PosterCreateDtoValidator : AbstractValidator<PosterCreateDto>
    {
        public PosterCreateDtoValidator()
        {
            RuleFor(x => x.SubCategoryId).NotEmpty().WithMessage("Kategoriya hissəsi boş olmamalıdır.");
            RuleFor(x => x.PosterName).NotEmpty().WithMessage("Elanın ad hissəsi boş olmamalıdır.").MinimumLength(3).WithMessage("Elanın adının uzunluğu 3-dən az ola bilməz!").MaximumLength(100).WithMessage("Elanın adının uzunluğu 100 dən böyük ola bilməz!");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Elanın qiyməti  boş olmamalıdır.").GreaterThan(0).WithMessage("Elanın qiyməti 0-dən az ola bilməz").LessThan(9999999).WithMessage("Elanın qiyməti 9999999-dən çox ola bilməz");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Elanın şəhər hissəsi boş olmamalıdır.");
            RuleFor(x => x.Describe).NotEmpty().WithMessage("Elanın təsvir hissəsi boş olmamalıdır.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Adınızı qeyd etməlisiniz.").MinimumLength(3).WithMessage("Adınızın uzunluğu 3-dən az ola bilməz!").MaximumLength(70).WithMessage("Adınızın uzunluğu 70 dən böyük ola bilməz!");
            RuleFor(x => x.Email).MinimumLength(5).WithMessage("Emailinizin uzunluğu 5-dən az ola bilməz!").MaximumLength(70).WithMessage("Emailinizin uzunluğu 70 dən böyük ola bilməz!");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Nömrənizi qeyd edin.").MinimumLength(15).WithMessage("Nömrənizi düzgün formatda qeyd edin!").MaximumLength(15).WithMessage("Nömrənizi düzgün formatda qeyd edin!");
        }
    }
}
