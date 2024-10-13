using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class ContractTypeMasterController : BaseController
    {
        private readonly IContractTypeMasterProvider _ContractTypeMasterProvider;
        public ContractTypeMasterController(IContractTypeMasterProvider ContractTypeMasterProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _ContractTypeMasterProvider = ContractTypeMasterProvider;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetContractTypeMasterList()
        {
            var result = _ContractTypeMasterProvider.GetContractTypeMasterList(GetPagingRequestModel());
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_ContractTypeMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        public PartialViewResult _View(string id)
        {
            ContractTypeMasterViewModel model = new ContractTypeMasterViewModel();
            model.ContractTypeMasterModel = _ContractTypeMasterProvider.GetById(_commonProvider.UnProtect(id));
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            ContractTypeMasterViewModel model = new ContractTypeMasterViewModel();
            int intId = _commonProvider.UnProtect(id);

            if (intId > 0)
            {
                model.IsEdit = true;
                model.ContractTypeMasterModel = _ContractTypeMasterProvider.GetById(intId);
            }
            return PartialView(model);
        }
        public JsonResult Save(ContractTypeMasterViewModel model)
        {
            return Json(_ContractTypeMasterProvider.Save(model.ContractTypeMasterModel, GetSessionProviderParameters()));
        }
    }
}
