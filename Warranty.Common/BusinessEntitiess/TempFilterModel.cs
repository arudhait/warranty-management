using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warranty.Common.BussinessEntities
{
    public class TempFilterModel
    {
        public string SearchText { get; set; }
        public string DateRange { get; set; }
        public List<string> LeadsStatus { get; set; }
        public List<string> SellersStatus { get; set; }
        public string Value_LeadsStatus { get; set; }
        public string Value_AllocatedDate { get; set; }
        public string Value_ReturnRefundStatus { get; set; }
        public string Value_AllocatedTo { get; set; }
    }
}
