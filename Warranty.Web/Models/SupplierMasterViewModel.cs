using Box.V2.Models.Request;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class SupplierMasterViewModel : BaseApplicationViewModel
    {
        public SupplierMasterModel SupplierMasterModel { get; set; }

        public TempFilterModel TempFilterModel { get; set; }

        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> ProductList { get; set; }
    }
}
