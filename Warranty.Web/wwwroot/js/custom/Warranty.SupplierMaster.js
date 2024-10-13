Warranty.SupplierMaster = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.SupplierMaster.Option = $.extend({}, Warranty.SupplierMaster.Option, options);
        Warranty.SupplierMaster.Option.Table = new $("#SupplierMasterTable").DataTable({
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
                type: "Post",
                url: UrlContent("SupplierMaster/GetSupplierMasterList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    return dtParms;
                },
                complete: function (response, result) {
                    var tableBottom = $(`#SupplierMasterTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                    var lgndrp = $(`#SupplierMasterTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#SupplierMasterTable_wrapper .top`).remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                {
                    data: "supplierMasterId", orderable: false, className: "text-center col-1",
                    render: function (data, type, row) {
                        var btnEdit = '', btnView = '', btnDelete = '';
                        btnEdit += '<a type="button" onclick="Warranty.SupplierMaster.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333"><i class="ri-edit-2-line editHover"></i></a>';
                        btnView += '<a  type="button" onclick="Warranty.SupplierMaster.View(\'' + row.encId + '\')" class="mr-2 fs-17 link-success" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="View"><i class="ri-eye-line viewHover"></i></a>';
                        btnDelete += '<a type="button" onclick="Warranty.SupplierMaster.Delete(\'' + row.encId + '\',\'' + false + '\')" class="mr-2 fs-17 link-danger" title="Delete" style="color:#333333"><i class="ri-delete-bin-line deleteHover"></i></a>';
                        return '<div class="justify-content-center hstack gap-2">' + btnEdit +btnView+ btnDelete + '</div>';
                    }
                },
                { data: "supplierName", name: "supplierName", autoWidth: true, className: "text-left" },
                { data: "emailId", name: "EmailId", autoWidth: true, className: "text-left" },
                { data: "contactNo", name: "ContactNo", autoWidth: true, className: "text-left" },
                { data: "address", name: "Address", autoWidth: true, className: "text-left" },
                { data: "stateCityPin", name: "StateCityPin", autoWidth: true, className: "text-left" },
                { data: "productMatserName", name: "ProductMatserName", autoWidth: true, className: "text-left" },
                { data: "supplierSku", name: "SupplierSku", autoWidth: true, className: "text-left" },
                { data: "productPrice", name: "ProductPrice", autoWidth: true, className: "text-left" },
                {
                    data: "isActive", name: "isActive", autoWidth: true, className: "text-left",
                    render: function (data, type, row) {
                        return row.isActive
                            ? '<span class="badge bg-success-subtle text-success">Active</span>'
                            : '<span class="badge bg-danger-subtle text-danger">In-Active</span>';
                    }
                },
                { data: "createdOnDate", name: "CreatedOnDate", autoWidth: true, className: "text-left" },
            ],
            order: [[1, 'desc']],
        });
    };
    this.Search = function () {
        Warranty.SupplierMaster.Option.Table.ajax.reload();
    }

    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/SupplierMaster/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#SupplierMasterForm"));
                $("#common-lg-dialog").modal('show');
            }
        })
    }

    this.Save = function () {
        if ($("#SupplierMasterForm").valid()) {
            $(".preloader").show();
            var formdata = $("#SupplierMasterForm").serialize();
            $.ajax({
                type: "Post",
                url: "/SupplierMaster/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.SupplierMaster.Option.Table.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-lg-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }

    this.View = function (id = '') {
        $('.preloader').show();
        $.ajax({
            type: "GET",
            url: "/SupplierMaster/_View/" + id,
            success: function (result) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(result);
                $("#common-lg-dialog").modal('show');
            },
        })
    }


    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to delete this Supplier Status?</h4>' : '<h4>Are you sure want to delete this Supplier Status?</h4>',
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
                    url: "/SupplierMaster/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.SupplierMaster.Option.Table.ajax.reload();
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

}