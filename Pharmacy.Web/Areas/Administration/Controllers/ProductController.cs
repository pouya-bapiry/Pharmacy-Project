using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.Product;
using Pharmacy.Application.Services.Interfaces;

namespace Pharmacy.Web.Areas.Administration.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Fields And Ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        #endregion

        #region Actions

        #region Product

        #region Filter

        [HttpGet("product-list")]
        public async Task<IActionResult> FilterProduct(FilterProductDto filter)
        {
            var product = await _productService.FilterProductsInAdmin(filter);
            return View(product);
        }


        #endregion

        #region Create
        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {

            var model = new CreateProductDto();
            return View(model);

        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDto product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
            var result = await _productService.CreateProduct(product, productImage);

            switch (result)
            {
                case CreateProductResult.HasNoImage:
                    TempData[WarningMessage] = "لطفا تصویر محصول را آپلود نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case CreateProductResult.ImageErrorType:
                    TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                    TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                    break;
                case CreateProductResult.Error:
                    TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد";
                    break;
                case CreateProductResult.Success:
                    TempData[SuccessMessage] = $"محصول مورد نظر با عنوان {product.Title} با موفقیت ثبت شد";
                    return RedirectToAction("FilterProduct", "Product");
            }
            }


            return View(product);
        }
        #endregion

        #region Edit
        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }

            //   ViewBag.Categories = await _productService.GetAllActiveProductCategories();

            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDto edit, long productId, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProductInAdmin(edit, productImage);



                switch (result)
                {
                    case EditProductResult.NotForUser:
                        TempData[WarningMessage] = "در ویرایش اطلاعات خطایی رخ داده است";
                        break;
                    case EditProductResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات وارد شده یافت نشد";
                        break;
                    case EditProductResult.ImageErrorType:
                        TempData[WarningMessage] = "لطفا تصویر محصول را طبق فرمت های ذکر شده وارد نمایید";
                        TempData[InfoMessage] = "فرمت تصاویر باید به صورت jpg, jpeg, png  باشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = $"ویرایش محصول {edit.Title} با موفقیت انجام شد";
                        return RedirectToAction("FilterProduct", "Product", new { area = "Administration" });

                }

            }
            return View();
            //    ViewBag.Categories = await _productService.GetAllActiveProductCategories();

        }


        #endregion


        #endregion



        #endregion
    }
}
