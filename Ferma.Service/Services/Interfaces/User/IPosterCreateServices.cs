using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPosterCreateServices
    {
        void PosterCheck(Poster Image);
        void ImagesCheck(Poster Images);
        void CreateImage(Poster Image, bool value);
        void SaveChange(Poster Poster);
        void SaveContext(Poster Poster);
    }
}
