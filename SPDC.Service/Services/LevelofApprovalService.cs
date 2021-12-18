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
    public interface ILevelofApprovalService
    {
        Task<List<LevelofApproval>> GetAll();
    }
    public class LevelofApprovalService : ILevelofApprovalService
    {
        IUnitOfWork _unitOfWork;
        ILevelofApprovalRepository _levelofApprovalRepository;

        public LevelofApprovalService(IUnitOfWork unitOfWork, ILevelofApprovalRepository levelofApprovalRepository)
        {
            _unitOfWork = unitOfWork;
            _levelofApprovalRepository = levelofApprovalRepository;
        }

        public Task<List<LevelofApproval>> GetAll()
        {
            return _levelofApprovalRepository.GetAll();
        }
    }
}