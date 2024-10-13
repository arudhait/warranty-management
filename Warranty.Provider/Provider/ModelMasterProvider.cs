using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.ADO;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class ModelMasterProvider : IModelMasterProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public ModelMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<ModelMasterModel> GetModelMasterList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ModelMasterModel> model = new DatatablePageResponseModel<ModelMasterModel>
            {
                data = new List<ModelMasterModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from l in unitOfWork.ModelMast.GetAll()
                                select new ModelMasterModel()
                                {
                                    ModelId = l.ModelId,

                                    
                                    ModelNo = l.ModelNo,
                                    IsActive = l.IsActive,
                                    CreatedDate = l.CreatedDate,
                                    CreatedOnString = l.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = l.CreatedBy,
                                    CreatedByName = l.CreatedByNavigation.UserName,
                                    Ip = l.Ip,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.ModelNo.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) 
                   
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.ModelId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ScheduleProvider=>GetList");
            }
            return model;
        }

        public ModelMasterModel GetById(int id)
        {
            ModelMasterModel model = new ModelMasterModel();
            try
            {
                var data = unitOfWork.ModelMast.GetAll(x => x.ModelId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<ModelMasterModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ScheduleProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(ModelMasterModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.ModelId = _commonProvider.UnProtect(inputModel.EncId);

                var _temp = unitOfWork.ModelMast.GetAll(x => x.ModelId == inputModel.ModelId).FirstOrDefault();
                ModelMast tableData = _mapper.Map(inputModel, _temp);

                if (_temp == null)
                {
                    unitOfWork.ModelMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Model Master added successfully";
                }
                else
                {
                    unitOfWork.ModelMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Model Master updated successfully";
                }

                unitOfWork.Save();


                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = ex.Message;
            }

            return model;
        }

        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                ModelMast product = unitOfWork.ModelMast.GetAll(x => x.ModelId == id).FirstOrDefault();
                if (product != null)
                {

                    returnResult.Message = "Model Master deleted successfully.";
                    product.IsActive = false;
                    unitOfWork.ModelMast.Update(product, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Model Master record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "ScheduleProvider=>Delete");
            }
            return returnResult;
        }
        #endregion

    }
}
