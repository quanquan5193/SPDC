using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service.Services
{
    public interface IClientService
    {
        Task<string> GetClientUrlByNameAsync(string name);
    }
    public class ClientService : IClientService
    {
        IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<string> GetClientUrlByNameAsync(string name)
        {
            return (await _clientRepository.GetSingleByCondition(x => x.ClientName.Equals(name))).ClientUrl;
        }
    }
}
