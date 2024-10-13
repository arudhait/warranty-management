using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IAllocationProvider
    {
        DatatablePageResponseModel<EnggMastModel> GetList(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProvider);
        EnggMastModel GetById(int id);
        DatatablePageResponseModel<TerritoryAllocationModel> GetDistrictStateList(int enggId, DatatablePageRequestModel datatablePageRequest);
        TerritoryAllocationModel GetDistrictState(int id, int allocationId);
        ResponseModel SaveDistrictState(TerritoryAllocationModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id);
    }
}
