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
            CreateMap<ChangePassword, Account>().ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName))
                                                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                                                 .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.DateUpdate))
                                                .ForMember(dest => dest.HasedPassword, opt => opt.MapFrom(src => src.HasedNewPassword));

        }
    }
}
