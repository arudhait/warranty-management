using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class LedgerProvider : ILedgerProvider
    {
        #region Variable
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion 

        #region Constructor
        public LedgerProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<LedgerModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<LedgerModel> model = new DatatablePageResponseModel<LedgerModel>
            {
                data = new List<LedgerModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.Ledger.GetAll()
                                select new LedgerModel()
                                {
                                    LedgerId = p.LedgerId,
                                    ProductMasterId = p.ProductMasterId,
                                    ProductName = p.ProductMaster.ProductName,
                                    Qty = p.Qty,
                                    Price = p.Price,
                                    Type = p.Type,
                                    TypeData = p.Type ? "Inward" : "Outward",
                                    Date = p.Date,
                                    DateString = p.Date.ToString(AppCommon.DateOnlyFormat),
                                    IsCredit = p.IsCredit,
                                    Remarks = p.Remarks,
                                    InwardOutwardItemId = p.InwardOutwardItemId,
                                    CreatedOn = p.CreatedOn,
                                    CreatedOnString = p.CreatedOn.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.ProductName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.LedgerId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "LedgerProvider=>GetList");
            }
            return model;
        }
        #endregion
    }
}
