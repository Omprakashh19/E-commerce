using SimpleShop.Models;
using SimpleShop.Data;
using SimpleShop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SimpleShop.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly AppDbContext _context;

        public PasswordResetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordReset reset)
        {
            _context.PasswordResets.Add(reset);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordReset?> GetByTokenAsync(string token)
        {
            return await _context.PasswordResets.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<PasswordReset?> GetByEmailAsync(string email)
        {
            return await _context.PasswordResets.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task DeleteAsync(PasswordReset reset)
        {
            _context.PasswordResets.Remove(reset);
            await _context.SaveChangesAsync();
        }
    }
}
