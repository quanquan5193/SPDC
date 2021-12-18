using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
    }

    public class UserDataViewModel
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
