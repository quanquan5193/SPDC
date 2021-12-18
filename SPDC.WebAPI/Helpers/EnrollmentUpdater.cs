using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Service.Services;
using System;

namespace SPDC.WebAPI.Helpers
{
    public class EnrollmentUpdater
    {
        public EnrollmentUpdater()
            : this(new BackGroundService(new UnitOfWork(new DbFactory()), new ApplicationRepository(new DbFactory())))
        {
        }

        IBackGroundService _backGroundService;

        internal EnrollmentUpdater(IBackGroundService backGroundService)
        {
            _backGroundService = backGroundService;
        }

        public async void UpdateEnrollment()
        {
            try
            {
                await _backGroundService.UpdateEnrollmentStatusToCompleted();
            }
            catch (Exception ex)
            {
                Log.Error("UpdateEnrollment");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
            }
        }

    }
}