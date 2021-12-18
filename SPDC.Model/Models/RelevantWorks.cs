using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SPDC.Model.Models
{
    [Table("RelevantWorks")]
    public class RelevantWorks
    {
        public RelevantWorks()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //public int ApplicationSetupId { get; set; }

        public bool ShowTwoYears { get; set; }

        public bool ShowThreeYears { get; set; }

        public bool ShowFourYears { get; set; }

        public bool ShowFiveYears { get; set; }

        public bool ShowThreeYearsLeak { get; set; }

        public bool ShowWorkingExperience { get; set; }

        public bool ShowLetterTemplate { get; set; }

        public string Specify { get; set; }


        public virtual ApplicationSetups ApplicationSetup { get; set; }

    }
}
