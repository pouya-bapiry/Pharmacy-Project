
using Pharmacy.Application.DTO.ProductOrder;
using Pharmacy.Domain.Entities.ProductOrder;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IOrderService : IAsyncDisposable
    {
        #region Order

        Task<long> AddOrderForUser(long userId);
        Task<Order> GetUserLatestOpenOrder(long userId);
        Task<int> GetTotalOrderPriceForPayment(long userId);
        Task<AddUserAddressResult> AddUserAddress(UserAddressDto address, long userId);
        Task<List<UserAddress>> GetAddressToUser(long userId);
        Task<UserAddressDto> GetUserAddressForOrder(long orderId);
        Task<UserAddressDto> GetExistUserAddress(long userId);
        public Task<int> GetSuccessOrder(long userId);
        public Task<int> GetCancelOrder(long userId);
        public Task<int> GetUnderProgressOrder(long userId);
        public Task<List<FilterUserOrderDto>> GetUserOrder(FilterUserOrderDto filter);
        Task<Order> GetOrderBy(long id);


        #endregion


        #region Order Details

        Task AddProductToOpenOrder(long userId, AddProductToOrderDto order);
        Task<UserOpenOrderDto> GetUserOpenOrderDetail(long userId);
        Task<bool> RemoveOrderDetail(long detailId, long userId);
        Task ChangeOrderDetailCount(long detailId, long userId, int count);
        Task<List<UserOrderDetailItemDto>> GetUserOrderDetailItem(long orderId, long userId);
        Task<List<UserOrderDetailItemDto>> GetOrderDetailItemForBikeDelivery(long orderId);



        #endregion
    }
}
