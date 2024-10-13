using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class LedgerModel
    {
        public int LedgerId { get; set; }
        public string EncId { get; set; }

        public int ProductMasterId { get; set; }
        public string ProductName { get; set; }

        public short Qty { get; set; }

        public decimal Price { get; set; }

        public bool Type { get; set; }
        public string TypeData { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }

        public bool IsCredit { get; set; }

        public string Remarks { get; set; } = null!;

        public long InwardOutwardItemId { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get; set; }

        public string Ip { get; set; } = null!;
    }
}
