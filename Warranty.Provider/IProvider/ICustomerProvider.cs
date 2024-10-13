using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface ICustomerProvider
    {
        DatatablePageResponseModel<CustMastModel> GetList(DatatablePageRequestModel datatablePageRequest);
        CustMastModel GetById(int id);
        ResponseModel Save(CustMastModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
