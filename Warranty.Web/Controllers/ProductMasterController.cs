using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class ProductMasterController : BaseController
    {
        #region Variables
        private IProductMasterProvider _ProductMasterProvider;
        #endregion
        public ProductMasterController(IProductMasterProvider ProductMasterProvider,ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _ProductMasterProvider = ProductMasterProvider;
        }

        public IActionResult Index()
        {
            ProductMasterViewModel model = new ProductMasterViewModel()
            {
                RoleId = _sessionManager.RoleId,
                TempFilterModel = GetAllTempFilter(),

            };
            return View(model);
        }

        [HttpPost]
        public JsonResult GetProductList()
        {
            return Json(_ProductMasterProvider.GetList(GetPagingRequestModel()));
        }

        public PartialViewResult _View(string id)
        {
            ProductMasterViewModel model = new ProductMasterViewModel();
            model.ProductMasterModel = _ProductMasterProvider.GetById(_commonProvider.UnProtect(id));
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            ProductMasterViewModel model = new ProductMasterViewModel();
            int intId = _commonProvider.UnProtect(id);          
            if (intId > 0)
            {
                model.IsEdit = true;
                model.ProductMasterModel = _ProductMasterProvider.GetById(intId);
            }
            return PartialView(model);
        }
        public JsonResult Save(ProductMasterViewModel model)
        {
            return Json(_ProductMasterProvider.Save(model.ProductMasterModel, GetSessionProviderParameters()));
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_ProductMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
    }
}
