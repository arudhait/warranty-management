Warranty.ModelMaster = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.ModelMaster.Option = $.extend({}, Warranty.ModelMaster.Option, options);
        Warranty.ModelMaster.Option.Table = new $("#ModelMasterTable").DataTable(
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
                    url: UrlContent("ModelMaster/GetModelMasterList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        return dtParms;
                    },
                    complete: function (response, result) {

                        var tableBottom = $(`#ModelMasterTable_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                        var lgndrp = $(`#ModelMasterTable_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#ModelMasterTable_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },

                columns: [
                    {
                        data: "modelId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var btnEdit = '', btnView = '', btnDelete = '';
                            btnEdit += '<a  type="button" onclick="Warranty.ModelMaster.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Edit Product"><i class="ri-edit-2-line editHover"></i></a>';
                            btnView += '<a  type="button" onclick="Warranty.ModelMaster.View(\'' + row.encId + '\')" class="mr-2 fs-17 link-success" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="View"><i class="ri-eye-line viewHover"></i></a>';
                            btnDelete += '<a  type="button" onclick="Warranty.ModelMaster.Delete(\'' + row.encId + '\',\'' + false + '\')"class="mr-2 fs-17 link-danger" style="color:#333333" data-toggle="tooltip" data-placement="bottom" title="Delete"><i class="ri-delete-bin-line deleteHover"></i></a>';
                            return '<div class="justify-content-center hstack gap-2">' + btnView + btnEdit + btnDelete + '</div>';
                        }
                    },                
                   
                    { data: "modelNo", name: "ModelNo", autoWidth: true, className: "text-left" },
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
                    { data: "createdOnString", name: "createdOnString", className: "text-left col-1" },

                ],
                order: [[1, 'desc']],
            });
    };

    this.Search = function () {
        Warranty.ModelMaster.Option.Table.ajax.reload();
    }
    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/ModelMaster/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#ModelMasterForm"));
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
                    url: "/ModelMaster/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.ModelMaster.Option.Table.ajax.reload();
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
        var formdata = $("#MasterModelView").serialize();
        $.ajax({
            type: "GET",
            url: "/ModelMaster/_View/" + id,
            success: function (result) {
                $(".preloader").hide();
                $("#common-DialogContent").html(result);
                $("#common-dialog").modal('show');
            },
        })
    }
    this.Save = function () {
        if ($("#ModelMasterForm").valid()) {
            $(".preloader").show();
            var formdata = $("#ModelMasterForm").serialize();
            $.ajax({
                type: "Post",
                url: "/ModelMaster/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.ModelMaster.Option.Table.ajax.reload();
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