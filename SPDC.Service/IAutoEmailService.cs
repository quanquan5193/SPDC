using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Service
{
    public interface IAutoEmailService
    {
        //Task ReExamReminder();

        //Task ReviewExamReminder();

        Task ClassCommencementReminder();

        Task NewlyUploadCourseMaterialNotify();

        Task PaymentReminder();
    }
}
