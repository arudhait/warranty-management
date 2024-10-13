using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class LedgerController : BaseController
    {
        #region Variables
        private ILedgerProvider _LedgerProvider;
        #endregion
        public LedgerController(ILedgerProvider LedgerProvider,ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _LedgerProvider = LedgerProvider;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetLedgerList()
        {
            return Json(_LedgerProvider.GetList(GetPagingRequestModel()));
        }
    }
}
