Warranty.Declaration = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.Declaration.Option = $.extend({}, Warranty.Declaration.Option, options);
        Warranty.Declaration.Option.Table = new $("#DeclarationTable").DataTable({
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
                url: UrlContent("Declaration/GetDeclarationList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    return dtParms;
                },
                complete: function (response, result) {
                    var tableBottom = $(`#DeclarationTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                    var lgndrp = $(`#DeclarationTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#DeclarationTable_wrapper .top`).remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                {
                    data: "declarationId", orderable: false, className: "text-center col-1",
                    render: function (data, type, row) {
                        var btnEdit = '', btnView = '', btnDelete = '';
                        btnEdit += '<a type="button" onclick="Warranty.Declaration.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit" style="color:#333333"><i class="ri-edit-2-line editHover"></i></a>';
                        btnDelete += '<a type="button" onclick="Warranty.Declaration.Delete(\'' + row.encId + '\',\'' + false + '\')" class="mr-2 fs-17 link-danger" style="color:#333333"><i class="ri-delete-bin-line deleteHover"></i></a>';
                        return '<div class="justify-content-center hstack gap-2">' + btnEdit + btnDelete + '</div>';
                    }
                },
                { data: "crmno", name: "Crmno", autoWidth: true, className: "text-left" },
                { data: "sparePartNo", name: "SparePartNo", autoWidth: true, className: "text-left" },
                { data: "name", name: "Name", autoWidth: true, className: "text-left" },
                { data: "partSn", name: "Part Sn", autoWidth: true, className: "text-left" },
                { data: "machineSn", name: "Machine Sn", autoWidth: true, className: "text-left" },
                { data: "signedBy", name: "Signed By", autoWidth: true, className: "text-left" },
                { data: "dateString", name: "DateString", autoWidth: true, className: "text-left" },
                {
                    data: "isActive", name: "isActive", autoWidth: true, className: "text-left",
                    render: function (data, type, row) {
                        return row.isActive
                            ? '<span class="badge bg-success-subtle text-success">Active</span>'
                            : '<span class="badge bg-danger-subtle text-danger">In-Active</span>';
                    }
                },
                { data: "createdOnString", name: "CreatedOnString", autoWidth: true, className: "text-left" },
                { data: "createdByName", name: "CreatedByName", autoWidth: true, className: "text-left" },
            ],
            order: [[1, 'desc']],
        });
    };
    this.Search = function () {
        Warranty.Declaration.Option.Table.ajax.reload();
    }

    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/Declaration/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                //Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#DeclarationForm"));
                $("#common-md-dialog").modal('show');
            }
        })
    }

    this.Save = function () {
        if ($("#DeclarationForm").valid()) {
            $(".preloader").show();
            var formdata = $("#DeclarationForm").serialize();
            $.ajax({
                type: "Post",
                url: "/Declaration/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.Declaration.Option.Table.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
            })
        }
    }

    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to Re-Activate this Decleration?</h4>' : '<h4>Are you sure want to delete this Decleration?</h4>',
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
                    url: "/Declaration/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.Declaration.Option.Table.ajax.reload();
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