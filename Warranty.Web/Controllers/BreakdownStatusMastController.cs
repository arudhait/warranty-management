using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class BreakdownStatusMastController : BaseController
    {
        #region Variables
        private IBreakdownStatusMasterProvider _BreakdownStatusMasterProvider;
        #endregion

        public BreakdownStatusMastController(IBreakdownStatusMasterProvider BreakdownStatusMasterProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _BreakdownStatusMasterProvider = BreakdownStatusMasterProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetStatusList()
        {
            return Json(_BreakdownStatusMasterProvider.GetList(GetPagingRequestModel()));
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            BreakdownStatusMastViewModel model = new BreakdownStatusMastViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.BreakdownStatusMastModel = _BreakdownStatusMasterProvider.GetById(intId);
            }
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_BreakdownStatusMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        public PartialViewResult _View(string id)
        {
            BreakdownStatusMastViewModel model = new BreakdownStatusMastViewModel();
            model.BreakdownStatusMastModel = _BreakdownStatusMasterProvider.GetById(_commonProvider.UnProtect(id));
            return PartialView(model);
        }
        public JsonResult Save(BreakdownStatusMastViewModel model)
        {
            return Json(_BreakdownStatusMasterProvider.Save(model.BreakdownStatusMastModel, GetSessionProviderParameters()));
        }
    }
}
