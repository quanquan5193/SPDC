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
    public interface ICommonDataService
    {
        Task<CommonData> GetByKey(string key);
        bool Update(CommonData model);
    }

    public class CommonDataService : ICommonDataService
    {
        private ICommonDataRepository _respository;
        private IUnitOfWork _unitOfWork;

        public CommonDataService(ICommonDataRepository respository, IUnitOfWork unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommonData> GetByKey(string key)
        {
            return await _respository.GetSingleByCondition(x => x.Key.Equals(key));
        }

        public bool Update(CommonData model)
        {
            var record = _respository.GetSingleById(model.Id);

            record.ValueDouble = model.ValueDouble;
            record.ValueInt = model.ValueInt;
            record.ValueString = model.ValueString;

            _respository.Update(record);
            _unitOfWork.Commit();
            return true;
        }
    }
}
