using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Paging;
using Pharmacy.Application.DTO.ProductOrder;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Product;
using Pharmacy.Domain.Entities.ProductOrder;
using Pharmacy.Domain.IRepository;
using Pharmacy.Application.Utilities;



namespace Pharmacy.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        #region Constructor

        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<ProductDiscount> _productDiscountRepository;
        private readonly IGenericRepository<ProductDiscountUse> _productDiscountUseRepository;
        private readonly IGenericRepository<UserAddress> _userAddressRepository;
        private readonly IGenericRepository<Product> _productRepository;





        public OrderService(IGenericRepository<Order> orderRepository, IGenericRepository<OrderDetail> orderDetailRepository,
            IGenericRepository<ProductDiscount> productDiscountRepository, IGenericRepository<ProductDiscountUse> productDiscountUseRepository,
            IGenericRepository<Product> productRepository, IGenericRepository<UserAddress> userAddressRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productDiscountRepository = productDiscountRepository;
            _productDiscountUseRepository = productDiscountUseRepository;
            _productRepository = productRepository;
            _userAddressRepository = userAddressRepository;

        }

        #endregion


        #region Order

        public async Task<long> AddOrderForUser(long userId)
        {
            var order = new Order { UserId = userId, OrderAcceptanceState = OrderAcceptanceState.UnderProgress };

            await _orderRepository.AddEntity(order);
            await _orderRepository.SaveChanges();

            return order.Id;

        }
        public async Task<Order> GetUserLatestOpenOrder(long userId)
        {
            if (!await _orderRepository.GetQuery().AnyAsync(x => x.UserId == userId && !x.IsPaid))
            {
                await AddOrderForUser(userId);
            }

            var userOpenOrder = await _orderRepository
                .GetQuery()
                .AsQueryable()

                .Include(x => x.OrderDetails)

                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductDiscounts)

                .Include(x => x.OrderDetails)

                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)

                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)

                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)


                .FirstOrDefaultAsync(x => x.UserId == userId && !x.IsPaid);


            return userOpenOrder;
        }
        public async Task<int> GetTotalOrderPriceForPayment(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);
            var totalPrice = 0;
            var discount = 0;

            foreach (var detail in userOpenOrder.OrderDetails.Where(x => !x.IsDelete))
            {
                var oneProductPrice = CalculateProductPrice(detail.Product);

                var productDiscount = await _productDiscountRepository.GetQuery()
                    .Include(x => x.ProductDiscountUse)
                    .OrderByDescending(x => x.CreateDate)
                    .FirstOrDefaultAsync(x =>
                        x.ProductId == detail.ProductId && x.ExpireDate >= DateTime.Now);

                if (productDiscount != null)
                {
                    discount = (int)Math.Ceiling(((oneProductPrice) * productDiscount.Percentage) / (decimal)100);
                }


                totalPrice += detail.Count * (oneProductPrice - discount);

                discount = 0;
            }


            return totalPrice;
        }
        public async Task<AddUserAddressResult> AddUserAddress(UserAddressDto address, long userId)
        {
            var similarUserAddress = await _userAddressRepository.GetQuery()
                .SingleOrDefaultAsync(x => x.OrderId == address.OrderId);

            if (similarUserAddress == null)
            {
                var addUserAddress = new UserAddress
                {
                    UserId = userId,
                    OrderId = address.OrderId,
                    Name = address.Name,
                    Family = address.Family,
                    State = address.State,
                    City = address.City,
                    Street = address.Street,
                    PostalCode = address.PostalCode != null ? address.PostalCode : null,
                    PlaqueNo = address.PlaqueNo != null ? address.PlaqueNo : null,
                    Company = address?.Company != null ? address.Company : null,
                    Mobile = address.Mobile,
                    Email = address.Email != null ? address.Email : "---",
                    Description = address?.Description != null ? address.Description : null,

                };

                await _userAddressRepository.AddEntity(addUserAddress);
                await _userAddressRepository.SaveChanges();

                return AddUserAddressResult.Success;
            }

            var userAddressDto = new UserAddressDto();
            return AddUserAddressResult.Success;

        }
        public async Task<List<UserAddress>> GetAddressToUser(long userId)
        {
            var user = await _userAddressRepository
                .GetQuery()
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .Take(3)
                .ToListAsync();

            return user;
        }
        public async Task<UserAddressDto> GetUserAddressForOrder(long orderId)
        {
            var userAddress = await _userAddressRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Order)
                .Select(x => new UserAddressDto
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    Name = x.Name,
                    Family = x.Family,
                    Company = x.Company,
                    State = x.State,
                    City = x.City,
                    Street = x.Street,
                    Mobile = x.Mobile,
                    Email = x.Email,
                    PostalCode = x.PostalCode,
                    PlaqueNo = x.PlaqueNo,
                    Description = x.Description,
                    Order = x.Order,
                })
                .SingleOrDefaultAsync(x => x.OrderId == orderId);

            return userAddress;
        }
        public async Task<UserAddressDto> GetAddressForBikeDeliveryOrder(long orderId)
        {
            var userAddress = await _userAddressRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.Order)
                .Select(x => new UserAddressDto
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    Name = x.Name,
                    Family = x.Family,
                    Company = x.Company,
                    State = x.State,
                    City = x.City,
                    Street = x.Street,
                    Mobile = x.Mobile,
                    Email = x.Email,
                    PostalCode = x.PostalCode,
                    PlaqueNo = x.PlaqueNo,
                    Description = x.Description,
                    Order = x.Order,
                })
                .SingleOrDefaultAsync(x => x.OrderId == orderId);

            if (userAddress == null)
            {
                return new UserAddressDto();
            }

            return userAddress;
        }
        public async Task<UserAddressDto> GetExistUserAddress(long userId)
        {
            var userAddress = await _userAddressRepository
                .GetQuery()
                .AsQueryable()
                .OrderBy(x => x.CreateDate)
                .LastOrDefaultAsync(x => x.UserId == userId);

            if (userAddress == null)
            {
                return null;
            }

            return new UserAddressDto
            {
                UserId = userId,
                Name = userAddress.Name,
                Family = userAddress.Family,
                Company = userAddress.Company,
                State = userAddress.State,
                City = userAddress.City,
                Street = userAddress.Street,
                PostalCode = userAddress.PostalCode,
                PlaqueNo = userAddress.PlaqueNo,
                Mobile = userAddress.Mobile,
                Email = userAddress.Email,

            };
        }
        public async Task<int> GetSuccessOrder(long userId)
        {
            var order = await _orderRepository.GetQuery()
                .AsQueryable()
                .Where(x => x.UserId == userId && x.IsPaid && x.OrderAcceptanceState == OrderAcceptanceState.PaymentSuccessful)
                .ToListAsync();

            return order.Count;
        }
        public async Task<int> GetCancelOrder(long userId)
        {
            var order = await _orderRepository.GetQuery()
                .AsQueryable()
                .Where(x => x.UserId == userId && !x.IsPaid && x.OrderAcceptanceState == OrderAcceptanceState.PaymentCancel)
                .ToListAsync();

            return order.Count;
        }
        public async Task<int> GetUnderProgressOrder(long userId)
        {
            var order = await _orderRepository.GetQuery()
                .AsQueryable()
                .Where(x => x.UserId == userId && x.IsPaid == false && x.TrackingCode == null && x.OrderAcceptanceState == OrderAcceptanceState.UnderProgress)
                .ToListAsync();

            return order.Count;
        }
        public async Task<List<FilterUserOrderDto>> GetUserOrder(FilterUserOrderDto filter)
        {
            var order = await _orderRepository
                .GetQuery()
                .Include(x => x.OrderDetails)
                .Include(x => x.UserAddress)
                .Where(x =>
                    x.UserId == filter.UserId &&
                    x.TrackingCode != null &&
                    !x.IsDelete)
                .OrderByDescending(x => x.Id)
                .Select(x => new FilterUserOrderDto
                {
                    Id=filter.Id,
                  TrackingCode = x.TrackingCode,
                  PaymentDate= x.PaymentDate,
                  OrderAmount=x.OrderAmount,
                  OrderAcceptanceState=filter.OrderAcceptanceState,
                  
                })
                .OrderByDescending(x => x.PaymentDate)
                .ToListAsync();

            return order;
        
            //#region State

            //switch (filter.FilterUserOrderState)
            //{
            //    case FilterUserOrderState.All:
            //        query = query.Where(x => !x.IsDelete);
            //        break;
            //    case FilterUserOrderState.PaymentSuccessful:
            //        query = query.Where(x => x.OrderAcceptanceState == OrderAcceptanceState.PaymentSuccessful && !x.IsDelete);
            //        break;
            //    case FilterUserOrderState.PaymentNotSuccessful:
            //        query = query.Where(x => x.OrderAcceptanceState == OrderAcceptanceState.PaymentNotSuccessful && !x.IsPaid && !x.IsDelete);
            //        break;
            //    case FilterUserOrderState.PaymentCancel:
            //        query = query.Where(x => x.OrderAcceptanceState == OrderAcceptanceState.PaymentCancel && x.IsPaid && !x.IsDelete);
            //        break;
            //    case FilterUserOrderState.UnderProgress:
            //        query = query.Where(x => x.OrderAcceptanceState == OrderAcceptanceState.UnderProgress && !x.IsDelete);
            //        break;
            //}

            //switch (filter.FilterPaymentMethod)
            //{
            //    case FilterPaymentMethod.All:
            //        query = query.Where(x => !x.IsDelete);
            //        break;

            //}

            //switch (filter.FilterOrderPeriodTime)
            //{
            //    case FilterOrderPeriodTime.All:
            //        query = query.Where(x => !x.IsDelete);
            //        break;

            //}

            //switch (filter.FilterOrderDelivered)
            //{
            //    case FilterOrderDelivered.All:
            //        query = query.Where(x => !x.IsDelete);
            //        break;

            //}

            //#endregion

            //#region Filter

            //if (filter.UserId != null && filter.UserId != 0)
            //{
            //    query = query.Where(x => x.UserId == filter.UserId);
            //}

            //if (!string.IsNullOrEmpty(filter.TrackingCode))
            //{
            //    query = query.Where(x => EF.Functions.Like(x.TrackingCode, $"%{filter.TrackingCode}%"));
            //}

          
          
            //#region Paging


            //var orderCount = await query.CountAsync();

            //var pager = Pager.Build(filter.PageId, orderCount, filter.TakeEntity,
            //    filter.HowManyShowPageAfterAndBefore);

            //var allEntities = await query.Paging(pager).ToListAsync();


            //#endregion

            //return filter.SetPaging(pager).SetUserOrders(allEntities);
        }
        public async Task<Order> GetOrderBy(long id)
        {
            var order = await _orderRepository
                .GetQuery()
                .AsQueryable()
                .Include(x => x.UserAddress)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return new Order();
            }

            return order;
        }

