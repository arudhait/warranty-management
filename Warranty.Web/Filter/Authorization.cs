using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;

namespace Warranty.Web.Filter
{
    public class Authorization : Attribute, IAuthorizationFilter
    {
        public short PageId { get; set; }
        public short[] Roles { get; set; }
        public short IsInvoice { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionManager = (ISessionManager)context.HttpContext.RequestServices.GetService(typeof(ISessionManager));
            var _CommonProvider = (ICommonProvider)context.HttpContext.RequestServices.GetService(typeof(ICommonProvider));
            bool isUnauthorized = false;
            int userId = sessionManager.UserId;
            int roleId = sessionManager.RoleId;
            if (userId == 0 || roleId == 0)
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.Result = new ObjectResult(new { error = "Forbidden" })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Index" } });
                    return;
                }

            }

            if (PageId > 0 && roleId > 0)
            {
                if (roleId == (int)Enumeration.Role.SuperAdmin)
                {
                    if (roleId == (int)Enumeration.Role.SuperAdmin)
                        return;

                    else if (PageId == (short)Enumeration.AppPages.UserMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Engineer && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Allocation && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.BreakDownList && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.BreakDownStatusMast && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.ContractList && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.ContractTypeMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Customer && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.DistrictMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Due_ExpriredAMC_CMCContract && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Due_ExpiredWarranty && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.Ledger && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.InwardOutward && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.ProductMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.StateMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.SupplierMaster && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                    else if (PageId == (short)Enumeration.AppPages.WarrantyList && roleId == (int)Enumeration.Role.SuperAdmin)
                        isUnauthorized = false;
                 

                    else if (roleId == (int)Enumeration.Role.SuperAdmin || roleId == (int)Enumeration.Role.ServiceEngineer)
                        isUnauthorized = false;

                }
                else if (roleId == (int)Enumeration.Role.SuperAdmin || roleId == (int)Enumeration.Role.ServiceEngineer && PageId > 0)
                {
                    if (PageId == (short)Enumeration.AppPages.Dashboard)
                        isUnauthorized = false;
                    if (PageId == (short)Enumeration.AppPages.Allocation)
                        isUnauthorized = false;
                }
                else
                    isUnauthorized = !_CommonProvider.IsHaveAccess(PageId, roleId);
            }
            else
            {
                isUnauthorized = true;
            }



            if (isUnauthorized)
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Unauthorized" }, { "action", "Index" } });
                    return;
                }
            }

        }

        public static bool IsAjaxRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            else
                return false;
        }
    }
}
