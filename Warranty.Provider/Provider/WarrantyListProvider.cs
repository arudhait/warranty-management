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
using Warranty.Repository.ADO;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;
using Box.V2.Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace Warranty.Provider.Provider
{
    public class WarrantyListProvider : IWarrantyListProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private DBConnectivity db = new DBConnectivity();
        #endregion

        #region Constructor
        public WarrantyListProvider(IMapper mapper, ICommonProvider commonProvider)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region  Warranty Method
        public DatatablePageResponseModel<WarrantyDetailsModel> GetWarrantyList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<WarrantyDetailsModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                from c in contractJoin.DefaultIfEmpty()
                                where p.StartDate.Date >= startDate.Date && p.EndDate.Date <= endDate.Date
                                select new WarrantyDetailsModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    CustId = p.CustId,
                                    DoctorName = p.Cust.DoctorName,
                                    SellingDate = p.SellingDate,
                                    SellingDateString = p.SellingDate.ToString(AppCommon.DateOnlyFormat),
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    InstalledBy = p.InstalledBy,
                                    InstalledByString = p.InstalledByNavigation.EnggName,
                                    CrmNo = p.CrmNo,
                                    NoOfServices = p.NoOfServices,
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDateString = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = (short?)(c != null ? c.ContractTypeId : (int?)null), // Handle null for ContractType
                                    ContractTypeName = c != null ? c.ContractType.ContractTypeName : string.Empty, // Handle null for ContractTypeName
                                }).ToList();
                }
                else
                {
                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                from c in contractJoin.DefaultIfEmpty()
                                select new WarrantyDetailsModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    CustId = p.CustId,
                                    DoctorName = p.Cust.DoctorName,
                                    SellingDate = p.SellingDate,
                                    SellingDateString = p.SellingDate.ToString(AppCommon.DateOnlyFormat),
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    InstalledBy = p.InstalledBy,
                                    InstalledByString = p.InstalledByNavigation.EnggName,
                                    CrmNo = p.CrmNo,
                                    NoOfServices = p.NoOfServices,
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedDateString = p.CreatedDate.ToString(AppCommon.DateOnlyFormat),
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = (short?)(c != null ? c.ContractTypeId : (int?)null), // Handle null for ContractType
                                    ContractTypeName = c != null ? c.ContractType.ContractTypeName : string.Empty, // Handle null for ContractTypeName
                                }).ToList();

                }
                model.recordsTotal = listData.Count();
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                    x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.WarrantyId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider=>GetProductList");
            }
            return model;
        }
        public WarrantyDetailsModel GetById(int id, bool IsAdd)
        {
            WarrantyDetailsModel model = new WarrantyDetailsModel();
            try
            {
                // Step 1: Fetch Warranty data (including ContractTypeId) from Warranty table
                var warrantyData = unitOfWork.WarrantyDet.GetAll(x => x.WarrantyId == id).FirstOrDefault();

                if (warrantyData != null)
                {
                    // Map warranty data to the WarrantyDetailsModel
                    model = _mapper.Map<WarrantyDetailsModel>(warrantyData);

                    // Protect the ID (for security purposes if needed)
                    model.EncId = _commonProvider.Protect(id);

                    model.StartDate = warrantyData.StartDate;
                    model.EndDate = warrantyData.EndDate;
                    model.SellingDate = warrantyData.SellingDate;
                    model.DoctorName = warrantyData.Cust.DoctorName;

                    // Step 2: Check if there is a ContractDet entry that has this WarrantyId and a valid ContractTypeId
                    var contractDetails = unitOfWork.ContractDet
                                            .GetAll(x => x.WarrantyId == warrantyData.WarrantyId && x.ContractTypeId != null)
                                            .FirstOrDefault();

                    if (contractDetails != null)
                    {
                        model.ContractDetModel = _mapper.Map<ContractDetModel>(contractDetails);
                    }
                    else
                    {
                        model.ContractDetModel = null; // Or set default values if preferred
                    }
                }
                else
                {
                    model.StartDate = DateTime.Today;
                    model.EndDate = DateTime.Today;
                    model.SellingDate = DateTime.Today;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider=>GetById");
            }

            return model;
        }
        public ResponseModel Save(WarrantyDetailsModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(inputModel.EncId))
                    inputModel.WarrantyId = _commonProvider.UnProtect(inputModel.EncId);

                if (!unitOfWork.WarrantyDet.Any(x => x.CustId == inputModel.CustId))
                {
                    model.IsSuccess = false;
                    model.Message = "Please select Doctor Name";
                    return model;
                }

                if (unitOfWork.WarrantyDet.Any(x => x.WarrantyId != inputModel.WarrantyId && x.CrmNo == inputModel.CrmNo))
                {
                    model.IsSuccess = false;
                    model.Message = "Warranty already exists with this name/email address";
                    return model;
                }

                var _temp = unitOfWork.WarrantyDet.GetAll(x => x.WarrantyId == inputModel.WarrantyId).FirstOrDefault();
                WarrantyDet tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.WarrantyDet.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Warranty added successfully";
                }
                else
                {
                    unitOfWork.WarrantyDet.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Warranty updated successfully";
                }
                unitOfWork.Save();
                model.IsSuccess = true;

                model.Result = tableData.WarrantyId;

                // Check if ContractTypeId is valid (greater than 0 or non-null, depending on your model)
                if (inputModel.ContractDetModel.ContractTypeId != null && inputModel.ContractDetModel.ContractTypeId > 0)
                {
                    // Check if ContractDet exists for the ContractId
                    var existingContract = unitOfWork.ContractDet.GetAll(x => x.WarrantyId == tableData.WarrantyId).FirstOrDefault();

                    if (existingContract != null)
                    {
                        // Update the existing contract record
                        existingContract.ContractTypeId = (short)inputModel.ContractDetModel.ContractTypeId;
                        existingContract.Amount = inputModel.ContractDetModel.Amount;
                        existingContract.ChequeDet = inputModel.ContractDetModel.ChequeDet;
                        existingContract.InvoiceNo = inputModel.ContractDetModel.InvoiceNo;
                        existingContract.StartDate = inputModel.ContractDetModel.StartDate;
                        existingContract.EndDate = inputModel.ContractDetModel.EndDate;
                        existingContract.AmtExcludTax = inputModel.ContractDetModel.AmtExcludTax;
                        existingContract.NoOfService = inputModel.ContractDetModel.NoOfService;
                        existingContract.Interval = inputModel.ContractDetModel.Interval;
                        existingContract.TakenBy = inputModel.ContractDetModel.TakenBy;
                        existingContract.IsActive = true;
                        existingContract.CreatedBy = sessionProvider.UserId;
                        existingContract.CreatedDate = DateTime.Now;         

                        unitOfWork.ContractDet.Update(existingContract, sessionProvider.UserId, sessionProvider.Ip);
                        model.Message = "Contract updated successfully";

                        // Save the changes to ContractDet
                        unitOfWork.Save();
                    }
                    else
                    {
                        // Insert a new contract record if it doesn't exist
                        ContractDet contractData = new ContractDet
                        {
                            ContractTypeId = (short)inputModel.ContractDetModel.ContractTypeId,
                            WarrantyId = tableData.WarrantyId,
                            Amount = inputModel.ContractDetModel.Amount,
                            ChequeDet = inputModel.ContractDetModel.ChequeDet,
                            InvoiceNo = inputModel.ContractDetModel.InvoiceNo,
                            StartDate = inputModel.ContractDetModel.StartDate,
                            EndDate = inputModel.ContractDetModel.EndDate,
                            AmtExcludTax = inputModel.ContractDetModel.AmtExcludTax,
                            NoOfService = inputModel.ContractDetModel.NoOfService,
                            Interval = inputModel.ContractDetModel.Interval,
                            TakenBy = inputModel.ContractDetModel.TakenBy,
                            IsActive = true,
                            CreatedBy = sessionProvider.UserId,
                            CreatedDate = DateTime.Now
                        };

                        unitOfWork.ContractDet.Insert(contractData, sessionProvider.UserId, sessionProvider.Ip);
                        model.Message = "Contract added successfully";

                        // Save the changes to ContractDet
                        unitOfWork.Save();

                        int newContractId = (int)contractData.ContractId;
                        inputModel.ContractDetModel.ContractId = newContractId;
                    }
                }
                else
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = AppCommon.ErrorMessage;
                AppCommon.LogException(ex, "WarrantyListProvider=>Save");
            }
            return model;
        }

        #endregion


        #region Model Details
        public DatatablePageResponseModel<ModelDetailModel> GetModelList(int warrantyId, DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ModelDetailModel> model = new DatatablePageResponseModel<ModelDetailModel>
            {
                data = new List<ModelDetailModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var listData = (from s in unitOfWork.ModelDet.GetAll(x => x.WarrantyId == warrantyId)
                                select new ModelDetailModel()
                                {
                                    ModelDetId = s.ModelDetId,
                                    WarrantyId = s.WarrantyId,
                                    ModelId = s.ModelId,
                                    ModelNo = s.Model.ModelNo,
                                    ModelSerialNo = s.ModelSerialNo
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
                    x.EncId = _commonProvider.Protect((int)x.ModelDetId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider=>GetList");
            }
            return model;
        }
        public ModelDetailModel GetModel(int id, int modelDetId)
        {
            ModelDetailModel model = new ModelDetailModel() { WarrantyId = id };
            try
            {
                var data = unitOfWork.ModelDet.GetAll(x => x.ModelDetId == modelDetId).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<ModelDetailModel>(data);

                    model.EncId = _commonProvider.Protect(id);
                    model.WarrantyId = id;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider => GetBuyProduct");
            }
            return model;
        }
        public ResponseModel SaveModelDatils(ModelDetailModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var warrantydata = unitOfWork.WarrantyDet.GetAll(x => x.WarrantyId == inputModel.WarrantyId).FirstOrDefault();
                if (warrantydata == null)
                {
                    model.IsSuccess = false;
                    model.Message = "Warranty Det not found!";
                    return model;
                }
                var _temp = unitOfWork.ModelDet.GetAll(x => x.ModelDetId == inputModel.ModelDetId).FirstOrDefault();
                ModelDet tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.ModelDet.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Model Details added successfully";
                }
                else
                {
                    unitOfWork.ModelDet.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Model Details updated successfully";
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
        #endregion


        #region Product Methods
        public DatatablePageResponseModel<ProbDetailModel> GetProbList(int warrantyId, DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ProbDetailModel> model = new DatatablePageResponseModel<ProbDetailModel>
            {
                data = new List<ProbDetailModel>(),
                draw = datatablePageRequest.Draw
            };
            try
            {
                var listData = (from s in unitOfWork.ProbDet.GetAll(x => x.WarrantyId == warrantyId)
                                select new ProbDetailModel()
                                {
                                    ProbId = s.ProbId,
                                    WarrantyId = s.WarrantyId,
                                    ProbName = s.ProbName,
                                    ProbSerialNo = s.ProbSerialNo
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
                    x.EncId = _commonProvider.Protect((int)x.ProbId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "Due_ExpiredWarrantyProvider=>GetList");
            }
            return model;
        }
        public ProbDetailModel GetProb(int id, int probId)
        {
            ProbDetailModel model = new ProbDetailModel() { WarrantyId = id };
            try
            {
                var data = unitOfWork.ProbDet.GetAll(x => x.ProbId == probId).FirstOrDefault();
                if (data != null)
                {
                    model = _mapper.Map<ProbDetailModel>(data);

                    model.EncId = _commonProvider.Protect(id);
                    model.WarrantyId = id;
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "WarrantyListProvider => GetBuyProduct");
            }
            return model;
        }
        public ResponseModel SaveProbDatils(ProbDetailModel inputModel, SessionProviderModel sessionProvider)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var warrantydata = unitOfWork.WarrantyDet.GetAll(x => x.WarrantyId == inputModel.WarrantyId).FirstOrDefault();
                if (warrantydata == null)
                {
                    model.IsSuccess = false;
                    model.Message = "Warranty Det not found!";
                    return model;
                }
                var _temp = unitOfWork.ProbDet.GetAll(x => x.ProbId == inputModel.ProbId).FirstOrDefault();
                ProbDet tableData = _mapper.Map(inputModel, _temp);
                if (_temp == null)
                {
                    unitOfWork.ProbDet.Insert(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Prob Details added successfully";
                }
                else
                {
                    unitOfWork.ProbDet.Update(tableData, sessionProvider.UserId, sessionProvider.Ip);
                    model.Message = "Prob Details updated successfully";
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

        #endregion
    }
}
