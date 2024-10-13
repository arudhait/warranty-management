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
    public class ProductMasterProvider : IProductMasterProvider
    {
        #region Variable
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion 

        #region Constructor
        public ProductMasterProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<ProductMasterModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ProductMasterModel> model = new DatatablePageResponseModel<ProductMasterModel>
            {
                data = new List<ProductMasterModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.ProductMaster.GetAll()
                                select new ProductMasterModel()
                                {
                                    ProductMasterId = p.ProductMasterId,
                                    ProductName = p.ProductName,
                                    Qty = p.Qty,
                                    Price = p.Price,
                                    Sku = p.Sku,
                                    BatchNo = p.BatchNo,
                                    Size = p.Size,
                                    Description = p.Description,
                                    Warranty = p.Warranty,
                                    IsActive = p.IsActive,
                                    CraetedDate = p.CraetedDate,
                                    CreatedOnString = p.CraetedDate.ToString(AppCommon.DateOnlyFormat),
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
                    x.EncId = _commonProvider.Protect(x.ProductMasterId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ProductMasterProvider=>GetList");
            }
            return model;
        }
        public ResponseModel Save(ProductMasterModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.ProductMasterId = _commonProvider.UnProtect(inputModel.EncId);
                if (unitOfWork.ProductMaster.Any(x => x.ProductMasterId != inputModel.ProductMasterId && x.ProductName == inputModel.ProductName))
                {
                    model.IsSuccess = false;
                    model.Message = "Product already exists with this name/email address";
                    return model;
                }
                var _temp = unitOfWork.ProductMaster.GetAll(x => x.ProductMasterId == inputModel.ProductMasterId).FirstOrDefault();
                ProductMaster tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    tableData.CraetedDate = DateTime.Now;
                    unitOfWork.ProductMaster.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Product added successfully";
                }
                else
                {
                    unitOfWork.ProductMaster.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Product updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "EnginnerProvider=>Save");
            }
            return model;
        }

        public ProductMasterModel GetById(int id)
        {
            ProductMasterModel model = new ProductMasterModel();
            try
            {
                var data = unitOfWork.ProductMaster.GetAll(x => x.ProductMasterId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<ProductMasterModel>(data);                 
                    model.EncId = _commonProvider.Protect(id);

                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ProductMasterProvider=>GetById");
            }
            return model;
        }
        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                ProductMaster product = unitOfWork.ProductMaster.GetAll(x => x.ProductMasterId == id).FirstOrDefault();
                if (product != null)
                {

                    returnResult.Message = "Product deleted successfully.";
                    product.IsActive = false;
                    unitOfWork.ProductMaster.Update(product, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "product record does not found.";
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
