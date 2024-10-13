Warranty.ContractList = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };

    this.AllList = function () {
        var options = {
            searching: false,
            paging: true,
            pageLength: 10,
            lengthMenu: [[10, 25, 50, 100, 500], [10, 25, 50, 100, 500]],
            processing: true,
            info: true,
            ajax: {
                type: "POST",
                url: UrlContent("ContractList/GetContractList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    dtParms.extra_search = $("#contractType").val();
                    dtParms.startDate = options.startDate || null;
                    dtParms.endDate = options.endDate || null;
                    return dtParms;
                },
                complete: function (response, result) {

                    var tableBottom = $(`#ContractListTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select")
                    var lgndrp = $(`#ContractListTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#ContractListTable_wrapper .top`).remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [

                
                { data: "doctorName", name: "DoctorName", autoWidth: true, className: "text-left" },
                { data: "startDateString", name: "StartDate", autoWidth: true, className: "text-left" },
                { data: "endDateString", name: "EndDate", autoWidth: true, className: "text-left" },

                { data: "contractTypeName", name: "ContractTypeId", autoWidth: true, className: "text-left" },

                { data: "createdByName", name: "CreatedByName", className: "text-left col-1" },
                { data: "createdDateString", name: "CreatedDateString", className: "text-left col-1" },

            ],
            order: [[1, 'desc']],
            dom: '<"top"flp>rt<"row btmpage mb-1 mt-2"<"col-12 col-md-4 txtSearchId"><"col-12 col-md-2 lgndrp"><"col-12 col-md-3"i><"col-12 col-md-3"p>>',
        };

        var dateRangePicker = flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            onClose: function (selectedDates) {
                var startDate = selectedDates[0] ? adjustForTimezone(selectedDates[0]) : null;
                var endDate = selectedDates[1] ? adjustForTimezone(selectedDates[1]) : null;
                options.startDate = startDate;
                options.endDate = endDate;

                if ($.fn.dataTable.isDataTable('#ContractListTable')) {
                    $("#ContractListTable").DataTable().ajax.reload();
                } else {
                    fetchContractListListData(null, null);
                }
            }
        });

        function adjustForTimezone(date) {
            let offset = date.getTimezoneOffset();
            let adjustedDate = new Date(date.getTime() - (offset * 60 * 1000));
            return adjustedDate.toISOString().split('T')[0];
        }

        function fetchContractListListData(startDate, endDate) {
            if ($.fn.dataTable.isDataTable('#ContractListTable')) {
                $("#ContractListTable").DataTable().clear().destroy();
            }

            Warranty.ContractList.Option = $.extend({}, Warranty.ContractList.Option, options);
            Warranty.ContractList.Option.Table = new $("#ContractListTable").DataTable(
                {
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
        fetchContractListListData(null, null);
    };

    this.Search = function () {
        Warranty.ContractList.Option.Table.ajax.reload();
    }
}