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
    public interface IBackGroundService
    {
        Task UpdateEnrollmentStatusToCompleted();
    }
    public class BackGroundService : IBackGroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationRepository _applicationRepository;
        public BackGroundService(IUnitOfWork unitOfWork, IApplicationRepository applicationRepository)
        {
            _unitOfWork = unitOfWork;
            _applicationRepository = applicationRepository;
        }


        public async Task UpdateEnrollmentStatusToCompleted()
        {
            if (_applicationRepository == null)
            {
                return;
            }
            var apps = await _applicationRepository.GetMulti(x => x.AdminAssignedClassModel.Lessons.OrderByDescending(y => y.Id).FirstOrDefault().Date < DateTime.Now
                                                        && x.EnrollmentStatusStorages.OrderByDescending(y => y.Id).FirstOrDefault().Status == (int)Common.Enums.EnrollmentStatus.Enrolled
                                                        , new string[] { "AdminAssignedClassModel.Lessons", "EnrollmentStatusStorages" });
            foreach (var app in apps)
            {
                app.EnrollmentStatusStorages.Add(new EnrollmentStatusStorage()
                {
                    ApplicationId = app.Id,
                    Status = (int)Common.Enums.EnrollmentStatus.Completed,
                    LastModifiedDate = DateTime.Now
                });
                _applicationRepository.Update(app);
            }
            _applicationRepository.Commit();
        }
    }
}
