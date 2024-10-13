using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class ModelMasterViewModel : BaseApplicationViewModel
    {
        public ModelMasterModel ModelMasterModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
