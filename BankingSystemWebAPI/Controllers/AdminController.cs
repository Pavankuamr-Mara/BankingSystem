using Microsoft.AspNetCore.Mvc;
using Infrastructure.Dtos;
using System.Web.Http.Description;
using Infrastructure.Services.Interfaces;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;

        [HttpGet]
        [Route("ViewAccounts")]
        [ResponseType(typeof(List<UserResponseDto>))]
        public async Task<IActionResult> GetUserAccounts()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("AddAccounts")]
        [ResponseType(typeof(AccountOverviewDto))]
        public async Task<IActionResult> AddAccount(AddAccountRequestDto requestDto)
        {
            var addedAccount = await _adminService.AddAccountAsync(requestDto);
            return Accepted(addedAccount);
        }

        [HttpDelete]
        [Route("RemoveAccount/{accountNumber:guid}")]
        [ResponseType(typeof(string))]
        public async Task<IActionResult> RemoveAccount(Guid accountNumber)
        {
            await _adminService.RemoveAccountAsync(accountNumber);
            return Ok("Account deleted successfully!");
        }
    }
}
