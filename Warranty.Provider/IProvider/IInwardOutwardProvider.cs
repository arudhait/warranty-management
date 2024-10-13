using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IInwardOutwardProvider
    {
        #region Inward / Outward
        DatatablePageResponseModel<InwardOutwardModel> GetList(DatatablePageRequestModel datatablePageRequest);
        InwardOutwardModel GetById(int id);
        ResponseModel Save(InwardOutwardModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel Delete(int id, SessionProviderModel sessionProvider);
        #endregion

        #region Inward / Outward Item
        DatatablePageResponseModel<InwardOutwardItemModel> GetItemList(int inwardOutwardId, DatatablePageRequestModel datatablePageRequest);
        InwardOutwardItemModel GetItem(int id, int inwardOutwardItemId);
        ResponseModel SaveItem(InwardOutwardItemModel inputModel, SessionProviderModel sessionProvider);
        ResponseModel DeleteItem(int id);
        decimal GetRate(int productMasterId);
        #endregion
    }
}
