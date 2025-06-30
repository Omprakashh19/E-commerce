using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.DTOs.OrderDto;
using SimpleShop.Models;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly AppDbContext _context;

        public UserAddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAddressAsync(int userId, ShippingAddressDto dto, bool isDefault = false)
        {
            if (isDefault)
            {
                var existingDefaults = await _context.UserShippingAddresses
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingDefaults)
                    addr.IsDefault = false;
            }

            var address = new UserShippingAddress
            {
                UserId = userId,
                FullName = dto.FullName,
                AddressLine = dto.AddressLine,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                IsDefault = isDefault
            };

            _context.UserShippingAddresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserShippingAddress>> GetAddressesAsync(int userId)
        {
            return await _context.UserShippingAddresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ToListAsync();
        }

        public async Task<UserShippingAddress?> GetDefaultAddressAsync(int userId)
        {
            return await _context.UserShippingAddresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }

        public async Task SetDefaultAddressAsync(int userId, int addressId)
        {
            var addresses = await _context.UserShippingAddresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            foreach (var addr in addresses)
                addr.IsDefault = addr.Id == addressId;

            await _context.SaveChangesAsync();
        }
    }
}
