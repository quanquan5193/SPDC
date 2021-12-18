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
    [Table("RelevantMemberships")]

    public class RelevantMemberships
    {
        public RelevantMemberships()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        //public int ApplicationSetupId { get; set; }

        public bool ShowMembershipTable { get; set; }

        public bool ShowTwoYears { get; set; }

        public bool ShowKnowledge { get; set; }

        public bool ShowBimBasic { get; set; }

        public virtual ApplicationSetups ApplicationSetup { get; set; }
    }
}
