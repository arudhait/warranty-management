using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BusinessEntitiess
{
    public class InwardOutwardItemModel
    {
        public long InwardOutwardItemId { get; set; }

        public int InwardOutwardId { get; set; }

        public int ProductMasterId { get; set; }

        public short Qty { get; set; }

        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string EncId { get; set; }
    }
}
