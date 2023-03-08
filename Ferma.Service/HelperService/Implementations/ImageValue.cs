using Ferma.Core.IUnitOfWork;
using Ferma.Service.HelperService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.HelperService.Implementations
{
    public class ImageValue : IImageValue
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageValue(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ValueStr(string key)
        {
            var valueObject = await _unitOfWork.ImageSettingRepository.GetAsync(x => !x.IsDelete && x.Key == key);
            var value = valueObject.Value;
            return value;
        }
        public async Task<int> ValueInt(string key)
        {
            var valueObject = await _unitOfWork.ImageSettingRepository.GetAsync(x => !x.IsDelete && x.Key == key);
            var value = int.Parse(valueObject.Value);
            return value;
        }
    }
}
