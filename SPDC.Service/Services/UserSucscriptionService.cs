using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Model.BindingModels;
using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SPDC.Service.Services
{
    public interface IUserSucscriptionService
    {
        Task<bool> CreateSubscription(UserSubscriptionBindingModel model);
        bool UnSubcribeWebsite(int id);
    }
    public class UserSucscriptionService : IUserSucscriptionService
    {
        IUnitOfWork _unitOfWork;
        IUserSubscriptionRepository _subscription;
        public UserSucscriptionService(IUnitOfWork unitOfWork, IUserSubscriptionRepository subscription)
        {
            _subscription = subscription;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateSubscription(UserSubscriptionBindingModel model)
        {
            var dataGet = await _subscription.GetSingleByCondition(x => x.Email.Equals(model.Email));
            var javaScriptSerializer = new JavaScriptSerializer();

            var listCourseSub = new List<int>();

            if (dataGet != null)
            {
                return false;
                //listCourseSub = javaScriptSerializer.Deserialize<List<int>>(dataGet.InterestedTypeOfCourse);
                //listCourseSub.AddRange(model.InterestedTypeOfCourse);
                //var newListCourseSub = listCourseSub.Distinct().ToList();

                //dataGet.InterestedTypeOfCourse = javaScriptSerializer.Serialize(newListCourseSub);
                //dataGet.Company = model.Company;
                //dataGet.Honorific = model.Honorific;
                //dataGet.FirstNameEN = model.FirstNameEN;
                //dataGet.LastNameEN = model.LastNameEN;
                //dataGet.FirstNameCN = model.FirstNameCN;
                //dataGet.LastNameCN = model.LastNameCN;
                //dataGet.PrefixMobilePhone = model.PrefixMobilePhone;
                //dataGet.MobilePhone = model.MobilePhone;
                //dataGet.Position = model.Position;
                //dataGet.Email = model.Email;
                //dataGet.CommunicationLanguage = model.CommunicationLanguage;
                //dataGet.IsSubscribe = true;

                //_subscription.Update(dataGet);
            }
            else
            {
                var subscription = new UserSubscription();
                subscription.Company = model.Company;
                subscription.Honorific = model.Honorific;
                subscription.FirstNameEN = model.FirstNameEN;
                subscription.LastNameEN = model.LastNameEN;
                subscription.FirstNameCN = model.FirstNameCN;
                subscription.LastNameCN = model.LastNameCN;
                subscription.PrefixMobilePhone = model.PrefixMobilePhone;
                subscription.MobilePhone = model.MobilePhone;
                subscription.Position = model.Position;
                subscription.Email = model.Email;
                subscription.InterestedTypeOfCourse = model.InterestedTypeOfCourse != null ? javaScriptSerializer.Serialize(model.InterestedTypeOfCourse) : "[]";
                subscription.CommunicationLanguage = model.CommunicationLanguage;
                subscription.IsSubscribe = true;

                _subscription.Add(subscription);
            }

            _unitOfWork.Commit();
            return true;
        }

        public bool UnSubcribeWebsite(int id)
        {
            var dataGet = _subscription.GetSingleById(id);

            if(dataGet != null)
            {
                dataGet.IsSubscribe = false;
                _subscription.Update(dataGet);
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
