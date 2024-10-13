using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class CustMastModel
    {
        public long CustId { get; set; }

        [Required(ErrorMessage = "Please enter Doctor Name")]
        public string DoctorName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Hospital Name")]
        public string HospitalName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Postal Address")]
        public string PostalAddress { get; set; } = null!;

        public string? Designation { get; set; }

        [Required(ErrorMessage = "Please enter Mobile No.")]
        public string MobileNo { get; set; } = null!;

        public string? PhoneNo { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter Pincode")]
        public string Pincode { get; set; } = null!;

        [Required(ErrorMessage = "Please Select a State")]
        public short StateId { get; set; }

        [Required(ErrorMessage = "Please select a District")]
        public int DistrictId { get; set; }

        public string? City { get; set; }

        [Required(ErrorMessage = "Please enter Pndt Certi No.")]
        public string PndtCertiNo { get; set; } = null!;

        public DateTime? RegDate { get; set; }

        [Required(ErrorMessage = "Please select a Status")]
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string CreatedDatestring { get; set; }
        public string CreatedByName { get; set; }
        public string EncId { get; set; }
        public string CityStatePin { get; set; }
    }
}
