using Microsoft.AspNetCore.Http;
using Pharmacy.Application.DTO.Product;
using Pharmacy.Application.DTO.ProductCategory;
using Pharmacy.Domain.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IProductService:IAsyncDisposable
    {

        #region Product
       // Task<FilterProductDto> FilterProducts(FilterProductDto filter);
        Task<FilterProductDto> FilterProductsInAdmin(FilterProductDto filter);
        Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage);
        Task<EditProductDto> GetProductForEdit(long productId);
        Task<EditProductResult> EditProductInAdmin(EditProductDto product, IFormFile productImage);

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
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto edit, IFormFile image);


        #endregion
    }
}
