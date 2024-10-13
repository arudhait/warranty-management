using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class InwardOutwardViewModel : BaseApplicationViewModel
    {
        public InwardOutwardModel InwardOutwardModel { get; set; }
        public InwardOutwardItemModel InwardOutwardItemModel { get; set;}
        public TempFilterModel TempFilterModel { get; set; }
        public List<SelectListItem> ProductList { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

    }
}
