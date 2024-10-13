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
    public class Due_ExpiredWarrantyController : BaseController
    {
        private IDue_ExpiredWarrantyProvider _Due_ExpiredWarrantyProvider;

        public Due_ExpiredWarrantyController(IDue_ExpiredWarrantyProvider Due_ExpiredWarrantyProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _Due_ExpiredWarrantyProvider = Due_ExpiredWarrantyProvider;

        }

        #region Method
        public IActionResult Index()
        {
            WarrantyListViewModel Model = new WarrantyListViewModel()
            {
                RoleId = _sessionManager.RoleId,
                TempFilterModel = GetAllTempFilter(),

            };
            return View();
        }
        public JsonResult GetExpiredWarranty()
        {
            var result = _Due_ExpiredWarrantyProvider.GetExpiredList(GetPagingRequestModel());
            return Json(result);
        }

        public JsonResult GetDueWarranty()
        {
            var result = _Due_ExpiredWarrantyProvider.GetDueList(GetPagingRequestModel());
            return Json(result);
        }

        public IActionResult Add(string id, bool view)
        {
            int intId = _commonProvider.UnProtect(id);

            WarrantyListViewModel model = new WarrantyListViewModel()
            {
                IsEdit = intId > 0,
                IsView = view,
                RoleId = _sessionManager.RoleId,
                DocotorList = GetDocotorList(),
                ModelList = GetModelList(),
                EngeeList = GetEngeeList(),
                ContractTypeList = GetContractTypeList(),
                WarrantyDetailsModel = _Due_ExpiredWarrantyProvider.GetById(intId),
            };
            return View(model);
        }
        #endregion

        #region Model
        public PartialViewResult _ModelData()
        {
            return PartialView();
        }
        public JsonResult GetModelList(int id)
        {
            return Json(_Due_ExpiredWarrantyProvider.GetModelList(id, GetPagingRequestModel()));
        }
        #endregion

        #region Prob
        public PartialViewResult _ProbData()
        {
            return PartialView();
        }
        public JsonResult GetProbList(int id)
        {
            return Json(_Due_ExpiredWarrantyProvider.GetProbList(id, GetPagingRequestModel()));
        }
        #endregion
    }
}
