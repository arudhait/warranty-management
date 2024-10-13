using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IDue_ExpiredWarrantyProvider
    {
        DatatablePageResponseModel<WarrantyDetailsModel> GetExpiredList(DatatablePageRequestModel datatablePageRequest);
        DatatablePageResponseModel<WarrantyDetailsModel> GetDueList(DatatablePageRequestModel datatablePageRequest);
        WarrantyDetailsModel GetById(int id);

        DatatablePageResponseModel<ModelDetailModel> GetModelList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
        DatatablePageResponseModel<ProbDetailModel> GetProbList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
    }
}
