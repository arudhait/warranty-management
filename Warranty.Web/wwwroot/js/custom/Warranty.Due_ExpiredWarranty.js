Warranty.Due_ExpiredWarranty = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };

    this.InitializeWarrantyTables = function (options) {
        Warranty.Due_ExpiredWarranty.Option = {
            year: options.year,
            month: options.month
        };
        // Initialize Due Warranty Table
        Warranty.Due_ExpiredWarranty.Option.Table = new $("#DueWarrantyTable").DataTable({
            searching: true,
            paging: true,
            serverSide: true,
            bPaginate: true,
            processing: true,
            bLengthChange: true,
            bInfo: true,
            async: false,
            lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
            pageLength: 10,
            dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-12 col-md-4 txtSearchId"><"col-12 col-md-2 lgndrp"><"col-12 col-md-3"i><"col-12 col-md-3"p>>',
            ajax: {
                type: "POST",
                url: UrlContent("Due_ExpiredWarranty/GetDueWarranty"),
                data: function (dtParms) {
                    dtParms.year = parseInt(Warranty.Due_ExpiredWarranty.Option.year, 10);
                    dtParms.month = parseInt(Warranty.Due_ExpiredWarranty.Option.month, 10);
                    dtParms.search.value = $("#txtSearch").val(); // Include search term
                    return dtParms;
                    
                },
                complete: function (response, result) {
                    var tableBottom = $(`#DueWarrantyTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                    var lgndrp = $(`#DueWarrantyTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#DueWarrantyTable_wrapper .top`).remove();

                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                {
                    data: "warrantyId",
                    name: "WarrantyId",
                    orderable: false,
                    className: "text-center col-1",
                    render: function (data, type, row) {
                        return '<a href="' + UrlContent("Due_ExpiredWarranty/Add?id=" + row.encId + "&view=true") + '" title="View" class="mr-2 fs-17 link-success" style="color:#333333"><i class="ri-eye-line viewHover"></i></a>';
                    }
                },
                {
                    data: "doctorName",
                    name: "DoctorName",
                    className: "text-left",
                    autoWidth: true,
                    render: function (data, type, row) {
                        return '<a href="' + (UrlContent("Due_ExpiredWarranty/Add?id=") + row.encId + "&view=" + true) + '" title="View"  class="mr-2 fs-0 fw-medium">' + data + '</a>';
                    }
                },
                { data: "endDateString", name: "EndDateString", autoWidth: true, className: "text-left" },
                { data: "installedByString", name: "InstalledByString", autoWidth: true, className: "text-left" },
                { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },
                { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                { data: "createdDateString", name: "CreatedDateString", className: "text-left col-1" },
            ],
            order: [[1, 'desc']],
        });

        /*Warranty.Due_ExpiredWarranty.Option = $.extend({}, Warranty.Due_ExpiredWarranty.Option, options);*/
        Warranty.Due_ExpiredWarranty.Option.TableExpired = $("#ExpiredWarrantyTable").DataTable({
            searching: false,
            serverSide: true,
            lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
            pageLength: 10,
            ajax: {
                type: "POST",
                url: UrlContent("Due_ExpiredWarranty/GetExpiredWarranty"),
                data: function (dtParms) {
                    dtParms.year = parseInt(Warranty.Due_ExpiredWarranty.Option.year, 10);
                    dtParms.month = parseInt(Warranty.Due_ExpiredWarranty.Option.month, 10);
                    dtParms.search.value = $("#txtSearch").val(); // Include search term
                    return dtParms;
                },
                complete: function (response, result) {
                    // If you want to include additional code for handling the completed AJAX request, you can add it here.
                },
            },
            columns: [
                {
                    data: "warrantyId",
                    name: "WarrantyId",
                    orderable: false,
                    className: "text-center col-1",
                    render: function (data, type, row) {
                        return '<a href="' + UrlContent("Due_ExpiredWarranty/Add?id=" + row.encId + "&view=true") + '" title="View" class="mr-2 fs-17 link-success" style="color:#333333"><i class="ri-eye-line viewHover"></i></a>';
                    }
                },
                {
                    data: "doctorName",
                    name: "DoctorName",
                    className: "text-left",
                    autoWidth: true,
                    render: function (data, type, row) {
                        return '<a href="' + (UrlContent("Due_ExpiredWarranty/Add?id=") + row.encId + "&view=" + true) + '" title="View"  class="mr-2 fs-0 fw-medium">' + data + '</a>';
                    }
                },
                { data: "endDateString", name: "EndDateString", autoWidth: true, className: "text-left" },
                { data: "installedByString", name: "InstalledByString", autoWidth: true, className: "text-left" },
                { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },
                { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                { data: "createdDateString", name: "CreatedDateString", className: "text-left col-1" },
            ],
            order: [[1, 'desc']],
        });
    };


    this.Search = function () {
        Warranty.Due_ExpiredWarranty.Option.Table.ajax.reload();
    }

    this.ModelList = function (id) {
        $.ajax({
            url: UrlContent("Due_ExpiredWarranty/_ModelData"),
            type: "GET",
            success: function (response) {
                $("#divModelList").html(response);
                Warranty.Due_ExpiredWarranty.Option.ModelTable = $("#ModelTable").DataTable(
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
                            url: UrlContent("Due_ExpiredWarranty/GetModelList/" + id),
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
    this.ProbList = function (id) {
        $.ajax({
            url: UrlContent("Due_ExpiredWarranty/_ProbData"),
            type: "GET",
            success: function (response) {
                $("#divProbList").html(response);
                Warranty.Due_ExpiredWarranty.Option.ProbTable = $("#ProbTable").DataTable(
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
                            url: UrlContent("Due_ExpiredWarranty/GetProbList/" + id),
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
}