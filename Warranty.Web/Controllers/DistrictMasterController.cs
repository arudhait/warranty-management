using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class DistrictMasterController : BaseController
    {
        #region Variable
        private IDistrictMastProvider _DistrictMastProvider;
        #endregion

        #region Constructor
        public DistrictMasterController(IDistrictMastProvider districtMastProvider,ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _DistrictMastProvider = districtMastProvider;
        }
        #endregion

        #region Method
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDistrictList()
        {
            return Json(_DistrictMastProvider.GetDistrictDetailList(GetPagingRequestModel()));
        }

        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            DistrictMastViewModel model = new DistrictMastViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.DistrictMastModel = _DistrictMastProvider.GetById(intId);
            }
            model.StateList = GetStateList();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Save(DistrictMastViewModel model)
        {
            return Json(_DistrictMastProvider.Save(model.DistrictMastModel, GetSessionProviderParameters()));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_DistrictMastProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        #endregion
    }
}
