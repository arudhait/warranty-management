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
    public class InwardOutwardController : BaseController
    {
        private IInwardOutwardProvider _InwardOutwardProvider;

        public InwardOutwardController(IInwardOutwardProvider InwardOutwardProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _InwardOutwardProvider = InwardOutwardProvider;
        }
        #region Method
        public IActionResult Index()
        {
            InwardOutwardViewModel Model = new InwardOutwardViewModel()
            {
                RoleId = _sessionManager.RoleId,
                TempFilterModel = GetAllTempFilter(),

            };
            return View();
        }
        [HttpPost]
        public JsonResult GetInwardOutwardList()
        {
            return Json(_InwardOutwardProvider.GetList(GetPagingRequestModel()));
        }
        public IActionResult Add(string id, bool view)
        {
            int intId = _commonProvider.UnProtect(id);

            InwardOutwardViewModel model = new InwardOutwardViewModel()
            {
                IsEdit = intId > 0,
                IsView = view,
                RoleId = _sessionManager.RoleId,
                ProductList = GetProductList(),
                CustomerList = GetCustomerList(),
                SupplierList = GetSupplierList(),
                InwardOutwardModel = _InwardOutwardProvider.GetById(intId),
            };
            return View(model);
        }
        public JsonResult Save(InwardOutwardViewModel model)
        {
            return Json(_InwardOutwardProvider.Save(model.InwardOutwardModel, GetSessionProviderParameters()));
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_InwardOutwardProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        #endregion

        #region InwardOutward Item 
        public PartialViewResult _InwardOutwardItem()
        {
            return PartialView();
        }
        public JsonResult GetItem(int id)
        {
            return Json(_InwardOutwardProvider.GetItemList(id, GetPagingRequestModel()));
        }
        public PartialViewResult _AddItem(int id, int inwardOutwardItemId)
        {
            InwardOutwardViewModel model = new InwardOutwardViewModel()
            {
                IsEdit = true,
                ProductList = GetProductList(),
                InwardOutwardItemModel = _InwardOutwardProvider.GetItem(id, inwardOutwardItemId)
            };
            return PartialView(model);
        }
        public JsonResult SaveItem(InwardOutwardViewModel model)
        {
            return Json(_InwardOutwardProvider.SaveItem(model.InwardOutwardItemModel, GetSessionProviderParameters()));
        }
        [HttpPost]
        public IActionResult DeleteItem(string id)
        {
            return Json(_InwardOutwardProvider.DeleteItem(_commonProvider.UnProtect(id)));
        }
        [HttpGet]
        public ActionResult<decimal> GetRate(int productMasterId)
        {
            try
            {
                decimal price = _InwardOutwardProvider.GetRate(productMasterId);
                return Json(price);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Error occurred while fetching rate." });
            }
        }
        #endregion
    }
}
