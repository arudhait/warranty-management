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
    public class StateMastProvider : IStateMastProvider
    {
        #region Variable
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion 

        #region Constructor
        public StateMastProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region IStateMastProvider
        public DatatablePageResponseModel<StateMastModel> GetStateDetailList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<StateMastModel> model = new DatatablePageResponseModel<StateMastModel>
            {
                data = new List<StateMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from s in unitOfWork.StateMast.GetAll()
                                select new StateMastModel()
                                {
                                    StateId = s.StateId,
                                    StateName = s.StateName,
                                    CreatedBy = s.CreatedBy,
                                    CreatedDate = s.CreatedDate,
                                    IsActive = s.IsActive,
                                    CreatedDateName = s.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    UpdatedBy = s.UpdatedBy,
                                    UpdatedDate = s.UpdatedDate,
                                    Ip = s.Ip

                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.StateName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) 
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.StateId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "StateMastProvider=>GetList");
            }
            return model;
        }


        public StateMastModel GetById(int id)
        {
            StateMastModel model = new StateMastModel();
            try
            {
                var data = unitOfWork.StateMast.GetAll(x => x.StateId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<StateMastModel>(data);
                    model.EncId = _commonProvider.Protect(id);

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "StateMastProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(StateMastModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                int stateid = 0;
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    stateid = _commonProvider.UnProtect(inputModel.EncId);
                if (unitOfWork.StateMast.Any(x => x.StateId!= stateid && x.StateName == inputModel.StateName))
                {
                    model.IsSuccess = false;
                    model.Message = "State Detail already exists with this name/email address";
                    return model;
                }
                short StateId = (short)stateid;
                var _temp = unitOfWork.StateMast.GetAll(x => x.StateId == stateid).FirstOrDefault();
                inputModel.StateName = inputModel.StateName.ToUpperInvariant();
                StateMast tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.StateMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "State List added successfully";
                }
                else
                {
                    unitOfWork.StateMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    tableData.StateId = StateId;
                    model.Message = "State List updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "StateMastProvider=>Save");
            }
            return model;
        }

        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                StateMast StateMast = unitOfWork.StateMast.GetAll(x => x.StateId == id).FirstOrDefault();
                if (StateMast != null)
                {

                    returnResult.Message = "State List deleted successfully.";
                    StateMast.IsActive = false;
                    unitOfWork.StateMast.Update(StateMast, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "State List was not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "StateMastProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
