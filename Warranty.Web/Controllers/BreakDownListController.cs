using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class BreakDownListController : BaseController
    {

        #region Variable
        private IBreakDownListProvider _BreakDownListProvider;
        #endregion

        #region Constructor
        public BreakDownListController(IBreakDownListProvider breakDownListProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _BreakDownListProvider = breakDownListProvider;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetBreakDownList(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
            {
                startDate = DateTime.MinValue; 
            }
            if (endDate == null)
            {
                endDate = DateTime.MaxValue; 
            }
            var result = _BreakDownListProvider.GetBreakdownListDetailList(GetPagingRequestModel(), startDate.Value, endDate.Value);
            return Json(result);
        }



        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            BreakDownListViewModel model = new BreakDownListViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.BreakdownDetModel = _BreakDownListProvider.GetById(intId);
            }
            model.GetCustomerList = GetCustomerList();
            model.GetActionMastList = GetActionMastList();
            model.GetBreakdownTypeList = GetBreakdownTypeList();
            model.GetEngineerList = GetEngineerList();
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult _View(string id)
        {
            BreakDownListViewModel model = new BreakDownListViewModel();
            model.BreakdownDetModel = _BreakDownListProvider.GetById(_commonProvider.UnProtect(id));
            model.GetCustomerList = GetCustomerList();
            model.GetActionMastList = GetActionMastList();
            model.GetBreakdownTypeList = GetBreakdownTypeList();
            model.GetEngineerList = GetEngineerList();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Save(BreakDownListViewModel model)
            {
            return Json(_BreakDownListProvider.Save(model.BreakdownDetModel, GetSessionProviderParameters()));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_BreakDownListProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
    }
}
