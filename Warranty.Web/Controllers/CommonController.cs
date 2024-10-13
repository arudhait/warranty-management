using Castle.Components.DictionaryAdapter.Xml;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Warranty.Common.CommonEntities;
using Warranty.Common.Utility;
using Warranty.Provider.IProvider;
using Warranty.Repository.Models;

namespace Warranty.Web.Controllers
{
    public class CommonController : BaseController
    {
        #region Variables

        private IUserMasterProvider _userMasterProvider;
        IWebHostEnvironment _webHostEnvironment;

        private readonly WarrantyManagementWebContext _dbContext;
        #endregion

        #region Constructor
        public CommonController(ICommonProvider commonProvider, ISessionManager sessionManager, IWebHostEnvironment webHostEnvironment, WarrantyManagementWebContext dbContext,
            IUserMasterProvider userMasterProvider) : base(commonProvider, sessionManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _userMasterProvider = userMasterProvider;
            _dbContext = dbContext;
        }
        #endregion
        public IActionResult Table(string table)
        {
            DeleteAllFilter();
            return RedirectToAction("Index", table);
        }

        #region Temp 
        public JsonResult SaveTempFilter(string SearchText, string LeadsStatus, string DateRange, string AllocatedTo, string AllocatedDate, string ReturnAndRefundStatus)
        {
            SetDataInTemp(AppCommon.TMP_SearchText, SearchText);
            SetDataInTemp(AppCommon.TMP_LeadsStatus, LeadsStatus);
            SetDataInTemp(AppCommon.TMP_DateRange, DateRange);
            SetDataInTemp(AppCommon.TMP_AllocatedTo, AllocatedTo);
            SetDataInTemp(AppCommon.TMP_AllocatedDate, AllocatedDate);
            SetDataInTemp(AppCommon.TMP_ReturnAndRefundStatus, ReturnAndRefundStatus);
            return Json(true);
        }
        #endregion



        #region Declaration Form
        [HttpGet]
        public ActionResult QuotationIndex()
        {
            return PartialView(); // Ensure you have a corresponding partial view
        }
        [HttpPost]
        public ActionResult _Quotation(string CRMNo, string SparePN, string Name, string PartSN, string MachineSN, string SignedBy)
        {
            string Date = DateTime.Now.ToString("dd/MM/yyyy");
          
            string documentPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Declaration");
            string downloadPath = Path.Combine(_webHostEnvironment.WebRootPath, "ExtraFiles", "Downloads");

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

           
            string docFilePath = Path.Combine(documentPath, "Declaration1.pdf");
            string fileName = $"Quotation_{Guid.NewGuid().ToString()}.pdf";
            string newDocFilePath = Path.Combine(downloadPath, fileName);

            if (System.IO.File.Exists(newDocFilePath))
                System.IO.File.Delete(newDocFilePath);

           
            using (PdfReader pdfReader = new PdfReader(System.IO.File.ReadAllBytes(docFilePath)))
            {
                using (var stream = new FileStream(newDocFilePath, FileMode.Create))
                {
                    using (PdfStamper pdfStamper = new PdfStamper(pdfReader, stream))
                    {
                        AcroFields pdfFormFields = pdfStamper.AcroFields;

                      
                        pdfFormFields.SetField("CRMNo", CRMNo);
                        pdfFormFields.SetField("SpareP/N", SparePN);
                        pdfFormFields.SetField("Name", Name);
                        pdfFormFields.SetField("PartS/N", PartSN);
                        pdfFormFields.SetField("MachineS/N", MachineSN);
                        pdfFormFields.SetField("Signedby", SignedBy);
                        pdfFormFields.SetField("Date1", Date);
                        pdfFormFields.SetField("Date2", Date);

                        
                        float fontSize = 30f; 
                        BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                        foreach (var fieldName in new[] { "CRMNo", "SpareP/N", "Name", "PartS/N", "MachineS/N", "Signedby", "Date1", "Date2" })
                        {
                            pdfFormFields.SetFieldProperty(fieldName, "textsize", fontSize, null);
                            pdfFormFields.SetFieldProperty(fieldName, "textfont", bf, null);
                        }

                       
                        pdfStamper.FormFlattening = true;
                    }
                }
            }

           
            byte[] fileBytes = System.IO.File.ReadAllBytes(newDocFilePath);
            return File(fileBytes, "application/pdf", fileName);
        }
        #endregion

    }
}
