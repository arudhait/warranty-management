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
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class AllocationProvider : IAllocationProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public AllocationProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion
        #region Methods
        public DatatablePageResponseModel<EnggMastModel> GetList(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProvider)
        {
            DatatablePageResponseModel<EnggMastModel> model = new DatatablePageResponseModel<EnggMastModel>
            {
                data = new List<EnggMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                // Retrieve RoleId and EnggId from session
                int roleId = sessionProvider.RoleId;
                int enggId = sessionProvider.EnggId;

                // Log the role and engineer information for debugging purposes
                Console.WriteLine($"RoleId from sessionProvider: {roleId}, EnggId from sessionProvider: {enggId}");

                // Base query to fetch all engineers
                var listData = (from p in unitOfWork.EnggMast.GetAll()
                                select new EnggMastModel()
                                {
                                    EnggId = p.EnggId,
                                    EnggName = p.EnggName,
                                    IsActive = p.IsActive,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDatestring = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    Ip = p.Ip,
                                }).AsQueryable();

                // Filter based on RoleId
                if (roleId == 3) // If the user is a ServiceEngineer
                {
                    listData = listData.Where(x => x.EnggId == enggId); // Only show data assigned to the logged-in engineer
                }

                // Get the total record count before applying filters
                model.recordsTotal = listData.Count();

                // Apply search filter if needed
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                        x.EnggName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    );
                }

                // Get the filtered record count
                model.recordsFiltered = listData.Count();

                // Apply sorting
                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                {
                    listData = listData.OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection);
                }

                // Apply pagination
                model.data = listData.Skip(datatablePageRequest.StartIndex)
                                     .Take(datatablePageRequest.PageSize)
                                     .ToList()
                                     .Select(x =>
                                     {
                                         x.EncId = _commonProvider.Protect(x.EnggId);
                                         return x;
                                     }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "AllocationProvider=>GetList");
            }

            return model;
        }



        public EnggMastModel GetById(int id)
        {
            EnggMastModel model = new EnggMastModel();
            try
            {
                var data = unitOfWork.EnggMast.GetAll(x => x.EnggId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<EnggMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                    model.EnggName = data.EnggName;

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "AllocationProvider=>GetById");
            }
            return model;
        }



        #endregion

        #region DistrictState Methods
        public DatatablePageResponseModel<TerritoryAllocationModel> GetDistrictStateList(int enggId, DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<TerritoryAllocationModel> model = new DatatablePageResponseModel<TerritoryAllocationModel>
            {
                data = new List<TerritoryAllocationModel>(),
                draw = datatablePageRequest.Draw
            };
            try
            {
                var listData = (from s in unitOfWork.TerritoryAllocation.GetAll(x => x.EnggId == enggId)
                                select new TerritoryAllocationModel()
                                {
                                    AlloctionId = s.AlloctionId,
                                    EnggId = s.EnggId,
                                    DistrictId = s.DistrictId,
                                    DistrictName = s.District.DistrictName,
                                    StateId = s.StateId,
                                    StateName = s.State.StateName
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    //listData = listData.Where(x =>
                    //    x.ProductName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    //).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.AlloctionId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "Due_ExpiredWarrantyProvider=>GetList");
            }
            return model;
        }
        public TerritoryAllocationModel GetDistrictState(int id,int allocationId)
        {
            TerritoryAllocationModel model = new TerritoryAllocationModel() { EnggId = id };
            try
            {
                var data = unitOfWork.TerritoryAllocation.GetAll(x => x.AlloctionId == allocationId).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<TerritoryAllocationModel>(data);

                    model.EncId = _commonProvider.Protect(id);
                    model.EnggId = id;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider => GetBuyProduct");
            }
            return model;
        }
        public ResponseModel SaveDistrictState(TerritoryAllocationModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {

                var _temp = unitOfWork.TerritoryAllocation.GetAll(x => x.AlloctionId == inputModel.AlloctionId).FirstOrDefault();
                TerritoryAllocation tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.TerritoryAllocation.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Territory Allocation added successfully";
                }
                else
                {
                    unitOfWork.TerritoryAllocation.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Territory Allocation Details updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "WarrantyListProvider => SaveModelDatils");
            }
            return model;
        }

        public ResponseModel Delete(int id)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                TerritoryAllocation territoryAllocation = unitOfWork.TerritoryAllocation.GetAll(x => x.AlloctionId == id).FirstOrDefault();
                if (territoryAllocation != null)
                {

                    returnResult.Message = "Territory Allocation deleted successfully.";
                    unitOfWork.TerritoryAllocation.Delete(territoryAllocation);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "TerritoryAllocation record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "ProductMasterProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
