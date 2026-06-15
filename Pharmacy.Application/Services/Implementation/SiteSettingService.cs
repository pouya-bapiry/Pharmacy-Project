using Eshop.Application.Utilities;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Site;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Site;
using Pharmacy.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
