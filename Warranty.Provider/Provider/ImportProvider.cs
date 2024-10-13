using AutoMapper;
using DinkToPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.BusinessEntitiess;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.ADO;
using Warranty.Repository.Repository;

namespace Warranty.Provider.Provider
{
    public class ImportProvider : IImportProvider
    {
        #region Variable 
        private ICommonProvider _commonProvider;
        DBConnectivity db = new DBConnectivity();
        UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public ImportProvider(ICommonProvider commonProvider, IMapper mapper)
        {
            _commonProvider = commonProvider;
            _mapper = mapper;
        }
        #endregion

        #region DownloadSpreadsheetReport

        //****************************************  Customer Import ***********************************
        public DatatableSpDataResponseModel<DataTable> DownloadCustomerReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }

                // Fetch data from database
                var leads = unitOfWork.CustMast.GetAll()
                    .Where(l => (string.IsNullOrEmpty(startDate) || l.CreatedDate >= DateTime.Parse(startDate))
                             && (string.IsNullOrEmpty(endDate) || l.CreatedDate <= DateTime.Parse(endDate)))
                    .ToList();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("CustId", typeof(int));
                dataTable.Columns.Add("DoctorName", typeof(string));
                dataTable.Columns.Add("HospitalName", typeof(string));
                dataTable.Columns.Add("PostalAddress", typeof(string));
                dataTable.Columns.Add("Designation", typeof(string));
                dataTable.Columns.Add("MobileNo", typeof(string));
                dataTable.Columns.Add("PhoneNo", typeof(string));
                dataTable.Columns.Add("Email", typeof(string));
                dataTable.Columns.Add("Pincode", typeof(int));
                dataTable.Columns.Add("StateName", typeof(string));
                dataTable.Columns.Add("DistrictName", typeof(string));
                dataTable.Columns.Add("City", typeof(string));
                dataTable.Columns.Add("PndtCertiNo", typeof(string));
               


                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["CustId"] = lead.CustId;
                    row["DoctorName"] = lead.DoctorName;
                    row["HospitalName"] = lead.HospitalName;
                    row["PostalAddress"] = lead.PostalAddress;
                    row["Designation"] = lead.Designation;
                    row["MobileNo"] = lead.MobileNo;
                    row["PhoneNo"] = lead.PhoneNo;
                    row["Email"] = lead.Email;
                    row["Pincode"] = lead.Pincode;
                    row["StateName"] = lead.State.StateName;
                    row["DistrictName"] = lead.District.DistrictName;
                    row["City"] = lead.City;
                    row["PndtCertiNo"] = lead.PndtCertiNo;
                                    

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateCustomerPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>Customer Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //*******************************   WarrantyList Import ****************************

