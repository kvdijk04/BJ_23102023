using AutoMapper;
using BJ.Contract.Translation;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface ILanguageService
    {
        Task<IEnumerable<LanguageDto>> GetLanguages();
        Task CreateLanguage(CreateLanguageDto createLanguageDto);
        Task UpdateLanguage(string id, UpdateLanguageDto updateLanguageDto);
    }
    public class LanguageService : ILanguageService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public LanguageService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public Task CreateLanguage(CreateLanguageDto createLanguageDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LanguageDto>> GetLanguages()
        {
            var language = await _context.Languages.AsNoTracking().ToListAsync();
            var languageDto = _mapper.Map<List<LanguageDto>>(language);
            return languageDto;
        }

        public Task UpdateLanguage(string id, UpdateLanguageDto updateLanguageDto)
        {
            throw new NotImplementedException();
        }
    }
}
