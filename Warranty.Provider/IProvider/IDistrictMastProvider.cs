using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IDistrictMastProvider
    {
        DatatablePageResponseModel<DistrictMastModel> GetDistrictDetailList(DatatablePageRequestModel datatablePageRequest);

        DistrictMastModel GetById(int id);

        ResponseModel Save(DistrictMastModel inputModel, SessionProviderModel sessionProvider);

        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
    }
}
