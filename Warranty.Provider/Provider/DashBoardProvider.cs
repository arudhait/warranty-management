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
using Warranty.Repository.Models;
using Warranty.Repository.Repository;
using Warranty.Common.BussinessEntities;
using Microsoft.AspNetCore.Http;
using Azure;
using static Warranty.Common.Utility.Enumeration;

namespace Warranty.Provider.Provider
{
    
    public class DashBoardProvider : IDashBoardProvider
    {
        #region Variables
        private UnitOfWork unitOfWork = new UnitOfWork();
        private ICommonProvider _commonProvider;
        private object reorderLevelCount;
        private object criticalLevelCount;
        private readonly IMapper _mapper;
        private readonly ISessionManager _sessionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly WarrantyManagementWebContext _WarrantyManagementWebContext;
        #endregion

        #region Constructor
        public DashBoardProvider(IMapper mapper, ICommonProvider commonProvider, IHttpContextAccessor httpContextAccessor, ISessionManager sessionManager, WarrantyManagementWebContext context)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
            _sessionManager = sessionManager;
            _WarrantyManagementWebContext = context;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        public DatatablePageResponseModel<CustMastModel> GetSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<CustMastModel> model = new DatatablePageResponseModel<CustMastModel>
            {
                data = new List<CustMastModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<CustMastModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    listData = (from p in unitOfWork.CustMast.GetAll()
                                    // Apply the date range filter here
                                select new CustMastModel()
                                {
                                    CustId = p.CustId,
                                    DoctorName = p.DoctorName,
                                    DistrictName = p.District.DistrictName,
                                    HospitalName = p.HospitalName,
                                    MobileNo = p.MobileNo,
                                    Email = p.Email,



                                }).ToList();
                }
                else
                {
                    listData = (from p in unitOfWork.CustMast.GetAll()
                                select new CustMastModel()
                                {
                                    CustId = p.CustId,
                                    DoctorName = p.DoctorName,
                                    DistrictName = p.District.DistrictName,
                                    HospitalName = p.HospitalName,
                                    MobileNo = p.MobileNo,
                                    Email = p.Email,
                                }).ToList();
                }

