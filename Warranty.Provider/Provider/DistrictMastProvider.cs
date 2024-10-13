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
using Warranty.Repository.Models;
using Warranty.Repository.Repository;


namespace Warranty.Provider.Provider
{
    public class DistrictMastProvider : IDistrictMastProvider
    {
        #region Variable
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion 

        #region Constructor
        public DistrictMastProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Method
        public DatatablePageResponseModel<DistrictMastModel> GetDistrictDetailList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<DistrictMastModel> model = new DatatablePageResponseModel<DistrictMastModel>
            {
                data = new List<DistrictMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from di in unitOfWork.DistrictMast.GetAll()
                                select new DistrictMastModel()
                                {
                                    DistrictId = di.DistrictId,
                                    DistrictName = di.DistrictName,
                                    StateId = di.StateId,
                                    StateName = di.State.StateName,
                                    CreatedBy = di.CreatedBy,
                                    CreatedDate = di.CreatedDate,
                                    CreatedDateName = di.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    UpdatedBy = di.UpdatedBy,
                                    UpdatedDate = di.UpdatedDate,
                                    IsActive = di.IsActive,
                                    Ip = di.Ip,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.DistrictName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                     x.StateName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.DistrictId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "DistrictMastProvider=>GetList");
            }
            return model;
        }


        public DistrictMastModel GetById(int id)
        {
            DistrictMastModel model = new DistrictMastModel();
            try
            {
                var data = unitOfWork.DistrictMast.GetAll(x => x.DistrictId== id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<DistrictMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "DistrictMastProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(DistrictMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.DistrictId = _commonProvider.UnProtect(inputModel.EncId);

                if (unitOfWork.DistrictMast.Any(x => x.DistrictId != inputModel.DistrictId && x.DistrictName.Equals(inputModel.DistrictName, StringComparison.OrdinalIgnoreCase)))
                {
                    model.IsSuccess = false;
                    model.Message = "District Detail already exists";
                    return model;
                }

                var _temp = unitOfWork.DistrictMast.GetAll(x => x.DistrictId == inputModel.DistrictId).FirstOrDefault();

                inputModel.DistrictName = inputModel.DistrictName.ToUpperInvariant();

                DistrictMast tableData = _mapper.Map(inputModel, _temp);

                if (_temp == null)
                {
                    unitOfWork.DistrictMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "District added successfully";
                }
                else
                {
                    unitOfWork.DistrictMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "District updated successfully";
                }

                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "DistrictMastProvider=>Save");
            }
            return model;
        }


        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
               DistrictMast DistrictMast = unitOfWork.DistrictMast.GetAll(x => x.DistrictId== id).FirstOrDefault();
                if (DistrictMast != null)
                {

                    returnResult.Message = "District Detail deleted successfully.";
                    DistrictMast.IsActive = false;
                    unitOfWork.DistrictMast.Update(DistrictMast, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "District record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "DistrictMastProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
