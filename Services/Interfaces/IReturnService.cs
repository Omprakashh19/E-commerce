using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleShop.DTOs;
using SimpleShop.Models;

namespace SimpleShop.Services.Interfaces
{
   public interface IReturnService
{
    Task SubmitReturnRequestAsync(ReturnRequestDto dto);
    Task ApproveReturnRequestAsync(int id);
    Task RejectReturnRequestAsync(int id);
    Task<List<ReturnRequest>> GetAllRequestsAsync();
}

}