using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using System.Data;
using Warranty.Provider.Provider;
using Warranty.Web.Filter;

namespace Warranty.Web.Controllers
{
    [Authorization(PageId = (short)Enumeration.AppPages.Dashboard, Roles = new short[]
  { (short)Enumeration.Role.SuperAdmin, (short)Enumeration.Role.ServiceEngineer })]
    public class ImportController : BaseController
    {
        private IImportProvider _ImportProvider;
        IWebHostEnvironment _webHostEnvironment;
        public ImportController(IImportProvider ImportProvider, ICommonProvider commonProvider, ISessionManager sessionManager, IWebHostEnvironment webHostEnvironment) : base(commonProvider, sessionManager)
        {
            _ImportProvider = ImportProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        //***************************************  Customer Report *******************************************************
        public JsonResult DownloadCustomerReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

           
            var myData = _ImportProvider.DownloadCustomerReport(new DatatablePageRequestModel
            {
                PageSize = int.MaxValue,
                SearchText = SearchText,
                SortColumnName = "CustId",
                SortDirection = "Asc",
                StartIndex = 0,
                DateRange = DateRange,
            }, GetSessionProviderParameters());

            if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
            {
               
                string fileName = "Customer_" + Guid.NewGuid().ToString();
                string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

              
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".xlsx";
                        filePath += ".xlsx";

                       
                        using (var package = new ExcelPackage(new FileInfo(filePath)))
                        {
                            var workSheet = package.Workbook.Worksheets.Add("Customer Data");
                            workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);

                           
                            workSheet.Cells.AutoFitColumns();
                            package.Save();
                        }
                    }
                    else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".pdf";
                        filePath += ".pdf"; 

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                         
                            Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f); // Changed to A2
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();

                            // Add title
                            Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                            Paragraph title = new Paragraph("Customer Report", titleFont)
                            {
                                Alignment = Element.ALIGN_CENTER,
                                SpacingAfter = 20f 
                            };
                            pdfDoc.Add(title);

                           
                            PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                            pdfTable.WidthPercentage = 100; 

                            
                            float[] columnWidths = new float[myData.data.Columns.Count];
                            for (int i = 0; i < myData.data.Columns.Count; i++)
                            {
                                columnWidths[i] = 100f / myData.data.Columns.Count; 
                            }
                            pdfTable.SetWidths(columnWidths);

                            AddTableHeaders(pdfTable, myData.data.Columns);

                            AddTableData(pdfTable, myData.data.Rows);

                            pdfDoc.Add(pdfTable);

