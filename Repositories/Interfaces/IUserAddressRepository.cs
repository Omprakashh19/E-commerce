using SimpleShop.DTOs.OrderDto;
using SimpleShop.Models;

namespace SimpleShop.Repositories.Interfaces
{
    public interface IUserAddressRepository
    {
        Task AddAddressAsync(int userId, ShippingAddressDto dto, bool isDefault = false);
        Task<List<UserShippingAddress>> GetAddressesAsync(int userId);
        Task<UserShippingAddress?> GetDefaultAddressAsync(int userId);
        Task SetDefaultAddressAsync(int userId, int addressId);
    }
}
