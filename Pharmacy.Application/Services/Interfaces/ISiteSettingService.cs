using Pharmacy.Application.DTO.Site;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface ISiteSettingService : IAsyncDisposable
    {
        #region Site Setting

        Task<SiteSettingDto> GetDefaultSiteSetting();

        Task<List<AboutUsDto>> GetAboutUs();

        Task<EditSiteSettingDto> GetSiteSettingForEdit(long id);
        Task<bool> EditSiteSetting(EditSiteSettingDto edit, string username);

        #endregion
    }
}
