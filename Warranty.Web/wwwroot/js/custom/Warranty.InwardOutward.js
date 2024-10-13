Warranty.InwardOutward = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.InwardOutward.Option = $.extend({}, Warranty.InwardOutward.Option, options);
        Warranty.InwardOutward.Option.Table = new $("#InwardOutwardTable").DataTable(
            {
                searching: false,
                paging: true,
                serverSide: true,
                bPaginate: true,
                processing: true,
                bPaginate: true,
                bLengthChange: true,
                bInfo: true,
                async: false,
                lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
                pageLength: 10,
                dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-12 col-md-4 txtSearchId"><"col-12 col-md-2 lgndrp"><"col-12 col-md-3"i><"col-12 col-md-3"p>>',

                ajax: {
                    type: "Post",
                    url: UrlContent("InwardOutward/GetInwardOutwardList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        return dtParms;
                    },
                    complete: function (response, result) {

                        var tableBottom = $(`#InwardOutwardTable_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                        var lgndrp = $(`#InwardOutwardTable_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#InwardOutwardTable_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },

                columns: [
                    {
                        data: "inwardOutwardId", name: "InwardOutwardId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var btnView = '', btnDelete = '';

                            btnView += '<a href="' + (UrlContent("InwardOutward/Add?id=") + row.encId + "&view=" + true) + '" title="View"  class="mr-2 fs-17 link-success" style="color:#333333"><i class="ri-eye-line viewHover" ></i></a>';
                            btnDelete += '<a type="button" class="mr-2 fs-17 link-danger" style="color:#333333" title="Delete" onclick="Warranty.InwardOutward.Delete(\'' + row.encId + '\');"><i class="ri-delete-bin-line deleteHover"></i></a>';

                            return '<div class="justify-content-center hstack gap-2">' + btnView  + btnDelete + '</div>';
                        }
                    },
                    {
                        data: "inwardOutwardId", name: "InwardOutwardId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var btnEdit = '';

                            btnEdit += '<a href="' + (UrlContent("InwardOutward/Add?id=") + row.encId + "&edit=" + true) + '" title="View"  class="mr-2 fs-0 fw-medium" title="Edit">' + data + '</a>';

                            return '<div class="justify-content-center hstack gap-2">' + btnEdit + '</div>';
                        }
                    },
                    {
                        data: "isType", name: "IsType", className: "text-left col-1",
                        render: function (data, type, row) {
                            var badge = ''
                            if (row.isType)
                                badge += '<span class="badge bg-success-subtle text-success">Inward</span>'
                            else
                                badge += '<span class="badge bg-danger-subtle text-danger">Outward</span>'
                            return badge;
                        }
                    },
                    { data: "dateOnString", name: "Date", autoWidth: true, className: "text-left" },
                    { data: "note", name: "Note", autoWidth: true, className: "text-left" },
                    {
                        data: "isActive", name: "isActive", className: "text-left col-1",
                        render: function (data, type, row) {
                            var badge = ''
                            if (row.isActive)
                                badge += '<span class="badge bg-success-subtle text-success">Active</span>'
                            else
                                badge += '<span class="badge bg-danger-subtle text-danger">In-Active</span>'
                            return badge;
                        }
                    },
                    { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                    { data: "createdOnstring", name: "CreatedOnstring", className: "text-left col-1" }

                ],
                order: [[1, 'desc']],
            });
    };

    this.Search = function () {
        Warranty.InwardOutward.Option.Table.ajax.reload();
    }


    this.Save = function (redirectionType = 0) {
        if ($("#InwardOutwardForm").valid()) {
            $(".preloader").show();
            var formdata = $("#InwardOutwardForm").serialize();
            $.ajax({
                type: "Post",
                url: UrlContent("InwardOutward/Save/"),
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        $("#hidInwardOutwardId").val(result.result);
                        // Enable the "Add Product" button
                        $("#addProductBtn").prop("disabled", false);
                        Warranty.InwardOutward.InwardOutwardItemList(result.result);
                        Swal.fire({

                            icon: "success",
                            html: result.message
                        }).then((x) => {
                            if (redirectionType == Warranty.Common.Redirection_Type.Save_And_Next) {
                                if (result.result != null && result.result != "" && typeof result.result != "undefined") {
                                    window.location.href = UrlContent("InwardOutward/Add?id=" + result.result);
                                } else {
                                    window.location.href = UrlContent("InwardOutward/Add");
                                }
                            } else if (redirectionType == Warranty.Common.Redirection_Type.Save_And_Close) {
                                window.location.href = UrlContent("InwardOutward");
                            }
                        })
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }
    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to delete this Inward / OutWard ?</h4>' : '<h4>Are you sure want to delete this Inward / OutWard ?</h4>',
            html: '',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: val == 'true' ? '#36BEA6' : '#f33c02',
            cancelButtonColor: '#a1aab2',
            confirmButtonText: val == 'true' ? '<i class="ri-check-line"></i> Activate' : '<i class="ri-delete-bin-line"></i> Delete',
            cancelButtonText: '<i class="ri-forbid-line"></i> Cancel'
        }).then((result) => {
            if (result.value) {
                $('.preloader').show();
                $.ajax({
                    type: "POST",
                    url: "/InwardOutward/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.InwardOutward.Option.Table.ajax.reload();
                            Warranty.Common.success(result.message);
                        }
                        else {
                            Warranty.Common.ToastrError(result.message);
                        }
                    }
                })
            }
        });
    }

    this.InwardOutwardItemList = function (id) {
        $.ajax({
            url: UrlContent("InwardOutward/_InwardOutwardItem"),
            type: "GET",
            success: function (response) {
                $("#divInwardOutwardItemList").html(response);
                Warranty.InwardOutward.Option.InwardOutwardItemTable = $("#InwardOutwardItemTable").DataTable(
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
                            url: UrlContent("InwardOutward/GetItem/" + id),
                            data: function (dtParms) {
                                dtParms.search.value = $("#txtSearch").val();
                                return dtParms;
                            },
                            complete: function (response, result) {
                                var tableBottom = $("#InwardOutwardItemTable_wrapper .btmpage").detach();
                                $(".top_pagging").prepend(tableBottom);
                                var lgndrp = $("#InwardOutwardItemTable_length").detach();
                                $(".lgndrp").prepend(lgndrp);
                                $("#InwardOutwardItemTable_wrapper .top").remove();
                                $("#searchId").removeClass("hide");
                            }
                        },
                        "columns": [
                            {
                                data: "inwardOutwardItemId", name: "InwardOutwardItemId", orderable: false, className: "text-center col-1",
                                render: function (data, type, row) {
                                    var btnEdit = '', btnDelete = '';
                                    btnEdit += '<a  type="button" onclick="Warranty.InwardOutward.AddItem(\'' + row.inwardOutwardItemId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="ri-edit-2-line editHover"></i></a>';
                                    btnDelete += '<a type="button" class="mr-2 fs-17 link-danger" style="color:#333333" title="Delete" onclick="Warranty.InwardOutward.DeleteItem(\'' + row.encId + '\');"><i class="ri-delete-bin-line deleteHover"></i></a>';

                                    return '<div class="justify-content-center hstack gap-2">' + btnEdit + btnDelete + '</div>';
                                }
                            },
                            { data: "productName", name: "ProductName", autoWidth: true, orderable: false, className: "text-left" },
                            { data: "price", name: "Price", autoWidth: true, orderable: false, className: "text-left" },
                            { data: "qty", name: "Qty", autoWidth: true, orderable: false, className: "text-left" }
                        ],
                        order: [1, "DESC"],

                    });
            }
        });
    }

    this.AddItem = function (InwardOutwardItem) {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/InwardOutward/_AddItem/",
            data: {
                id: $("#hidInwardOutwardId").val(),
                InwardOutwardItemId: InwardOutwardItem
            },
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                $.validator.unobtrusive.parse($("#ItemForm"));
                $("#common-md-dialog").modal('show');
                $("#ProductMasterId").change(function () {
                    var productMasterId = $(this).val();
                    fetchRate(productMasterId);
                });
            }
        })
    }
    this.SaveItem = function () {
        if ($("#ItemForm").valid()) {
            $(".preloader").show();
            var formdata = $("#ItemForm").serialize();
            $.ajax({
                type: "Post",
                url: "/InwardOutward/SaveItem",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.InwardOutward.Option.InwardOutwardItemTable.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }
    function fetchRate(productMasterId) {
        $.ajax({
            type: "GET",
            url: "/InwardOutward/GetRate/",
            data: { productMasterId: productMasterId },
            success: function (price) {
                $("#Price").val(price);
            }
        });
    }
    this.DeleteItem = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to delete this Inward / OutWard Item?</h4>' : '<h4>Are you sure want to delete this Inward / OutWard Item?</h4>',
            html: '',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: val == 'true' ? '#36BEA6' : '#f33c02',
            cancelButtonColor: '#a1aab2',
            confirmButtonText: val == 'true' ? '<i class="ri-check-line"></i> Activate' : '<i class="ri-delete-bin-line"></i> Delete',
            cancelButtonText: '<i class="ri-forbid-line"></i> Cancel'
        }).then((result) => {
            if (result.value) {
                $('.preloader').show();
                $.ajax({
                    type: "POST",
                    url: "/InwardOutward/DeleteItem",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.InwardOutward.Option.InwardOutwardItemTable.ajax.reload();
                            Warranty.Common.success(result.message);
                        }
                        else {
                            Warranty.Common.ToastrError(result.message);
                        }
                    }
                })
            }
        });
    }
}