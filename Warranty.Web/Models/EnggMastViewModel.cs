using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;

namespace Warranty.Web.Models
{
    public class EnggMastViewModel : BaseApplicationViewModel
    {
        public EnggMastModel EnggMastModel { get; set; }
        public UserMastModel UserMastModel { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        public TempFilterModel TempFilterModel { get; set; }
    }
}
