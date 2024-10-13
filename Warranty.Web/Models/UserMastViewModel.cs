using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class UserMastViewModel : BaseApplicationViewModel
    {
        public UserMastModel UserMaster { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
