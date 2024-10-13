Warranty.Allocation = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.Allocation.Option = $.extend({}, Warranty.Allocation.Option, options);
        Warranty.Allocation.Option.Table = new $("#AllocationTable").DataTable(
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
                    url: UrlContent("Allocation/GetAllocationList"),
                    data: function (dtParms) {
                        dtParms.search.value = $("#txtSearch").val();
                        return dtParms;
                    },
                    complete: function (response, result) {

                        var tableBottom = $(`#AllocationTable_wrapper .btmpage`).detach();
                        $(".top_pagging").prepend(tableBottom);
                        $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                        var lgndrp = $(`#AllocationTable_length`).detach();
                        $(".lgndrp").prepend(lgndrp);
                        $(`#AllocationTable_wrapper .top`).remove();
                        $("#searchId").removeClass("hide");
                    }
                },

                columns: [
                    
                    {
                        data: "enggId", name: "EnggId", orderable: false, className: "text-center col-1",
                        render: function (data, type, row) {
                            var renderResult = '';

                            renderResult += '<a href="' + (UrlContent("Allocation/Add?id=") + row.encId + "&view=" + true) + '" title="View"  class="mr-2 fs-17 link-success" style="color:#333333"><i class="ri-eye-line viewHover" ></i></a>';

                            return '<div class="justify-content-center hstack gap-2">' + renderResult + '</div>';
                        }
                    },
                    {
                        data: "enggName", name: "enggName", className: "text-center", autoWidth: true,
                        render: function (data, type, row) {
                            var renderResult = "";
                            renderResult += '<a href="' + (UrlContent("Allocation/Add?id=") + row.encId + "&edit=" + true) + '" title="View"  class="mr-2 fs-0 fw-medium" title="Edit">' + data + '</a>';

                            return renderResult;
                        }
                    },
                   
                    { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                    { data: "createdDatestring", name: "createdDatestring", className: "text-left col-1" }

                ],
                order: [[1, 'desc']],
            });
    };

    this.Search = function () {
        Warranty.Allocation.Option.Table.ajax.reload();
    }
    this.DistrictStateList = function (id) {
        $.ajax({
            url: UrlContent("Allocation/_DistrictStateData"),
            type: "GET",
            success: function (response) {
                $("#divDistrictAllocationList").html(response);
                Warranty.Allocation.Option.DistrictStatetable = $("#DistrictStatetable").DataTable(
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
                            url: UrlContent("Allocation/GetDistrictstateList/" + id),
                            data: function (dtParms) {
                                dtParms.search.value = $("#txtSearch").val();
                                return dtParms;
                            },
                            complete: function (response, result) {
                                var tableBottom = $("#DistrictStatetable_wrapper .btmpage").detach();
                                $(".top_pagging").prepend(tableBottom);
                                var lgndrp = $("#DistrictStatetable_length").detach();
                                $(".lgndrp").prepend(lgndrp);
                                $("#DistrictStatetable_wrapper .top").remove();
                                $("#searchId").removeClass("hide");
                            }
                        },
                        "columns": [
                           
                            {
                                data: "allocationId", orderable: false, className: "text-center",
                                render: function (data, type, row) {
                                    var btnDelete = '';
                                    btnDelete += '<a type="button" class="mr-2 fs-17 link-danger" style="color:#333333" title="Delete District/State" onclick="Warranty.Allocation.Delete(\'' + row.encId + '\');"><i class="ri-delete-bin-line deleteHover"></i></a>';
                                    return '<div class="justify-content-center hstack gap-2">' + btnDelete + '</div>';
                                }
                            },
                            { data: "districtName", name: "DistrictName", autoWidth: true, orderable: false, className: "text-left" },
                            { data: "stateName", name: "StateName", autoWidth: true, orderable: false, className: "text-left" },
                           
                        ],
                        order: [0, "DESC"],

                    });
            }   
        });
    }


    this.AddDistrictState = function (id = hidEnggId, territoryAllocation) {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/Allocation/_AddDistrictState/" + id,
            data: {
                id: $("#hidEnggId").val(),
                allocationId: territoryAllocation
            },
            success: function (data) {
                $(".preloader").hide();
                $("#common-md-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#DistrictStateForm"));
                $("#common-md-dialog").modal('show');
            }
        });
    }
   
    this.SaveDistrictState = function (id = $('#hidEnggId').val()) {
        console.log("id")
        if ($("#DistrictStateForm").valid()) {
            $(".preloader").show();
            var formData = $("#DistrictStateForm").serialize();

            $.ajax({
                type: "POST",
                url: `/Allocation/SaveDistrictState/${id}`,
                data: formData,
                success: function (result) {
                    $(".preloader").hide();

                    if (result.isSuccess) {
                        $("#hidEnggId").val(result.result);

                      
                       // Warranty.DistrictStateList.Option.Table.ajax.reload();
                        Warranty.Common.ToastrSuccess(result.message, "right", "top");
                        $("#common-md-dialog").modal('hide');

                        window.location.reload();
                    } else {
                        Warranty.Common.ToastrError(result.message, "right", "top");
                    }
                },
                error: function () {
                    $(".preloader").hide();
                    Warranty.Common.ToastrError("An error occurred while saving.", "right", "top");
                }
            });
        }
    };
    
    this.Delete = function (id) {
        Swal.fire({
            title: '<h4><b>Are you sure you want to delete this District/State?<b></h4>',
            html: '',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#f33c02',
            cancelButtonColor: '#a1aab2',
            confirmButtonText: '<i class="ri-delete-bin-line"></i> Delete',
            cancelButtonText: '<i class="ri-forbid-line"></i> Cancel'
        }).then((result) => {
            if (result.value) {
                $('.preloader').show();
                $.ajax({
                    type: "POST",
                    url: UrlContent("Allocation/Delete/" + id),        
                    success: function (result) {
                        console.log(id);
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Success',
                                html: result.message,
                            });

                            Warranty.Allocation.Option.DistrictStatetable.ajax.reload();
                            window.location.reload();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                html: result.message,
                            })
                        }
                    }
                })
            }
        });
    }
   
}