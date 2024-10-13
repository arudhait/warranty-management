using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using Warranty.Web.Models;

namespace Warranty.Web.Models
{
    public class MasterViewModel : BaseApplicationViewModel
    {
        public ForgotPasswordModel ForgotPassword { get; set; }
        public LoginModel Login { get; set; }
        public AuthResultModel authResultModel { get; set; }
        public UserMastModel UserMaster { get; set; }

        public List<SelectListItem> RoleList { get; set; }

    }
}
