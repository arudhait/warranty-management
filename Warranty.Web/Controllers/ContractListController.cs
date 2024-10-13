using Microsoft.AspNetCore.Mvc;
using System.Web.WebPages;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Web.Filter;
using Warranty.Web.Models;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class ContractListController : BaseController
    {
        private IContractListProvider _ContractListProvider;

        public ContractListController(IContractListProvider contractListProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _ContractListProvider = contractListProvider;

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
        public JsonResult GetContractList(DateTime? startDate = null,DateTime? endDate = null )
        {
            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            }
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            }
            var result = (_ContractListProvider.GetContractList(GetPagingRequestModel(), startDate.Value, endDate.Value));
            return Json(result);
        }
    }
    #endregion
}
