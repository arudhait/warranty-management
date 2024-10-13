Warranty.UserMaster = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
    };
    this.Init = function (options) {
        Warranty.UserMaster.Option = $.extend({}, Warranty.UserMaster.Option, options);
        Warranty.UserMaster.Option.Table = new $("#UserMasterTableId").DataTable(
            {
                responsive: true,
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
                    url: UrlContent("UserMaster/GetList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        dtParms.RoleId = $("#ddIndexRole").val();
                        dtParms.extra_search = $("#ddStatus").val();
                        return dtParms;
                    },
                    complete: function (response, result) {
                        var tableBottom = $(`#UserMasterTableId_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                        var lgndrp = $(`#UserMasterTableId_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#UserMasterTableId_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },
                columns: [
                    {
                        data: "userId", orderable: false, className: "text-center",
                        render: function (data, type, row) {
                            var btnEdit = '', btnDelete = '', btnReset = '';
                            btnReset = '<a type="button" onclick="Warranty.UserMaster.Reset(\'' + row.encId + '\')" class="mr-2 link-success fs-17" style="color:#333333" title="Reset Password"  style="background-color:#7460EE"><i class="las la-key resetHover"></i></a>'
                            if (row.isActive) {
                                btnEdit += '<a type="button" onclick="Warranty.UserMaster.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" ><i class="ri-edit-2-line editHover"></i></a>';
                                btnResetToMail = '<a type="button" onclick="Warranty.Common.SendRessetPasswordMail(\'' + row.encId + '\')" class=" link-info fs-17" style="color:#333333" title="Send link to mail for reset password"  style="background-color:#ff7f41"><i class="ri-mail-send-line resetHover"></i></a>'
                                btnDelete += '<a type="button" class="mr-2 fs-17 link-danger" style="color:#333333" title="Delete User" onclick="Warranty.UserMaster.Delete(\'' + row.encId + '\');"><i class="ri-delete-bin-line deleteHover"></i></a>';
                            } else {
                                btnEdit += '<a type="button" onclick="Warranty.UserMaster.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" ><i class="ri-edit-2-line editHover"></i></a>';
                                btnReset = '<a type="button" onclick="Warranty.UserMaster.Reset(\'' + row.encId + '\')" class="mr-2 link-success fs-17" style="color:#333333" title="Reset Password"  style="background-color:#7460EE"><i class="las la-key resetHover""></i></a>'
                                btnResetToMail = '<a type="button" onclick="Warranty.Common.SendRessetPasswordMail(\'' + row.encId + '\')" class=" link-info fs-17" style="color:#333333" title="Send link to mail for reset password"  style="background-color:#ff7f41"><i class="ri-mail-send-line resetHover"></i></a>'
                            }
                            return '<div class="justify-content-center hstack gap-2">' + btnResetToMail + btnReset + btnEdit + btnDelete + '</div>';
                        }
                    },
                    { data: 'userId', name: 'userId', autoWidth: true, className: "text-left", visible: false },
                    { data: 'userName', name: 'UserName', autoWidth: true, className: "text-left" },
                    { data: 'email', name: 'Email', autoWidth: true, className: "text-left" },
                    {
                        data: "userTypeName", name: "userTypeName", autoWidth: true, className: "text-left", render: function (data, type, row, meta) {
                            return Warranty.Common.GetRollerStatusHtml(data);
                        }
                    },              
                    {
                        data: "userStatus", name: "UserStatus", autoWidth: true, className: "text-left",
                        render: function (data, type, row) {
                            var badge = ''
                            if (row.isActive)
                                badge += '<span class="badge bg-success-subtle text-success">Active</span>'
                            else
                                badge += '<span class="badge bg-danger-subtle text-danger">In-Active</span>'
                            return badge;
                        }
                    },
                    { data: "createdOnString", name: "CreatedOnString", autoWidth: true, className: "text-left" }
                ],
                order: [[1, 'desc']],
            });
    };
    this.Search = function () {
        Warranty.UserMaster.Option.Table.ajax.reload();
    }

    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to Re-Activate this user?</h4>' : '<h4>Are you sure want to delete this user?</h4>',
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
                    url: "/UserMaster/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.UserMaster.Option.Table.ajax.reload();
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

    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/UserMaster/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#AddUserform"));
                $("#common-md-dialog").modal('show');
            }
        })
    }

    this.Save = function () {
        if ($("#AddUserform").valid()) {
            $(".preloader").show();
            var formdata = $("#AddUserform").serialize();
            $.ajax({
                type: "Post",
                url: "/UserMaster/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.UserMaster.Option.Table.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }

    this.Reset = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/UserMaster/_Reset/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                $("#common-md-dialog").modal('show');
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#UserPassword"));
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
        })
    }

    this.SavePassword = function () {
        if ($("#UserPassword").valid()) {
            $(".preloader").show();
            var formdata = $("#UserPassword").serialize();
            $.ajax({
                type: "Post",
                url: "/UserMaster/SaveResetPassword/",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        $("#common-md-dialog").modal("hide");
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        Warranty.UserMaster.Option.Table.ajax.reload();
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }
}