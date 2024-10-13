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
    public class Due_ExpiredAMC_CMCContractController : BaseController
    {
        private IAMC_CMCExpiredContractProvider _AMC_CMCExpiredContractProvider;

        public Due_ExpiredAMC_CMCContractController(IAMC_CMCExpiredContractProvider AMC_CMCExpiredContractProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _AMC_CMCExpiredContractProvider = AMC_CMCExpiredContractProvider;

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
        public JsonResult GetExpiredAMC_CMCContract()
        {
            return Json(_AMC_CMCExpiredContractProvider.GetExpiredList(GetPagingRequestModel()));
        }
        public JsonResult GetDueAMC_CMCContract()
        {
            return Json(_AMC_CMCExpiredContractProvider.GetDueList(GetPagingRequestModel()));
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
                WarrantyDetailsModel = _AMC_CMCExpiredContractProvider.GetById(intId),
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
            return Json(_AMC_CMCExpiredContractProvider.GetModelList(id, GetPagingRequestModel()));
        }
        #endregion

        #region Prob
        public PartialViewResult _ProbData()
        {
            return PartialView();
        }
        public JsonResult GetProbList(int id)
        {
            return Json(_AMC_CMCExpiredContractProvider.GetProbList(id, GetPagingRequestModel()));
        }
        #endregion
    }
}
