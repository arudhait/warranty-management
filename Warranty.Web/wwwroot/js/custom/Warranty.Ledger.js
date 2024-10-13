Warranty.Ledger = new function () {
    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };
    this.Init = function (options) {
        Warranty.Ledger.Option = $.extend({}, Warranty.Ledger.Option, options);
        Warranty.Ledger.Option.Table = new $("#LedgerTable").DataTable({
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
                url: UrlContent("Ledger/GetLedgerList"),
                data: function (dtParms) {
                    dtParms.search.value = $("#txtSearch").val();
                    return dtParms;
                },
                complete: function (response, result) {
                    var tableBottom = $(`#LedgerTable_wrapper .btmpage`).detach();
                    $(".top_pagging").prepend(tableBottom);
                    $(".dataTables_length select").removeClass("form-control form-control-sm").addClass("form-select");
                    var lgndrp = $(`#LedgerTable_length`).detach();
                    $(".lgndrp").prepend(lgndrp);
                    $(`#LedgerTable_wrapper .top`).remove();
                    $("#searchId").removeClass("hide");
                }
            },
            columns: [
                {
                    data: "ledgerId", orderable: false, className: "text-center col-1", visible: false,
                    render: function (data, type, row) {
                        var btnEdit = '', btnView = '', btnDelete = '';
                        btnEdit += '<a type="button" onclick="Warranty.Ledger.Add(\'' + row.encId + '\')" class="mr-2 link-primary fs-17" title="Edit State" style="color:#333333"><i class="ri-edit-2-line editHover"></i></a>';
                        btnDelete += '<a type="button" onclick="Warranty.Ledger.Delete(\'' + row.encId + '\',\'' + false + '\')" class="mr-2 fs-17 link-danger" title="Delete State" style="color:#333333"><i class="ri-delete-bin-line deleteHover"></i></a>';
                        return '<div class="justify-content-center hstack gap-2">' + btnEdit + btnDelete + '</div>';
                    }
                },
                { data: "productName", name: "ProductName", autoWidth: true, className: "text-left" },
                { data: "qty", name: "Qty", autoWidth: true, className: "text-left" },
                { data: "price", name: "Price", autoWidth: true, className: "text-left" },
                { data: "typeData", name: "Type", autoWidth: true, className: "text-left" },
                { data: "dateString", name: "DateString", autoWidth: true, className: "text-left" },
                { data: "isCredit", name: "IsCredit", autoWidth: true, className: "text-left" },
                { data: "remarks", name: "Remarks", autoWidth: true, className: "text-left" },
                { data: "inwardOutwardItemId", name: "InwardOutwardItemId", autoWidth: true, className: "text-center" },    
                { data: "createdByName", name: "CreatedByName", autoWidth: true, className: "text-left" },
                { data: "createdOnString", name: "CreatedOnString", autoWidth: true, className: "text-left" },
               
            ],
            order: [[0, 'desc']],
        });
    };
    this.Search = function () {
        Warranty.Ledger.Option.Table.ajax.reload();
    }

}