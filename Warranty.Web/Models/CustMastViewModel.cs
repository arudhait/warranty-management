using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class CustMastViewModel : BaseApplicationViewModel
    {
        public CustMastModel CustMastModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> DistrictList { get; set; }
    }
}
