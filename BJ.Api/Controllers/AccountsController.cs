using BJ.Application.Service;
using BJ.Contract.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BJ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;
        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        [Authorize]
        [HttpGet]

        public async Task<IEnumerable<AccountDto>> GetSizes()
        {

            return await _accountService.GetAccounts();

        }
        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>

        [HttpGet("{id}")]

        public async Task<IActionResult> GetAccountById(Guid id)
        {
            if (await _accountService.GetAccountById(id) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(await _accountService.GetAccountById(id));

        }
        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Account(Guid id, [FromBody] UpdateAccountDto updateAccountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();

                }
                if (await _accountService.GetAccountById(id) == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                await _accountService.UpdateAccount(id, updateAccountDto);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Tạo mới tài khoản
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto createAccountDto)
        {
            try
            {
                await _accountService.CreateAccount(createAccountDto);


                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        public async Task<string> Login(LoginDto loginDto)
        {
            try
            {
                var a = await _accountService.Login(loginDto);
                return a;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
