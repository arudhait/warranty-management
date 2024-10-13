using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using System.Linq.Dynamic.Core;
using Warranty.Provider.IProvider;
using Warranty.Repository.ADO;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class ContractListProvider : IContractListProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public ContractListProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods


        public DatatablePageResponseModel<WarrantyDetailsModel> GetContractList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<WarrantyDetailsModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId
                                where p.StartDate.Date >= startDate.Date && p.EndDate.Date <= endDate.Date
                                select new WarrantyDetailsModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    CustId = p.CustId,
                                    DoctorName = p.Cust.DoctorName,
                                    SellingDate = p.SellingDate,
                                    SellingDateString = p.SellingDate.ToString(AppCommon.DateOnlyFormat),
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    InstalledBy = p.InstalledBy,
                                    InstalledByString = p.InstalledByNavigation.EnggName,
                                    CrmNo = p.CrmNo,
                                    NoOfServices = p.NoOfServices,
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDateString = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = c.ContractTypeId,
                                    ContractTypeName = c.ContractType.ContractTypeName,
                                }).ToList();
                }
                else
                {
                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId
                                select new WarrantyDetailsModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    CustId = p.CustId,
                                    DoctorName = p.Cust.DoctorName,
                                    SellingDate = p.SellingDate,
                                    SellingDateString = p.SellingDate.ToString(AppCommon.DateOnlyFormat),
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    InstalledBy = p.InstalledBy,
                                    InstalledByString = p.InstalledByNavigation.EnggName,
                                    CrmNo = p.CrmNo,
                                    NoOfServices = p.NoOfServices,
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDateString = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = c.ContractTypeId,
                                    ContractTypeName = c.ContractType.ContractTypeName,
                                }).ToList();


                }
                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

                    ).ToList();
                }

                model.recordsFiltered = listData.Count();


                if (!string.IsNullOrEmpty(datatablePageRequest.ExtraSearch))
                {
                    int status = Convert.ToInt32(datatablePageRequest.ExtraSearch);
                    if (status == 1)
                    {
                        listData = listData.Where(x => x.ContractTypeId == 1).ToList();
                    }
                    else if (status == 2)
                    {
                        listData = listData.Where(x => x.ContractTypeId == 2).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.WarrantyId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "DashboardProvider=>GetProductList");
            }
            return model;
        }
        #endregion
    }
}
