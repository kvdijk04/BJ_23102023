using AutoMapper;
using BJ.Contract.Account;
using BJ.Domain.Entities;


namespace BJ.Application.Mapping
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<CreateAccountDto, Account>();


            CreateMap<UpdateAccountDto, Account>();
        }
    }
}
