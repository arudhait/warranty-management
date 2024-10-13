Warranty.BreakdownList = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };

    this.AllList = function () {
        var options = {
            // DataTable configuration options
            searching: false,
            paging: true,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
            processing: true,
            info: true,
            dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-12 col-md-4 txtSearchId"><"col-12 col-md-2 lgndrp"><"col-12 col-md-3"i><"col-12 col-md-3"p>>',
            ajax: {
                type: "POST",
                url: UrlContent("BreakDownList/GetBreakDownList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    dtParms.extra_search = $("#conclusion").val();
                    dtParms.startDate = options.startDate || null;
                    dtParms.endDate = options.endDate || null;
                    return dtParms;
                },
                complete: function (response, result) {
                    var tableBottom = $('#BreakdownListTable_wrapper .btmpage').detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                    var lgndrp = $('#BreakdownListTable_length').detach();
                    $(".lgndrp").prepend(lgndrp);
                    $('#BreakdownListTable_wrapper .top').remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                // Your column definitions remain unchanged
                {
                    data: "breakdownId",
                    orderable: false,
                    className: "text-center col-1",
                    render: function (data, type, row) {
                        var btnEdit = `<a type="button" onclick="Warranty.BreakdownList.Add('${row.encId}')" class="mr-2 link-primary fs-17" title="Edit"><i class="ri-edit-2-line editHover"></i></a>`;
                        var btnView = `<a type="button" onclick="Warranty.BreakdownList.View('${row.encId}')" class="mr-2 fs-17 link-success" title="View"><i class="ri-eye-line viewHover"></i></a>`;
                        var btnDelete = `<a type="button" onclick="Warranty.BreakdownList.Delete('${row.encId}', '${false}')" class="mr-2 fs-17 link-danger" title="Delete"><i class="ri-delete-bin-line deleteHover"></i></a>`;
                        return `<div class="justify-content-center hstack gap-2">${btnView}${btnEdit}${btnDelete}</div>`;
                    }
                },
                { data: "breakdownId", orderable: false, className: "text-center col-1", visible: false },
                { data: "doctorName", name: "doctorName", className: "text-left col-1" },
                { data: "callRegDateString", name: "CallRegDateString", className: "text-left col-1" },
                { data: "breakdownType", name: "BreakdownType", className: "text-left col-1" },
                {
                    data: "conclusion",
                    name: "Conclusion",
                    className: "text-left col-1",
                    render: function (data, type, row) {
                        let genderText = '';
                        let badgeClass = '';

                        if (typeof data === 'string' && (data.toLowerCase() === 'Continue' || data.toLowerCase() === 'Close')) {
                            genderText = data.charAt(0).toUpperCase() + data.slice(1);
                        } else if (typeof data === 'number') {
                            if (data === 1) {
                                genderText = 'Continue';
                            } else if (data === 2) {
                                genderText = 'Close';
                            }
                        }
                        if (genderText === 'Continue') {
                            badgeClass = 'badge bg-info';
                        } else if (genderText === 'Close') {
                            badgeClass = 'badge bg-danger';
                        }
                        return genderText ? `<span class="${badgeClass}" style="color:white;">${genderText}</span>` : '';
                    }
                },
                { data: "engineerName", name: "engineerName", className: "text-left col-1" },
                { data: "enggFirstVisitDateString", name: "EnggFirstVisitDateString", className: "text-left col-1" },
                { data: "crmNo", name: "crmNo", className: "text-left col-1" },
                { data: "problems", name: "problems", autoWidth: true, className: "text-left" },
                { data: "reqActionName", name: "ReqActionName", autoWidth: true, className: "text-left" },
                { data: "actionTakenName", name: "ActionTakenName", autoWidth: true, className: "text-left" },
                {
                    data: "isActive", name: "isActive", className: "text-left",
                    render: function (data, type, row) {
                        var badge = ''
                        if (row.isActive)
                            badge += '<span class="badge bg-success-subtle text-success">Active</span>'
                        else
                            badge += '<span class="badge bg-danger-subtle text-danger">In-Active</span>'
                        return badge;
                    }
                },
                { data: "createdDateString", name: "CreatedDateString", className: "text-left" },
                { data: "createdByName", name: "CreatedByName", className: "text-left" },
            ],
            order: [[1, 'desc']],
          
        };

        var dateRangePicker = flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            onClose: function (selectedDates) {
                var startDate = selectedDates[0] ? adjustForTimezone(selectedDates[0]) : null;
                var endDate = selectedDates[1] ? adjustForTimezone(selectedDates[1]) : null;
                options.startDate = startDate;
                options.endDate = endDate;

                if ($.fn.dataTable.isDataTable('#BreakdownListTable')) {
                    $("#BreakdownListTable").DataTable().ajax.reload(); 
                } else {
                    fetchBreakdownListData(null, null); 
                }
            }
        });

        function adjustForTimezone(date) {
            let offset = date.getTimezoneOffset();
            let adjustedDate = new Date(date.getTime() - (offset * 60 * 1000));
            return adjustedDate.toISOString().split('T')[0];
        }

        function fetchBreakdownListData(startDate, endDate) {
            if ($.fn.dataTable.isDataTable('#BreakdownListTable')) {
                $("#BreakdownListTable").DataTable().clear().destroy();
            }

            Warranty.BreakdownList.Option = $.extend({}, Warranty.BreakdownList.Option, options);
            Warranty.BreakdownList.Option.Table = new $("#BreakdownListTable").DataTable({
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
        fetchBreakdownListData(null, null);
    };

    this.Search = function () {
        Warranty.BreakdownList.Option.Table.ajax.reload();
    }


    this.Add = function (id = '') {
        $(".preloader").show();
        $.ajax({
            type: "GET",
            url: "/BreakDownList/_Details/" + id,
            success: function (data) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(data);
                Warranty.Common.InitMask();
                $.validator.unobtrusive.parse($("#BreakdownListForm"));
                $("#common-lg-dialog").modal('show');
            }
        })
    }

    this.Save = function () {
        if ($("#BreakdownListForm").valid()) {
            $(".preloader").show();
            var formdata = $("#BreakdownListForm").serialize();
            $.ajax({
                type: "Post",
                url: "/BreakDownList/Save",
                data: formdata,
                success: function (result) {
                    $(".preloader").hide();
                    if (result.isSuccess) {
                        Warranty.BreakdownList.Option.Table.ajax.reload();
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
        var formdata = $("#BreakdownListForm").serialize();
        $.ajax({
            type: "GET",
            url: "/BreakDownList/_View/" + id,
            success: function (result) {
                $(".preloader").hide();
                $("#common-lg-DialogContent").html(result);
                $("#common-lg-dialog").modal('show');
            },
        })
    }

    this.Delete = function (id, val) {
        Swal.fire({
            title: val == 'true' ? '<h4>Are you sure want to delete this Breakdown Status?</h4>' : '<h4>Are you sure want to delete this Breakdown Status?</h4>',
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
                    url: "/BreakDownList/Delete",
                    data: {
                        id: id,
                    },
                    success: function (result) {
                        $('.preloader').hide();
                        if (result.isSuccess) {
                            Warranty.BreakdownList.Option.Table.ajax.reload();
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