using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Data;
using SimpleShop.Models;
using SimpleShop.Repositories.Interfaces;

namespace SimpleShop.Repositories
{
    public class ReturnRepository : IReturnRepository
    {

        private readonly AppDbContext _context;

        public ReturnRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReturnRequest request)
        {
            _context.ReturnRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReturnRequest>> GetAllAsync()
        {
            var result = await (from rr in _context.ReturnRequests
                                join o in _context.Orders on rr.OrderId equals o.Id
                                select new ReturnRequest
                                {
                                    Id = rr.Id,
                                    OrderId = rr.OrderId,
                                    Reason = rr.Reason,
                                    Status = rr.Status,
                                    RequestedAt = rr.RequestedAt,
                                    Order = o
                                }).ToListAsync();

            return result;
        }

        public async Task<ReturnRequest?> GetByIdAsync(int id)
        {
            var result = await (from rr in _context.ReturnRequests
                                join o in _context.Orders on rr.OrderId equals o.Id
                                where rr.Id == id
                                select new ReturnRequest
                                {
                                    Id = rr.Id,
                                    OrderId = rr.OrderId,
                                    Reason = rr.Reason,
                                    Status = rr.Status,
                                    RequestedAt = rr.RequestedAt,
                                    Order = o
                                }).FirstOrDefaultAsync();

            return result;
        }

        public async Task UpdateAsync(ReturnRequest request)
        {
            _context.ReturnRequests.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}