using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IStateMastProvider
    {
        DatatablePageResponseModel<StateMastModel> GetStateDetailList(DatatablePageRequestModel datatablePageRequest);

        StateMastModel GetById(int id);

        ResponseModel Save(StateMastModel inputModel, SessionProviderModel sessionProvider);

        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
