using Warranty.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Models;
using Warranty.Web.Filter;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class StateMasterController : BaseController
    {

        #region Variables
        private IStateMastProvider _StateMastProvider;
        #endregion

        #region Constructor
        public StateMasterController(IStateMastProvider stateMastProvider , ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _StateMastProvider = stateMastProvider;
        }
        #endregion


        #region Method
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStateList()
        {
            return Json(_StateMastProvider.GetStateDetailList(GetPagingRequestModel()));
        }


        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            StateMastViewModel model = new StateMastViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.StateMastModel = _StateMastProvider.GetById(intId);
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Save(StateMastViewModel model)
        {
            return Json(_StateMastProvider.Save(model.StateMastModel, GetSessionProviderParameters()));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_StateMastProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        #endregion
    }


}


