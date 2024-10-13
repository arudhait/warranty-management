using AutoMapper;
using System.Linq;
using System.Linq.Dynamic.Core;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class AMC_CMCExpiredContractProvider : IAMC_CMCExpiredContractProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private readonly IMapper _mapper;
        private readonly ISessionManager _sessionManager;
        #endregion

        #region Constructor
        public AMC_CMCExpiredContractProvider(IMapper mapper, ICommonProvider commonProvider, ISessionManager sessionManager)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
            _sessionManager = sessionManager;
        }
        #endregion

        #region Methods
      

        public DatatablePageResponseModel<ContractDetModel> GetExpiredList(DatatablePageRequestModel datatablePageRequest)
        {
            var model = new DatatablePageResponseModel<ContractDetModel>
            {
                data = new List<ContractDetModel>(),
                draw = datatablePageRequest.Draw
            };

            // Get the selected year and month or default to the current year and month
            int selectedYear = datatablePageRequest.Year != default(int) ? datatablePageRequest.Year : DateTime.Now.Year;
            int selectedMonth = datatablePageRequest.Month != default(int) ? datatablePageRequest.Month : DateTime.Now.Month;

            // Fetch all data as a List
            List<ContractDetModel> listData = (from c in unitOfWork.ContractDet.GetAll()
                                               join p in unitOfWork.WarrantyDet.GetAll() on c.WarrantyId equals p.WarrantyId into contractGroup
                                               from p in contractGroup.DefaultIfEmpty()
                                               where c.EndDate.Month == selectedMonth && c.EndDate.Year == selectedYear // Fetching EndDate from ContractDet table
                                               select new ContractDetModel
                                               {
                                                   ContractId = c.ContractId,
                                                   WarrantyId = p.WarrantyId, // Handling null case if no match is found
                                                   DoctorName = p != null ? p.Cust.DoctorName : string.Empty,
                                                   StartDate = (DateTime)(p != null ? p.StartDate : (DateTime?)null),
                                                   StartDateString = p != null ? p.StartDate.ToString(AppCommon.DateOnlyFormat) : string.Empty,
                                                   EndDate = (DateTime)c.EndDate, // Using EndDate from ContractDet table
                                                   EndDateString = c.EndDate.ToString(AppCommon.DateOnlyFormat),
                                                   Interval = p.Interval,
                                                   CreatedDate = p != null ? p.CreatedDate : (DateTime?)null,
                                                   TakenBy = c.TakenBy,
                                                   CreatedBy = p != null ? p.CreatedBy : 0,
                                                   CreatedByName = p != null && p.CreatedByNavigation != null ? p.CreatedByNavigation.UserName : string.Empty,
                                                   ContractTypeId = c.ContractTypeId,
                                                   ContractTypeName = c.ContractType != null ? c.ContractType.ContractTypeName : string.Empty
                                               }).ToList(); // Fetching as List

            // Filter by engineer ID if applicable
            var engineerId = _sessionManager.EnggId;
            if (engineerId > 0)
            {
                listData = listData.Where(p => p.TakenBy == engineerId).ToList(); // Convert to List after filtering
            }

            // Apply year and month filter if provided
            if (datatablePageRequest.Year > 0 && datatablePageRequest.Month > 0)
            {
                DateTime startDate = new DateTime(datatablePageRequest.Year, datatablePageRequest.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1); // End of the month

                listData = listData.Where(p => p.EndDate >= startDate && p.EndDate <= endDate).ToList(); // Convert to List after filtering
            }

            // Count total records
            model.recordsTotal = listData.Count();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(datatablePageRequest.SearchText))
            {
                var searchTextLower = datatablePageRequest.SearchText.ToLower();
                listData = listData.Where(x => x.DoctorName.ToLower().Contains(searchTextLower)).ToList(); // Convert to List after filtering
            }

            // Count filtered records
            model.recordsFiltered = listData.Count();

            // Apply sorting if provided
            if (!string.IsNullOrWhiteSpace(datatablePageRequest.SortColumnName) && !string.IsNullOrWhiteSpace(datatablePageRequest.SortDirection))
            {
                // Use reflection to dynamically sort the list
                var propertyInfo = typeof(ContractDetModel).GetProperty(datatablePageRequest.SortColumnName);
                if (propertyInfo != null)
                {
                    // Perform sorting using LINQ
                    if (datatablePageRequest.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        listData = listData.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        listData = listData.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                    }
                }
            }

            // Paginate the results
            model.data = listData.Skip(datatablePageRequest.StartIndex)
                                 .Take(datatablePageRequest.PageSize)
                                 .Select(x =>
                                 {
                                     x.EncId = _commonProvider.Protect((int)x.WarrantyId);
                                     return x;
                                 }).ToList(); // Final conversion to List for data

            return model;
        }



        public DatatablePageResponseModel<ContractDetModel> GetDueList(DatatablePageRequestModel datatablePageRequest)
        {
            DatatablePageResponseModel<ContractDetModel> model = new DatatablePageResponseModel<ContractDetModel>
            {
                data = new List<ContractDetModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                // Get the selected year and month or default to the current year and month
                int selectedYear = datatablePageRequest.Year != default(int) ? datatablePageRequest.Year : DateTime.Now.Year;
                int selectedMonth = datatablePageRequest.Month != default(int) ? datatablePageRequest.Month : DateTime.Now.Month;

                DateTime selStartDate = new DateTime(selectedYear, selectedMonth, 1);
                DateTime selEndDate = new DateTime(selectedYear, selectedMonth, DateTime.DaysInMonth(selectedYear, selectedMonth));

                List<ContractDetModel> listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                                   join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                                   from c in contractJoin.DefaultIfEmpty()
                                                   where c.EndDate.Date >= selStartDate && c.StartDate.Date <= selEndDate // Ensure end date is current or later
                                                   select new ContractDetModel()
                                                   {
                                                       ContractId = c.ContractId,
                                                       ContractTypeId = c.ContractTypeId,
                                                       Amount = c.Amount,
                                                       ChequeDet = c.ChequeDet,
                                                       InvoiceNo = c.InvoiceNo,
                                                       AmtExcludTax = c.AmtExcludTax,
                                                       IsActive = c.IsActive,
                                                       WarrantyId = c.WarrantyId,
                                                       DoctorName = p.Cust.DoctorName, // Fetching DoctorName from WarrantyDet
                                                       StartDate = c.StartDate,
                                                       StartDateString = c.StartDate.ToString(AppCommon.DateOnlyFormat),
                                                       EndDate = c.EndDate,
                                                       EndDateString = c.EndDate.ToString(AppCommon.DateOnlyFormat),
                                                       Interval = c.Interval,
                                                       NoOfService = c.NoOfService,
                                                       CreatedDate = c.CreatedDate,
                                                       TakenBy = c.TakenBy,
                                                       CreatedBy = c.CreatedBy,
                                                       CreatedByName = c.CreatedByNavigation.UserName,
                                                       ContractTypeName = c.ContractType.ContractTypeName
                                                   }).ToList();



               

                // Create a final list to store filtered results
                List<ContractDetModel> finalList = new List<ContractDetModel>();

                foreach (var warranty in listData)
                {
                    DateTime intervalStartDate = warranty.StartDate;
                    DateTime intervalEndDate = warranty.EndDate;

                    if (intervalEndDate.Year == selectedYear && intervalEndDate.Month == selectedMonth)
                    {
                        ContractDetModel newModel = new ContractDetModel();
                        newModel = _mapper.Map<ContractDetModel>(warranty);
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
                            ContractDetModel newModel = new ContractDetModel();
                            newModel = _mapper.Map<ContractDetModel>(warranty);
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
                                    ContractDetModel newModel = new ContractDetModel();
                                    // mapping
                                    newModel = _mapper.Map<ContractDetModel>(warranty);
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
                    finalList = finalList.Where(p => p.TakenBy == engineerId).ToList();
                }

                // Filter based on search text
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    finalList = finalList.Where(x =>
                        x.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())).ToList();
                }

                model.recordsFiltered = finalList.Count;

                // Handle sorting
                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                {
                    finalList = finalList.AsQueryable()
                                         .OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection)
                                         .ToList();
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
