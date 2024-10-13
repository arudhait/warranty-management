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
using Warranty.Repository.ADO;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class BreakDownListProvider : IBreakDownListProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public BreakDownListProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Method

        public DatatablePageResponseModel<BreakdownDetModel> GetBreakdownListDetailList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<BreakdownDetModel> model = new DatatablePageResponseModel<BreakdownDetModel>
            {
                data = new List<BreakdownDetModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<BreakdownDetModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    listData = (from b in unitOfWork.BreakdownDet.GetAll()
                                where b.CreatedDate.Date >= startDate.Date && b.CreatedDate.Date <= endDate.Date
                                select new BreakdownDetModel()
                                {
                                    BreakdownId = b.BreakdownId,
                                    CustId = b.CustId,
                                    DoctorName = b.Cust.DoctorName,
                                    CallRegDate = b.CallRegDate,
                                    CallRegDateString = b.CallRegDate.ToString(AppCommon.DateOnlyFormat),
                                    TypeId = b.TypeId,
                                    BreakdownType = b.Type.BreakdownStatusName,
                                    EnggId = b.EnggId,
                                    EngineerName = b.Engg.EnggName,
                                    EnggFirstVisitDate = b.EnggFirstVisitDate,
                                    EnggFirstVisitDateString = b.EnggFirstVisitDate.ToString(AppCommon.DateOnlyFormat),
                                    CrmNo = b.CrmNo,
                                    Problems = b.Problems,
                                    ReqAction = b.ReqAction,
                                    ReqActionName = b.ActionTakenNavigation.ActionName,
                                    ActionTaken = b.ActionTaken,
                                    ActionTakenName = b.ActionTakenNavigation.ActionName,
                                    Conclusion = b.Conclusion,
                                    IsActive = b.IsActive,
                                    CreatedDate = b.CreatedDate,
                                    CreatedDateString = b.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = b.CreatedBy,
                                    CreatedByName = b.CreatedByNavigation.UserName,
                                    UpdatedBy = b.UpdatedBy,
                                    UpdatedDate = b.UpdatedDate
                                }).ToList();
                }
                else
                {
                     listData = (from b in unitOfWork.BreakdownDet.GetAll()
                                select new BreakdownDetModel()
                                {
                                    BreakdownId = b.BreakdownId,
                                    CustId = b.CustId,
                                    DoctorName = b.Cust.DoctorName,
                                    CallRegDate = b.CallRegDate,
                                    CallRegDateString = b.CallRegDate.ToString(AppCommon.DateOnlyFormat),
                                    TypeId = b.TypeId,
                                    BreakdownType = b.Type.BreakdownStatusName,
                                    EnggId = b.EnggId,
                                    EngineerName = b.Engg.EnggName,
                                    EnggFirstVisitDate = b.EnggFirstVisitDate,
                                    EnggFirstVisitDateString = b.EnggFirstVisitDate.ToString(AppCommon.DateOnlyFormat),
                                    CrmNo = b.CrmNo,
                                    Problems = b.Problems,
                                    ReqAction = b.ReqAction,
                                    ReqActionName = b.ReqActionNavigation.ActionName,
                                    ActionTaken = b.ActionTaken,
                                    ActionTakenName = b.ActionTakenNavigation.ActionName ,
                                    Conclusion = b.Conclusion,
                                    IsActive = b.IsActive,
                                    CreatedDate = b.CreatedDate,
                                    CreatedDateString = b.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = b.CreatedBy,
                                    CreatedByName = b.CreatedByNavigation.UserName,
                                    UpdatedBy = b.UpdatedBy,
                                    UpdatedDate = b.UpdatedDate
                                }).ToList();
                }

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                    x.EngineerName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                if (!string.IsNullOrEmpty(datatablePageRequest.ExtraSearch))
                {
                    int status = Convert.ToInt32(datatablePageRequest.ExtraSearch);
                    if (status == 1)
                    {
                        listData = listData.Where(x => x.Conclusion == 1).ToList();
                    }
                    else if (status == 2)
                    {
                        listData = listData.Where(x => x.Conclusion == 2).ToList();
                    }
                }
            
                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.ProtectInt64(x.BreakdownId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "DashboardProvider=>GetProductList");
            }
            return model;
        }

        public BreakdownDetModel GetById(int id)
        {
            BreakdownDetModel model = new BreakdownDetModel();
            try
            {
                var data = unitOfWork.BreakdownDet.GetAll(x => x.BreakdownId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<BreakdownDetModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                    model.CustName = data.Cust.DoctorName;
                    model.EngineerName = data.Engg.EnggName;

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "BreakDownListProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(BreakdownDetModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.BreakdownId = (short)_commonProvider.UnProtect(inputModel.EncId);

                var _temp = unitOfWork.BreakdownDet.GetAll(x => x.BreakdownId == inputModel.BreakdownId).FirstOrDefault();
                BreakdownDet tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.BreakdownDet.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Breakdown List added successfully";
                }
                else
                {
                    unitOfWork.BreakdownDet.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Breakdown List updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "BreakDownListProvider=>Save");
            }
            return model;
        }

        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                BreakdownDet breakdownStatus = unitOfWork.BreakdownDet.GetAll(x => x.BreakdownId == id).FirstOrDefault();
                if (breakdownStatus != null)
                {

                    returnResult.Message = "Breakdown List deleted successfully.";
                    breakdownStatus.IsActive = false;
                    unitOfWork.BreakdownDet.Update(breakdownStatus, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Breakdown List record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "BreakDownListProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
