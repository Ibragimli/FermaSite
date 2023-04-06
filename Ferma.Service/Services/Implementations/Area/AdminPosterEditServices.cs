using Ferma.Core.Entites;
using Ferma.Core.Enums;
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
    public class AdminPosterEditServices : IAdminPosterEditServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPosterEditServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CheckPostEdit(Poster poster)
        {
            if (poster.Id == 0)
                throw new NotFoundException("Error");
            if (poster.PosterFeatures.SubCategoryId == 0)
                throw new ItemNullException("Kategoriya hissəsi boş ola bilməz");
            if (poster.PosterFeatures.Name == null)
                throw new ItemNullException("Elanın ad hissəsi boş ola bilməz");
            if (poster.PosterFeatures.Describe == null)
                throw new ItemNullException("Təsvir hissəsi boş ola bilməz");
            if (poster.PosterFeatures.Name.Length > 100)
                throw new ItemNullException("Elanın ad hissəsinin uzunluğu  maksimum 100  ola bilər");
            if (poster.PosterFeatures.Describe.Length > 3000)
                throw new ItemNullException("Elanın ad hissəsinin uzunluğu  maksimum 3000  ola bilər");

        }

        public async Task EditPoster(Poster poster)
        {
            var oldPoster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == poster.Id, "PosterFeatures");
            if (oldPoster == null)
                throw new NotFoundException("Error");

            oldPoster.PosterFeatures.Name = poster.PosterFeatures.Name;
            oldPoster.PosterFeatures.Describe = poster.PosterFeatures.Describe;
            oldPoster.PosterFeatures.SubCategoryId = poster.PosterFeatures.SubCategoryId;
            await _unitOfWork.CommitAsync();
        }
        public async Task PosterDisabled(int id)
        {
            Poster poster = new Poster();
            if (id != 0)
                poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id);

            var time = new DateTime(0001, 01, 01, 8, 36, 44);

            if (poster == null)
                throw new ItemNotFoundException("Elan tapılmadı");
            poster.PosterFeatures.PosterStatus = PosterStatus.Disabled;
            poster.PosterFeatures.IsPremium = false;
            poster.PosterFeatures.IsVip = false;
            poster.PosterFeatures.ExpirationDatePremium = time;
            poster.PosterFeatures.ExpirationDateVip = time;
            await _unitOfWork.CommitAsync();
        }
        public async Task PosterActive(int id)
        {
            Poster poster = new Poster();
            DateTime now = DateTime.UtcNow;
            if (id != 0)
                poster = await _unitOfWork.PosterRepository.GetAsync(x => x.Id == id);

            if (poster == null)
                throw new ItemNotFoundException("Elan tapılmadı");

            poster.PosterFeatures.PosterStatus = PosterStatus.Active;
            poster.PosterFeatures.ExpirationDateActive = now.AddDays(30);
            poster.PosterFeatures.ExpirationDateDisabled = now.AddDays(90);
            poster.PosterFeatures.IsDisabled = false;

            await _unitOfWork.CommitAsync();
        }

    }
}
