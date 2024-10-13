Warranty.ProductMaster = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.ProductMaster.Option = $.extend({}, Warranty.ProductMaster.Option, options);
        Warranty.ProductMaster.Option.Table = new $("#ProductMasterTable").DataTable(
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
                    url: UrlContent("ProductMaster/GetProductList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        return dtParms;
                    },
                    complete: function (response, result) {
                        var tableBottom = $(`#ProductMasterTable_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                        var lgndrp = $(`#ProductMasterTable_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#ProductMasterTable_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },

                columns: [
                    {
                        data: "productMasterId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var btnEdit = '', btnView = '', btnDelete = '';
                            btnEdit += '<a  type="button" onclick="Warranty.ProductMaster.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Edit Product"><i class="ri-edit-2-line editHover"></i></a>';
                            btnView += '<a  type="button" onclick="Warranty.ProductMaster.View(\'' + row.encId + '\')" class="mr-2 fs-17 link-success" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="View Detail"><i class="ri-eye-line viewHover"></i></a>';
                            btnDelete += '<a  type="button" onclick="Warranty.ProductMaster.Delete(\'' + row.encId + '\',\'' + false + '\')"class="mr-2 fs-17 link-danger" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Delete Product"><i class="ri-delete-bin-line deleteHover"></i></a>';
                            return '<div class="justify-content-center hstack gap-2">' + btnView + btnEdit + btnDelete + '</div>';
                        }
                    },

                    { data: "productName", name: "ProductName", autoWidth: true, className: "text-left col-2" },
                    { data: "qty", name: "Qty", autoWidth: true, className: "text-left" },
                    { data: "price", name: "Price", autoWidth: true, className: "text-left" },
                    { data: "sku", "name": "Sku", "autoWidth": true, className: "text-left" },
                    { data: "batchNo", name: "BatchNo", autoWidth: true, className: "text-left" },
                    { data: "size", name: "Size", autoWidth: true, className: "text-left" },
                    { data: "description", name: "Description", autoWidth: true, className: "text-left" },
                    { data: "warranty", name: "Warranty", autoWidth: true, className: "text-left" },
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
                    { data: "createdOnString", name: "CreatedOnString", className: "text-left col-1", }
                ],
                order: [[0, 'desc']],
            });
    };

    this.Search = function () {
        Warranty.ProductMaster.Option.Table.ajax.reload();
    }
    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/ProductMaster/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#ProductForm"));
                $("#common-md-dialog").modal('show');
            }
        })
    }
    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to Re-Activate this product?</h4>' : '<h4>Are you sure want to delete this product?</h4>',
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
                    url: "/ProductMaster/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.ProductMaster.Option.Table.ajax.reload();
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

    this.View = function (id = '') {
        $('.preloader').show();
        var formdata = $("#PMView").serialize();
        $.ajax({
            type: "GET",
            url: "/ProductMaster/_View/" + id,
            success: function (result) {
                $(".preloader").hide();
                $("#common-DialogContent").html(result);
                $("#common-dialog").modal('show');
            },
        })
    }
    this.Save = function () {
        if ($("#ProductForm").valid()) {
            $(".preloader").show();
            var formdata = $("#ProductForm").serialize();
            $.ajax({
                type: "Post",
                url: "/ProductMaster/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.ProductMaster.Option.Table.ajax.reload();
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