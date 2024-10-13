using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class SupplierMasterController : BaseController
    {

        #region Variable
        private ISupplierMasterProvider _SupplyMasterProvider;
        #endregion

        #region Construtor
        public SupplierMasterController(ISupplierMasterProvider supplyMasterProvider,ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _SupplyMasterProvider = supplyMasterProvider;
            _commonProvider = commonProvider;
            _sessionManager = sessionManager;
        }
        #endregion


        #region
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSupplierMasterList()
        {
            var result = _SupplyMasterProvider.GetSupplierMasterList(GetPagingRequestModel());
            return Json(result);
        }

        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            SupplierMasterViewModel model = new SupplierMasterViewModel();
            int intId = _commonProvider.UnProtect(id);
            if (intId > 0)
            {
                model.IsEdit = true;
                model.SupplierMasterModel = _SupplyMasterProvider.GetById(intId);
            }
            model.StateList = GetStateList();
            model.ProductList = GetProductList();
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Save(SupplierMasterViewModel model)
        {
            return Json(_SupplyMasterProvider.Save(model.SupplierMasterModel, GetSessionProviderParameters()));
        }


        [HttpGet]
        public PartialViewResult _View(string id)
        {
            SupplierMasterViewModel model = new SupplierMasterViewModel();
            model.SupplierMasterModel = _SupplyMasterProvider.GetById(_commonProvider.UnProtect(id));
            model.StateList = GetStateList();
            model.ProductList = GetProductList();
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_SupplyMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        #endregion

    }
}
