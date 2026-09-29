using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Web.PresentationExtensions;


namespace ServiceHost.ViewComponents
{


    #region Site Header



    public class SiteHeaderViewComponent : ViewComponent
    {

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public SiteHeaderViewComponent(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orderDetail = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            ViewBag.OrderDetailCount = orderDetail.Details.Count;
            return View("SiteHeader");
        }
    }

    #endregion

    #region Site Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteSettingService _siteSettingService;

        public SiteFooterViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
                var siteSetting = await _siteSettingService.GetDefaultSiteSetting();
                return View("SiteFooter", siteSetting);
            
          
        }
    }
    #endregion

    #region Mega Menu

    public class MegaMenuViewComponent : ViewComponent
    {
        //private readonly IProductService _productService;

        //public MegaMenuViewComponent(IProductService productService)
        //{
        //    _productService = productService;
        //}

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public MegaMenuViewComponent(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var orderDetail = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            ViewBag.OrderDetailCount = orderDetail.Details.Count;
            return View("MegaMenu");
        }
    }
    #endregion

    #region Latest Arrivals

    public class LatestArrivalProductViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestArrivalProductViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestArrival = await _productService.GetLatestArrivalProducts(15);
            return View("LatestArrivalProduct", latestArrival);
        }
    }

    #endregion

    #region Product Discount Amazing

    public class ProductDiscountAmazingViewComponent : ViewComponent
    {
        private readonly IProductDiscountService _productDiscountService;

        public ProductDiscountAmazingViewComponent(IProductDiscountService productDiscountService)
        {
            _productDiscountService = productDiscountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var discountAmazing = await _productDiscountService.GetProductDiscountAmazing();
            return View("ProductDiscountAmazing", discountAmazing);
        }
    }
    #endregion

    #region Cart Canvas

    public class CartCanvasViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        public CartCanvasViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            return View("CartCanvas", openOrder);
        }
    }

    #endregion

    #region Latest Blogs

    public class LatestBlog : ViewComponent
    {
        private readonly IBlogService _blogService;

        public LatestBlog(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latest = await _blogService.GetLatestBlogs(15);
            return View("LatestBlog", latest);
        }
    }

    #endregion
}

