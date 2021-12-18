using SPDC.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPDC.Model.ViewModels
{
    public class ParticularBindingModel
    {
        public int Id { get; set; }
        [Required]
        public string SurnameEN { get; set; }
        [Required]
        public string GivenNameEN { get; set; }
        public string SurnameCN { get; set; }
        public string GivenNameCN { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public string HKIDNo { get; set; }
        public string PassportNo { get; set; }
        public DateTime? PassportExpiredDate { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string MobileNumberPrefix { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string RegionEN { get; set; }
        public string RegionCN { get; set; }
        public string DistrictEN { get; set; }
        public string DistrictCN { get; set; }
        public string StreetNumberEN { get; set; }
        public string StreetNumberCN { get; set; }
        public string StreetRoadEN { get; set; }
        public string StreetRoadCN { get; set; }
        public string EstateQuartersVillageEN { get; set; }
        public string EstateQuartersVillageCN { get; set; }
        public string BuildingEN { get; set; }
        public string BuildingCN { get; set; }
        public string FloorEN { get; set; }
        public string FloorCN { get; set; }
        public string RmFtUnitEN { get; set; }
        public string RmFtUnitCN { get; set; }
        public string EducationLevelEN { get; set; }
        public string EducationLevelCN { get; set; }
        public string PresentEmployer { get; set; }
        public string Position { get; set; }
        public int Honorific { get; set; }

        public bool SameAddress { get; set; }

        public string ResidentialRegionEN { get; set; }

        public string ResidentialDistrictEN { get; set; }

        public string ResidentialStreetNumberEN { get; set; }

        public string ResidentialStreetRoadEN { get; set; }

        public string ResidentialEstateQuartersVillageEN { get; set; }

        public string ResidentialBuildingEN { get; set; }

        public string ResidentialFloorEN { get; set; }

        public string ResidentialRmFtUnitEN { get; set; }

        public string ResidentialRegionCN { get; set; }

        public string ResidentialDistrictCN { get; set; }

        public string ResidentialStreetNumberCN { get; set; }

        public string ResidentialStreetRoadCN { get; set; }

        public string ResidentialEstateQuartersVillageCN { get; set; }

        public string ResidentialBuildingCN { get; set; }

        public string ResidentialFloorCN { get; set; }

        public string ResidentialRmFtUnitCN { get; set; }

        public bool IsPrimamy { get; set; }

        public bool IsSecondary { get; set; }

        public bool IsTechInst { get; set; }

        public bool IsUniversityCollege { get; set; }

        public int ApplicationId { get; set; }
    }
}
