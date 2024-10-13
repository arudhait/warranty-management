using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IAMC_CMCExpiredContractProvider
    {
        DatatablePageResponseModel<ContractDetModel> GetExpiredList(DatatablePageRequestModel datatablePageRequest);
        DatatablePageResponseModel<ContractDetModel> GetDueList(DatatablePageRequestModel datatablePageRequest);
        WarrantyDetailsModel GetById(int id);

        DatatablePageResponseModel<ModelDetailModel> GetModelList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
        DatatablePageResponseModel<ProbDetailModel> GetProbList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
    }
}