        public DatatableSpDataResponseModel<DataTable> DownloadWarrantyListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }

                // Fetch data from the database with joins
                var leads = (from w in unitOfWork.WarrantyList.GetAll()
                             join wd in unitOfWork.WarrantyDet.GetAll() on w.WarrantyId equals wd.WarrantyId
                             join c in unitOfWork.ContractDet.GetAll() on wd.WarrantyId equals c.WarrantyId into contractGroup
                             from contract in contractGroup.DefaultIfEmpty() // Left join to include warranties without contracts
                             join ct in unitOfWork.ContractDet.GetAll() on contract.ContractTypeId equals ct.ContractTypeId into contractTypeGroup
                             from contractType in contractTypeGroup.DefaultIfEmpty() // Left join to include contracts without a type
                             where (string.IsNullOrEmpty(startDate) || wd.CreatedDate >= DateTime.Parse(startDate))
                                   && (string.IsNullOrEmpty(endDate) || wd.CreatedDate <= DateTime.Parse(endDate))
                             select new
                             {
                                 w.WarrantyId,
                                 DoctorName = wd.Cust.DoctorName,
                                 SellingDate = wd.SellingDate,
                                 StartDate = wd.StartDate,
                                 EndDate = wd.EndDate,
                                 InstalledByString = wd.InstalledByNavigation.EnggName,
                                 w.CrmNo,
                                 w.NoOfServices,
                                 w.Interval,
                                 ContractTypeName = contractType != null ? contractType.ContractType.ContractTypeName : string.Empty // Replace 'Name' with the correct property
                             }).ToList();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("Doctor Name", typeof(string));
                dataTable.Columns.Add("Selling Date", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Installed By", typeof(string));
                dataTable.Columns.Add("CrmNo", typeof(string));
                dataTable.Columns.Add("No Of Services", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["Doctor Name"] = lead.DoctorName;
                    row["Selling Date"] = lead.SellingDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Installed By"] = lead.InstalledByString;
                    row["CrmNo"] = lead.CrmNo;
                    row["No Of Services"] = lead.NoOfServices;
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }


        public byte[] GenerateWarrantyListPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>WarrantyList Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //************************************* Breakdown List Import ***********************************

        public DatatableSpDataResponseModel<DataTable> DownloadBreakDownListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }

                // Fetch data using left joins
                var leads = (from bd in unitOfWork.BreakdownDet.GetAll()
                             join cust in unitOfWork.CustMast.GetAll() on bd.CustId equals cust.CustId into custGroup
                             from cust in custGroup.DefaultIfEmpty() // Left join to include breakdowns without a customer
                             join bs in unitOfWork.BreakdownStatusMast.GetAll() on bd.TypeId equals bs.BreakdownStatusId into bsGroup
                             from bs in bsGroup.DefaultIfEmpty() // Left join to include breakdowns without a status
                             join eng in unitOfWork.EnggMast.GetAll() on bd.EnggId equals eng.EnggId into engGroup
                             from eng in engGroup.DefaultIfEmpty() // Left join to include breakdowns without an engineer
                             join reqAct in unitOfWork.ActionMast.GetAll() on bd.ReqAction equals reqAct.ActionId into reqActGroup
                             from reqAct in reqActGroup.DefaultIfEmpty() // Left join to include breakdowns without a requested action
                             join actTaken in unitOfWork.ActionMast.GetAll() on bd.ActionTaken equals actTaken.ActionId into actTakenGroup
                             from actTaken in actTakenGroup.DefaultIfEmpty() // Left join to include breakdowns without an action taken
                             where (string.IsNullOrEmpty(startDate) || bd.CreatedDate >= DateTime.Parse(startDate))
                                   && (string.IsNullOrEmpty(endDate) || bd.CreatedDate <= DateTime.Parse(endDate))
                             select new
                             {
                                 BreakdownId = bd.BreakdownId,
                                 DoctorName = cust != null ? cust.DoctorName : string.Empty,
                                 CallRegDate = bd.CallRegDate,
                                 BreakdownType = bs != null ? bs.BreakdownStatusName : string.Empty,
                                 EngineerName = eng != null ? eng.EnggName : string.Empty,
                                 EnggFirstVisitDate = bd.EnggFirstVisitDate,
                                 CrmNo = bd.CrmNo,
                                 Problems = bd.Problems,
                                 ReqActionName = reqAct != null ? reqAct.ActionName : string.Empty,
                                 ActionTakenName = actTaken != null ? actTaken.ActionName : string.Empty,
                                 Conclusion = bd.Conclusion
                             }).ToList();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("BreakdownId", typeof(int));
                dataTable.Columns.Add("DoctorName", typeof(string));
                dataTable.Columns.Add("CallReg Date", typeof(string));
                dataTable.Columns.Add("Breakdown Type", typeof(string));
                dataTable.Columns.Add("Engineer Name", typeof(string));
                dataTable.Columns.Add("Engg FirstVisit Date", typeof(string));
                dataTable.Columns.Add("CrmNo", typeof(string));
                dataTable.Columns.Add("Problems", typeof(string));
                dataTable.Columns.Add("Req ActionName", typeof(string));
                dataTable.Columns.Add("ActionTaken Name", typeof(string));
                dataTable.Columns.Add("Conclusion", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["BreakdownId"] = lead.BreakdownId;
                    row["DoctorName"] = lead.DoctorName;
                    row["CallReg Date"] = lead.CallRegDate.ToString(AppCommon.DateOnlyFormat);
                    row["Breakdown Type"] = lead.BreakdownType;
                    row["Engineer Name"] = lead.EngineerName;
                    row["Engg FirstVisit Date"] = lead.EnggFirstVisitDate.ToString(AppCommon.DateOnlyFormat);
                    row["CrmNo"] = lead.CrmNo;
                    row["Problems"] = lead.Problems;
                    row["Req ActionName"] = lead.ReqActionName;
                    row["ActionTaken Name"] = lead.ActionTakenName;
                    row["Conclusion"] = lead.Conclusion;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }


        public byte[] GenerateBreakDownListPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>BreakDownList Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //******************************** DueExpiery Report import *******************************

        public DatatableSpDataResponseModel<DataTable> DownloadExpiredReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }


                var currentDate = DateTime.Now;
                var currentMonth = currentDate.Month;
                var currentYear = currentDate.Year;

                var leads = (from p in unitOfWork.WarrantyDet.GetAll()
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
                             }).ToList();



                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("Doctor Name", typeof(string));
                dataTable.Columns.Add("Selling Date", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Installed By", typeof(string));
                dataTable.Columns.Add("CrmNo", typeof(string));
                dataTable.Columns.Add("No Of Services", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["Doctor Name"] = lead.DoctorName;
                    row["Selling Date"] = lead.SellingDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Installed By"] = lead.InstalledByString;
                    row["CrmNo"] = lead.CrmNo;
                    row["No Of Services"] = lead.NoOfServices;
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateExpiredPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>Warranty-Expired Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //******************************** DueExpiery Report import *******************************

        public DatatableSpDataResponseModel<DataTable> DownloadDueWarrantyReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }


                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                var leads = (from p in unitOfWork.WarrantyDet.GetAll()
                             join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId into contractJoin // Perform left join
                             from c in contractJoin.DefaultIfEmpty() // Default if no contract match
                             where p.EndDate.Year >= currentYear // Filter by EndDate for the current year
                                   && !(p.EndDate.Year == currentYear && p.EndDate.Month == currentMonth) // Exclude current month
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

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("DoctorName", typeof(string));
                dataTable.Columns.Add("Selling Date", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Installed By", typeof(string));
                dataTable.Columns.Add("CrmNo", typeof(string));
                dataTable.Columns.Add("No Of Services", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["DoctorName"] = lead.DoctorName;
                    row["Selling Date"] = lead.SellingDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Installed By"] = lead.InstalledByString;
                    row["CrmNo"] = lead.CrmNo;
                    row["No Of Services"] = lead.NoOfServices;
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateDueWarrantyPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>Due-Warranty Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //******************************** ContractList import *******************************

        public DatatableSpDataResponseModel<DataTable> DownloadContractListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                // Parse date range
                string startDate = "", endDate = "";
                if (!string.IsNullOrEmpty(datatablePageRequest.DateRange) && datatablePageRequest.DateRange.Contains("to"))
                {
                    string[] dateRangeParts = datatablePageRequest.DateRange.Split("to", StringSplitOptions.RemoveEmptyEntries);
                    if (dateRangeParts.Length == 2)
                    {
                        startDate = dateRangeParts[0].Trim();
                        endDate = dateRangeParts[1].Trim();
                    }
                }

                // Fetch data from database, including the ContractTypeName from ContractTypeMast
                var leads = (from p in unitOfWork.WarrantyDet.GetAll()
                             join c in unitOfWork.ContractDet.GetAll() on p.WarrantyId equals c.WarrantyId
                             join ct in unitOfWork.ContractTypeMast.GetAll() on c.ContractTypeId equals ct.ContractTypeId // Joining ContractTypeMast table
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
                                 ContractTypeId = c.ContractTypeId,
                                 ContractTypeName = ct.ContractTypeName // Fetching ContractTypeName
                             }).ToList();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("Doctor Name", typeof(string));
                dataTable.Columns.Add("Selling Date", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Installed By", typeof(string));
                dataTable.Columns.Add("CrmNo", typeof(string));
                dataTable.Columns.Add("No Of Services", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string)); // Added column for ContractTypeName

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["Doctor Name"] = lead.DoctorName;
                    row["Selling Date"] = lead.SellingDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat); // Adjust for formatting
                    row["Installed By"] = lead.InstalledByString;
                    row["CrmNo"] = lead.CrmNo;
                    row["No Of Services"] = lead.NoOfServices;
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName; // Added ContractTypeName value

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateContractListPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>Contract Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }

        //******************************** Expired AMC/CMC import *******************************

        public DatatableSpDataResponseModel<DataTable> DownloadAMCCMCExpiredReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                var currentDate = DateTime.Now;
                var currentMonth = currentDate.Month;
                var currentYear = currentDate.Year;

                IQueryable<ContractDetModel> leads = (from c in unitOfWork.ContractDet.GetAll()
                                                      join p in unitOfWork.WarrantyDet.GetAll() on c.WarrantyId equals p.WarrantyId
                                                      where p.EndDate.Month == currentMonth && p.EndDate.Year == currentYear
                                                      select new ContractDetModel()
                                                      {
                                                          WarrantyId = p.WarrantyId,
                                                          DoctorName = p.Cust.DoctorName,
                                                          StartDate = p.StartDate,
                                                          StartDateString = p.StartDate.ToString(AppCommon.DateOnlyFormat),
                                                          EndDate = p.EndDate,
                                                          EndDateString = p.EndDate.ToString(AppCommon.DateOnlyFormat),
                                                          Interval = p.Interval,
                                                          CreatedDate = p.CreatedDate,
                                                          CreatedBy = p.CreatedBy,
                                                          CreatedByName = p.CreatedByNavigation.UserName,
                                                          ContractTypeId = c.ContractTypeId,
                                                          ContractTypeName = c.ContractType.ContractTypeName
                                                      }).AsQueryable();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("Doctor Name", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["Doctor Name"] = lead.DoctorName;
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat);
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat);
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateAMCCMCExpiredPdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>Expired AMC/CMC Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }


        //******************************** DUE AMC/CMC import *******************************

        public DatatableSpDataResponseModel<DataTable> DownloadAMCCMCDueReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel)
        {
            DatatableSpDataResponseModel<DataTable> model = new DatatableSpDataResponseModel<DataTable>()
            {
                data = new DataTable(),
            };

            try
            {
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                IQueryable<ContractDetModel> leads = (from p in unitOfWork.WarrantyDet.GetAll()
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
                                                          CreatedDate = p.CreatedDate,
                                                          CreatedBy = p.CreatedBy,
                                                          CreatedByName = p.CreatedByNavigation.UserName,
                                                          ContractTypeName = c.ContractType.ContractTypeName, // Ensure ContractTypeName is fetched correctly
                                                      }).AsQueryable();

                // Create DataTable structure
                DataTable dataTable = new DataTable();

                // Define columns
                dataTable.Columns.Add("WarrantyId", typeof(int));
                dataTable.Columns.Add("Doctor Name", typeof(string));
                dataTable.Columns.Add("Start Date", typeof(string));
                dataTable.Columns.Add("End Date", typeof(string));
                dataTable.Columns.Add("Interval", typeof(string));
                dataTable.Columns.Add("ContractType Name", typeof(string));

                // Populate DataTable
                foreach (var lead in leads)
                {
                    DataRow row = dataTable.NewRow();
                    row["WarrantyId"] = lead.WarrantyId;
                    row["Doctor Name"] = lead.DoctorName;
                    row["Start Date"] = lead.StartDate.ToString(AppCommon.DateOnlyFormat);
                    row["End Date"] = lead.EndDate.ToString(AppCommon.DateOnlyFormat);
                    row["Interval"] = lead.Interval;
                    row["ContractType Name"] = lead.ContractTypeName;

                    dataTable.Rows.Add(row);
                }

                // Assign DataTable to model
                model.data = dataTable;
            }
            catch (Exception ex)
            {
                AppCommon.LogException(ex, "ImportProvider=>DownloadSpreadsheetReport");
            }

            return model;
        }

        public byte[] GenerateAMCCMCDuePdf(DataTable dataTable)
        {
            var pdfConverter = new BasicConverter(new PdfTools());

            // Convert the DataTable to HTML for PDF
            string htmlContent = "<h2>DUE AMC/CMC Report</h2><table border='1' cellpadding='5' cellspacing='0'>";
            htmlContent += "<thead><tr>";

            foreach (DataColumn column in dataTable.Columns)
            {
                htmlContent += $"<th>{column.ColumnName}</th>";
            }

            htmlContent += "</tr></thead><tbody>";

            foreach (DataRow row in dataTable.Rows)
            {
                htmlContent += "<tr>";
                foreach (var item in row.ItemArray)
                {
                    htmlContent += $"<td>{item}</td>";
                }
                htmlContent += "</tr>";
            }

            htmlContent += "</tbody></table>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A2 // Changed to A2
        },
                Objects = {
            new ObjectSettings() {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "[page]/[toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Report Footer", Line = true }
            }
        }
            };

            return pdfConverter.Convert(doc);
        }
    }
}
#endregion