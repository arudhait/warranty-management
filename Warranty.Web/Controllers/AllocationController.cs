using Microsoft.AspNetCore.Mvc;
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
    public class AllocationController : BaseController
    {
        #region Variables
        private IAllocationProvider _AllocationProvider;
        #endregion

        public AllocationController(IAllocationProvider AllocationProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _AllocationProvider = AllocationProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetAllocationList()
        {
            return Json(_AllocationProvider.GetList(GetPagingRequestModel(), GetSessionProviderParameters()));
        }
        public IActionResult Add(string id, bool view)
        {
            int intId = _commonProvider.UnProtect(id);

            AllocationViewModel model = new AllocationViewModel()
            {
                IsEdit = intId > 0,
                IsView = view,
                RoleId = _sessionManager.RoleId,
                DocotorList = GetDocotorList(),
                ModelList = GetModelList(),
                EngeeList = GetEngeeList(),
                DistrictList = GetDistrictList(),
                StateList = GetStateList(),
                ContractTypeList = GetContractTypeList(),
                EnggMastModel = _AllocationProvider.GetById(intId),
            };

           

            return View(model);
        }



        public PartialViewResult _DistrictStateData()
        {
            return PartialView();
        }
        public JsonResult GetDistrictstateList(int id)
        {
            return Json(_AllocationProvider.GetDistrictStateList(id, GetPagingRequestModel()));
        }

        public PartialViewResult _AddDistrictState(int id,int allocationId)
        {
            AllocationViewModel model = new AllocationViewModel()
            {
                IsEdit = true,
                ModelList = GetModelList(),
                EngeeList = GetEngeeList(),
                DistrictList = GetDistrictList(),
                StateList = GetStateList(),
                TerritoryAllocationModel = _AllocationProvider.GetDistrictState(id, allocationId)
            };
            return PartialView(model);
        }
        public JsonResult SaveDistrictState(AllocationViewModel model)
        {
            return Json(_AllocationProvider.SaveDistrictState(model.TerritoryAllocationModel, GetSessionProviderParameters()));
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_AllocationProvider.Delete(_commonProvider.UnProtect(id)));
        }
    }
}
