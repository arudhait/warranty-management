using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IWarrantyListProvider
    {
        #region Warranty Method
        DatatablePageResponseModel<WarrantyDetailsModel> GetWarrantyList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate);
       
        WarrantyDetailsModel GetById(int id, bool IsAdd);
        ResponseModel Save(WarrantyDetailsModel inputModel, SessionProviderModel sessionProvider);
        #endregion

        #region Model Details
        DatatablePageResponseModel<ModelDetailModel> GetModelList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
        ModelDetailModel GetModel(int id, int modelDetId);
        ResponseModel SaveModelDatils(ModelDetailModel inputModel, SessionProviderModel sessionProvider);
        #endregion

        #region Prob Details
        DatatablePageResponseModel<ProbDetailModel> GetProbList(int warrantyId, DatatablePageRequestModel datatablePageRequest);
        ProbDetailModel GetProb(int id, int probId);
        ResponseModel SaveProbDatils(ProbDetailModel inputModel, SessionProviderModel sessionProvider);
        #endregion
    }
}