#endregion

        #region Order Detail

        public async Task AddProductToOpenOrder(long userId, AddProductToOrderDto order)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);

            var similarOrder = openOrder.OrderDetails.SingleOrDefault(x =>
                x.ProductId == order.ProductId && !x.IsDelete);

            if (similarOrder == null)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = openOrder.Id,
                    ProductId = order.ProductId,
                    Count = order.Count,
                };

                await _orderDetailRepository.AddEntity(orderDetail);
                await _orderDetailRepository.SaveChanges();
            }
            else
            {
                similarOrder.Count += order.Count;

                _orderDetailRepository.EditEntity(similarOrder);
                await _orderDetailRepository.SaveChanges();
            }


        }
        public async Task<UserOpenOrderDto> GetUserOpenOrderDetail(long userId)
        {
            var userOpenOrder = await GetUserLatestOpenOrder(userId);

            var cart = new UserOpenOrderDto
            {
                UserId = userId,
                Description = userOpenOrder.Description,
                Details = userOpenOrder.OrderDetails
                    .Where(x => !x.IsDelete)
                    .Select(x => new UserOpenOrderDetailItemDto
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Count = x.Count,
                        ProductTitle = x.Product.Title,
                        ProductCode = x.Product.Code,
                        ProductPrice = CalculateProductPrice(x.Product),
                        ProductImage = x.Product.Image,
                        DiscountExpireDate = x.Product.ProductDiscounts
                            .OrderByDescending(p => p.CreateDate)
                            .FirstOrDefault()?.ExpireDate,
                        DiscountPercentage = x.Product.ProductDiscounts
                        .OrderByDescending(p => p.CreateDate)
                        .FirstOrDefault(p => p.ExpireDate >= DateTime.Now)?.Percentage
                    })
                    .ToList()
            };


            return cart;
        }
        public async Task<bool> RemoveOrderDetail(long detailId, long userId)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var orderDetail = openOrder.OrderDetails.SingleOrDefault(x => x.Id == detailId);

            if (orderDetail == null)
            {
                return false;
            }

            _orderDetailRepository.DeleteEntity(orderDetail);
            await _orderDetailRepository.SaveChanges();

            return true;
        }
        public async Task ChangeOrderDetailCount(long detailId, long userId, int count)
        {
            var openOrder = await GetUserLatestOpenOrder(userId);
            var detail = openOrder.OrderDetails.SingleOrDefault(x => x.Id == detailId);

            if (detail != null)
            {
                if (count > 0)
                {
                    detail.Count = count;
                }
                else
                {
                    _orderDetailRepository.DeleteEntity(detail);
                }

                _orderDetailRepository.EditEntity(detail);
                await _orderDetailRepository.SaveChanges();
            }
        }
        public async Task<List<UserOrderDetailItemDto>> GetUserOrderDetailItem(long orderId, long userId)
        {
            var order = await _orderRepository
                .GetQuery()
                .AsQueryable()

                .Include(x=>x.UserAddress)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)

                .FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);

            if (order == null)
            {
                return null;
            }

            var discount = await _productDiscountRepository
                .GetQuery()
                .AsQueryable()
                .Select(x => new { x.ProductId, x.Percentage, x.ExpireDate }).ToListAsync();

            var items = order.OrderDetails.Where(x => !x.IsDelete).Select(x => new UserOrderDetailItemDto
            {
                OrderId = x.Id,
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                ProductCode = x.Product.Code,
                Count = x.Count,
                ProductPrice = x.ProductPrice,
                OriginalProductPrice = CalculateProductPrice(x.Product),
                MainProductPrice = CalculateProductPrice(x.Product) * x.Count,
                ProductImage = x.Product.Image
            }).ToList();



            foreach (var item in items)
            {
                item.DiscountPercentage = discount
                        .FirstOrDefault(x => x.ProductId == item.ProductId && x.ExpireDate >= DateTime.Now)?.Percentage;

                item.DiscountPrice = item.MainProductPrice - item.ProductPrice;

            }

            return items;

        }

        public async Task<List<UserOrderDetailItemDto>> GetOrderDetailItemForBikeDelivery(long orderId)
        {
            var order = await _orderRepository
                .GetQuery()
                .AsQueryable()



                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)



                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
            {
                return null;
            }

            var discount = await _productDiscountRepository
                .GetQuery()
                .AsQueryable()
                .Select(x => new { x.ProductId, x.Percentage, x.ExpireDate }).ToListAsync();

            var items = order.OrderDetails.Where(x => !x.IsDelete).Select(x => new UserOrderDetailItemDto
            {
                OrderId = x.Id,
                ProductId = x.ProductId,
                ProductTitle = x.Product.Title,
                ProductCode = x.Product.Code,

                Count = x.Count,
                ProductPrice = x.ProductPrice,
                OriginalProductPrice = CalculateProductPrice(x.Product),
                MainProductPrice = CalculateProductPrice(x.Product) * x.Count,
                ProductImage = x.Product.Image
            }).ToList();



            foreach (var item in items)
            {
                item.DiscountPercentage = discount
                        .FirstOrDefault(x => x.ProductId == item.ProductId && x.ExpireDate >= DateTime.Now)?.Percentage;

                item.DiscountPrice = item.MainProductPrice - item.ProductPrice;

            }

            return items;
        }

        #endregion

        #region Choose Product Price based on Size, Selected and Main Price

        private static int CalculateProductPrice(Product product)
        {
            // Initialize the product price with the main price
            var productPrice = product.Price;



            return productPrice;
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_orderRepository != null)
            {
                await _orderRepository.DisposeAsync();
            }
            if (_orderDetailRepository != null)
            {
                await _orderDetailRepository.DisposeAsync();
            }
            if (_productRepository != null)
            {
                await _productRepository.DisposeAsync();
            }
            if (_productDiscountRepository != null)
            {
                await _productDiscountRepository.DisposeAsync();
            }
            if (_productDiscountUseRepository != null)
            {
                await _orderDetailRepository.DisposeAsync();
            }
            if (_userAddressRepository != null)
            {
                await _userAddressRepository.DisposeAsync();
            }

        }

        #endregion
    }
}