                            pdfDoc.Close();
                        }
                    }

                    model.IsSuccess = true;
                    model.Message = fileName;  
                }
                catch (Exception ex)
                {
                    model.IsSuccess = false;
                    model.Message = "Error generating the file: " + ex.Message;
                }
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "No data available.";
            }

            return Json(model);
        }


        //***************************************  WarrantyList Report *******************************************************

        public JsonResult DownloadWarrantyListReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

           
            var myData = _ImportProvider.DownloadWarrantyListReport(new DatatablePageRequestModel
            {
                PageSize = int.MaxValue,
                SearchText = SearchText,
                SortColumnName = "WarrantyId", 
                SortDirection = "Asc",
                StartIndex = 0,
                DateRange = DateRange,
            }, GetSessionProviderParameters());

            if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
            {
               
                string fileName = "WarrantyList_" + Guid.NewGuid().ToString();
                string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

               
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, fileName);

                try
                {
                    if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".xlsx";
                        filePath += ".xlsx";

                       
                        using (var package = new ExcelPackage(new FileInfo(filePath)))
                        {
                            var workSheet = package.Workbook.Worksheets.Add("Warranty Data");
                            workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);

                          
                            workSheet.Cells.AutoFitColumns();
                            package.Save();
                        }
                    }
                    else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName += ".pdf";
                        filePath += ".pdf"; 

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                           
                            Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();

                           
                            Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                            Paragraph title = new Paragraph("Warranty List Report", titleFont)
                            {
                                Alignment = Element.ALIGN_CENTER,
                                SpacingAfter = 20f // Add some spacing after the title
                            };
                            pdfDoc.Add(title);

                          
                            PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                            pdfTable.WidthPercentage = 100; // Full width

                            
                            float[] columnWidths = new float[myData.data.Columns.Count];
                            for (int i = 0; i < myData.data.Columns.Count; i++)
                            {
                                columnWidths[i] = 100f / myData.data.Columns.Count;
                            }
                            pdfTable.SetWidths(columnWidths);

                           
                            AddTableHeaders(pdfTable, myData.data.Columns);
                            AddTableData(pdfTable, myData.data.Rows);

                            pdfDoc.Add(pdfTable);
                            pdfDoc.Close();
                        }
                    }

                    model.IsSuccess = true;
                    model.Message = fileName;  
                }
                catch (Exception ex)
                {
                    model.IsSuccess = false;
                    model.Message = "Error generating the file: " + ex.Message;
                }
            }
            else
            {
                model.IsSuccess = false;
                model.Message = "No data available.";
            }

            return Json(model);
        }

        //************************************ BreakDownList Report *****************************************

        public JsonResult DownloadBreakDownListReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
                
                var myData = _ImportProvider.DownloadBreakDownListReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "BreakdownId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                    
                    string fileName = "BreakDownList_" + Guid.NewGuid().ToString();
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

                  
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                            
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("BreakDown Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("BreakDown Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }


        //***************************************  ExpiredWarranty Report *******************************************************

        public JsonResult DownloadExpiredReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
                
                var myData = _ImportProvider.DownloadExpiredReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "WarrantyId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                    
                    string fileName = "ExpiredWarrantyList_" + Guid.NewGuid().ToString();
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

                    
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                           
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("Expired-Warranty Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("Expired-Warranty Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }


        //***************************************  DUE-Warranty Report *******************************************************

        public JsonResult DownloadDueWarrantyReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
               
                var myData = _ImportProvider.DownloadDueWarrantyReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "WarrantyId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                   
                    string fileName = "DueWarrantyList_" + Guid.NewGuid().ToString();
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

                  
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                           
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("Due-Warranty Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("Due-Warranty Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }

        //***************************************  ContractList Report *******************************************************

        public JsonResult DownloadContractListReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
              
                var myData = _ImportProvider.DownloadContractListReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "WarrantyId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                  
                    string fileName = "ContractList_" + Guid.NewGuid().ToString();
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

                   
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                       
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("Contract Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("Contract Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }


        //***************************************  Expired AMC/CMC Report *******************************************************

        public JsonResult DownloadAMCCMCExpiredReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
               
                var myData = _ImportProvider.DownloadAMCCMCExpiredReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "WarrantyId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                    
                    string fileName = "ExpiredAMCCMCList_" + Guid.NewGuid().ToString();
                  
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");
                    Console.WriteLine("Directory Path: " + directoryPath);  // Debugging


                    
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                           
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("Expired AMC/CMC Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("Expired AMC/CMC Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }


        //***************************************  DUE AMC/CMC Report *******************************************************

        public JsonResult DownloadAMCCMCDueReport(string SearchText, string DateRange, string FileFormat)
        {
            ResponseModel model = new ResponseModel();

            try
            {
               
                var myData = _ImportProvider.DownloadAMCCMCDueReport(new DatatablePageRequestModel
                {
                    PageSize = int.MaxValue,
                    SearchText = SearchText,
                    SortColumnName = "WarrantyId",
                    SortDirection = "Asc",
                    StartIndex = 0,
                    DateRange = DateRange,
                }, GetSessionProviderParameters());

                if (myData != null && myData.data != null && myData.data.Rows.Count > 0)
                {
                    
                    string fileName = "DUEAMCCMCList_" + Guid.NewGuid().ToString();
                    string directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

                    
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    string filePath = Path.Combine(directoryPath, fileName);

                    try
                    {
                        if (FileFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".xlsx";
                            filePath += ".xlsx";

                            
                            using (var package = new ExcelPackage(new FileInfo(filePath)))
                            {
                                var workSheet = package.Workbook.Worksheets.Add("Due AMC/CMC Data");
                                workSheet.Cells[1, 1].LoadFromDataTable(myData.data, true);
                                workSheet.Cells.AutoFitColumns();
                                package.Save();
                            }
                        }
                        else if (FileFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName += ".pdf";
                            filePath += ".pdf";

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A2, 20f, 20f, 30f, 30f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                Font titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                                Paragraph title = new Paragraph("Due AMC/CMC Report", titleFont)
                                {
                                    Alignment = Element.ALIGN_CENTER,
                                    SpacingAfter = 20f
                                };
                                pdfDoc.Add(title);
                                PdfPTable pdfTable = new PdfPTable(myData.data.Columns.Count);
                                pdfTable.WidthPercentage = 100;
                                float[] columnWidths = new float[myData.data.Columns.Count];
                                for (int i = 0; i < myData.data.Columns.Count; i++)
                                {
                                    columnWidths[i] = 100f / myData.data.Columns.Count;
                                }
                                pdfTable.SetWidths(columnWidths);
                                AddTableHeaders(pdfTable, myData.data.Columns);
                                AddTableData(pdfTable, myData.data.Rows);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                            }
                        }

                        model.IsSuccess = true;
                        model.Message = fileName;
                    }
                    catch (Exception ex)
                    {
                        model.IsSuccess = false;
                        model.Message = "Error generating the file: " + ex.Message;

                    }
                }
                else
                {
                    model.IsSuccess = false;
                    model.Message = "No data available.";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Message = "An error occurred: " + ex.Message;

            }

            return Json(model);
        }

        
        private void AddTableHeaders(PdfPTable table, DataColumnCollection columns)
        {
            Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            foreach (DataColumn column in columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, headerFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    BackgroundColor = new BaseColor(200, 200, 200), 
                    Padding = 5f
                };
                table.AddCell(cell);
            }
        }

      
        private void AddTableData(PdfPTable table, DataRowCollection rows)
        {
            Font dataFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            foreach (DataRow row in rows)
            {
                foreach (var cellData in row.ItemArray)
                {
                    PdfPCell dataCell = new PdfPCell(new Phrase(cellData.ToString(), dataFont))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Padding = 5f
                    };
                    table.AddCell(dataCell);
                }
            }
        }    
    
        
    }
}
