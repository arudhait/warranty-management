using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class SupplierMasterModel
    {
        public int SupplierMasterId { get; set; }

        public string SupplierName { get; set; } = null!;

        public string EmailId { get; set; } = null!;

        public string ContactNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string City { get; set; } = null!;

        public short StateId { get; set; }

        public string StateName { get; set; } 

        public string ZipCode { get; set; } = null!;

        public int ProductMatserId { get; set; }
        public string ProductMatserName { get; set; }

        public string SupplierSku { get; set; } = null!;

        public string ProductPrice { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedOnDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string Ip { get; set; } = null!;

        public string EncId { get; set; } 
        public string StateCityPin { get; set;}

    }
}
