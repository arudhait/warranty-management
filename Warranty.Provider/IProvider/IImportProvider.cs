using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warranty.Common.CommonEntities;

namespace Warranty.Provider.IProvider
{
    public interface IImportProvider
    {
        DatatableSpDataResponseModel<DataTable> DownloadCustomerReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateCustomerPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadWarrantyListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateWarrantyListPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadBreakDownListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateBreakDownListPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadExpiredReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateExpiredPdf(DataTable dataTable);

        DatatableSpDataResponseModel<DataTable> DownloadDueWarrantyReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateDueWarrantyPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadContractListReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateContractListPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadAMCCMCExpiredReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateAMCCMCExpiredPdf(DataTable dataTable);


        DatatableSpDataResponseModel<DataTable> DownloadAMCCMCDueReport(DatatablePageRequestModel datatablePageRequest, SessionProviderModel sessionProviderModel);
        byte[] GenerateAMCCMCDuePdf(DataTable dataTable);
    }
}
