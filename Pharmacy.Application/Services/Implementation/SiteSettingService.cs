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

        public SiteSettingService(IGenericRepository<SiteSetting> siteRepository)
        {
            _siteRepository = siteRepository;
        }

        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            _siteRepository.DisposeAsync();
        }
        #endregion

        #region Methods
        public async Task<SiteSettingDto> GetDefaultSiteSetting()
        {
           var siteSetting= await _siteRepository.GetQuery().AsQueryable().Select(x=>new SiteSettingDto
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

           }).FirstOrDefaultAsync(x=>x.IsDefault);
            return siteSetting?? new SiteSettingDto();
        }

        #endregion

    }
}
