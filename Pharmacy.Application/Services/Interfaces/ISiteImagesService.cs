using Microsoft.AspNetCore.Http;
using Pharmacy.Application.DTO.Site.Slider;
using Pharmacy.Domain.Entities.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface ISiteImagesService:IAsyncDisposable
    {

        #region Slider

        Task<List<FilterSliderDto>> GetAllActiveSlider();
        Task<List<FilterSliderDto>> GetAllSlider();
        Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage);
        Task<EditSliderDto> GetSliderForEdit(long sliderId);
        Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage, string username);
        Task<bool> ActiveSlider(long sliderId, string username);
        Task<bool> DeActiveSlider(long sliderId, string username);

        #endregion

    }
}
