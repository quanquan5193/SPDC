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
    public interface ICourseMasterDataService
    {
        Task<List<MediumOfInstruction>> GetMediumOfInstructionsAsync();
        Task<List<ProgrammeLeader>> GetProgrammeLeadersAsync();
        Task<List<LevelofApproval>> GetLevelofApprovalsAsync();
        Task<List<Lecturer>> GetLecturersAsync();
    }
    public class CourseMasterDataService : ICourseMasterDataService
    {
        IUnitOfWork _unitOfWork;
        IMediumOfInstructionRepository _mediumOfInstructionRepository;
        IProgrammeLeaderRepository _programmeLeaderRepository;
        ILecturerRepository _lecturerRepository;
        ILevelofApprovalRepository _levelofApprovalRepository;

        public CourseMasterDataService(IUnitOfWork unitOfWork,
            IMediumOfInstructionRepository mediumOfInstructionRepository,
            IProgrammeLeaderRepository programmeLeaderRepository,
            ILecturerRepository lecturerRepository,
            ILevelofApprovalRepository levelofApprovalRepository)
        {
            _unitOfWork = unitOfWork;
            _mediumOfInstructionRepository = mediumOfInstructionRepository;
            _programmeLeaderRepository = programmeLeaderRepository;
            _lecturerRepository = lecturerRepository;
            _levelofApprovalRepository = levelofApprovalRepository;
        }

        public async Task<List<Lecturer>> GetLecturersAsync()
        {
            return await _lecturerRepository.GetAll();
        }

        public async Task<List<LevelofApproval>> GetLevelofApprovalsAsync()
        {
            return await _levelofApprovalRepository.GetAll();
        }

        public async Task<List<MediumOfInstruction>> GetMediumOfInstructionsAsync()
        {
            return await _mediumOfInstructionRepository.GetAll();
        }

        public async Task<List<ProgrammeLeader>> GetProgrammeLeadersAsync()
        {
            return await _programmeLeaderRepository.GetAll();
        }
    }
}
