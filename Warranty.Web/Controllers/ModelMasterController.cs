
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
    public class ModelMasterController : BaseController
    {
        private readonly IModelMasterProvider _ModelMasterProvider;
        public ModelMasterController(IModelMasterProvider ModelMasterProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _ModelMasterProvider = ModelMasterProvider;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetModelMasterList()
        {
            var result = _ModelMasterProvider.GetModelMasterList(GetPagingRequestModel());
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            return Json(_ModelMasterProvider.Delete(_commonProvider.UnProtect(id), GetSessionProviderParameters()));
        }
        public PartialViewResult _View(string id)
        {
            ModelMasterViewModel model = new ModelMasterViewModel();
            model.ModelMasterModel = _ModelMasterProvider.GetById(_commonProvider.UnProtect(id));
            return PartialView(model);
        }
        [HttpGet]
        public PartialViewResult _Details(string id)
        {
            ModelMasterViewModel model = new ModelMasterViewModel();
            int intId = _commonProvider.UnProtect(id);
           
            if (intId > 0)
            {
                model.IsEdit = true;
                model.ModelMasterModel = _ModelMasterProvider.GetById(intId);
            }
            return PartialView(model);
        }
        public JsonResult Save(ModelMasterViewModel model)
        {
            return Json(_ModelMasterProvider.Save(model.ModelMasterModel, GetSessionProviderParameters()));
        }
    }
}
