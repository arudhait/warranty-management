using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class ProductMasterViewModel : BaseApplicationViewModel
    {
        public ProductMasterModel ProductMasterModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
