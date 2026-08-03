using Microsoft.AspNetCore.Http;
using Pharmacy.Application.DTO.Product;
using Pharmacy.Application.DTO.ProductCategory;
using Pharmacy.Application.DTO.ProductColor;
using Pharmacy.Application.DTO.ProductFeatures;
using Pharmacy.Application.DTO.ProductGallery;
using Pharmacy.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {

        #region Product
         Task<FilterProductDto> FilterProducts(FilterProductDto filter);
        Task<FilterProductDto> FilterProductsInAdmin(FilterProductDto filter);
        Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage);
        Task<EditProductDto> GetProductForEdit(long productId);
        Task<EditProductResult> EditProductInAdmin(EditProductDto product, IFormFile? productImage);

        //Task<List<Product>> GetProductWithMaximumView(int take);
        //Task<List<Product>> GetLatestArrivalProducts(int take);
        //Task<ProductDetailsDto> GetProductDetails(long productId);
        #endregion

        #region Product Category

        Task<FilterProductCategoryDto> FilterProductCategory(FilterProductCategoryDto filter);
        Task<FilterProductCategoryDto> FilterProductSubCategory(FilterProductCategoryDto filter, long? parentId);
        Task<List<ProductCategory>> GetAllProductCategoriesBy(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategories();
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto category, IFormFile image);
        Task<EditProductCategoryDto> GetProductCategoryForEdit(long categoryId);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto edit, IFormFile? image);
        Task<bool> ActiveCategory(long categoryId);
        Task<bool> DeActiveCategory(long categoryId);


        #endregion

        #region Product Color
        Task<List<FilterProductColorDto>> GetAllProductColorInAdminPanel(long productId);
        Task<CreateProductColorResult> CreateProductColor(CreateProductColorDto color, long productId);
        Task<EditProductColorDto> GetProductColorForEdit(long colorId);
        Task<EditProductColorResult> EditProductColor(EditProductColorDto color, long colorId);
        #endregion

        #region Product Features
        Task<List<FilterProductFeatureDto>> GettAllActiveProductFeatures(long productId);
        Task<CreateProductFeatureResult> CreateProductFeature(CreateProductFeatureDto feature, long productId);
        Task<EditProductFeatureDto> GetProductFeatureForEdit(long featureId);
        Task<EditProductFeatureResult> EditProductFeature(EditProductFeatureDto feature,long featureId);
        #endregion

        #region ProductGallery

        Task<List<FilterProductGallery>> FilterProductGalleries(long productId);
        Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGallery gallery, long productId, IFormFile galleryImage);
        Task<EditProductGallery> GetProductGalleryForEdit(long galleryId);
        Task<EditProductGalleryResult> EditProductGallery(EditProductGallery gallery, long galleryId, IFormFile galleryImage);

        #endregion

    }
}
