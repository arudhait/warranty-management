using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class LedgerViewModel : BaseApplicationViewModel
    {
        public LedgerModel LedgerModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
