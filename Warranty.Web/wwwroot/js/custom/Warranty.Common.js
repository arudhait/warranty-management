Warranty.Common = new function () {

    this.ToastrSuccess = function (msg, position, gravity) {
        Toastify({
            newWindow: true,
            text: msg,
            gravity: gravity,
            position: position,
            className: "bg-success",
            stopOnFocus: true,
            offset: {
                x: 50, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
            },
            duration: 3000,
            close: true,
            style: "style",
        }).showToast();
    }

    this.ToastrError = function (msg, position, gravity) {
        Toastify({
            newWindow: true,
            text: msg,
            gravity: gravity,
            position: position,
            className: "bg-danger",
            stopOnFocus: true,
            offset: {
                x: 50, // horizontal axis - can be a number or a string indicating unity. eg: '2em'
            },
            duration: 3000,
            close: true,
            style: "style",
        }).showToast();
    }


    this.Option = {
        FaxDocumentList: null,
        CheckEligibilityTable: null,
    };
    this.Redirection_Type = {
        Stay_On_Same_Page: 1,
        Save_And_Next: 2,
        Save_And_Close: 3,
    }
    this.ToastrRemove = function () {
        toastr.remove();
    }

    this.PropertyType = {
        Commercial: 1,
        Resident: 2,
    }
    this.Role = {
        SuperAdmin: 1,
        Administrator: 2,

    }
    this.InitDatePicker = function () {
        $('.date-picker').datepicker({
            orientation: "bottom",
            autoclose: true,
            format: 'mm/dd/yyyy',
            todayHighlight: true,
        });
        CMS_Container.Common.InitDateKeyEvent();
    }
    this.InitDateKeyEvent = function () {
        $('.date-picker').on('keypress', function (e) {
            var Id = "#" + $(this).attr("Id");
            var key = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
            if ($(Id).val().length < 10 && key != 47 && key != 45) {
                FormatDate(e, this);
            }
            else {
                e.preventDefault();
                return false;
            }
        });
    }

    this.MaskLastCharacters = 4;
    this.MaskString = function (value, num) {
        if (!value || isNaN(num)) {
            return value;
        }
        if (num >= value.length) {
            return value;
        }
        const maskedValue = '*'.repeat(value.length - num) + value.slice(value.length - num);
        return maskedValue;
    }
    this.RefreshCaptcha = function () {
        $.ajax({
            url: UrlContent("Account/RefreshCaptcha"),
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                $(".imgCaptcha").attr("src", data);
            }
        });
    }
    this.EncodeString = function (value) {
        if (value == null || value == "" || typeof value == "undefined")
            return "";
        return value.replaceAll("\n", "").replaceAll("\r", "").replaceAll(" ", "-SP-").replaceAll("~", "-TLD-").replaceAll("!", "-EX-").replaceAll("@", "-AT-").replaceAll("$", "-DL-").replaceAll("^", "-CRT-").replaceAll("_", "-UN-").replaceAll("/", "-SL-").replaceAll(".", "-DT-").replaceAll("*", "-ST-").replaceAll("#", "-HS-").replaceAll("%", "-PR-").replaceAll("&", "-AD-").replaceAll("(", "-OB-").replaceAll(")", "-CB-").replaceAll("+", "-PL-").replaceAll(":", "-CLN-").replaceAll(",", "-CMA-").replaceAll("?", "-QM-").replaceAll("<", "-LT-").replaceAll(">", "-GT-").replaceAll("[", "-BBO-").replaceAll("]", "-BBC-").replaceAll("{", "-CBO-").replaceAll("}", "-CBC-").replaceAll("'", "-QT-").replaceAll("\"", "-DQT-");
    }
    this.Login = function () {
        $("#divMsg").removeClass("show").addClass("hide");
        if ($("#loginform").valid()) {
            $(".preloader").show();
            encryptPwd();
            var formData = $("#loginform").serialize();
            $.ajax({
                url: UrlContent("Account/Login"),
                type: "POST",
                data: formData,
                success: function (data) {
                    $(".preloader").hide();
                    if (data.isSuccess) {
                        window.location = UrlContent("DashBoard");
                    } else {
                        $("#txtPassword").val("");
                        //CMS_Container.Common.ToastrError(data.message);
                        $("#lblError").text(data.message);
                        $("#divloginMsg").removeClass("hide").addClass("show");
                        Warranty.Common.RefreshCaptcha();
                    }
                },
            });
        } else {
            $("#loginform").addClass("was-validated");
        }
    }
    this.InitMask = function () {
        $(".fax-inputmask").inputmask("999 999-9999");
        $(".phone-inputmask").inputmask("(999) 999-9999");
        $(".ssn-inputmask").inputmask("999-99-9999");
        $(".zipcode-inputmask").inputmask("999999");
        $(".otp-inputmask").inputmask("999999");
        $(".amount-inputmask").inputmask("000-000-000-00");
        $(".kitid-inputmask").inputmask({
            mask: "******-######-****",
            definitions: {
                '*': {
                    validator: '[A-Za-z0-9]',
                    cardinality: 1,
                },
                '#': {
                    validator: '[0-9]',
                    cardinality: 1
                }
            },
            onBeforePaste: function (pastedValue, opts) {
                return pastedValue.replace("-", "");
            },
        });


        $(".pan-inputmask").inputmask({
            mask: "AAAAA9999A",
            definitions: {
                "A": { validator: "[a-zA-Z]", cardinality: 1 } // Allow both uppercase and lowercase letters
            }
        }).on('input', function (event) {
            var inputVal = $(this).val();
            $(this).val(inputVal.toUpperCase());
        });

        $(".gst-inputmask").inputmask({
            mask: "99AAAAA9999A9Z9",
            definitions: {
                "A": { validator: "[a-zA-Z]", cardinality: 1 }, // Allow both uppercase and lowercase letters
                "9": { validator: "[0-9]", cardinality: 1 }, // Only digits allowed
                "Z": { validator: "[a-zA-Z0-9]", cardinality: 1 } // Alphanumeric character allowed
            }
        }).on('input', function (event) {
            var inputVal = $(this).val();
            $(this).val(inputVal.toUpperCase());
        });


        $(".email-inputmask").inputmask({
            mask: "*{1,30}[.*{1,30}][.*{1,30}][.*{1,30}]@*{1,30}[.*{2,6}][.*{1,2}]",
            greedy: false,
            onBeforePaste: function (pastedValue, opts) {
                return pastedValue.replace("mailto:", "");
            },
            definitions: {
                '*': {
                    validator: "[0-9A-Za-z!#$%&'*+/=?^_`{|}~\-]",
                    cardinality: 1
                }
            }
        });
    }
    this.FormatPhoneNumber = function (phoneNumber) {
        if (phoneNumber != null && phoneNumber != "" && typeof phoneNumber != "undefined") {

            // remove all non-digit characters from the input string
            const cleaned = phoneNumber.replace(/\D/g, '');

            // check if the cleaned phone number is a valid length
            if (cleaned.length !== 10) {
                return phoneNumber;
            }

            // extract the area code and phone number segments
            const areaCode = cleaned.substring(0, 3);
            const phoneSegment1 = cleaned.substring(3, 6);
            const phoneSegment2 = cleaned.substring(6, 10);

            // concatenate the segments with the formatting characters
            return `(${areaCode}) ${phoneSegment1}-${phoneSegment2}`;
        } else {
            return phoneNumber
        }
    }
    this.formatZipcode = function (Zipcode) {
        if (Zipcode && Zipcode.length === 6) {
            return Zipcode.substring(0, 3) + ' ' + Zipcode.substring(3);
        }
        return Zipcode;
    }


    this.validatePAN = function (pan) {
        // PAN should be exactly 10 characters long
        if (pan && pan.length === 10) {
            // Check if first five characters are letters in uppercase
            var firstFiveChars = pan.substring(0, 5);
            var firstFiveCharsRegex = /^[A-Z]{5}$/;
            if (!firstFiveCharsRegex.test(firstFiveChars)) {
                return false;
            }

            // Check if next four characters are digits
            var nextFourChars = pan.substring(5, 9);
            var nextFourCharsRegex = /^\d{4}$/;
            if (!nextFourCharsRegex.test(nextFourChars)) {
                return false;
            }

            // Check if last character is a letter in uppercase
            var lastChar = pan.charAt(9);
            var lastCharRegex = /^[A-Z]$/;
            if (!lastCharRegex.test(lastChar)) {
                return false;
            }

            // If all conditions are met, PAN is valid
            return true;
        }

        // If length is not 10 or input is empty, PAN is invalid
        return false;
    }

    this.ChangePassword = function () {
        $.ajax({
            type: "GET",
            url: "/Account/ChangePassword/",
            success: function (data) {
                $("#common-DialogContent").html(data);
                // $.validator.unobtrusive.parse($("#ChangePwdForm"));
                $("#common-dialog").modal('show');

               
                $(".password-addon").click(function () {
                    var $passwordInput = $(this).siblings(".password-input");
                    if ($passwordInput.attr("type") === "password") {
                        $passwordInput.attr("type", "text");
                        $(this).children().removeClass().addClass("ri-eye-off-line");
                    } else {
                        $passwordInput.attr("type", "password");
                        $(this).children().removeClass().addClass("ri-eye-line");
                    }
                });
            }
        });
    }
    this.SavePassword = function () {

        //if ($("#ChangePwdForm").valid()) {
        $(".preloader").show();
        var formdata = $("#ChangePwdForm").serialize();
        $.ajax({
            type: "POST",
            url: "/Account/ChangePassword",
            data: formdata,
            success: function (result) {
                $(".preloader").hide();
                if (result.isSuccess) {
                    top.location.href = UrlContent("Account/Logout");
                    $("#common-dialog").modal("hide");
                }
                else {
                    Warranty.Common.ToastrError(result.message);
                }
            },
            error: function (textStatus, errorThrown) {
            }
        });
        // }
    }
    this.isNumber = function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if ((charCode > 31 && charCode < 48) || charCode > 57) {
            return false;
        }
        return true;
    }
    this.FormatMoney = function (value) {
        if (value != null && typeof value != "undefined") {
            const formatter = new Intl.NumberFormat('en-US', {
                style: 'currency',
                currency: 'USD',
            });
            return formatter.format(value);
        } else
            return "";
    }
    this.validateGST = function (gstNumber) {
        // Regular expression to match GST format
        var gstRegex = /^(\d{2})([A-Z]{5})(\d{4})([A-Z]{1})(\d{1})([A-Z\d]{1})$/;

        // Check if GST number matches the pattern
        if (gstNumber && gstNumber.match(gstRegex)) {
            return true; // GST number is valid
        }

        return false; // GST number is invalid
    }

    this.MenuLink = function (link) {
        var url = window.location.protocol + "//" + window.location.host + "/" + link;
        var path = url.replace(window.location.protocol + "//" + window.location.host + "/", "");
        var element = $('ul#sidebarnav a').filter(function () {
            return this.href === url || this.href === path;// || url.href.indexOf(this.href) === 0;
        });
        element.parentsUntil(".sidebar-nav").each(function (index) {
            if ($(this).is("li") && $(this).children("a").length !== 0) {
                $(this).children("a").addClass("active");
                $(this).parent("ul#sidebarnav").length === 0
                    ? $(this).addClass("active")
                    : $(this).addClass("selected");
            }
            else if (!$(this).is("ul") && $(this).children("a").length === 0) {
                $(this).addClass("selected");

            }
            else if ($(this).is("ul")) {
                $(this).addClass('in');
            }

        });

    }
    this.Reset = function (id = '') {
        if ($("#ResetPassword").valid()) {
            encryptPwdWithoutUsername();
            $(".preloader").show();
            $.ajax({
                type: "GET",
                url: UrlContent("Account/Reset/" + id),
                success: function (data) {
                    $("#common-lg-DialogContent").html(data);
                    $("#common-lg-dialog").modal('show');
                    Warranty.Common.InitMask();
                    $.validator.unobtrusive.parse($("#UserPassword"));
                    $(".preloader").hide();
                    $(".pwd").click(function () {
                        if ($(this).children().hasClass("fa fa-eye")) {
                            $(this).children().removeClass().addClass("fa fa-eye-slash");
                            $(this).parent().next().attr("type", "text");
                        }
                        else {
                            $(this).children().removeClass().addClass("fa fa-eye");
                            $(this).parent().next().attr("type", "password");
                            $("#txtPassword").val("");
                            //CMS_Container.Common.ToastrError(data.message);
                            $("#lblError").text(data.message);
                            $("#divloginMsg").removeClass("hide").addClass("show");
                            Warranty.Common.RefreshCaptcha();
                        }
                    });
                }
            });
        }
        else {
            $("#ResetPassword").addClass("was-validated");
        }
    }

    this.SendRessetPasswordMail = function (id) {
        Swal.fire({
            title: '<h4>Are you sure want to send link to mail for reset password?</h4>',
            html: '',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#36BEA6',
            cancelButtonColor: '#a1aab2',
            confirmButtonText: '<i class="ri-check-line"></i> Yes',
            cancelButtonText: '<i class="ri-forbid-line"></i> Cancel'
        }).then((result) => {
            if (result.value) {
                $('.preloader').show();
                $.ajax({
                    type: "POST",
                    url: "/Account/SendRessetPasswordMail",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.Common.ToastrSuccess(result.message);
                        }
                        else {
                            Warranty.Common.ToastrError(result.message);
                        }
                    }
                })
            }
        });
    }
    this.GetAudioRecordHtml = function (data) {
        if (data > 0) {
            return '<span class="ri-checkbox-circle-line btn btn-sm btn-success iconAddrStatus" title="' + data + ' Audio Recording Added" ></span>';
        }
        else {
            return '<span class=" ri-information-line btn btn-sm btn-secondary iconAddrStatus" title="No Audio Recording Added"></span>';
        }
    }
    this.GetRecordStatusHtml = function (isOld) {
        if (isOld) {
            return '<img src="' + UrlContent("assets/images/old.png") + '" />'
        }
        else {
            return '<img  src="' + UrlContent("assets/images/new1.png") + '" />'
        }
    }
    this.ShowAddrStatusInfo = function (addressStatus, addressVerifiedOn) {
        var showMsg = '';
        if (addressStatus == "true")
            showMsg = "<span style='color:green'>Address: Verified On " + addressVerifiedOn + " - Address Valid</span>";
        else if (addressStatus == "false")
            showMsg = "<span style='color:maroon'>Address: Verified On " + addressVerifiedOn + " - Address Invalid</span>";
        else {
            showMsg = "<span style='color:black'>Address: Verification is pending</span>";
        }
        $("#addressStatusDialogContent").html(showMsg);
        $("#addressStatusdialog").modal('show');
    }



    this.GetLeadStatusHtml = function (data) {
        switch (data.toLowerCase()) {
            case 'closed':
                return '<span class="badge bg-danger">' + data + '</span>';
            case 'reject':
                return '<span class="badge bg-warning">' + data + '</span>';
            case 'in-progress': // Ensure to use the correct casing for consistency
                return '<span class="badge bg-secondary">' + data + '</span>';
            case 'completed':
                return '<span class="badge bg-success">' + data + '</span>';
            case 'new':
                return '<span class="badge bg-primary">' + data + '</span>';
            default:
                return '<span class="badge bg-info">' + data + '</span>'; // Fallback for unknown statuses
        }
    }


    this.GetRollerStatusHtml = function (data) {
        switch (data.toLowerCase()) {
            case 'sub admin':
                return '<span class="badge bg-primary">' + data + '</span>';
            case 'Service Engineer':
                return '<span class="badge bg-info">' + data + '</span>';
            case 'Aegent':
                return '<span class="badge  bg-secondary">' + data + '</span>';
            default:
                return '<span class="badge bg-info">' + data + '</span>';
        }
    }



    this.OnFilterClick = function (dataClass) {
        if ($("." + dataClass).hasClass("filter")) {
            $("." + dataClass).removeClass("filter");
            $("#" + dataClass).addClass("hide");
        } else {
            $("#" + dataClass).removeClass("hide");
            $("." + dataClass).addClass("filter");
        }
        Warranty.Common.ApplyFilter();
    }
    this.ApplyFilter = function () {
        var LeadStatus = $(".LeadStatus.filter");
        var SellerStatus = $(".SellerStatus.filter");
        var EVStatus = $(".EVStatus.filter");
        var ShipmentStatus = $(".ShipmentStatus.filter");
        var AddressStatus = $(".AddressStatus.filter");
        var BPO = $(".BPO.filter");
        var SNSStatus = $(".SNSStatus.filter");
        var QAStatus = $(".QAStatus.filter");
        var RecordStatus = $(".RecordStatus.filter");
        var DNCStatus = $(".DNCStatus.filter");
        var DCSStatus = $(".DCSStatus.filter");
        var BPOCenter = $(".BPOCenter.filter");
        var AudioStatus = $(".AudioStatus.filter");
        var ReturnAndRefundStatus = $(".ReturnAndRefundStatus.filter");
        listLeadStatus = [];
        listSellerStatus = [];
        listEVStatus = [];
        listShipmentStatus = [];
        listAddressStatus = [];
        listBPO = [];
        listSNSStatus = [];
        listQAStatus = [];
        listRecordStatus = [];
        listDNCStatus = [];
        listDCSStatus = [];
        listBPOCenter = [];
        listAudioStatus = [];
        listReturnAndRefundStatus = [];
        $.each(LeadStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listLeadStatus.push(childrens[i].value);
            }
        })
        $.each(SellerStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listSellerStatus.push(childrens[i].value);
            }
        })
        $.each(EVStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listEVStatus.push(childrens[i].value);
            }
        })
        $.each(ShipmentStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listShipmentStatus.push(childrens[i].value);
            }
        })
        $.each(AddressStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listAddressStatus.push(childrens[i].value);
            }
        })
        $.each(BPO, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listBPO.push(childrens[i].value);
            }
        })
        $.each(SNSStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listSNSStatus.push(childrens[i].value);
            }
        })
        $.each(QAStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listQAStatus.push(childrens[i].value);
            }
        })
        $.each(RecordStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listRecordStatus.push(childrens[i].value);
            }
        })
        $.each(DNCStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listDNCStatus.push(childrens[i].value);
            }
        })
        $.each(DCSStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listDCSStatus.push(childrens[i].value);
            }
        })
        $.each(BPOCenter, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listBPOCenter.push(childrens[i].value);
            }
        })
        $.each(AudioStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listAudioStatus.push(childrens[i].value);
            }
        })
        $.each(ReturnAndRefundStatus, function (index, data) {
            let childrens = $(data)[0].children;
            for (var i = 0; i < childrens.length; i++) {
                if (childrens[i].className == "dataValue")
                    listReturnAndRefundStatus.push(childrens[i].value);
            }
        })
        $("#hdnLeadStatus").val('');
        $("#hdnSellerStatus").val('');
        $("#hdnEVStatus").val('');
        $("#hdnShipmentStatus").val('');
        $("#hdnAddressStatus").val('');
        $("#hdnBPO").val('');
        $("#hdnSNSStatus").val('');
        $("#hdnQAStatus").val('');
        $("#hdnRecordStatus").val('');
        $("#hdnDNCStatus").val('');
        $("#hdnDCSStatus").val('');
        $("#hdnBPOCenter").val('');
        $("#hdnAudioStatus").val('');
        $("#hdnReturnAndRefundStatus").val('');
        if (listLeadStatus.length > 0)
            $("#hdnLeadStatus").val(listLeadStatus.join(";"));
        if (listSellerStatus.length > 0)
            $("#hdnSellerStatus").val(listSellerStatus.join(";"));
        if (listEVStatus.length > 0)
            $("#hdnEVStatus").val(listEVStatus.join(";"));
        if (listShipmentStatus.length > 0)
            $("#hdnShipmentStatus").val(listShipmentStatus.join(";"));
        if (listAddressStatus.length > 0)
            $("#hdnAddressStatus").val(listAddressStatus.join(";"));
        if (listBPO.length > 0)
            $("#hdnBPO").val(listBPO.join(";"));
        if (listSNSStatus.length > 0)
            $("#hdnSNSStatus").val(listSNSStatus.join(";"));
        if (listQAStatus.length > 0)
            $("#hdnQAStatus").val(listQAStatus.join(";"));
        if (listRecordStatus.length > 0)
            $("#hdnRecordStatus").val(listRecordStatus.join(";"));
        if (listDNCStatus.length > 0)
            $("#hdnDNCStatus").val(listDNCStatus.join(";"));
        if (listDCSStatus.length > 0)
            $("#hdnDCSStatus").val(listDCSStatus.join(";"));
        if (listBPOCenter.length > 0)
            $("#hdnBPOCenter").val(listBPOCenter.join(";"));
        if (listAudioStatus.length > 0)
            $("#hdnAudioStatus").val(listAudioStatus.join(";"));
        if (listReturnAndRefundStatus.length > 0)
            $("#hdnReturnAndRefundStatus").val(listReturnAndRefundStatus.join(";"));
        if (CMS_Container.Leads != null && CMS_Container.Leads.Option != null && CMS_Container.Leads.Option.Table != null) {
            CMS_Container.Leads.Option.Table.ajax.reload();
        }
        if (CMS_Container.SellerLead != null && CMS_Container.SellerLead.Option != null && CMS_Container.SellerLead.Option.Table != null) {
            CMS_Container.SellerLead.Option.Table.ajax.reload();
        }
        if (CMS_Container.Dcs != null && CMS_Container.Dcs.Option != null && CMS_Container.Dcs.Option.Table != null) {
            CMS_Container.Dcs.Option.Table.ajax.reload();
        }
        if (CMS_Container.Eligibility != null && CMS_Container.Eligibility.Option != null && CMS_Container.Eligibility.Option.Table != null) {
            CMS_Container.Eligibility.Option.Table.ajax.reload();
        }
        if (CMS_Container.Audit != null && CMS_Container.Audit.Option != null && CMS_Container.Audit.Option.AuditTable != null) {
            CMS_Container.Audit.Option.AuditTable.ajax.reload();
        }
        if (CMS_Container.PatientConfirmation != null && CMS_Container.PatientConfirmation.Option != null && CMS_Container.PatientConfirmation.Option.Table != null) {
            CMS_Container.PatientConfirmation.Option.Table.ajax.reload();
        }
        if (CMS_Container.Shipment != null && CMS_Container.Shipment.Option != null && CMS_Container.Shipment.Option.Table != null) {
            CMS_Container.Shipment.Option.Table.ajax.reload();
        }
        if (CMS_Container.Billing != null && CMS_Container.Billing.Option != null && CMS_Container.Billing.Option.Table != null) {
            CMS_Container.Billing.Option.Table.ajax.reload();
        }
        CMS_Container.Common.SaveFilterInTemp();
    }
    this.SaveFilterInTemp = function () {
        $.ajax({
            type: "GET",
            data: {
                SearchText: $("#txtSearch").val(),
                LeadsStatus: $("#hdnLeadStatus").val(),
                SellersStatus: $("#hdnSellerStatus").val(),
                EVStatus: $("#hdnEVStatus").val(),
                ShipmentStatus: $("#hdnShipmentStatus").val(),
                AddressStatus: $("#hdnAddressStatus").val(),
                BPO: $("#hdnBPO").val(),
                DateRange: $("#hdnCreatedDate").val(),
                SNSStatus: $("#hdnSNSStatus").val(),
                QAStatus: $("#hdnQAStatus").val(),
                RecordStatus: $("#hdnRecordStatus").val(),
                DNCStatus: $("#hdnDNCStatus").val(),
                DCSStatus: $("#hdnDCSStatus").val(),
                BPOCenter: $("#hdnBPOCenter").val(),
                AudioStatus: $("#hdnAudioStatus").val(),
                ReturnAndRefundStatus: $("#hdnReturnAndRefundStatus").val(),
            },
            url: UrlContent("Common/SaveTempFilter"),
            success: function (data) {

            }
        })
    }
    this.ClearDateFilter = function () {
        $("#hdnCreatedDate").val('');
        $("#startDate").val('');
        Warranty.Common.ApplyFilter();
    }


    this.ValidateAddress = function () {
        $('.preloader').show();
        var model = {
            LeadId: $("#LeadId").val(),
            Add1: $("#Address1").val(),
            Add2: $("#Address2").val(),
            City: $("#City").val(),
            StateId: $("#StateId").val(),
            ProductMasterId: $("#ProductMasterId").val(),
            Country: "US",
            ZipCode: $("#ZipCode").val(),
            LastName: $("#LastName").val(),
            FirstName: $("#FirstName").val(),
            Phone: $("#Phone").val(),
            Fax: $("#phyfax").val(),
            suffix: $("#suffix").val(),
        }
        $.ajax({
            url: UrlContent("Common/ValidateAddress"),
            type: 'POST',
            data: model,
            success: function (data) {
                if (data.isSuccess) {
                    $("#Address1").val(data.add1);
                    $("#Address2").val(data.add2);
                    $("#City").val(data.city);
                    $("#StateId").val(data.stateId).trigger("change");
                    $("#ZipCode").val(data.zipCode);
                    /* $("#Phone").val(data.Phone);
                     $("#phyfax").val(data.fax);
                     $("#suffix").val(data.suffix);*/
                    $("#AddressVerifiedOn").val(data.addressVerifiedOn);
                    $("#IsAddressVerified").val(true);
                    $("#addressVerifiedIcon").removeClass('hide');
                    $("#notAddressVerifiedIcon").addClass('hide');
                    $(".preloader").hide();
                    Warranty.Common.ToastrError(data.message);
                }
                else {
                    $(".preloader").hide();
                    $("#IsAddressVerified").val(false);
                    $("#addressVerifiedIcon").addClass('hide');
                    $("#notAddressVerifiedIcon").removeClass('hide');
                    Warranty.Common.ToastrError(data.message)
                };
            },
            error: function (errorr) {
                $(".preloader").hide();
            }
        });
    }

 
    this.ViewVerification = function (id) {
        $.ajax({
            url: UrlContent("Common/_ViewVerification"),
            data: {
                id: id
            },
            success: function (response) {
                if (response.includes("txtResponseModalPopupId")) {
                    $("#common-md-DialogContent").html(response);
                    $("#common-md-dialog").modal("show")
                } else {
                    $("#common-xxl-DialogContent").html(response);
                    $("#common-xxl-dialog").modal("show")
                }
            }
        });
    }
 

}

