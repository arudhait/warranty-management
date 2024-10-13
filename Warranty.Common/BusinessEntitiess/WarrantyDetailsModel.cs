using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class WarrantyDetailsModel
    {
        public long WarrantyId { get; set; }

        [Required(ErrorMessage = "Please select a Doctor Name")]
        public long CustId { get; set; }

        [Required(ErrorMessage = "Please select a Selling Date")]
        public DateTime SellingDate { get; set; }

        [Required(ErrorMessage = "Please select a Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please select an End Date")]
        public DateTime EndDate { get; set; }

        public int? InstalledBy { get; set; }

        public string? CrmNo { get; set; }

        public short? NoOfServices { get; set; }

        public string? Interval { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string CreatedByName { get; set; }
        public short? ContractTypeId { get; set; }
        public string EncId { get; set; }

        [Required(ErrorMessage = "Please select a Doctor Name")]
        public string DoctorName { get; set; }
        public string EngineerName { get; set; }
        public string CreatedDateString { get; set; }
        public string SellingDateString { get; set; }
        public string EndDateString { get; set; }

        public ContractDetModel ContractDetModel { get; set; }
        public string InstalledByString { get; set; }
        public decimal? Amount { get; set; }
        public string ChequeDet { get; set; }
        public string InvoiceNo { get; set; }
        public decimal? AmtExcludTax { get; set; }
        public short? NoOfService { get; set; }
        public int? TakenBy { get; set; }
        public bool? IsActive { get; set; }
        public string StartDateString { get; set; }
        public string ContractTypeName { get; set; }
        public long ContractId { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
