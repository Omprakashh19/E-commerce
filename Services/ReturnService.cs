using SimpleShop.DTOs;
using SimpleShop.Models;
using SimpleShop.Repositories.Interfaces;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IReturnRepository _returnRepository;
        private readonly IOrderRepository _orderRepository;

        public ReturnService(IReturnRepository returnRepository, IOrderRepository orderRepository)
        {
            _returnRepository = returnRepository;
            _orderRepository = orderRepository;
        }

        public async Task SubmitReturnRequestAsync(ReturnRequestDto dto)
        {
            var request = new ReturnRequest
            {
                OrderId = dto.OrderId,
                Reason = dto.Reason,
                Status = "Pending",
                RequestedAt = DateTime.UtcNow
            };

            await _returnRepository.AddAsync(request);
        }

        public async Task ApproveReturnRequestAsync(int id)
        {
            var request = await _returnRepository.GetByIdAsync(id);
            if (request == null) throw new Exception("Return request not found.");
            request.Status = "Approved";
            await _returnRepository.UpdateAsync(request);
        }

        public async Task RejectReturnRequestAsync(int id)
        {
            var request = await _returnRepository.GetByIdAsync(id);
            if (request == null) throw new Exception("Return request not found.");
            request.Status = "Rejected";
            await _returnRepository.UpdateAsync(request);
        }

        public async Task<List<ReturnRequest>> GetAllRequestsAsync()
        {
            return await _returnRepository.GetAllAsync();
        }
    }

}
