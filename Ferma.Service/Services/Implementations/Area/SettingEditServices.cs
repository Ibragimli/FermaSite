using Ferma.Core.IUnitOfWork;
using Ferma.Service.CustomExceptions;
using Ferma.Service.Dtos.Area;
using Ferma.Service.HelperService.Interfaces;
using Ferma.Service.Services.Interfaces.Area;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.Area
{
    public class SettingEditServices : ISettingEditServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManageImageHelper _settingImage;
        private readonly IWebHostEnvironment _env;

        public SettingEditServices(IUnitOfWork unitOfWork, IManageImageHelper settingImage, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _settingImage = settingImage;
            _env = env;
        }


        public async Task<SettingEditDto> GetSearch(int Id)
        {
            var setting = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == Id);
            if (setting == null)
                throw new ItemNotFoundException("Parametr tapılmadı");


            SettingEditDto settingEditDto = new SettingEditDto
            {
                Id = setting.Id,
                Key = setting.Key,
                KeyImageFile = setting.KeyImageFile,
                Value = setting.Value,
            };
            return settingEditDto;
        }

        public async Task SettingEdit(SettingEditDto SettingEdit)
        {
            if (SettingEdit.Value == null)
                throw new ItemNotFoundException("Dəyər adı boş ola bilməz!");

            var lastSetting = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == SettingEdit.Id);

            if (lastSetting == null)
                throw new ItemNotFoundException("Parametr tapilmadı!");

            if (SettingEdit.Value != null)
                lastSetting.Value = SettingEdit.Value;

            if (SettingEdit.KeyImageFile != null)
            {
                lastSetting.KeyImageFile = SettingEdit.KeyImageFile;
                _settingImage.PosterCheck(lastSetting.KeyImageFile);
                _settingImage.DeleteFile(lastSetting.Value, "setting");
                lastSetting.Value = _settingImage.FileSave(lastSetting.KeyImageFile, "setting");
                lastSetting.ModifiedDate = DateTime.UtcNow.AddHours(4);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task<SettingEditDto> IsExists(int id)
        {
            var SettingExist = await _unitOfWork.SettingRepository.GetAsync(x => x.Id == id);
            if (SettingExist == null)
                throw new ItemNotFoundException("ERROR");
            SettingEditDto editDto = new SettingEditDto
            {
                Value = SettingExist.Value,
                Id = SettingExist.Id,
            };
            return editDto;
        }
    }
}
