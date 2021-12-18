using SPDC.Data.Repositories;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{
    public interface IVersionService
    {
        Task<VersionManagement> GetVersion(string type);
    }
    public class VersionService : IVersionService
    {
        private IVersionRepository _versionRepository;
        public VersionService(IVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        public async Task<VersionManagement> GetVersion(string type)
        {
            return (await _versionRepository.GetMulti(x => x.Type.Equals(type))).LastOrDefault();
        }
    }
}
