using Pharmacy.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.DTO.ProductOrder;
using Pharmacy.Web.Areas.User.Controllers;
using Pharmacy.Web.PresentationExtensions;
using ServiceHost.Http;


namespace ServiceHost.Areas.User.Controllers
{
    public class OrderController : UserBaseController
    {
        #region Constructor

        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public static int TotalDiscount;

        private string MerchantId { get; }


        public OrderController(IOrderService orderService, IUserService userService,  IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _orderService = orderService;
            _userService = userService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            MerchantId = configuration.GetSection("payment")["merchant"];

        }

        #endregion


        #region Add Product to Open order

        [AllowAnonymous]
        [HttpPost("add-product-to-order"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDto order)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await _orderService.AddProductToOpenOrder(User.GetUserId(), order);
                    return JsonResponseStatus.SendStatus(
                        JsonResponseStatusType.Success,
                        "محصول مورد نظر با موفقیت ثبت شد",
                        null);
                }

                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Danger,
                    "برای ثبت محصول در سبد خرید ابتدا باید وارد سایت شوید",
                    null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "در ثبت اطلاعات خطایی رخ داد", null);
        }


        #endregion

        #region Remove Product From Cart

        [HttpGet("remove-order-item/{detailId}")]
        public async Task<IActionResult> RemoveProductFromOrder(long detailId)
        {
            var result = await _orderService.RemoveOrderDetail(detailId, User.GetUserId());

            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر از سبد خرید شما حذف شد";
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "محصول مورد نظر از سبد خرید شما حذف شد",
                    null);
            }

            TempData[ErrorMessage] = "محصول مورد نظر در سبد خرید شما یافت نشد";
            return JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Danger,
                "محصول مورد نظر در سبد خرید شما یافت نشد",
                null);


        }

        #endregion

        #region Open Cart

        [HttpGet("cart/{userId}")]
        public async Task<IActionResult> UserOpenOrder(long userId)
        {
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            return View(openOrder);
        }

        #endregion

        #region Open Order Partial

        [HttpGet("change-detail-count/{detailId}/{count}")]
        public async Task<IActionResult> ChangeDetailOrderCount(long detailId, int count)
        {
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());
            await _orderService.ChangeOrderDetailCount(detailId, User.GetUserId(), count);
            return PartialView(openOrder);
        }

        #endregion

        #region Add User Address to Order

        [HttpGet("user-address/{userId}")]
        public async Task<IActionResult> AddUserAddress(long userId)
        {
            var userAddress = await _orderService.GetExistUserAddress(userId);
            var openOrder = await _orderService.GetUserOpenOrderDetail(userId);

            ViewBag.totalPriceWithoutDiscount = openOrder.GetTotalPriceWithoutDiscount();
            ViewBag.totalPriceWithDiscount = openOrder.GetTotalPriceWithDiscount();
            ViewBag.totalDiscountPrice = openOrder.GetTotalDiscountPrice();

            TotalDiscount = (int)openOrder.GetTotalDiscountPrice();

            return View(userAddress);
        }

        [HttpPost("user-address/{userId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserAddress(UserAddressDto address, long userId)
        {
            var openOrder = await _orderService.GetUserLatestOpenOrder(userId);
            address.OrderId = openOrder.Id;

            var result = await _orderService.AddUserAddress(address, userId);

            switch (result)
            {
                case AddUserAddressResult.Error:
                    TempData[ErrorMessage] = "ثبت و ذخیره اطلاعات با شکست مواجه شد";
                    break;
                case AddUserAddressResult.OrderExist:
                    TempData[ErrorMessage] = "سفارش خود را مجددا ثبت نمایید";
                    break;
                case AddUserAddressResult.Success:
                    TempData[InfoMessage] = "اطلاعات شما با موفقیت ذخیره و ثبت گردید";
                    return RedirectToAction("PayUserOrderPrice", "Order");
            }

            return View();
        }

        #endregion

       

        

        

        #region User Order List

        [HttpGet("user-order")]
        public async Task<IActionResult> GetUserOrder(FilterUserOrderDto filter)
        {
            filter.TakeEntity = 5;
            filter.UserId = User.GetUserId();
            filter.FilterUserOrderState = FilterUserOrderState.All;

            var userOrder = await _orderService.GetUserOrder(filter);

            return View(userOrder);
        }

        #endregion

        #region User Order Detail Items

        [HttpGet("user-order-detail/{orderId}")]
        public async Task<IActionResult> GetUserOrderDetailItem(long orderId)
        {
            var orderDetailItem = await _orderService.GetUserOrderDetailItem(orderId, User.GetUserId());
            return View(orderDetailItem);
        }

        #endregion

        #region User Order Address

        [HttpGet("user-order-address/{orderId}")]
        public async Task<IActionResult> UserOrderAddress(long orderId)
        {
            var userAddress = await _orderService.GetUserAddressForOrder(orderId);
            if (userAddress == null)
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            return View(userAddress);
        }

        #endregion

    }
}
