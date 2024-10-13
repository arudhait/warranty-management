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
    public class SupplierMasterProvider : ISupplierMasterProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public SupplierMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Method
        public DatatablePageResponseModel<SupplierMasterModel> GetSupplierMasterList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<SupplierMasterModel> model = new DatatablePageResponseModel<SupplierMasterModel>
            {
                data = new List<SupplierMasterModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from s in unitOfWork.SupplierMaster.GetAll()
                                select new SupplierMasterModel()
                                {
                                    SupplierMasterId = s.SupplierMasterId,
                                    SupplierName = s.SupplierName,
                                    EmailId = s.EmailId,
                                    ContactNo =s.ContactNo,
                                    Address = s.Address,
                                    StateId = s.StateId,
                                    StateCityPin = $"{s.State.StateName}, {s.City}, {s.ZipCode}",
                                    ProductMatserId = s.ProductMatserId,
                                    ProductMatserName = s.ProductMatser.ProductName,
                                    SupplierSku = s.SupplierSku,
                                    ProductPrice = s.ProductPrice,
                                    IsActive = s.IsActive,
                                    CreatedBy = s.CreatedBy,
                                    CreatedByName = s.CreatedByNavigation.UserName,
                                    CreatedOn = s.CreatedOn,
                                    CreatedOnDate = s.CreatedOn.ToString(AppCommon.DateOnlyFormat),
                                    UpdatedBy = s.UpdatedBy,
                                    UpdatedOn = s.UpdatedOn,
                                }).ToList();

                model.recordsTotal = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.SupplierName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())||
                    x.EmailId.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.SupplierMasterId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "SupplyMasterProvider=>GetProductList");
            }
            return model;
        }


        public SupplierMasterModel GetById(int id)
        {
            SupplierMasterModel model = new SupplierMasterModel();
            try
            {
                var data = unitOfWork.SupplierMaster.GetAll(x => x.SupplierMasterId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<SupplierMasterModel>(data);
                    model.EncId = _commonProvider.Protect(id);

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "SupplyMasterProvider=>GetById");
            }
            return model;
        }

        public ResponseModel Save(SupplierMasterModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.SupplierMasterId = _commonProvider.UnProtect(inputModel.EncId);
                if (unitOfWork.SupplierMaster.Any(x => x.SupplierMasterId != inputModel.SupplierMasterId && x.SupplierName== inputModel.SupplierName))
                {
                    model.IsSuccess = false;
                    model.Message = "SupplierMaster Detail already exists with this name/email address";
                    return model;
                }
                var _temp = unitOfWork.SupplierMaster.GetAll(x => x.SupplierMasterId == inputModel.SupplierMasterId).FirstOrDefault();
                SupplierMaster tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.SupplierMaster.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "SupplierMaster data added successfully";
                }
                else
                {
                    unitOfWork.SupplierMaster.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "SupplierMaster data updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "SupplyMasterProvider=>Save");
            }
            return model;
        }

        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                SupplierMaster supplierMaster = unitOfWork.SupplierMaster.GetAll(x => x.SupplierMasterId == id).FirstOrDefault();
                if (supplierMaster != null)
                {

                    returnResult.Message = "Supplier data deleted successfully.";
                    supplierMaster.IsActive = false;
                    unitOfWork.SupplierMaster.Update(supplierMaster, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Supplier data was not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "SupplyMasterProvider=>Delete");
            }
            return returnResult;
        }
        #endregion
    }
}
