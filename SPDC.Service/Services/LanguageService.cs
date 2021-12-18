using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using SPDC.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetLanguages();
        Task<Language> GetLanguageByCode(string code);
        Language GetLanguageById(int id);
    }
    public class LanguageService : ILanguageService
    {
        ILanguageRepository _repository;
        IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork, ILanguageRepository repository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Language>> GetLanguages()
        {
            return await _repository.GetAll();
        }

        public async Task<Language> GetLanguageByCode(string code)
        {
            var lang = await _repository.GetSingleByCondition(x => x.Code == code);
            return lang;
        }

        public Language GetLanguageById(int id)
        {
            return _repository.GetSingleById(id);
        }
    }
}
