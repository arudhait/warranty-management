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
    public class InwardOutwardProvider : IInwardOutwardProvider
    {

        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public InwardOutwardProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region Inward / Outward
        public DatatablePageResponseModel<InwardOutwardModel> GetList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<InwardOutwardModel> model = new DatatablePageResponseModel<InwardOutwardModel>
            {
                data = new List<InwardOutwardModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from p in unitOfWork.InwardOutward.GetAll()
                                select new InwardOutwardModel()
                                {
                                    InwardOutwardId = p.InwardOutwardId,
                                    CustId = p.CustId,
                                    SupplierMasterId = p.SupplierMasterId,
                                    IsType = p.IsType,
                                    Date = p.Date,
                                    DateOnString = p.Date.ToString(AppCommon.DateOnlyFormat),
                                    Note = p.Note,
                                    IsActive = p.IsActive,
                                    CreatedOn = p.CreatedOn,
                                    CreatedOnstring = p.CreatedOn.ToString(AppCommon.DateOnlyFormat),
                                    UpdatedBy = p.UpdatedBy,
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    Ip = p.Ip,
                                }).ToList();

                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.Note.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect(x.InwardOutwardId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "InwardOutwardProvider=>GetList");
            }
            return model;
        }
        public InwardOutwardModel GetById(int id)
        {
            InwardOutwardModel model = new InwardOutwardModel();
            try
            {
                var data = unitOfWork.InwardOutward.GetAll(x => x.InwardOutwardId == id).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<InwardOutwardModel>(data);
                    model.EncId = _commonProvider.Protect(id);
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "InwardOutwardProvider=>GetById");
            }
            return model;
        }
        public ResponseModel Save(InwardOutwardModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.InwardOutwardId = _commonProvider.UnProtect(inputModel.EncId);

                // Check for duplicate records only for other records with the same Note
                if (unitOfWork.InwardOutward.Any(x => x.InwardOutwardId != inputModel.InwardOutwardId && x.Note == inputModel.Note))
                {
                    model.IsSuccess = false;
                    model.Message = "Inward / Outward already exists with this note.";
                    return model;
                }

                var _temp = unitOfWork.InwardOutward.GetAll(x => x.InwardOutwardId == inputModel.InwardOutwardId).FirstOrDefault();
                InwardOutward tableData = _mapper.Map(inputModel, _temp);

                if (_temp == null)
                {
                    // Insert new record
                    unitOfWork.InwardOutward.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Inward / Outward added successfully";
                }
                else
                {
                    // Update existing record
                    unitOfWork.InwardOutward.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Inward / Outward updated successfully";
                }

                unitOfWork.Save();

                model.Result = tableData.InwardOutwardId;
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "InwardOutwardProvider=>Save");
            }

            return model;
        }

        public ResponseModel Delete(int id, SessionProviderModel sessionProvider)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                InwardOutward enginner = unitOfWork.InwardOutward.GetAll(x => x.InwardOutwardId == id).FirstOrDefault();
                if (enginner != null)
                {

                    returnResult.Message = "Inward / Outward deleted successfully.";
                    enginner.IsActive = false;
                    unitOfWork.InwardOutward.Update(enginner, sessionProvider.UserId, sessionProvider.Ip);
                    unitOfWork.Save();
                    returnResult.IsSuccess = true;
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Inward / Outward record does not found.";
                }
            }

            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "InwardOutwardProvider=>Delete");
            }
            return returnResult;
        }
        #endregion

        #region Inward / Outward Item
        public DatatablePageResponseModel<InwardOutwardItemModel> GetItemList(int inwardOutwardId, DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<InwardOutwardItemModel> model = new DatatablePageResponseModel<InwardOutwardItemModel>
            {
                data = new List<InwardOutwardItemModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from s in unitOfWork.InwardOutwardItem.GetAll(x => x.InwardOutwardId == inwardOutwardId)
                                select new InwardOutwardItemModel()
                                {
                                    InwardOutwardId = s.InwardOutwardId,
                                    InwardOutwardItemId = s.InwardOutwardItemId,
                                    ProductMasterId = s.ProductMasterId,
                                    ProductName = s.ProductMaster.ProductName,
                                    Price = s.Price,
                                    Qty = s.Qty,
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
                    x.EncId = _commonProvider.Protect((int)x.InwardOutwardItemId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "InwardOutwardProvider=>GetList");
            }
            return model;
        }
        public InwardOutwardItemModel GetItem(int id, int inwardOutwardItemId)
        {
            InwardOutwardItemModel model = new InwardOutwardItemModel() { InwardOutwardId = id };
            try
            {
                var data = unitOfWork.InwardOutwardItem.GetAll(x => x.InwardOutwardItemId == inwardOutwardItemId).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<InwardOutwardItemModel>(data);

                    model.EncId = _commonProvider.Protect(id);
                    model.InwardOutwardId = id;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "InwardOutwardProvider => GetBuyProduct");
            }
            return model;
        }
        public ResponseModel SaveItem(InwardOutwardItemModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var inwardData = unitOfWork.InwardOutward.GetAll(x => x.InwardOutwardId == inputModel.InwardOutwardId).FirstOrDefault();
                if (inwardData == null)
                {
                    model.IsSuccess = false;
                    model.Message = "Inward / Outward Id is not found!";
                    return model;
                }

                var inwardOutwardItem = unitOfWork.InwardOutwardItem.GetAll(x => x.InwardOutwardItemId == inputModel.InwardOutwardItemId).FirstOrDefault();
                InwardOutwardItem tableData = _mapper.Map(inputModel, inwardOutwardItem);

                if (inwardOutwardItem == null)
                {
                    unitOfWork.InwardOutwardItem.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Inward / Outward Item added successfully";
                }
                else
                {
                    unitOfWork.InwardOutwardItem.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Inward / Outward Item updated successfully";
                }

                unitOfWork.Save();

                Ledger ledgerEntry = new Ledger
                {
                    ProductMasterId = tableData.ProductMasterId,
                    Qty = (short)(inwardData.IsType ? tableData.Qty : -tableData.Qty), 
                    Price = tableData.Price,
                    InwardOutwardItemId = tableData.InwardOutwardItemId, 
                    CreatedBy = sessionProvider.UserId,
                    Ip = sessionProvider.Ip,
                    CreatedOn = DateTime.Now,
                    Date = DateTime.Now,
                    Remarks = inwardData.IsType ? "Inward Entry" : "Outward Entry",
                    IsCredit = inwardData.IsType, 
                    Type = inwardData.IsType        
                };

                unitOfWork.Ledger.Insert(ledgerEntry, sessionProvider.UserId, sessionProvider.Ip);
                unitOfWork.Save();

                model.IsSuccess = true;
                model.Message = "Ledger entry successfully added.";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "InwardOutwardProvider => SaveModelDetails");
            }

            return model;
        }
        
        public ResponseModel DeleteItem(int id)
        {
            ResponseModel returnResult = new ResponseModel();
            try
            {
                // Fetch the InwardOutwardItem by ID
                InwardOutwardItem inwardOutwardItem = unitOfWork.InwardOutwardItem
                    .GetAll(x => x.InwardOutwardItemId == id)
                    .FirstOrDefault();

                if (inwardOutwardItem != null)
                {
                    // Store the InwardOutwardItemId in a variable
                    int inwardOutwardItemId = (int)inwardOutwardItem.InwardOutwardItemId;

                    // Fetch the corresponding entry from the Ledger table using the InwardOutwardItemId
                    Ledger ledgerItem = unitOfWork.Ledger
                        .GetAll(x => x.InwardOutwardItemId == inwardOutwardItemId)
                        .FirstOrDefault();

                    if (ledgerItem != null)
                    {
                        // Delete the record from the Ledger table
                        unitOfWork.Ledger.Delete(ledgerItem);

                        unitOfWork.Save();
                    }

                    // Delete the InwardOutwardItem itself
                    unitOfWork.InwardOutwardItem.Delete(inwardOutwardItem);
                    unitOfWork.Save();

                    returnResult.IsSuccess = true;
                    returnResult.Message = "Inward / Outward Item and associated Ledger entry deleted successfully.";
                }
                else
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "Inward / Outward Item record not found.";
                }
            }
            catch (Exception ex)
            {
                returnResult.IsSuccess = false;
                returnResult.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "InwardOutwardProvider=>Delete");
            }

            return returnResult;
        }

        public decimal GetRate(int productMasterId)
        {
            var product = unitOfWork.ProductMaster.GetAll(x => x.ProductMasterId == productMasterId).FirstOrDefault();

            if (product != null)
            {
                return product.Price;
            }
            else
            {
                throw new InvalidOperationException("Product not found for the given ProductMasterId.");
            }
        }
        #endregion
    }
}
