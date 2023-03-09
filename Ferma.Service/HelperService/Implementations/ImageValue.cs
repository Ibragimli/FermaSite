using Ferma.Core.IUnitOfWork;
using Ferma.Data.Datacontext;
using Ferma.Service.HelperService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.HelperService.Implementations
{
    public class ImageValue : IImageValue
    {
        private readonly DataContext _context;

        public ImageValue(DataContext context)
        {
            _context = context;
        }
        public string ValueStr(string key)
        {
            var value = _context.ImageSettings.Where(x => !x.IsDelete).FirstOrDefault(x => x.Key == key).Value;
            return value;
        }
        public int ValueInt(string key)
        {
            var valueStr = _context.ImageSettings.Where(x => !x.IsDelete).FirstOrDefault(x => x.Key == key).Value;
            int value = int.Parse(valueStr);
            return value;
        }
        //private readonly IUnitOfWork _unitOfWork;

        //public ImageValue(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        //public async Task<string> ValueStr(string key)
        //{
        //    var valueObject = await _unitOfWork.ImageSettingRepository.GetAsync(x => !x.IsDelete && x.Key == key);
        //    var value = valueObject.Value;
        //    return value;
        //}
        //public async int ValueInt(string key)
        //{
        //    var valueObject = await _unitOfWork.ImageSettingRepository.GetAsync(x => !x.IsDelete && x.Key == key);
        //    var value = int.Parse(valueObject.Value);
        //    return value;
        //}
    }
}
