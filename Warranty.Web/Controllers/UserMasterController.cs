using Warranty.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Common.BusinessEntitiess;
using Warranty.Web.Models;
using Warranty.Web.Filter;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin})]
    public class UserMasterController : BaseController
    {
        private IUserMasterProvider _userMasterProvider;
        public UserMasterController(IUserMasterProvider userMasterProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _userMasterProvider = userMasterProvider;
        }

        [HttpGet]
        public IActionResult Index()
        {          
            var model = new UserMastViewModel()
            {
                RoleId = _sessionManager.RoleId,
                MenuNameId = "User",
                RoleList = GetRoleDropdownList(),
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult GetList()
        {
            return Json(_userMasterProvider.GetUserList(GetPagingRequestModel()));
        }
        public PartialViewResult _Details(string id)
        {
            UserMastViewModel model = new UserMastViewModel();
            model.RoleList = GetRoleDropdownList();

            if (!string.IsNullOrEmpty(id))
            {
                model.IsEdit = true;
                model.UserMaster = _userMasterProvider.GetById(_commonProvider.UnProtect(id));

            }
            else
            {
                model.UserMaster = new UserMastModel()
                {
                    RoleId = 2,
                    IsActive = true
                };
            }
            return PartialView(model);
        }

        public PartialViewResult _Reset(string id)
        {
            UserMastViewModel model = new UserMastViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                model.IsEdit = true;
                model.UserMaster = _userMasterProvider.GetUserById(_commonProvider.UnProtect(id));
            }
            else
                model.UserMaster = new UserMastModel() { IsActive = true };
            return PartialView(model);
        }
        public JsonResult SaveResetPassword(UserMastViewModel model)
        {
            return Json(_userMasterProvider.SaveResetPassword(model.UserMaster, GetSessionProviderParameters()));
        }
        public JsonResult Save(UserMastViewModel model)
        {
            return Json(_userMasterProvider.Save(model.UserMaster, GetSessionProviderParameters()));
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            return Json(_userMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
    }
}
