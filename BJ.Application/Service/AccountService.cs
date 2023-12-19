using AutoMapper;
using BJ.Application.Ultities;
using BJ.Contract.Account;
using BJ.Contract.Config;
using BJ.Domain.Entities;
using BJ.Persistence.ApplicationContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BJ.Application.Service
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAccounts();
        Task CreateAccount(CreateAccountDto createAccountDto);
        Task UpdateAccount(Guid id, UpdateAccountDto updateAccountDto);
        Task<AccountDto> GetAccountById(Guid id);
        Task<AccountDto> GetAccountByEmail(string email);

        Task<PagedViewModel<AccountDto>> GetPaging(GetListPagingRequest getListPagingRequest);
        Task<string> Login(LoginDto loginDto);

    }
    public class AccountService : IAccountService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AppSetting _appSettings;


        public AccountService(BJContext context, IMapper mapper, IConfiguration configuration, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _appSettings = optionsMonitor.CurrentValue;

        }

        public async Task CreateAccount(CreateAccountDto createAccountDto)
        {
            createAccountDto.Id = Guid.NewGuid();

            createAccountDto.CreatedDate = DateTime.Now;

            createAccountDto.ModifiedDate = DateTime.Now;

            createAccountDto.HasedPassword = Password.HashedPassword(createAccountDto.Password);
            if (createAccountDto.Role == 1)
            {
                createAccountDto.AuthorizeRole = Contract.AuthorizeRole.AdminRole;
            }
            if (createAccountDto.Role == 2)
            {
                createAccountDto.AuthorizeRole = Contract.AuthorizeRole.MarketingRole;
            }
            Account account = _mapper.Map<Account>(createAccountDto);

            _context.Add(account);
            await _context.SaveChangesAsync();

        }

        public async Task<PagedViewModel<AccountDto>> GetPaging([FromQuery] GetListPagingRequest getListPagingRequest)
        {
            if (getListPagingRequest.PageSize == 0)
            {
                getListPagingRequest.PageSize = Convert.ToInt32(_configuration.GetValue<float>("PageSize:Account"));
            }
            var pageResult = getListPagingRequest.PageSize;
            var pageCount = Math.Ceiling(_context.Accounts.Count() / (double)pageResult);
            var query = _context.Accounts.OrderBy(x => x.CreatedDate).AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(getListPagingRequest.Keyword))
            {
                query = query.Where(x => x.EmployeeName.Contains(getListPagingRequest.Keyword));
                pageCount = Math.Ceiling(query.Count() / (double)pageResult);
            }


            var totalRow = await query.CountAsync();
            var data = await query.Skip((getListPagingRequest.PageIndex - 1) * pageResult)
                                    .Take(pageResult)
                                    .Select(x => new AccountDto()
                                    {
                                        Id = x.Id,
                                        UserName = x.UserName,
                                        EmployeeName = x.EmployeeName,
                                        Active = x.Active,
                                        LastLogin = x.LastLogin,
                                    }).ToListAsync();
            var subCategoryResponse = new PagedViewModel<AccountDto>
            {
                Items = data,
                PageIndex = getListPagingRequest.PageIndex,
                PageSize = getListPagingRequest.PageSize,
                TotalRecord = (int)pageCount,
            };
            return subCategoryResponse;
        }


        public async Task<AccountDto> GetAccountById(Guid id)
        {
            var item = await _context.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item == null) return null;
            return _mapper.Map<AccountDto>(item);

        }

        public async Task<IEnumerable<AccountDto>> GetAccounts()
        {
            var account = await _context.Accounts.OrderByDescending(x => x.CreatedDate).AsNoTracking().ToListAsync();
            var accountDto = _mapper.Map<List<AccountDto>>(account);
            return accountDto;
        }

        public async Task UpdateAccount(Guid id, UpdateAccountDto updateAccountDto)
        {
            var item = await _context.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (item != null)
            {
                if(updateAccountDto.Password != null)
                {
                    updateAccountDto.HasedPassword = Password.HashedPassword(updateAccountDto.Password);
                }
                else
                {
                    updateAccountDto.HasedPassword = item.HasedPassword;

                }
                updateAccountDto.ModifiedDate = DateTime.Now;

                if (updateAccountDto.Role == 1)
                {
                    updateAccountDto.AuthorizeRole = Contract.AuthorizeRole.AdminRole;
                }
                if (updateAccountDto.Role == 2)
                {
                    updateAccountDto.AuthorizeRole = Contract.AuthorizeRole.MarketingRole;
                }

                _context.Update(_mapper.Map(updateAccountDto, item));

                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            string password = Password.HashedPassword(loginDto.Password);

            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName && password == x.HasedPassword);

            if (account != null && account.Active == true)
            {

                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);


                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, account.UserName),
                        new Claim(ClaimTypes.Role, account.AuthorizeRole.ToString()),
                        new Claim(ClaimTypes.Name, account.UserName),
                    }),

                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)

                };

                account.LastLogin = DateTime.Now;

                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();

                var token = jwtTokenHandler.CreateToken(tokenDescription);

                //var accessToken = jwtTokenHandler.WriteToken(token);

                //var refreshToken =  GenerateRefreshToken();

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return null;
            }
        }

        public async Task<AccountDto> GetAccountByEmail(string email)
        {
            var item = await _context.Accounts.FirstOrDefaultAsync(x => x.UserName.Equals(email));
            if (item == null) return null;
            return _mapper.Map<AccountDto>(item);
        }
    }
}
