using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Site.Banner;
using Pharmacy.Application.DTO.Site.Slider;
using Pharmacy.Application.Extensions;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Site;
using Pharmacy.Domain.IRepository;

namespace Pharmacy.Application.Services.Implementation
{
    public class SiteImagesService : ISiteImagesService
    {
        #region Fields and Ctor

        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _siteBannerRepository;

        public SiteImagesService(IGenericRepository<Slider> sliderRepository, IGenericRepository<SiteBanner> bannerRepository)
        {
            _sliderRepository = sliderRepository;
            _siteBannerRepository = bannerRepository;
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
                    UserName=x.UserName

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
        public async Task<EditSliderResult> EditSlider(EditSliderDto edit, IFormFile? sliderImage, IFormFile? mobileSliderImage, string username)
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
            //if (edit.Description==null)
            //{
            //    edit.Description = mainSlider.Description; 
            //}
            mainSlider.Description = edit.Description;
            //mainSlider.IsActive = edit.IsActive;
            mainSlider.LastUpdateDate = DateTime.Now;

            _sliderRepository.EditEntityByUser(mainSlider, username);
            await _sliderRepository.SaveChanges();

            return EditSliderResult.Success;
        }




        #endregion



        #endregion

        #region Site Banners

        public async Task<List<FilterBannerDto>> GetBannersByPlacement(BannerPlacement placement)
        {
            return await _siteBannerRepository
                .GetQuery()
                .AsQueryable()
                .Where(b => b.Placement == placement).Select(x => new FilterBannerDto
                {
                    ColSize = x.ColSize,
                    Description = x.Description,
                    Url = x.Url,
                   

                })
                .ToListAsync();

        }
        public async Task<List<FilterBannerDto>> GetAllBanners()
        {
            return await _siteBannerRepository
            .GetQuery()
            .AsQueryable()
            .Select(x=>new FilterBannerDto 
            { 
            Id = x.Id,
            Url = x.Url,    
                ColSize = x.ColSize,
                Description = x.Description,
                CreateDate =x.CreateDate,
                ImageName = x.ImageName,
                LastUpdateDate=x.LastUpdateDate,
                UserName=x.UserName,
                IsDelete=x.IsDelete,
               
                
            
            }).OrderByDescending(x=>x.CreateDate).ToListAsync();
        }
        public async Task<CreateBannerResult> CreateBanner(CreateBannerDto banner, IFormFile bannerImage, string username)
        {
            if (bannerImage != null && bannerImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(bannerImage.FileName);
                bannerImage.AddImageToServer(imageName, PathExtension.BannerOriginServer,
                    100, 100, PathExtension.BannerThumbServer);

                var newBanner = new SiteBanner
                {
                    Placement = (BannerPlacement)banner.Placement,
                    ColSize = banner.ColSize,
                    Url = banner.Url,
                    ImageName = imageName,
                    Description = banner.Description

                };

                _siteBannerRepository.AddEntityByUser(newBanner, username);
                await _siteBannerRepository.SaveChanges();

                return CreateBannerResult.Success;
            }

            return CreateBannerResult.Error;
        }
        public async Task<EditBannerDto> GetBannerForEdit(long bannerId)
        {
            var banner = await _siteBannerRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == bannerId);

            if (banner == null)
            {
                return null;
            }

            return new EditBannerDto
            {
                Id = banner.Id,
                Placement = (EditBannerDto.BannerPlacement)banner.Placement,
                Url = banner.Url,
                ImageName = banner.ImageName,
                ColSize = banner.ColSize,
                Description = banner.Description,
            };
        }
        public async Task<EditBannerResult> EditBanner(EditBannerDto edit, IFormFile bannerImage, string username)
        {
            var mainBanner = await _siteBannerRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == edit.Id);

            if (mainBanner == null)
            {
                return EditBannerResult.Error;
            }

            if (bannerImage != null && bannerImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(bannerImage.FileName);
                bannerImage.AddImageToServer(imageName, PathExtension.BannerOriginServer,
                    100, 100, PathExtension.BannerThumbServer, mainBanner.ImageName);

                mainBanner.ImageName = imageName;
            }

            mainBanner.Id = edit.Id;
            mainBanner.Placement = (BannerPlacement)edit.Placement;
            mainBanner.Url = edit.Url;
            mainBanner.ColSize = edit.ColSize;
            mainBanner.Description = edit.Description;

            _siteBannerRepository.EditEntityByUser(mainBanner, username);
            await _siteBannerRepository.SaveChanges();

            return EditBannerResult.Success;

        }
        public async Task<bool> ActiveBanner(long bannerId, string username)
        {
            var banner = await _siteBannerRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == bannerId);

            if (banner == null)
            {
                return false;
            }

            banner.IsDelete = false;

            _siteBannerRepository.EditEntityByUser(banner, username);
            await _siteBannerRepository.SaveChanges();

            return true;
        }
        public async Task<bool> DeActiveBanner(long bannerId, string username)
        {
            var banner = await _siteBannerRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == bannerId);

            if (banner == null)
            {
                return false;
            }

            banner.IsDelete = true;

            _siteBannerRepository.EditEntityByUser(banner, username);
            await _siteBannerRepository.SaveChanges();

            return true;
        }


        #endregion
    }
}
