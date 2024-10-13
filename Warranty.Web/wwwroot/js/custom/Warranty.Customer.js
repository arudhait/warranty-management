Warranty.Customer = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.Customer.Option = $.extend({}, Warranty.Customer.Option, options);
        Warranty.Customer.Option.Table = new $("#CustomerTable").DataTable(
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
                    url: UrlContent("Customer/GetCustomerList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        return dtParms;
                    },
                    complete: function (response, result) {

                        var tableBottom = $(`#CustomerTable_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                        var lgndrp = $(`#CustomerTable_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#CustomerTable_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },

                columns: [
                    {
                        data: "custId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var btnEdit = '', btnView = '', btnDelete = '';
                            btnEdit += '<a  type="button" onclick="Warranty.Customer.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Edit"><i class="ri-edit-2-line editHover"></i></a>';
                            btnView += '<a  type="button" onclick="Warranty.Customer.View(\'' + row.encId + '\')" class="mr-2 fs-17 link-success" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="View"><i class="ri-eye-line viewHover"></i></a>';
                            btnDelete += '<a  type="button" onclick="Warranty.Customer.Delete(\'' + row.encId + '\',\'' + false + '\')"class="mr-2 fs-17 link-danger" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="ri-delete-bin-line deleteHover"></i></a>';
                            return '<div class="justify-content-center hstack gap-2">' + btnView + btnEdit + btnDelete + '</div>';
                        }
                    },
                    { data: "doctorName", name: "DoctorName", autoWidth: true, className: "text-left" },
                    { data: "hospitalName", name: "HospitalName", autoWidth: true, className: "text-left" },
                    { data: "designation", name: "Designation", autoWidth: true, className: "text-left" },
                    { data: "mobileNo", name: "MobileNo", autoWidth: true, className: "text-left" },
                    { data: "email", name: "Email", autoWidth: true, className: "text-left" },
                    { data: "districtName", name: "DistrictName", autoWidth: true, className: "text-left" },
                    { data: "cityStatePin", name: "CityStatePin", autoWidth: true, className: "text-left" },
                    { data: "pndtCertiNo", name: "PndtCertiNo", autoWidth: true, className: "text-v" },
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
                    { data: "createdDatestring", name: "createdDatestring", className: "text-left col-1" }

                ],
                order: [[1, 'desc']],
            });
    };

    this.Search = function () {
        Warranty.Customer.Option.Table.ajax.reload();
    }
    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/Customer/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#CustomerForm"));
                $("#common-lg-dialog").modal('show');
            }
        })
    }
    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to delete this Customer?</h4>' : '<h4>Are you sure want to delete this Customer?</h4>',
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
                    url: "/Customer/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.Customer.Option.Table.ajax.reload();
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
        var formdata = $("#CustomerView").serialize();
        $.ajax({
            type: "GET",
            url: "/Customer/_View/" + id,
            success: function (result) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(result);
                $("#common-lg-dialog").modal('show');
            },
        })
    }
    this.Save = function () {
        if ($("#CustomerForm").valid()) {
            $(".preloader").show();
            var formdata = $("#CustomerForm").serialize();
            $.ajax({
                type: "Post",
                url: "/Customer/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.Customer.Option.Table.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-lg-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }
}