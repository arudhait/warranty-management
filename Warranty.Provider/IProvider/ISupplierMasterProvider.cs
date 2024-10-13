using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface ISupplierMasterProvider
    {
        DatatablePageResponseModel<SupplierMasterModel> GetSupplierMasterList(DatatablePageRequestModel datatablePageRequest);
        SupplierMasterModel GetById(int id);
        ResponseModel Save(SupplierMasterModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);

    }
}
