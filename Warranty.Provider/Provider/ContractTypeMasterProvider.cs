using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.ADO;
using Warranty.Repository.Models;
using System.Linq.Dynamic.Core;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class ContractTypeMasterProvider : IContractTypeMasterProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public ContractTypeMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<ContractTypeMasterModel> GetContractTypeMasterList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ContractTypeMasterModel> model = new DatatablePageResponseModel<ContractTypeMasterModel>
            {
                data = new List<ContractTypeMasterModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from l in unitOfWork.ContractTypeMast.GetAll()
                                select new ContractTypeMasterModel()
                                {
                                    ContractTypeId = l.ContractTypeId,
                                    ContractTypeName = l.ContractTypeName,
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
                    x.ContractTypeName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.ContractTypeId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ContractTypeMasterProvider=>GetList");
            }
            return model;
        }

        public ContractTypeMasterModel GetById(int id)
        {
            ContractTypeMasterModel model = new ContractTypeMasterModel();
            try
            {
                var data = unitOfWork.ContractTypeMast.GetAll(x => x.ContractTypeId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<ContractTypeMasterModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ContractTypeMasterProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(ContractTypeMasterModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.ContractTypeId = (short)_commonProvider.UnProtect(inputModel.EncId);

                var _temp = unitOfWork.ContractTypeMast.GetAll(x => x.ContractTypeId == inputModel.ContractTypeId).FirstOrDefault();
                ContractTypeMast tableData = _mapper.Map(inputModel, _temp);

                if (_temp == null)
                {
                    unitOfWork.ContractTypeMast.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Contract Type Master added successfully";
                }
                else
                {
                    unitOfWork.ContractTypeMast.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Contract Type Master updated successfully";
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
                ContractTypeMast product = unitOfWork.ContractTypeMast.GetAll(x => x.ContractTypeId == id).FirstOrDefault();
                if (product != null)
                {

                    returnResult.Message = "Contract Type Master deleted successfully.";
                    product.IsActive = false;
                    unitOfWork.ContractTypeMast.Update(product, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Contract Type Master record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "ContractTypeMasterProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
