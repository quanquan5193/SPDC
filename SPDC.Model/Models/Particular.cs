using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.Models
{
    [Table("Particular")]
    public partial class Particular
    {
        public Particular()
        {
            ParticularTrans = new HashSet<ParticularTran>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string SurnameEN { get; set; }

        [Required]
        [StringLength(256)]
        public string GivenNameEN { get; set; }

        [StringLength(256)]
        public string SurnameCN { get; set; }

        [StringLength(256)]
        public string GivenNameCN { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public bool? Gender { get; set; }

        [MaxLength(66)]
        public byte[] HKIDNo { get; set; }

        [MaxLength(66)]
        public byte[] PassportNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PassportExpiryDate { get; set; }

        [MaxLength(66)]
        [Index("INDEX_MOBILENUM",  IsUnique = true)]
        public byte[] MobileNumber { get; set; }

        [StringLength(256)]
        public string MobileNumberPrefix { get; set; }

        [StringLength(256)]
        public string TelNo { get; set; }

        [StringLength(256)]
        public string FaxNo { get; set; }

        [StringLength(256)]
        public string RegionEN { get; set; }

        [StringLength(256)]
        public string RegionCN { get; set; }

        [StringLength(256)]
        public string DistrictEN { get; set; }

        [StringLength(256)]
        public string DistrictCN { get; set; }

        [StringLength(256)]
        public string StreetNumberEN { get; set; }

        [StringLength(256)]
        public string StreetNumberCN { get; set; }

        [StringLength(256)]
        public string StreetRoadEN { get; set; }

        [StringLength(256)]
        public string StreetRoadCN { get; set; }

        [StringLength(256)]
        public string EstateQuartersVillageEN { get; set; }

        [StringLength(256)]
        public string EstateQuartersVillageCN { get; set; }

        [StringLength(256)]
        public string BuildingEN { get; set; }

        [StringLength(256)]
        public string BuildingCN { get; set; }

        [StringLength(256)]
        public string FloorEN { get; set; }

        [StringLength(256)]
        public string FloorCN { get; set; }

        [StringLength(256)]
        public string RmFtUnitEN { get; set; }

        [StringLength(256)]
        public string RmFtUnitCN { get; set; }

        public bool SameAddress { get; set; }

        [StringLength(256)]
        public string ResidentialRegionEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialDistrictEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialStreetNumberEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialStreetRoadEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialEstateQuartersVillageEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialBuildingEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialFloorEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialRmFtUnitEN { get; set; }
        
        [StringLength(256)]
        public string ResidentialRegionCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialDistrictCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialStreetNumberCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialStreetRoadCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialEstateQuartersVillageCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialBuildingCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialFloorCN { get; set; }
        
        [StringLength(256)]
        public string ResidentialRmFtUnitCN { get; set; }

        public bool IsPrimamy { get; set; }

        public bool IsSecondary { get; set; }

        public bool IsTechInst { get; set; }

        public bool IsUniversityCollege { get; set; }

        [StringLength(256)]
        public string EducationLevelEN { get; set; }

        [StringLength(256)]
        public string EducationLevelCN { get; set; }

        public bool? RelatedQualifications1Check { get; set; }

        public bool? RelatedQualifications2Check { get; set; }

        [StringLength(256)]
        public string RelatedQualifications2Year { get; set; }

        public bool? RelatedQualifications3Check { get; set; }
        public int Honorific { get; set; }
        public string HKIDNoEncrypted { get; set; }
        public string PassportNoEncrypted { get; set; }
        public string MobileNumberEncrypted { get; set; }
        [StringLength(256)]
        public string InterestedTypeOfCourse { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ParticularTran> ParticularTrans { get; set; }
    }
}
