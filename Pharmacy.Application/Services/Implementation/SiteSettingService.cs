using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Site;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Site;
using Pharmacy.Domain.IRepository;

namespace Pharmacy.Application.Services.Implementation
{
    public class SiteSettingService : ISiteSettingService
    {
        #region Fields
        private readonly IGenericRepository<SiteSetting> _siteRepository;
        private readonly IGenericRepository<AboutUs> _aboutUsRepository;

        public SiteSettingService(IGenericRepository<SiteSetting> siteRepository, IGenericRepository<AboutUs> aboutUsRepository)
        {
            _siteRepository = siteRepository;
            _aboutUsRepository = aboutUsRepository;
        }

        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            _siteRepository.DisposeAsync();
        }


        #endregion

        #region Methods

        #region Footer
        public async Task<SiteSettingDto> GetDefaultSiteSetting()
        {
            var siteSetting = await _siteRepository.GetQuery().AsQueryable().Select(x => new SiteSettingDto
            {
                SiteName = x.SiteName,
                Email = x.Email,
                Address = x.Address,
                CopyRight = x.CopyRight,
                FooterText = x.FooterText,
                IsDefault = x.IsDefault,
                MapScript = x.MapScript,
                Mobile = x.Mobile,
                Phone = x.Phone,
                CreateDate = x.CreateDate.ToStringShamsiDate(),
                LastUpdateDate = x.LastUpdateDate.ToStringShamsiDate()

            }).FirstOrDefaultAsync(x => x.IsDefault);
            return siteSetting ?? new SiteSettingDto();
        }


        public async Task<EditSiteSettingDto> GetSiteSettingForEdit(long id)
        {
            var setting = await _siteRepository
               .GetQuery()
               .AsQueryable()
               .SingleOrDefaultAsync(x => x.Id == id);
            if (setting == null)
            {
                return null;
            }

            return new EditSiteSettingDto
            {
                Id = setting.Id,
                Address = setting.Address,
                CopyRight = setting.CopyRight,
                Email = setting.Email,
                FooterText = setting.FooterText,
                IsDefault = setting.IsDefault,
                Mobile = setting.Mobile,
                Phone = setting.Phone
            };
        }

        public async Task<bool> EditSiteSetting(EditSiteSettingDto edit, string username)
        {

            var mainSetting = await _siteRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (mainSetting == null)
            {
                return false;
            }
            mainSetting.Id = edit.Id;
            mainSetting.Address = edit.Address;
            mainSetting.CopyRight = edit.CopyRight;
            mainSetting.Email = edit.Email;
            mainSetting.FooterText = edit.FooterText;
            mainSetting.Mobile = edit.Mobile;
            mainSetting.Phone = edit.Phone;

            _siteRepository.EditEntityByUser(mainSetting, username);
            _siteRepository.SaveChanges();
            return true;
        }

        #endregion

        #region About Us

        public async Task<List<AboutUsDto>> GetAboutUs()
        {

            return await _aboutUsRepository.GetQuery().AsQueryable()
                .Select(x => new AboutUsDto
                {
                    HeaderTitle = x.HeaderTitle,
                    Description = x.Description

                }).ToListAsync();
        }

        #endregion


        #endregion

    }
}
