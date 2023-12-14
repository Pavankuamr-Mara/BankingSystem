using Infrastructure.Dtos;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankingController(IBankingService bankingService) : ControllerBase
    {
        private readonly IBankingService _bankingService = bankingService;

        [HttpGet]
        [Route("ViewBalance/{accountNumber:guid}")]
        public async Task<IActionResult> GetBalance(Guid accountNumber)
        {
            return Ok(await _bankingService.GetBalanceAsync(accountNumber));
        }

        [HttpPost]
        [Route("DepositMoney")]
        public async Task<IActionResult> AddMoney(AddMoneyRequestDto requestDto)
        {
            await _bankingService.AddMoneyAsync(requestDto);
            return Ok();
        }

        [HttpPost]
        [Route("WithdrawMoney")]
        public async Task<IActionResult> RemoveMoney(RemoveMoneyRequestDto requestDto)
        {
            await _bankingService.RemoveMoneyAsync(requestDto);
            return Ok();
        }
    }
}
