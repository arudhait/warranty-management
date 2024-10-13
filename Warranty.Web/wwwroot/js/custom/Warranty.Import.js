Warranty.Import = {
    DownloadCustomerReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadCustomerReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat, // Pass file format to backend (Excel or PDF)              
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                    // Trigger file download
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadWarrantyListReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadWarrantyListReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat, // Pass file format to backend (Excel or PDF)              
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                    // Trigger file download
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadBreakDownListReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadBreakDownListReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat, // Pass file format to backend (Excel or PDF)              
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                    // Trigger file download
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadExpiredReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadExpiredReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat, // Pass file format to backend (Excel or PDF)              
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                    // Trigger file download
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadDueWarrantyReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadDueWarrantyReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat, // Pass file format to backend (Excel or PDF)              
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                    // Trigger file download
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadContractListReport: function (fileFormat) {
        // Show preloader while downloading
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadContractListReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat,            
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                   
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadAMCCMCExpiredReport: function (fileFormat) {
       
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadAMCCMCExpiredReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat,            
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                   
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },

    DownloadAMCCMCDueReport: function (fileFormat) {
       
        $(".preloader").show();
        $.ajax({
            url: UrlContent("Import/DownloadAMCCMCDueReport"),
            type: "POST",
            data: {
                SearchText: $("#txtSearch").val(),
                DateRange: $("#hdnCreatedDate").val(),
                FileFormat: fileFormat,            
            },
            success: function (response) {
                $(".preloader").hide();

                if (response.isSuccess) {
                   
                    window.location.href = UrlContent("ExtraFiles/Downloads/" + response.message);
                } else {
                    Warranty.Common.ToastrError(response.message);
                }
            },
            error: function () {
                $(".preloader").hide();
                Warranty.Common.ToastrError("An error occurred while generating the report.");
            }
        });
    },
};
