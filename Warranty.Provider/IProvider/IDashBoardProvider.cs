using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IDashBoardProvider
    {
        DatatablePageResponseModel<CustMastModel> GetSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);
        DatatablePageResponseModel<WarrantyDetailsModel> GetDueSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);
        DatatablePageResponseModel<WarrantyDetailsModel> GetExpiredSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);
        DatatablePageResponseModel<ContractDetModel> GetDueAMCCMSSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);
        DatatablePageResponseModel<ContractDetModel> GetExpiredAMCCMSSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);


        DashBoardModel GetAllCount(SessionProviderModel sessionProviderModel);
        // DashBoardModel GetContractCount(SessionProviderModel sessionProviderModel);

        DatatablePageResponseModel<TerritoryAllocationModel> GetDistrictStateList(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProvider);
    }
}
