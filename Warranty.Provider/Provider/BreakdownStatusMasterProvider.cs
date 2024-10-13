using AutoMapper;
using System.Linq.Dynamic.Core;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class BreakdownStatusMasterProvider : IBreakdownStatusMasterProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BreakdownStatusMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<BreakdownStatusMastModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<BreakdownStatusMastModel> model = new DatatablePageResponseModel<BreakdownStatusMastModel>
            {
                data = new List<BreakdownStatusMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.BreakdownStatusMast.GetAll()
                                select new BreakdownStatusMastModel()
                                {
                                    BreakdownStatusId = p.BreakdownStatusId,
                                    BreakdownStatusName = p.BreakdownStatusName,
                                    IsActive = (bool)p.IsActive,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDatestring = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.BreakdownStatusName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.BreakdownStatusId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "BreakdownStatusMasterProvider=>GetList");
            }
            return model;
        }
        public BreakdownStatusMastModel GetById(int id)
        {
            BreakdownStatusMastModel model = new BreakdownStatusMastModel();
            try
            {
                var data = unitOfWork.BreakdownStatusMast.GetAll(x => x.BreakdownStatusId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<BreakdownStatusMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "BreakdownStatusMasterProvider=>GetById");
            }
            return model;
        }
        public ResponseModel Save(BreakdownStatusMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.BreakdownStatusId = (short)_commonProvider.UnProtect(inputModel.EncId);

                if (unitOfWork.BreakdownStatusMast.Any(x => x.BreakdownStatusId != inputModel.BreakdownStatusId && x.BreakdownStatusName == inputModel.BreakdownStatusName))
                {
                    model.IsSuccess = false;
                    model.Message = "Breakdown Status already exists with this name/email address";
                    return model;
                }
                var _temp = unitOfWork.BreakdownStatusMast.GetAll(x => x.BreakdownStatusId == inputModel.BreakdownStatusId).FirstOrDefault();
                BreakdownStatusMast tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.BreakdownStatusMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Breakdown Status added successfully";
                }
                else
                {
                    unitOfWork.BreakdownStatusMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Breakdown Status updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "BreakdownStatusMasterProvider=>Save");
            }
            return model;
        }
        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                BreakdownStatusMast breakdownList = unitOfWork.BreakdownStatusMast.GetAll(x => x.BreakdownStatusId == id).FirstOrDefault();
                if (breakdownList != null)
                {

                    returnResult.Message = "Breakdown Status deleted successfully.";
                    breakdownList.IsActive = false;
                    unitOfWork.BreakdownStatusMast.Update(breakdownList, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Breakdown Status record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "BreakdownStatusMasterProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
