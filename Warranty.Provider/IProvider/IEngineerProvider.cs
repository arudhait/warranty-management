using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IEngineerProvider
    {
        DatatablePageResponseModel<EnggMastModel> GetList(DatatablePageRequestModel datatablePageRequest);
        EnggMastModel GetById(int id);
        EnggMastModel GetByAllocationId(int id);
        ResponseModel SaveAllocation(EnggMastModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Save(EnggMastModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