                model.recordsTotal = listData.Count();
                //if (datatablePageRequest.ClientMasterId > 0)
                //    listData = listData.Where(x => x.CustId == datatablePageRequest.Cu).ToList();

                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(c =>
                        c.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                        c.HospitalName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                        c.DistrictName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }
                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.CustId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ClientProductMappingProvider=>GetList");
            }
            return model;
        }
        public DatatablePageResponseModel<WarrantyDetailsModel> GetDueSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                int superadmin = (int)Enumeration.Role.SuperAdmin;
                var engineerId = _sessionManager.EnggId; // Current engineer's ID
                IEnumerable<WarrantyDetailsModel> listData;

                // Prepare the base query
                var query = from p in unitOfWork.WarrantyDet.GetAll()
                            join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                            from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                            where p.EndDate.Year >= DateTime.Now.Year // Filter by EndDate for the current year
                                  && !(p.EndDate.Year == DateTime.Now.Year && p.EndDate.Month == DateTime.Now.Month) // Exclude current month
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
                            };

                // Check if the current user is SuperAdmin or not
                if (_sessionManager.RoleId == superadmin)
                {
                    // If SuperAdmin, get all records
                    listData = query.ToList();
                }
                else
                {
                    // If not SuperAdmin, filter by EnggId
                    listData = query.Where(p => p.InstalledBy == engineerId).ToList();
                }

                model.recordsTotal = listData.Count();
                model.totalDueCount = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(c =>
                        c.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.CustId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ClientProductMappingProvider=>GetList");
            }
            return model;
        }



        public DatatablePageResponseModel<WarrantyDetailsModel> GetExpiredSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<WarrantyDetailsModel> model = new DatatablePageResponseModel<WarrantyDetailsModel>
            {
                data = new List<WarrantyDetailsModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {             
                IEnumerable<WarrantyDetailsModel> listData;

                var currentDate = DateTime.Now;
                var currentMonth = currentDate.Month;
                var currentYear = currentDate.Year;

                // Prepare the base query
                listData = (from p in unitOfWork.WarrantyDet.GetAll()
                            join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractGroup
                            from c in contractGroup.DefaultIfEmpty() // Left join to include warranties without contracts
                            where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear // Filter for current month and year
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
                                ContractTypeId = c != null ? c.ContractTypeId : null // Add ContractTypeId, handling potential null
                            });

                // Determine role
                int superadmin = (int)Enumeration.Role.SuperAdmin;
                var engineerId = _sessionManager.EnggId; // Current engineer's ID

                // Check if the current user is SuperAdmin
                if (_sessionManager.RoleId == superadmin)
                {
                    // If SuperAdmin, use all records
                    listData = listData.ToList();
                }
                else
                {
                    // If not SuperAdmin, filter by EnggId
                    listData = listData.Where(p => p.InstalledBy == engineerId).ToList();
                }

                model.recordsTotal = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(c =>
                        c.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())
                    ).ToList();
                }

                model.recordsFiltered = listData.Count();

                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();

                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.CustId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ClientProductMappingProvider=>GetExpiredSearchList");
            }
            return model;
        }

        public DatatablePageResponseModel<ContractDetModel> GetDueAMCCMSSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<ContractDetModel> model = new DatatablePageResponseModel<ContractDetModel>
            {
                data = new List<ContractDetModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<ContractDetModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    var currentYear = DateTime.Now.Year;
                    var currentMonth = DateTime.Now.Month;

                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                 join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                 from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                                 where p.EndDate.Year >= currentYear // Filter by EndDate for the current year
                                       && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth) // Exclude current month
                                       && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC") // Filter for CMC or AMC
                                 select new ContractDetModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    DoctorName = p.Cust.DoctorName,
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    Interval = p.Interval,
                                    TakenBy = c.TakenBy,
                                    CreatedDate = p.CreatedDate,
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeName = c.ContractType.ContractTypeName, // Ensure ContractTypeName is fetched correctly
                                }).ToList();
                }
                else
                {
                    var currentDate = DateTime.Now;
                    var currentMonth = currentDate.Month;
                    var currentYear = currentDate.Year;
                    listData = (from p in unitOfWork.WarrantyDet.GetAll()
                                                         join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                                         from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                                                         where p.EndDate.Year >= currentYear // Filter by EndDate for the current year
                                                               && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth) // Exclude current month
                                                               && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC") // Filter for CMC or AMC
                                select new ContractDetModel()
                                {
                                    WarrantyId = p.WarrantyId,
                                    DoctorName = p.Cust.DoctorName,
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    Interval = p.Interval,
                                    TakenBy = c.TakenBy,
                                    CreatedDate = p.CreatedDate,
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeName = c.ContractType.ContractTypeName, // Ensure ContractTypeName is fetched correctly
                                }).ToList();




                    model.recordsTotal = listData.Count();
                    //if (datatablePageRequest.ClientMasterId > 0)
                    //    listData = listData.Where(x => x.CustId == datatablePageRequest.Cu).ToList();

                    int superadmin = (int)Enumeration.Role.SuperAdmin;
                    var engineerId = _sessionManager.EnggId; // Current engineer's ID

                    // Check if the current user is SuperAdmin
                    if (_sessionManager.RoleId == superadmin)
                    {
                        // If SuperAdmin, use all records
                        listData = listData.ToList();
                    }
                    else
                    {
                        // If not SuperAdmin, filter by EnggId
                        listData = listData.Where(p => p.TakenBy == engineerId).ToList();
                    }

                    if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                    {
                        listData = listData.Where(c =>
                            c.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

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
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ClientProductMappingProvider=>GetList");
            }
            return model;
        }
        public DatatablePageResponseModel<ContractDetModel> GetExpiredAMCCMSSearchList(DatatablePageRequestModel datatablePageRequest, DateTime startDate, DateTime endDate)
        {
            DatatablePageResponseModel<ContractDetModel> model = new DatatablePageResponseModel<ContractDetModel>
            {
                data = new List<ContractDetModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                IEnumerable<ContractDetModel> listData;
                if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
                {
                    var currentDate = DateTime.Now;
                    var currentMonth = currentDate.Month;
                    var currentYear = currentDate.Year;

                    listData = (from c in unitOfWork.ContractDet.GetAll() // First, fetch ContractDet data
                                join p in unitOfWork.WarrantyDet.GetAll() on c.WarrantyId equals p.WarrantyId // Fetch WarrantyDet data based on WarrantyId
                                where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear // Filter for current month and year
                                select new ContractDetModel()
                                {
                                    WarrantyId = p.WarrantyId,    // Get WarrantyId from WarrantyDet
                                    DoctorName = p.Cust.DoctorName, // Fetch the DoctorName using CustId from WarrantyDet
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = c.ContractTypeId, // Get ContractTypeId from ContractDet
                                    ContractTypeName = c.ContractType.ContractTypeName // Get ContractTypeName from ContractDet
                                }).ToList();
                }
                else
                {
                    var currentDate = DateTime.Now;
                    var currentMonth = currentDate.Month;
                    var currentYear = currentDate.Year;

                    listData = (from c in unitOfWork.ContractDet.GetAll() // First, fetch ContractDet data
                                join p in unitOfWork.WarrantyDet.GetAll() on c.WarrantyId equals p.WarrantyId // Fetch WarrantyDet data based on WarrantyId
                                where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear // Filter for current month and year
                                select new ContractDetModel()
                                {
                                    WarrantyId = p.WarrantyId,    // Get WarrantyId from WarrantyDet
                                    DoctorName = p.Cust.DoctorName, // Fetch the DoctorName using CustId from WarrantyDet
                                    StartDate = p.StartDate,
                                    StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                    EndDate = p.EndDate,
                                    EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                    Interval = p.Interval,
                                    CreatedDate = p.CreatedDate,
                                    CreatedBy = p.CreatedBy,
                                    CreatedByName = p.CreatedByNavigation.UserName,
                                    ContractTypeId = c.ContractTypeId, // Get ContractTypeId from ContractDet
                                    ContractTypeName = c.ContractType.ContractTypeName // Get ContractTypeName from ContractDet
                                }).ToList();



                    model.recordsTotal = listData.Count();
                    //if (datatablePageRequest.ClientMasterId > 0)
                    //    listData = listData.Where(x => x.CustId == datatablePageRequest.Cu).ToList();

                    int superadmin = (int)Enumeration.Role.SuperAdmin;
                    var engineerId = _sessionManager.EnggId; // Current engineer's ID

                    // Check if the current user is SuperAdmin
                    if (_sessionManager.RoleId == superadmin)
                    {
                        // If SuperAdmin, use all records
                        listData = listData.ToList();
                    }
                    else
                    {
                        // If not SuperAdmin, filter by EnggId
                        listData = listData.Where(p => p.TakenBy == engineerId).ToList();
                    }

                    if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                    {
                        listData = listData.Where(c =>
                            c.DoctorName.ToLower().Contains(datatablePageRequest.SearchText.ToLower())

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
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ClientProductMappingProvider=>GetList");
            }
            return model;
        }


        public DashBoardModel GetAllCount(SessionProviderModel sessionProviderModel)
        {
            DashBoardModel dashBoardModel = new DashBoardModel() { UserHeaderDetail = new UserHeaderDetailModel() };

            int totalDueCount = 0;
            int totalExpiredCount = 0;
            int totalDueAMCCMCCount = 0;
            int totalExpiredAMCCMCCount = 0;
            int totalEnggCount = 0;
            int totalCustCount = 0;

            try
            {
                var engineerid = _sessionManager.EnggId;
                // Get the current year and month
                int currentYear = DateTime.Now.Year;
                int currentMonth = DateTime.Now.Month;

                if (engineerid > 0)
                {
                    // Filter data by EnggId when engineerid is greater than 0
                    totalDueCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                     join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                     from c in contractJoin.DefaultIfEmpty()
                                     where p.EndDate.Year >= currentYear
                                           && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth)
                                           && p.InstalledBy == engineerid // Filter by EnggId
                                     select p).Count();
                }
                else
                {
                    // Get total data without filtering by EnggId
                    totalDueCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                     join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                     from c in contractJoin.DefaultIfEmpty()
                                     where p.EndDate.Year >= currentYear
                                           && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth)
                                     select p).Count();
                }

                if (engineerid > 0)
                {
                    totalExpiredCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                         join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractGroup
                                         from c in contractGroup.DefaultIfEmpty() // Left join to include warranties without contracts
                                         where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear
                                         && p.InstalledBy == engineerid
                                         select p).Count();
                }
                else
                {
                    totalExpiredCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                         join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractGroup
                                         from c in contractGroup.DefaultIfEmpty() // Left join to include warranties without contracts
                                         where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear // Filter for current month and year
                                         select p).Count();
                }

                if (engineerid > 0)
                {
                    totalDueAMCCMCCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                           join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                           from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                                           where p.EndDate.Year >= currentYear // Filter by EndDate for the current year
                                                 && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth) // Exclude current month
                                                 && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC")
                                              && c.TakenBy == engineerid
                                           select p).Count();
                }
                else
                {
                    totalDueAMCCMCCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                           join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                                           from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                                           where p.EndDate.Year >= currentYear // Filter by EndDate for the current year
                                                 && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth) // Exclude current month
                                                 && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC")
                                           select p).Count();
                }
               

                if(engineerid > 0)
                {
                    totalExpiredAMCCMCCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                               join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                               from c in contractJoin.DefaultIfEmpty()
                                               where p.EndDate.Year == currentYear
                                                     && p.EndDate.Month == currentMonth
                                                     && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC")
                                                     && p.InstalledBy == engineerid
                                               select p).Count();
                }
                else
                {
                    totalExpiredAMCCMCCount = (from p in unitOfWork.WarrantyDet.GetAll()
                                               join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin
                                               from c in contractJoin.DefaultIfEmpty()
                                               where p.EndDate.Year == currentYear
                                                     && p.EndDate.Month == currentMonth
                                                     && (c.ContractType.ContractTypeName == "CMC" || c.ContractType.ContractTypeName == "AMC")
                                               select p).Count();
                }

                int superadminRoleId = (int)Enumeration.Role.SuperAdmin;

                // Check if the logged-in user is a SuperAdmin
                if (_sessionManager.RoleId == superadminRoleId)
                {
                    // If SuperAdmin, show total count of engineers
                    totalEnggCount = (from p in unitOfWork.EnggMast.GetAll() select p).Count();
                }
                else
                {
                    // If not SuperAdmin, show only 1
                    totalEnggCount = 1;
                }
                totalCustCount = (from p in unitOfWork.CustMast.GetAll() select p).Count();



                // Setting user details
                dashBoardModel.UserHeaderDetail.FullName = _sessionManager.FirstName + " " + _sessionManager.LastName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return new DashBoardModel
            {
                DueCount = totalDueCount,
                ExpiredCount = totalExpiredCount,
                DueAMCCMCCount = totalDueAMCCMCCount,
                ExpiredAMCCMCCount = totalExpiredAMCCMCCount,
                TotalEnggCount = totalEnggCount,
                TotalCustCount = totalCustCount,
                FirstName = _sessionManager.FirstName,
                LastName = _sessionManager.LastName
            };
        }


        //public DashBoardModel GetContractCount(SessionProviderModel sessionProviderModel)
        //{
        //    DashBoardModel dashBoardModel = new DashBoardModel() { UserHeaderDetail = new UserHeaderDetailModel() };

        //    int totalDueCount = 0;
        //    int totalExpiredCount = 0;

        //    try
        //    {
        //        int currentYear = DateTime.Now.Year;
        //        int currentMonth = DateTime.Now.Month;


        //        dashBoardModel.UserHeaderDetail.FullName = _sessionManager.FirstName + " " + _sessionManager.LastName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("An error occurred: " + ex.Message);
        //    }

        //    // Return the counts and user details in the model
        //    return new DashBoardModel
        //    {
        //        contractDuecount = totalDueCount,
        //        ContractExpiredCount = totalExpiredCount,
        //    };
        //}


        public DatatablePageResponseModel<TerritoryAllocationModel> GetDistrictStateList(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProvider)
        {
            var model = new DatatablePageResponseModel<TerritoryAllocationModel>
            {
                data = new List<TerritoryAllocationModel>(),
                draw = datatablePageRequest.Draw
            };

            try
            {
                var engineerId = sessionProvider.EnggId;

                if (engineerId <= 0)
                {
                    throw new Exception("Engineer ID not found in session.");
                }

                Console.WriteLine($"Incoming EngineerId from sessionProvider: {engineerId}");

                // Fetching the list using LINQ
                var listData = (from s in unitOfWork.TerritoryAllocation.GetAll()
                                where s.EnggId == engineerId // Use EngineerId for filtering
                                select new TerritoryAllocationModel()
                                {
                                    EnggId = s.EnggId,
                                    EnggName = s.Engg.EnggName, // Fetch EnggName from Engg_Mast table via navigation property
                                    DistrictId = s.DistrictId,
                                    DistrictName = s.District.DistrictName,
                                    StateId = s.StateId,
                                    StateName = s.State.StateName,
                                    AlloctionId = s.AlloctionId // Ensure you include the necessary properties
                                }).ToList();

                // Total records before filtering
                model.recordsTotal = listData.Count();

                // Search logic
                if (!string.IsNullOrEmpty(datatablePageRequest.SearchText))
                {
                    listData = listData.Where(x =>
                        x.DistrictName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                        x.StateName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) ||
                        x.EnggName.ToLower().Contains(datatablePageRequest.SearchText.ToLower()) // Include EnggName in search
                    ).ToList();
                }

                // Total records after filtering
                model.recordsFiltered = listData.Count();

                // Sorting logic
                if (!string.IsNullOrEmpty(datatablePageRequest.SortColumnName) && !string.IsNullOrEmpty(datatablePageRequest.SortDirection))
                {
                    listData = listData.AsQueryable().OrderBy(datatablePageRequest.SortColumnName + " " + datatablePageRequest.SortDirection).ToList();
                }

                // Paging logic
                model.data = listData.Skip(datatablePageRequest.StartIndex).Take(datatablePageRequest.PageSize).ToList().Select(x =>
                {
                    x.EncId = _commonProvider.Protect((int)x.AlloctionId);
                    return x;
                }).ToList();
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "GetDistrictStateList");
                model.recordsTotal = 0;
                model.recordsFiltered = 0;
            }
            return model;
        }


    }
}
