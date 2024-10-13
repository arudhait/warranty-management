using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class ProductMasterModel
    {
        public int ProductMasterId { get; set; }
        public string EncId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Qty { get; set; } = null!;

        public decimal Price { get; set; }

        public string Sku { get; set; } = null!;

        public string BatchNo { get; set; } = null!;

        public string Size { get; set; } = null!;

        public string Description { get; set; } = null!;

        public short Warranty { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime CraetedDate { get; set; }
        public string CreatedOnString { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string Ip { get; set; } = null!;

    }
}
