using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IBreakdownStatusMasterProvider
    {
        DatatablePageResponseModel<BreakdownStatusMastModel> GetList(DatatablePageRequestModel datatablePageRequest);
        BreakdownStatusMastModel GetById(int id);
        ResponseModel Save(BreakdownStatusMastModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
