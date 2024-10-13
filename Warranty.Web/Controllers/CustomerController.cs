using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class CustomerController : BaseController
    {
        #region Variables
        private ICustomerProvider _CustomerProvider;
        #endregion

        public CustomerController(ICustomerProvider CustomerProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _CustomerProvider = CustomerProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetCustomerList()
        {
            return Json(_CustomerProvider.GetList(GetPagingRequestModel()));
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            CustMastViewModel model = new CustMastViewModel();
            int intId = _commonProvider.UnProtect(id);
            model.StateList = GetStateList();
            model.DistrictList = GetDistrictList();
            if (intId > 0)
            {
                model.IsEdit = true;
                model.CustMastModel = _CustomerProvider.GetById(intId);
            }
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_CustomerProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        public PartialViewResult _View(string id)
        {
            CustMastViewModel model = new CustMastViewModel();
            model.CustMastModel = _CustomerProvider.GetById(_commonProvider.UnProtect(id));
            model.StateList = GetStateList();
            model.DistrictList = GetDistrictList();
            return PartialView(model);
        }
        public JsonResult Save(CustMastViewModel model)
        {
            return Json(_CustomerProvider.Save(model.CustMastModel, GetSessionProviderParameters()));
        }
    }
}
