using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Site.Slider;
using Pharmacy.Application.Extensions;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Site;
using Pharmacy.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Implementation
{
    public class SiteImagesService : ISiteImagesService
    {
        #region Fields and Ctor

        private readonly IGenericRepository<Slider> _sliderRepository;

        public SiteImagesService(IGenericRepository<Slider> sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }


        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {

            if (_sliderRepository != null)
            {
                await _sliderRepository.DisposeAsync();
            }
        }
        #endregion

        #region Slider

        #region Get
        public async Task<List<FilterSliderDto>> GetAllActiveSlider()
        {
            return await _sliderRepository.GetQuery()
                .Where(s => s.IsActive && !s.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .Select(x => new FilterSliderDto
                {
                    Description = x.Description,
                    ImageName = x.ImageName,
                    Link = x.Link,
                    MobileImageName = x.MobileImageName,


                })
                .ToListAsync();
        }

        public async Task<List<FilterSliderDto>> GetAllSlider()
        {

            return await _sliderRepository.GetQuery().Where(x => !x.IsDelete).Select(x => new FilterSliderDto
            {
                Id = x.Id,  
                Description = x.Description,
                ImageName = x.ImageName,
                Link = x.Link,
                MobileImageName = x.MobileImageName,
                IsActive = x.IsActive,
                CreateDate= x.CreateDate.ToStringShamsiDate(),
                LastUpdateDate=x.LastUpdateDate.ToStringShamsiDate(),
                

            }).ToListAsync();
        }
        #endregion

        #region Create

        public async Task<CreateSliderResult> CreateSlider(CreateSliderDto slider, IFormFile sliderImage, IFormFile mobileSliderImage)
        {
            if (sliderImage.IsImage() && mobileSliderImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(sliderImage.FileName);
                sliderImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                    100, 100, PathExtension.SliderThumbServer);

                var mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(mobileSliderImage.FileName);
                sliderImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                    100, 100, PathExtension.MobileSliderThumbServer);


                var newSlider = new Slider
                {
                    Link = slider.Link,
                    Description = slider.Description,
                    ImageName = imageName,
                    MobileImageName = mobileImageName,
                    IsActive = true,
                    CreateDate = DateTime.Now
                };

                await _sliderRepository.AddEntity(newSlider);
                await _sliderRepository.SaveChanges();
                return CreateSliderResult.Success;
            }
            return CreateSliderResult.Error;


        }
        #endregion

        #region Active deActive Slider
        public async Task<bool> ActiveSlider(long sliderId, string username)
        {
            var slider = _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return false;
            }

            slider.Result.IsActive = true;
            slider.Result.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntityByUser(slider.Result, username);
            await _sliderRepository.SaveChanges();

            return true;
        }


        public async Task<bool> DeActiveSlider(long sliderId, string username)
        {
            var slider = _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return false;
            }

            slider.Result.IsActive = false;
            slider.Result.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntityByUser(slider.Result, username);
            await _sliderRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Edit 
        public async Task<EditSliderDto> GetSliderForEdit(long sliderId)
        {
            var slider = await _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == sliderId);

            if (slider == null)
            {
                return null;
            }

            return new EditSliderDto
            {
                Id = slider.Id,
                ImageName = slider.ImageName,
                MobileImageName = slider.MobileImageName,
                Link = slider.Link,
                Description = slider.Description,
                IsActive = slider.IsActive
            };
        }
        public async Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile sliderImage, IFormFile mobileSliderImage, string username)
        {
            var mainSlider = await _sliderRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);

            if (mainSlider == null)
            {
                return EditSliderResult.NotFound;
            }


            if (sliderImage != null && sliderImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(sliderImage.FileName);
                sliderImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                    100, 100, PathExtension.SliderThumbServer);
                mainSlider.ImageName = imageName;
            }

            if (mobileSliderImage != null && mobileSliderImage.IsImage())
            {
                var mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(mobileSliderImage.FileName);
                mobileSliderImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                    100, 100, PathExtension.MobileSliderThumbServer);

                mainSlider.MobileImageName = mobileImageName;

            }


            mainSlider.Link = edit.Link;
            mainSlider.Description = edit.Description;
            //mainSlider.IsActive = edit.IsActive;
            mainSlider.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntityByUser(mainSlider, username);
            await _sliderRepository.SaveChanges();

            return EditSliderResult.Success;
        }




        #endregion



        #endregion

    }
}
