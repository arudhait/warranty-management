using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class InwardOutwardModel
    {
        public int InwardOutwardId { get; set; }
        public bool IsType { get; set; }
        public long? CustId { get; set; }
        public int? SupplierMasterId { get; set; }
        public DateTime Date { get; set; }

        public string? Note { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string Ip { get; set; } = null!;
        public string CreatedOnstring { get; set; }
        public string CreatedByName { get; set; }
        public string EncId { get; set; }
        public string DateOnString { get; set; }
    }
}
