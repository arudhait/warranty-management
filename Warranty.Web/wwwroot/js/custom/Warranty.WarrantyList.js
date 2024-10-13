Warranty.WarrantyList = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };

    this.AllList = function () {
        var options = {
            searching: false,
            paging: true,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
            processing: true,
            info: true,
            ajax: {
                type: "POST",
                url: UrlContent("WarrantyList/GetWarrantyList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    dtParms.startDate = options.startDate || null;
                    dtParms.endDate = options.endDate || null;
                    return dtParms;
                },
                complete: function (response, result) {
                    var tableBottom = $(`#WarrantyListTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                    var lgndrp = $(`#WarrantyListTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#WarrantyListTable_wrapper .top`).remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                {
                    data: "warrantyId", name: "WarrantyId", orderable: false, className: "text-center col-1",
                    render: function (data, type, row) {
                        var renderResult = '';

                        renderResult += '<a href="' + (UrlContent("WarrantyList/Add?id=") + row.encId + "&view=" + true) + '" title="View"  class="mr-2 fs-17 link-success" style="color:#333333"><i class="ri-eye-line viewHover" ></i></a>';

                        return '<div class="justify-content-center hstack gap-2">' + renderResult + '</div>';
                    }
                },
                {
                    data: "warrantyId", name: "warrantyId", className: "text-center", autoWidth: true,
                    render: function (data, type, row) {
                        var renderResult = "";
                        renderResult += '<a href="' + (UrlContent("WarrantyList/Add?id=") + row.encId + "&edit=" + true) + '" title="View"  class="mr-2 fs-0 fw-medium" title="Edit">' + data + '</a>';

                        return renderResult;
                    }
                },

                { data: "doctorName", name: "DoctorName", autoWidth: true, className: "text-left" },
                { data: "startDateString", name: "StartDate", autoWidth: true, className: "text-left" },
                { data: "endDateString", name: "EndDate", autoWidth: true, className: "text-left" },
                { data: "installedByString", name: "InstalledBy", autoWidth: true, className: "text-left" },
                { data: "contractTypeName", name: "ContractTypeId", autoWidth: true, className: "text-left" },
                { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                { data: "createdDateString", name: "CreatedDateString", className: "text-left col-1" },
            ],
            order: [[1, 'desc']],
            dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-12 col-md-4 txtSearchId"><"col-12 col-md-2 lgndrp"><"col-12 col-md-3"i><"col-12 col-md-3"p>>',
        };
        var dateRangePicker = flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            onClose: function (selectedDates) {
                var startDate = selectedDates[0] ? adjustForTimezone(selectedDates[0]) : null;
                var endDate = selectedDates[1] ? adjustForTimezone(selectedDates[1]) : null;
                options.startDate = startDate;
                options.endDate = endDate;
                if ($.fn.dataTable.isDataTable('#WarrantyListTable')) {
                    $("#WarrantyListTable").DataTable().ajax.reload();
                } else {
                    fetchWarrentyListListData(null, null);
                }
            }
        });
        function adjustForTimezone(date) {
            let offset = date.getTimezoneOffset();
            let adjustedDate = new Date(date.getTime() - (offset * 60 * 1000));
            return adjustedDate.toISOString().split('T')[0];
        }
        function fetchWarrentyListListData(startDate, endDate) {
            if ($.fn.dataTable.isDataTable('#WarrantyListTable')) {
                $("#WarrantyListTable").DataTable().clear().destroy();
            }
            Warranty.WarrantyList.Option = $.extend({}, Warranty.WarrantyList.Option, options);
            Warranty.WarrantyList.Option.Table = new $("#WarrantyListTable").DataTable(
                {
                    searching: options.searching,
                    paging: options.paging,
                    serverSide: true,
                    processing: options.processing,
                    bLengthChange: true,
                    bInfo: options.info,
                    lengthMenu: options.lengthMenu,
                    pageLength: options.pageLength,
                    dom: options.dom,
                    ajax: options.ajax,
                    columns: options.columns,
                    order: options.order,
                });
        }
        fetchWarrentyListListData(null, null);
    };

    this.Search = function () {
        Warranty.WarrantyList.Option.Table.ajax.reload();
    }
    this.Save = function (redirectionType = 0) {
        if ($("#WarrantyListForm").valid()) {
            $(".preloader").show();
            var formdata = $("#WarrantyListForm").serialize();
            $.ajax({
                type: "Post",
                url: UrlContent("WarrantyList/Save/"),
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        $("#hidWarrantyId").val(result.result);
                        // Enable the "Add Product" button
                        $("#addProductBtn").prop("disabled", false);
                        $("#addProbBtn").prop("disabled", false);
                        Warranty.WarrantyList.ModelList(result.result);
                        Warranty.WarrantyList.ProbList(result.result);
                        Swal.fire({

                            icon: "success",
                            html: result.message
                        }).then((x) => {
                            if (redirectionType == Warranty.Common.Redirection_Type.Save_And_Next) {
                                if (result.result != null && result.result != "" && typeof result.result != "undefined") {
                                    window.location.href = UrlContent("WarrantyList/Add?id=" + result.result);
                                } else {
                                    window.location.href = UrlContent("WarrantyList/Add");
                                }
                            } else if (redirectionType == Warranty.Common.Redirection_Type.Save_And_Close) {
                                window.location.href = UrlContent("WarrantyList");
                            }
                        })
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }
    this.ModelList = function (id) {
        $.ajax({
            url: UrlContent("WarrantyList/_ModelData"),
            type: "GET",
            success: function (response) {
                $("#divModelList").html(response);
                Warranty.WarrantyList.Option.ModelTable = $("#ModelTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: "true",
                        processing: true,
                        bPaginate: true,
                        bLengthChange: true,
                        bInfo: true,
                        async: false,
                        lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                        pageLength: 100,
                        dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-sm-5 txtSearchId"><"col-sm-2 lgndrp"><"col-sm-2"i><"col-sm-3"p>>',
                        ajax: {
                            type: "Post",
                            url: UrlContent("WarrantyList/GetModelList/" + id),
                            data: function (dtParms) {
                                dtParms.search.value = $("#txtSearch").val();
                                return dtParms;
                            },
                            complete: function (response, result) {
                                var tableBottom = $("#ModelTable_wrapper .btmpage").detach();
                                $(".top_pagging").prepend(tableBottom);
                                var lgndrp = $("#ModelTable_length").detach();
                                $(".lgndrp").prepend(lgndrp);
                                $("#ModelTable_wrapper .top").remove();
                                $("#searchId").removeClass("hide");
                            }
                        },
                        "columns": [
                            { data: "modelNo", name: "ModelNo", autoWidth: true, orderable: false, className: "text-left" },
                            { data: "modelSerialNo", name: "ModelSerialNo", autoWidth: true, orderable: false, className: "text-left" }
                        ],
                        order: [0, "DESC"],

                    });
            }
        });
    }

    this.AddModelDetails = function (modelDet) {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/WarrantyList/_AddModelDet/",
            data: {
                id: $("#hidWarrantyId").val(),
                modelDetId: modelDet
            },
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                $.validator.unobtrusive.parse($("#ModelDetilsForm"));
                $("#common-md-dialog").modal('show');
            }
        })
    }
    this.SaveModelDet = function () {
        if ($("#ModelDetilsForm").valid()) {
            $(".preloader").show();
            var formdata = $("#ModelDetilsForm").serialize();
            $.ajax({
                type: "Post",
                url: "/WarrantyList/SaveModelDet",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.WarrantyList.Option.ModelTable.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }

    this.ProbList = function (id) {
        $.ajax({
            url: UrlContent("WarrantyList/_ProbData"),
            type: "GET",
            success: function (response) {
                $("#divProbList").html(response);
                Warranty.WarrantyList.Option.ProbTable = $("#ProbTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: "true",
                        processing: true,
                        bPaginate: true,
                        bLengthChange: true,
                        bInfo: true,
                        async: false,
                        lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                        pageLength: 100,
                        dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-sm-5 txtSearchId"><"col-sm-2 lgndrp"><"col-sm-2"i><"col-sm-3"p>>',
                        ajax: {
                            type: "Post",
                            url: UrlContent("WarrantyList/GetProbList/" + id),
                            data: function (dtParms) {
                                dtParms.search.value = $("#txtSearch").val();
                                return dtParms;
                            },
                            complete: function (response, result) {
                                var tableBottom = $("#ProbTable_wrapper .btmpage").detach();
                                $(".top_pagging").prepend(tableBottom);
                                var lgndrp = $("#ProbTable_length").detach();
                                $(".lgndrp").prepend(lgndrp);
                                $("#ProbTable_wrapper .top").remove();
                                $("#searchId").removeClass("hide");
                            }
                        },
                        "columns": [
                            { data: "probName", name: "ProbName", autoWidth: true, orderable: false, className: "text-left" },
                            { data: "probSerialNo", name: "ProbSerialNo", autoWidth: true, orderable: false, className: "text-left" }
                        ],
                        order: [0, "DESC"],

                    });
            }
        });

    }
    this.AddProbDetails = function (probDet) {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/WarrantyList/_AddProbDet/",
            data: {
                id: $("#hidWarrantyId").val(),
                probId: probDet
            },
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                $.validator.unobtrusive.parse($("#ProbDetilsForm"));
                $("#common-md-dialog").modal('show');
            }
        })
    }
    this.SaveProbDet = function () {
        if ($("#ProbDetilsForm").valid()) {
            $(".preloader").show();
            var formdata = $("#ProbDetilsForm").serialize();
            $.ajax({
                type: "Post",
                url: "/WarrantyList/SaveProbDet",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.WarrantyList.Option.ProbTable.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }

}