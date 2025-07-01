using Microsoft.AspNetCore.Mvc;
using SimpleShop.DTOs;
using SimpleShop.Models;
using SimpleShop.Services.Interfaces;

namespace SimpleShop.Controllers
{
    [ApiController]
    [Route("api/returns")]
    public class ReturnController : ControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestReturn([FromBody] ReturnRequestDto dto)
        {
            await _returnService.SubmitReturnRequestAsync(dto);
            return Ok(new { message = "Return request submitted successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _returnService.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            await _returnService.ApproveReturnRequestAsync(id);
            return Ok(new { message = "Return request approved." });
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            await _returnService.RejectReturnRequestAsync(id);
            return Ok(new { message = "Return request rejected." });
        }
    }

}
