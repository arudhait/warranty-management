using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class BreakDownListViewModel : BaseApplicationViewModel
    {
        public BreakdownDetModel BreakdownDetModel { get; set; }
        public TempFilterModel TempFilterModel { get; set; }

        public List<SelectListItem> GetCustomerList { get; set; }
        public List<SelectListItem> GetActionMastList { get; set; }
        public List<SelectListItem> GetBreakdownTypeList { get; set; }
        public List<SelectListItem> GetEngineerList { get; set; }
    }
}
