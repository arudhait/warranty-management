using Microsoft.AspNetCore.Mvc;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Provider.Provider;
using Warranty.Repository.Repository;
using Warranty.Web.Filter;
using Warranty.Web.Models;
using static Warranty.Common.Utility.Enumeration;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
   { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class DashBoardController : BaseController
    {
        #region Vriable
        private IDashBoardProvider _DashBoardProvider;
        #endregion
        public DashBoardController(IDashBoardProvider dashBoardProvider, ICommonProvider commonProvider, ISessionManager sessionManager) : base(commonProvider, sessionManager)
        {
            _DashBoardProvider = dashBoardProvider;
        }

        public IActionResult Index()
        {
            SessionProviderModel sessionProviderModel = new SessionProviderModel();
            

            var warrentymodel = _DashBoardProvider.GetAllCount(sessionProviderModel);

            

            DashBoardViewModel dashboardViewModel = new DashBoardViewModel
            {
                TotalDueCount = warrentymodel.DueCount,
                TotalExpiredCount = warrentymodel.ExpiredCount,
                ContractTotalDueCount = warrentymodel.DueAMCCMCCount,
                ContractTotalExpiredCount = warrentymodel.ExpiredAMCCMCCount,
                TotalEnggCount = warrentymodel.TotalEnggCount,
                TotalCustCount = warrentymodel.TotalCustCount,
                FirstName = warrentymodel.FirstName,
                LastName = warrentymodel.LastName,
                RoleId = _sessionManager.RoleId
            };

            return View(dashboardViewModel);
        }

        public PartialViewResult _CustomerList()
        {
            return PartialView();
        }
        public JsonResult SearchList(string startDate, string endDate, int id)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetSearchList(GetPagingRequestModel(), startDateTime ?? DateTime.MinValue, endDateTime ?? DateTime.MinValue);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }
        public PartialViewResult _DueWarratyList()
        {
            return PartialView();
        }
        public JsonResult DueSearchList(string startDate, string endDate, int id)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetDueSearchList(GetPagingRequestModel(), startDateTime ?? DateTime.MinValue, endDateTime ?? DateTime.MinValue);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }
        public PartialViewResult _ExpriredWarratyList()
        {
            return PartialView();
        }
        public JsonResult ExpriredSearchList(string startDate, string endDate, int id)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetExpiredSearchList(GetPagingRequestModel(),startDateTime ?? DateTime.MinValue, endDateTime ?? DateTime.MinValue);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }

        /// <summary>
        /// AMCCMSWarranty
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _DueAMCMCWarratyList()
        {
            return PartialView();
        }
        public JsonResult DueAMCCMSSearchList(string startDate, string endDate, int id)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetDueAMCCMSSearchList(GetPagingRequestModel(), startDateTime ?? DateTime.MinValue, endDateTime ?? DateTime.MinValue);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }
        public PartialViewResult _ExpriredAMCCMSWarratyList()
        {
            return PartialView();
        }
        public JsonResult ExpriredAMCCMSSearchList(string startDate, string endDate, int id)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetExpiredAMCCMSSearchList(GetPagingRequestModel(), startDateTime ?? DateTime.MinValue, endDateTime ?? DateTime.MinValue);
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }

        public PartialViewResult _DistrictStateList()
        {
            return PartialView();
        }

        public JsonResult DistrictStateSearchList(string startDate, string endDate)
        {
            try
            {
                DateTime? startDateTime = null;
                DateTime? endDateTime = null;

                if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
                {
                    startDateTime = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    endDateTime = parsedEndDate;
                }
                var result = _DashBoardProvider.GetDistrictStateList(GetPagingRequestModel(), GetSessionProviderParameters());
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while processing the request." });
            }
        }

    }
}
