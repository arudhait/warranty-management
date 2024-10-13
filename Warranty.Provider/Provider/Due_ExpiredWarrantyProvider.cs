using AutoMapper;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Linq;
using System.Linq.Dynamic.Core;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Models;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class Due_ExpiredWarrantyProvider : IDue_ExpiredWarrantyProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private readonly ISessionManager _sessionManager;
        #endregion

        #region Constructor
        public Due_ExpiredWarrantyProvider(IMapper mapper, ICommonProvider commonProvider, ISessionManager sessionManager)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
            _sessionManager = sessionManager;
        }
        #endregion

        #region Methods
        public DatatablePageResponseModel<WarrantyDetailsModel> GetExpiredList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            IQueryable<WarrantyDetailsModel> listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                                         join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractGroup
                                                         from c in contractGroup.DefaultIfEmpty()
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
                                                             ContractTypeId = c != null ? c.ContractTypeId : null // Handle potential null
                                                         }).AsQueryable();
            var engineerId = _sessionManager.EnggId;
            if (engineerId > 0)
            {
                listData = listData.Where(p => p.InstalledBy == engineerId).AsQueryable();
            }
            if (datatablePageRequest.Year != default(int) && datatablePageRequest.Month != default(int))
            {
                listData = listData.Where(p =>
                    p.EndDate.Year == datatablePageRequest.Year &&
                    p.EndDate.Month == datatablePageRequest.Month).AsQueryable();
            }
            else
            {
                listData = listData.Where(p =>
                p.EndDate.Month == DateTime.Now.Month && p.EndDate.Year == DateTime.Now.Year).AsQueryable();
            }
            model.recordsTotal = listData.Count();
            // Apply search filter if provided
            if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
            {
                listData = listData.Where(x =>
                    x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()));
            }
            model.recordsFiltered = listData.Count();
            // Apply sorting if provided
            if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
            {
                listData = listData.OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection);
            }
            // Paginate the results
            model.data = listData.Skip(datatablePageRequest.StartIndex)
                                 .Take(datatablePageRequest.PageSize)
                                 .ToList() // Fetch data as List
                                 .Select(x =>
                                 {
                                     x.EncId = _commonProvider.Protect((int)x.WarrantyId);
                                     return x;
                                 }).ToList();
            return model;
        }
        public DatatablePageResponseModel<WarrantyDetailsModel> GetDueList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                // Get the selected year and month or default to the current year and month
                int selectedYear = datatablePageRequest.Year != default(int) ? datatablePageRequest.Year : DateTime.Now.Year;
                int selectedMonth = datatablePageRequest.Month != default(int) ? datatablePageRequest.Month : DateTime.Now.Month;

                DateTime selStartDate = new DateTime(selectedYear, selectedMonth, 1);
                DateTime selEndDate = new DateTime(selectedYear, selectedMonth, DateTime.DaysInMonth(selectedYear, selectedMonth));

                // Fetch warranty data
                List<WarrantyDetailsModel> listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                                       join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                                       from c in contractJoin.DefaultIfEmpty()
                                                       where p.EndDate.Date >= selStartDate && p.StartDate <= selEndDate // Ensure end date is current or later
                                                       && p.NoOfServices > 0
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
                                                           ContractTypeId = (short?)(c != null ? c.ContractTypeId : (int?)null),
                                                           ContractTypeName = c != null ? c.ContractType.ContractTypeName : string.Empty,
                                                       }).ToList();

                

                // Create a final list to store filtered results
                List<WarrantyDetailsModel> finalList = new List<WarrantyDetailsModel>();

                foreach (var warranty in listData)
                {
                    DateTime intervalStartDate = warranty.StartDate;
                    DateTime intervalEndDate = warranty.EndDate;

                    if (intervalEndDate.Year == selectedYear && intervalEndDate.Month == selectedMonth)
                    {
                        WarrantyDetailsModel newModel = new WarrantyDetailsModel();
                        newModel = _mapper.Map<WarrantyDetailsModel>(warranty);
                        newModel.DueDate = intervalStartDate;
                        finalList.Add(newModel);
                    }
                    else
                    {
                        // Calculate the number of months for the warranty duration
                        int numberOfMonths = ((intervalEndDate.Year - intervalStartDate.Year) * 12) + intervalEndDate.Month - intervalStartDate.Month;

                        int monthsToAdd = (int)(numberOfMonths / (Convert.ToInt32(warranty.Interval) > 0 ? Convert.ToInt32(warranty.Interval) : 1));
                        intervalStartDate = intervalStartDate.AddMonths(monthsToAdd);
                        if (intervalStartDate.Year == selectedYear && intervalStartDate.Month == selectedMonth)
                        {
                            WarrantyDetailsModel newModel = new WarrantyDetailsModel();
                            newModel = _mapper.Map<WarrantyDetailsModel>(warranty);
                            newModel.DueDate = intervalStartDate;
                            finalList.Add(newModel);
                        }
                        else
                        {
                            for (int i = 2; i < Convert.ToInt32(warranty.Interval); i++)
                            {
                                // Increment the interval start date by the calculated interval
                                intervalStartDate = intervalStartDate.AddMonths(monthsToAdd);

                                // Check if the interval month matches the selected month and year
                                if (intervalStartDate.Year == selectedYear && intervalStartDate.Month == selectedMonth)
                                {
                                    WarrantyDetailsModel newModel = new WarrantyDetailsModel();
                                    // mapping
                                    newModel = _mapper.Map<WarrantyDetailsModel>(warranty);
                                    newModel.DueDate = intervalStartDate;
                                    finalList.Add(newModel);
                                    break; // Found a matching warranty, break to avoid duplicates
                                }
                            }
                        }
                    }
                }

                model.recordsTotal = finalList.Count; // Store the total warranty count in recordsTotal

                var engineerId = _sessionManager.EnggId;
                if (engineerId > 0)
                {
                    finalList = finalList.Where(p => p.InstalledBy == engineerId).ToList();
                }

               

                model.recordsFiltered = finalList.Count;

                // Handle sorting
                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                {
                    finalList = finalList.AsQueryable()
                                         .OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection)
                                         .ToList();
                }
                // Filter based on search text
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    finalList = finalList.Where(x =>
                        x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())).ToList();
                }

                // Prepare final data for the response
                model.data = finalList.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.WarrantyId);
                    return x;
                }).ToList();


                
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "Due_ExpiredWarrantyProvider=>GetList");
            }

            return model;
        }
        public WarrantyDetailsModel GetById(int id)
        {
            WarrantyDetailsModel model = new WarrantyDetailsModel();
            try
            {

                var warrantyData = unitOfWork.WarrantyDet.GetAll(x => x.WarrantyId == id).FirstOrDefault();

                if (warrantyData != null)
                {

                    model = _mapper.Map<WarrantyDetailsModel>(warrantyData);


                    model.EncId = _commonProvider.Protect(id);


                    var contractDetails = unitOfWork.ContractDet
                                            .GetAll(x => x.WarrantyId == warrantyData.WarrantyId && x.ContractTypeId != null)
                                            .FirstOrDefault();

                    if (contractDetails != null)
                    {
                        model.ContractDetModel = _mapper.Map<ContractDetModel>(contractDetails);
                    }
                    else
                    {
                        model.ContractDetModel = null;
                    }
                }
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "Due_ExpiredWarrantyProvider=>GetById");
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
                AppCommon.LogException(ex, "Due_ExpiredWarrantyProvider=>GetList");
            }
            return model;
        }
        #endregion

        #region Prob Details
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
        #endregion
    }
}
