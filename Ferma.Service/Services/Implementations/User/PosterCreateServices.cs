using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Implementations.User
{

    public class PosterCreateServices : IPosterCreateServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _manageImageHelper;

        public PosterCreateServices(IUnitOfWork unitOfWork, IManageImageHelper manageImageHelper) : base()
        {
            _unitOfWork = unitOfWork;
            _manageImageHelper = manageImageHelper;
        }

        public async void CreateImage(Poster Image, bool value)
        {
            PosterImage Posterimage = new PosterImage
            {
                IsPoster = value,
                Poster = Image,
                Image = _manageImageHelper.FileSave(Image, "poster"),
            };
            await _unitOfWork.PosterImageRepository.InsertAsync(Posterimage);
        }

        public void ImagesCheck(Poster Images)
        {
            throw new NotImplementedException();
        }

        public void PosterCheck(Poster Image)
        {
            throw new NotImplementedException();
        }

        public async void SaveChange(Poster Poster)
        {
            await _unitOfWork.PosterRepository.InsertAsync(Poster);
        }
        public async void SaveContext(Poster Poster)
        {
            await _unitOfWork.CommitAsync();
        }
    }
}
