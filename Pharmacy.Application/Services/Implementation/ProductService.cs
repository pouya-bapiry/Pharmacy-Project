using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Paging;
using Pharmacy.Application.DTO.Product;
using Pharmacy.Application.Extensions;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Product;
using Pharmacy.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Fields and ctor

        private readonly IGenericRepository<Product> _productRepository;

        public ProductService(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_productRepository != null)
            {
                await _productRepository.DisposeAsync();
            }

        }
        #endregion

        #region Product

        #region Filter

        public async Task<FilterProductDto> FilterProductsInAdmin(FilterProductDto filter)
        {

            var query = _productRepository
                .GetQuery()
                .AsQueryable();

            #region State

            switch (filter.ProductState)
            {
                case FilterProductState.All:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case FilterProductState.Active:
                    query = query.Where(x => x.IsActive.Value);
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(x => !x.IsActive.Value);
                    break;
            }

            switch (filter.OrderBy)
            {
                case FilterProductOrderBy.CreateDateDescending:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDateAscending:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.PriceDescending:
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case FilterProductOrderBy.PriceAscending:
                    query = query.OrderBy(x => x.Price);
                    break;
                case FilterProductOrderBy.ViewDescending:
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case FilterProductOrderBy.SellCountDescending:
                    query = query.OrderByDescending(x => x.SellCount);
                    break;
                case FilterProductOrderBy.SellCountAscending:
                    query = query.OrderBy(x => x.SellCount);
                    break;
            }

            #region Filter

            if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
            }

            #endregion

            #region Paging

            var productCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, productCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetProduct(allEntities);

            #endregion
        }


        #endregion

        #region Create
        public async Task<CreateProductResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {
            if (productImage == null)
            {
                return CreateProductResult.HasNoImage;
            }

            if (!productImage.IsImage())
            {
                return CreateProductResult.ImageErrorType;
            }

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer,
                100, 100, PathExtension.ProductThumbServer);

            //create Product

            var newProduct = new Product
            {
                Title = product.Title,
                Code = product.Code,
                Price = product.Price,
                Image = imageName,
                IsActive = product.IsActive,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                ViewCount = 0,
                SellCount = 0

            };

      

            await _productRepository.AddEntity(newProduct);
            await _productRepository.SaveChanges();

            return CreateProductResult.Success;

        }

        #endregion

        #region Edit


        public async Task<EditProductDto> GetProductForEdit(long productId)
        {
            var product = await _productRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                    (x => x.Id == productId);

           

            return new EditProductDto
            {
                Id = productId,
                Title = product.Title,
                Code = product.Code,
                Price = product.Price,
                ProductImage = product.Image,
                IsActive = (bool)product.IsActive,
                Description = product.Description,
              
                ShortDescription = product.ShortDescription,
            };
        }

        public async Task<EditProductResult> EditProductInAdmin(EditProductDto product, IFormFile productImage)
        {
            var mainProduct = await _productRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == product.Id);


            if (mainProduct == null)
            {
                return EditProductResult.NotFound;
            }

            mainProduct.Id = product.Id;
            mainProduct.Title = product.Title;
            mainProduct.Code = product.Code;
            mainProduct.Price = product.Price;
            mainProduct.IsActive = product.IsActive;
            mainProduct.Description = product.Description;
            mainProduct.ShortDescription = product.ShortDescription;


            //Product Image

            if (productImage != null && productImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
                productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer,
                    100, 100, PathExtension.ProductThumbServer, mainProduct.Image);

                mainProduct.Image = imageName;
            }



         

            _productRepository.EditEntity(mainProduct);
            await _productRepository.SaveChanges();

            return EditProductResult.Success;
        }


        #endregion


        #endregion

    }
}

