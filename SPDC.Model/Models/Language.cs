using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Languages")]
    public class Language
    {
        public Language()
        {
            ApplicationTrans = new HashSet<ApplicationTran>();
            CourseCategorieTrans = new HashSet<CourseCategorieTran>();
            CourseLocationTrans = new HashSet<CourseLocationTran>();
            CourseTrans = new HashSet<CourseTran>();
            EmployerRecommendationTrans = new HashSet<EmployerRecommendationTran>();
            InvoiceItemTrans = new HashSet<InvoiceItemTran>();
            //InvoiceTypes = new HashSet<InvoiceType>();
            ParticularTrans = new HashSet<ParticularTran>();
            //PaymentTransactionTrans = new HashSet<PaymentTransactionTran>();
            QualificationsTrans = new HashSet<QualificationsTran>();
            //TransactionTypes = new HashSet<TransactionType>();
            Users = new HashSet<ApplicationUser>();
            WorkExperienceTrans = new HashSet<WorkExperienceTran>();
            ModuleTrans = new HashSet<ModuleTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        public virtual ICollection<ApplicationTran> ApplicationTrans { get; set; }
                
        public virtual ICollection<CourseCategorieTran> CourseCategorieTrans { get; set; }
                
        public virtual ICollection<CourseLocationTran> CourseLocationTrans { get; set; }
                       
        public virtual ICollection<CourseTran> CourseTrans { get; set; }       
                
        public virtual ICollection<EmployerRecommendationTran> EmployerRecommendationTrans { get; set; }
                                
        public virtual ICollection<InvoiceItemTran> InvoiceItemTrans { get; set; }
                
        //public virtual ICollection<InvoiceType> InvoiceTypes { get; set; }
                        
        public virtual ICollection<ParticularTran> ParticularTrans { get; set; }
                
        //public virtual ICollection<PaymentTransactionTran> PaymentTransactionTrans { get; set; }
                
        public virtual ICollection<QualificationsTran> QualificationsTrans { get; set; }
                
        //public virtual ICollection<TransactionType> TransactionTypes { get; set; }
                
        public virtual ICollection<ApplicationUser> Users { get; set; }
                
        public virtual ICollection<WorkExperienceTran> WorkExperienceTrans { get; set; }

        public virtual ICollection<ModuleTran> ModuleTrans { get; set; }
    }

}
