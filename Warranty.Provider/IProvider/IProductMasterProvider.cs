using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IProductMasterProvider
    {
        DatatablePageResponseModel<ProductMasterModel> GetList(DatatablePageRequestModel datatablePageRequest);
        ResponseModel Save(ProductMasterModel inputModel, SessionProviderModel sessionProvider);
        ProductMasterModel GetById(int id);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
