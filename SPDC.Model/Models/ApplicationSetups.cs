using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("ApplicationSetups")]
    public class ApplicationSetups
    {
        public ApplicationSetups()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public bool ShowEducationLevel { get; set; }

        //public int RelevantMembershipId { get; set; }

        //public int RelevantWorkId { get; set; }

        public bool ShowRecommendation { get; set; }

        public bool ShowDocument { get; set; }

        public bool ShowCitf { get; set; }

        public bool ShowReceipt { get; set; }

        public string FundingSchema { get; set; }


        public virtual Course Course { get; set; }

        public virtual RelevantMemberships RelevantMembership { get; set; }

        public virtual RelevantWorks RelevantWork { get; set; }
    }
}
