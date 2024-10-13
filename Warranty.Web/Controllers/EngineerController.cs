using Microsoft.AspNetCore.Mvc;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
 { (short)Enumeration.Role.SuperAdmin})]
    public class EngineerController : BaseController
    {
        #region Variables
        private IEngineerProvider _EngineerProvider;
        #endregion

        public EngineerController(IEngineerProvider EngineerProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _EngineerProvider = EngineerProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetEngineerList()
        {
            return Json(_EngineerProvider.GetList(GetPagingRequestModel()));
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            EnggMastViewModel model = new EnggMastViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.EnggMastModel = _EngineerProvider.GetById(intId);
            }
            return PartialView(model);
        }
        public PartialViewResult _Allocation(string id)
        {
            EnggMastViewModel model = new EnggMastViewModel();
            model.RoleList = GetRoleDropdownList();

            if (!string.IsNullOrEmpty(id))
            {
                model.IsEdit = true;
                model.EnggMastModel = _EngineerProvider.GetByAllocationId(_commonProvider.UnProtect(id));
            }
            else
            {
                model.UserMastModel = new UserMastModel()
                {
                    IsActive = true
                };
            }
            return PartialView(model);
        }
        public JsonResult SaveAllocation(EnggMastViewModel model)
        {
            return Json(_EngineerProvider.SaveAllocation(model.EnggMastModel, GetSessionProviderParameters()));
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_EngineerProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        public PartialViewResult _View(string id)
        {
            EnggMastViewModel model = new EnggMastViewModel();
            model.EnggMastModel = _EngineerProvider.GetById(_commonProvider.UnProtect(id));
            return PartialView(model);
        }
        public JsonResult Save(EnggMastViewModel model)
        {
            return Json(_EngineerProvider.Save(model.EnggMastModel, GetSessionProviderParameters()));
        }
    }
}
