using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IContractTypeMasterProvider
    {
        DatatablePageResponseModel<ContractTypeMasterModel> GetContractTypeMasterList(DatatablePageRequestModel datatablePageRequest);
        ContractTypeMasterModel GetById(int id);
        ResponseModel Save(ContractTypeMasterModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);

    }
}
