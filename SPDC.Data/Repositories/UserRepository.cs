using SPDC.Common;
using SPDC.Data.Infrastructure;
using SPDC.Model.Models;
using SPDC.Model.ViewModels.BatchApplication;
using SPDC.Model.ViewModels.BatchPayment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Data.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<BatchApplicationApplicant> GetAllCICNumber();

        Task<List<ApplicationUser>> GetListUsesBatcchPaymentByCICnumber(string cicNumber);
        Task<List<ApplicationUser>> GetListUsesBatcchPaymentByName(string name);

        //BatchPaymentItemViewModel GetBatchPaymentItem(int userId, bool isChineseName);
    }
    public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<BatchApplicationApplicant> GetAllCICNumber()
        {
            return dbSet.Include("Particular").Where(x => x.CICNumber != null).Select(x =>
            new BatchApplicationApplicant()
            {
                Id = x.Id,
                CICNumber = x.CICNumber,
                GivenNameCN = x.Particular.GivenNameCN,
                GivenNameEN = x.Particular.GivenNameEN,
                SurnameEN = x.Particular.SurnameEN,
                SurnameCN = x.Particular.SurnameCN
            }).ToArray();
        }

        public Task<List<ApplicationUser>> GetListUsesBatcchPaymentByCICnumber(string cicNumber)
        {
            var query = dbSet.Include("Particular");

            query = query.Where(x =>
            x.CICNumber.Contains(cicNumber) && x.Applications.Any(v =>
            v.Invoices.OrderByDescending(n => n.Id).FirstOrDefault().Status == (int)Common.Enums.InvoiceStatus.Offered &&
            v.Invoices.OrderByDescending(m => m.Id).FirstOrDefault().InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)));

            var result = query.ToListAsync();

            return result;
        }

        public Task<List<ApplicationUser>> GetListUsesBatcchPaymentByName(string name)
        {
            var query = dbSet.Include("Particular");

            query = query.Where(x =>
            x.Applications.Any(v =>
            v.Invoices.OrderByDescending(n => n.Id).FirstOrDefault().Status == (int)Common.Enums.InvoiceStatus.Offered &&
            v.Invoices.OrderByDescending(m => m.Id).FirstOrDefault().InvoiceItems.Any(c => c.InvoiceItemTypeId == (int)Common.Enums.InvoiceItemType.CourseFee)) &&
            (x.Particular.SurnameEN + x.Particular.GivenNameEN).ToLower().Contains(name) || (x.Particular.SurnameCN + x.Particular.GivenNameCN).ToLower().Contains(name)
            );

            var result = query.ToListAsync();
            return result;
        }



        //public BatchPaymentItemViewModel GetBatchPaymentItem(int userId, bool isChineseName)
        //{
        //    var query = dbSet.Include("Applications.Course").Include("Applications.Invoices").Include("Particular").Include("Course.TargetClasses");
        //    var result = query.FirstOrDefault(z => z.Id == userId && z.Applications.Any(x => x.Invoices.LastOrDefault().Status == (int)Common.Enums.InvoiceStatus.Offered))
        //        .UsersToBatchPaymentItemViewModel(isChineseName);

        //    return result;
        //}
    }
}
