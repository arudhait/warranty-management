using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IModelMasterProvider
    {
        DatatablePageResponseModel<ModelMasterModel> GetModelMasterList(DatatablePageRequestModel datatablePageRequest);
        ModelMasterModel GetById(int id);
        ResponseModel Save(ModelMasterModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
