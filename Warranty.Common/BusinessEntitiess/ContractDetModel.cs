using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ContractDetModel
    {
        

        public long ContractId { get; set; }

        public short ContractTypeId { get; set; }

        public long WarrantyId { get; set; }

        public decimal? Amount { get; set; }

        public string? ChequeDet { get; set; }

        public string? InvoiceNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal? AmtExcludTax { get; set; }

        public short? NoOfService { get; set; }

        public string? Interval { get; set; }

        public int? TakenBy { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDatestring { get; set; }
        public string CreatedByName { get; set; }
        public string ContractTypeName { get; set; }
        public string StartOnString { get; set; }
        public string EncId { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string DoctorName { get; set; }

        public string? CrmNo { get; set; }
        public string InstalledByString { get; set; }
        public string SellingDateString { get; set; }
        public DateTime SellingDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
