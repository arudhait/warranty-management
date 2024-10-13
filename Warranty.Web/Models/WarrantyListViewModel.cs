
using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class WarrantyListViewModel : BaseApplicationViewModel
    {
        public WarrantyDetailsModel WarrantyDetailsModel { get; set; }
        public ContractDetModel ContractDetModel { get; set; }
        public ModelDetailModel ModelDetailModel { get; set; }
        public ProbDetailModel ProbDetailModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
        public List<SelectListItem> DocotorList { get; set; }
        public List<SelectListItem> ModelList { get; set; }
        public List<SelectListItem> EngeeList { get; set; }
        public List<SelectListItem> ContractTypeList { get; set; }
    }
}
