using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Repository.Models;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class WarrantyListController : BaseController
    {
        private IWarrantyListProvider _WarrantyListProvider;

        public WarrantyListController(IWarrantyListProvider WarrantyListProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _WarrantyListProvider = WarrantyListProvider;

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
        public JsonResult GetWarrantyList(DateTime? startDate = null, DateTime? endDate = null)
        {
            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }
            var result = (_WarrantyListProvider.GetWarrantyList(GetPagingRequestModel(),startDate.Value,endDate.Value));
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
                WarrantyDetailsModel = _WarrantyListProvider.GetById(intId, true),
            };
            return View(model);
        }
        public JsonResult Save(WarrantyListViewModel model)
        {
            return Json(_WarrantyListProvider.Save(model.WarrantyDetailsModel, GetSessionProviderParameters()));
           }
        #endregion

        #region Model 
        public PartialViewResult _ModelData()
        {
            return PartialView();
        }
        public JsonResult GetModelList(int id)
        {
            return Json(_WarrantyListProvider.GetModelList(id, GetPagingRequestModel()));
        }
        public PartialViewResult _AddModelDet(int id, int modelDetId)
        {
            WarrantyListViewModel model = new WarrantyListViewModel()
            {
                IsEdit = true,
                ModelList = GetModelList(),
                ModelDetailModel = _WarrantyListProvider.GetModel(id, modelDetId)
            };
            return PartialView(model);
        }
        public JsonResult SaveModelDet(WarrantyListViewModel model)
        {
            return Json(_WarrantyListProvider.SaveModelDatils(model.ModelDetailModel, GetSessionProviderParameters()));
        }
        #endregion

        #region Prob 
        public PartialViewResult _ProbData()
        {
            return PartialView();
        }
        public JsonResult GetProbList(int id)
        {
            return Json(_WarrantyListProvider.GetProbList(id, GetPagingRequestModel()));
        }
        public PartialViewResult _AddProbDet(int id, int probId)
        {
            WarrantyListViewModel model = new WarrantyListViewModel()
            {
                IsEdit = true,
                ModelList = GetModelList(),
                ProbDetailModel = _WarrantyListProvider.GetProb(id, probId)
            };
            return PartialView(model);
        }
        public JsonResult SaveProbDet(WarrantyListViewModel model)
        {
            return Json(_WarrantyListProvider.SaveProbDatils(model.ProbDetailModel, GetSessionProviderParameters()));
        }
        #endregion
    }

}

