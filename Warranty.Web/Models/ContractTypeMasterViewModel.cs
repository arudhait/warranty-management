using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class ContractTypeMasterViewModel : BaseApplicationViewModel
    {
        public ContractTypeMasterModel ContractTypeMasterModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
