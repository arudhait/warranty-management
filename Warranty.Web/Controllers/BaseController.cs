using Warranty.Common.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warranty.Common.BussinessEntities;
using Warranty.Common.CommonEntities;
using System.Globalization;
using Warranty.Provider.IProvider;

namespace Warranty.Web.Controllers
{
    public class BaseController : Controller
    {

        #region Variables
        protected ICommonProvider _commonProvider;
        public ISessionManager _sessionManager;

        #endregion

        #region Constructor
        public BaseController(ICommonProvider commonProvider, ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _commonProvider = commonProvider;
        }
        #endregion

        #region Method
        [NonAction]
        protected SessionProviderModel GetSessionProviderParameters()
        {
            SessionProviderModel sessionProviderModel = new SessionProviderModel
            {
                UserId = _sessionManager.UserId,
                Username = _sessionManager.Username,
                Email = _sessionManager.Emailid,
                Ip = _sessionManager.GetIP(),
                FirstName = _sessionManager.FirstName,
                LastName = _sessionManager.LastName,
                RoleId = _sessionManager.RoleId,
                RoleName = _sessionManager.RoleName,
                EnggId = _sessionManager.EnggId,

            };
            return sessionProviderModel;
        }
        public DatatablePageRequestModel GetPagingRequestModel()
        {
            DatatablePageRequestModel model = new DatatablePageRequestModel
            {
                StartIndex = AppCommon.ConvertToInt32(HttpContext.Request.Form["start"]),
                PageSize = AppCommon.ConvertToInt32(HttpContext.Request.Form["length"]),
                SearchText = HttpContext.Request.Form["search[value]"],
                SortColumnName = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"],
                SortDirection = HttpContext.Request.Form["order[0][dir]"],
                Draw = HttpContext.Request.Form["draw"],
            };
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "extra_search"))
                model.ExtraSearch = HttpContext.Request.Form["extra_search"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "LeadId"))
                model.LeadId = AppCommon.ConvertToInt32(HttpContext.Request.Form["LeadId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "statusId"))
                model.StatusId = AppCommon.ConvertToInt32(HttpContext.Request.Form["statusId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "startdatesearch"))
                model.StartDateFilter = HttpContext.Request.Form["startdatesearch"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "enddatesearch"))
                model.EndDateFilter = HttpContext.Request.Form["enddatesearch"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "RoleId") && !string.IsNullOrEmpty(HttpContext.Request.Form["RoleId"]))
                model.RoleId = AppCommon.ConvertToInt32(HttpContext.Request.Form["RoleId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "LeadStatusId") && !string.IsNullOrEmpty(HttpContext.Request.Form["LeadStatusId"]))
                model.LeadStatusId = AppCommon.ConvertToInt32(HttpContext.Request.Form["LeadStatusId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "LeadStatus") && !string.IsNullOrEmpty(HttpContext.Request.Form["LeadStatus"]))
                model.LeadsStatus = HttpContext.Request.Form["LeadStatus"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "BuyerStatusId") && !string.IsNullOrEmpty(HttpContext.Request.Form["BuyerStatusId"]))
                model.LeadStatusId = AppCommon.ConvertToInt32(HttpContext.Request.Form["BuyerStatusId"]);
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "BuyerStatus") && !string.IsNullOrEmpty(HttpContext.Request.Form["buyerStatus"]))
                model.LeadsStatus = HttpContext.Request.Form["BuyerStatus"];
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "DateRange") && !string.IsNullOrEmpty(HttpContext.Request.Form["DateRange"]))
                model.DateRange = HttpContext.Request.Form["DateRange"];

            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "year"))
            {
                string yearString = HttpContext.Request.Form["year"];
                if (int.TryParse(yearString, out int year))
                {
                    model.Year = year; // Successfully parsed
                }
                else
                {
                   
                    model.Year = default(int); 
                }
            }

          
            if (HttpContext.Request.Form != null && HttpContext.Request.Form.Any(p => p.Key == "month"))
            {
                string monthString = HttpContext.Request.Form["month"];
                if (int.TryParse(monthString, out int month))
                {
                    model.Month = month; // Successfully parsed
                }
                else
                {
                   
                    model.Month = default(int); 
                }
            }
            return model;
        }
        #endregion

        #region DropDown Methods

        [NonAction]
        public List<SelectListItem> GetMonthDrpList()
        {
            var list = new List<SelectListItem>();
            for (int monthId = 1; monthId <= 12; monthId++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthId);
                list.Add(new SelectListItem { Value = monthId.ToString(), Text = monthName });
            }
            return list;
        }
        protected List<SelectListItem> GetRoleDropdownList()
        {
            return (from s in _commonProvider.GetRoleDropdownList(GetSessionProviderParameters()).ToList()
                    select new SelectListItem()
                    {
                        Value = s.UserTypeId.ToString(),
                        Text = s.UserTypeName
                    }).ToList();
        }

        public List<string> GetMonthYearList(int year)
        {
            List<string> monthList = new List<string>();

            for (int monthId = 1; monthId <= 12; monthId++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(monthId).ToUpper();
                string formattedMonth = $"{monthName} {year}";
                monthList.Add(formattedMonth);
            }

            return monthList;
        }
        public static List<int> GetMonthList()
        {
            List<int> monthList = new List<int>();

            int currentMonth = DateTime.Now.Month;

            for (int monthId = currentMonth; monthId >= 1; monthId--)
            {
                monthList.Add(monthId);
            }

            return monthList;
        }
        [NonAction]
        public List<SelectListItem> GetYearDrpList(bool isUptoCurrentYear = false)
        {
            var list = new List<SelectListItem>();
            int currentYear = DateTime.Now.Year;

           
            for (int i = 5; i >= 0; i--)
            {
                int year = currentYear - i;
                list.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }

           
            if (!isUptoCurrentYear)
            {
                for (int i = 1; i < 5; i++)
                {
                    int year = currentYear + i;
                    list.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
                }
            }

            return list;
        }


        protected List<SelectListItem> GetStateList()
        {
            return (from s in _commonProvider.GetStateList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.StateId.ToString(),
                        Text = s.StateName
                    }).ToList();
        }
        protected List<SelectListItem> GetDocotorList()
        {
            return (from s in _commonProvider.GetDocotorList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.CustId.ToString(),
                        Text = s.DoctorName
                    }).ToList();
        }
        protected List<SelectListItem> GetModelList()
        {
            return (from s in _commonProvider.GetModelList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.ModelId.ToString(),
                        Text = s.ModelNo
                    }).ToList();
        }
        protected List<SelectListItem> GetProductList()
        {
            return (from s in _commonProvider.GetProductList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.ProductMasterId.ToString(),
                        Text = s.ProductName
                    }).ToList();
        }
        protected List<SelectListItem> GetContractTypeList()
        {
            return (from s in _commonProvider.GetContractTypeList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.ContractTypeId.ToString(),
                        Text = s.ContractTypeName
                    }).ToList();
        }
        protected List<SelectListItem> GetEngeeList()
        {
            return (from s in _commonProvider.GetEngeeList().ToList()
                    select new SelectListItem()
                    {
                        Value = s.EnggId.ToString(),
                        Text = s.EnggName
                    }).ToList();
        }

        protected List<SelectListItem> GetDistrictList()
        {
            return (from d in _commonProvider.GetDistrictList().ToList()
                    select new SelectListItem()
                    {
                        Value = d.DistrictId.ToString(),
                        Text = d.DistrictName
                    }).ToList();
        }

        protected List<SelectListItem> GetCustomerList()
        {
            return (from c in _commonProvider.GetCustomerList()
                    select new SelectListItem()
                    {
                        Value = c.CustId.ToString(),
                        Text = c.DoctorName,
                    }).OrderBy(x => x.Text).ToList();
        }

        protected List<SelectListItem> GetSupplierList()
        {
            return (from c in _commonProvider.GetSupplierList()
                    select new SelectListItem()
                    {
                        Value = c.SupplierMasterId.ToString(),
                        Text = c.SupplierName,
                    }).OrderBy(x => x.Text).ToList();
        }
        protected List<SelectListItem> GetActionMastList()
        {
            return (from a in _commonProvider.GetActionMastList().ToList()
                    select new SelectListItem()
                    {
                        Value = a.ActionId.ToString(),
                        Text = a.ActionName
                    }).ToList();
        }

        protected List<SelectListItem> GetBreakdownTypeList()
        {
            return (from bt in _commonProvider.GetBreakdownTypeList().ToList()
                    select new SelectListItem()
                    {
                        Value = bt.BreakdownStatusId.ToString(),
                        Text = bt.BreakdownStatusName
                    }).ToList();
        }

        protected List<SelectListItem> GetEngineerList()
        {
            return (from e in _commonProvider.GetEngineerList().ToList()
                    select new SelectListItem()
                    {
                        Value = e.EnggId.ToString(),
                        Text = e.EnggName
                    }).ToList();
        }

        #endregion

        #region Temp Data Methods

        public void DeleteAllFilter()

        {
            DeleteTempData(AppCommon.TMP_SearchText);
            DeleteTempData(AppCommon.TMP_LeadsStatus);
            DeleteTempData(AppCommon.TMP_SellersStatus);
            DeleteTempData(AppCommon.TMP_SupplierMaster);
            DeleteTempData(AppCommon.TMP_AllocatedTo);
            DeleteTempData(AppCommon.TMP_AllocatedDate);
            DeleteTempData(AppCommon.TMP_ReturnAndRefundStatus);
            DeleteTempData(AppCommon.TMP_DateRange);
            DeleteTempData(AppCommon.TMP_ClientMaster);
        }
        public TempFilterModel GetAllTempFilter()
        {
            TempFilterModel model = new TempFilterModel();
            model.SearchText = GetDataFromTemp(AppCommon.TMP_SearchText);
            model.DateRange = GetDataFromTemp(AppCommon.TMP_DateRange);
            model.Value_AllocatedTo = GetDataFromTemp(AppCommon.TMP_AllocatedTo);
            model.Value_AllocatedDate = GetDataFromTemp(AppCommon.TMP_AllocatedDate);
            return model;
        }
        public void SetDataInTemp(string TempDataKey, string data)
        {
            TempData[TempDataKey] = null;
            TempData[TempDataKey] = data;
            KeepTempData(TempDataKey);
        }
        public string GetDataFromTemp(string TempDataKey)
        {
            string data = "";
            if (TempData[TempDataKey] != null)
            {
                data = TempData[TempDataKey].ToString();
                KeepTempData(TempDataKey);
            }
            return data;
        }
        public void KeepTempData(string TempDataKey)
        {
            if (TempData[TempDataKey] != null)
                TempData.Keep();
        }
        public void DeleteTempData(string TempDataKey)
        {
            TempData[TempDataKey] = null;
        }
        #endregion
    }
}
