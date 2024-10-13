using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class AllocationViewModel : BaseApplicationViewModel
    {
        public TerritoryAllocationModel TerritoryAllocationModel { get; set; }
        public EnggMastModel EnggMastModel { get; set; }
       
        public TempFilterModel TempFilterModel { get; set; }
        public List<SelectListItem> DocotorList { get; set; }
        public List<SelectListItem> ModelList { get; set; }
        public List<SelectListItem> DistrictList { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> EngeeList { get; set; }
        public List<SelectListItem> ContractTypeList { get; set; }
    }
}
