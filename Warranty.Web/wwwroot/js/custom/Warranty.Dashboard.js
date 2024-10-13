Warranty.Dashboard = new function () {

    this.Option = {
        Table: null,
        TableId: "",
        SearchId: "",
        Id: "",
    };

    this.CustomerList = function () {
        var dateRangePicker = flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            onClose: function (selectedDates) {
                var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                fetchCustomerData(startDate, endDate);
                fetchDueExpiredData(startDate, endDate);
                
            }
        });

        function fetchCustomerData(startDate, endDate) {
            $.ajax({
                url: UrlContent("DashBoard/_CustomerList"),
                type: "POST",
                data: {
                    startDate: startDate,
                    endDate: endDate
                },
                success: function (response) {
                    $("#divCustomerList").html(response);
                    initializeCustomerDataTable(startDate, endDate);
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        }

        // Function to initialize DataTable for Client Product Mapping
        function initializeCustomerDataTable(startDate, endDate) {
            Warranty.Dashboard.Option.CustomerListTable = $("#CustomerListTable").DataTable(
                {
                    searching: false,
                    paging: true,
                    serverSide: true,
                    processing: true,
                    bLengthChange: false,
                    bInfo: false,
                    async: false,
                    lengthMenu: [[10], [10]],
                    pageLength: 10,
                    ajax: {
                        url: UrlContent("DashBoard/SearchList/"),
                        type: "POST",
                        data: function (dtParms) {
                            dtParms.search.value = $("#CustomerListTable_filter input").val();
                            dtParms.startDate = startDate;
                            dtParms.endDate = endDate;
                            return dtParms;
                        },
                    },
                    columns: [
                       
                        { data: "doctorName", name: "DoctorName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                        { data: "hospitalName", name: "HospitalName", autoWidth: false },
                        { data: "mobileNo", name: "MobileNo", autoWidth: true },
                        { data: "districtName", name: "DistrictName", autoWidth: true },
                        { data: "email", name: "Email", autoWidth: true },

                    ],
                    order: [[4, 'desc']],
                }
            );

        }
       
        fetchCustomerData(null, null);
       
       this.DueWarratyList = function () {
        var dateRangePicker = flatpickr("#dateRange", {
            mode: "range",
            dateFormat: "Y-m-d",
            onClose: function (selectedDates) {
                var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                fetchDueData(startDate, endDate);

            }
        });

        
        function fetchDueData(startDate, endDate) {
            $.ajax({
                url: UrlContent("DashBoard/_DueWarratyList"),
                type: "POST",
                data: {
                    startDate: startDate,
                    endDate: endDate
                },
                success: function (response) {
                    $("#divDueWarratyList").html(response);
                    initializeDueWarrantyListTable(startDate, endDate);
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        }

       
        function initializeDueWarrantyListTable(startDate, endDate) {
            Warranty.Dashboard.Option.DueWarrantyListTable = $("#DueWarrantyListTable").DataTable(
                {
                    searching: false,
                    paging: true,
                    serverSide: true,
                    processing: true,
                    bLengthChange: false,
                    bInfo: false,
                    async: false,
                    lengthMenu: [[10], [10]],
                    pageLength: 10,
                    ajax: {
                        url: UrlContent("DashBoard/DueSearchList/"),
                        type: "POST",
                        data: function (dtParms) {
                            dtParms.search.value = $("#DueWarrantyListTable_filter input").val();
                            dtParms.startDate = startDate;
                            dtParms.endDate = endDate;
                            return dtParms;
                        },
                    },
                    columns: [

                        { data: "doctorName", name: "DoctorName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                        { data: "endDateString", name: "EndDateString", autoWidth: false, className: "text-left" },
                        { data: "installedByString", name: "InstalledByString", autoWidth: true, className: "text-left" },
                        { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },
                   
                       
                        

                    ],
                    order: [[1, 'desc']],
                });

        }
        fetchDueData(null, null);
        }

     this.ExpriredWarratyList = function () {
            var dateRangePicker = flatpickr("#dateRange", {
                mode: "range",
                dateFormat: "Y-m-d",
                onClose: function (selectedDates) {
                    var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                    var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                    fetchExpiredData(startDate, endDate);

                }
            });

           
            function fetchExpiredData(startDate, endDate) {
                $.ajax({
                    url: UrlContent("DashBoard/_ExpriredWarratyList"),
                    type: "POST",
                    data: {
                        startDate: startDate,
                        endDate: endDate
                    },
                    success: function (response) {
                        $("#divExpiredWarratyList").html(response);
                        initializeExpiredWarrantyListTable(startDate, endDate);
                    },
                    error: function (xhr, status, error) {
                        console.error(error);
                    }
                });
            }

          
            function initializeExpiredWarrantyListTable(startDate, endDate) {
                Warranty.Dashboard.Option.ExpriredWarrantyTable = $("#ExpriredWarrantyTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: true,
                        processing: true,
                        bLengthChange: false,
                        bInfo: false,
                        async: false,
                        lengthMenu: [[10], [10]],
                        pageLength: 10,
                        ajax: {
                            url: UrlContent("DashBoard/ExpriredSearchList/"),
                            type: "POST",
                            data: function (dtParms) {
                                dtParms.search.value = $("#ExpriredWarrantyTable_filter input").val();
                                dtParms.startDate = startDate;
                                dtParms.endDate = endDate;
                                return dtParms;
                            },
                        },
                        columns: [

                            { data: "doctorName", name: "DoctorName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                            { data: "endDateString", name: "EndDateString", autoWidth: false, className: "text-left" },
                            { data: "installedByString", name: "InstalledByString", autoWidth: true, className: "text-left" },
                            { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },



                        ],
                        order: [[1, 'desc']],
                    });

            }
            fetchExpiredData(null, null);
        }
        //AMCCMS Due/Expired Report List
        this.DueAMCCMCWarratyList = function () {
            var dateRangePicker = flatpickr("#dateRange", {
                mode: "range",
                dateFormat: "Y-m-d",
                onClose: function (selectedDates) {
                    var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                    var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                    fetchDueAMCCMCData(startDate, endDate);

                }
            });

            // Function to fetch and display data for Client Product Mapping
            function fetchDueAMCCMCData(startDate, endDate) {
                $.ajax({
                    url: UrlContent("DashBoard/_DueAMCMCWarratyList"),
                    type: "POST",
                    data: {
                        startDate: startDate,
                        endDate: endDate
                    },
                    success: function (response) {
                        $("#divDueAMCCMSWarratyList").html(response);
                        initializeDueAMCCMCWarrantyListTable(startDate, endDate);
                    },
                    error: function (xhr, status, error) {
                        console.error(error);
                    }
                });
            }

           
            function initializeDueAMCCMCWarrantyListTable(startDate, endDate) {
                Warranty.Dashboard.Option.DueAMCCMCWarrantyListTable = $("#DueAMCCMCWarrantyListTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: true,
                        processing: true,
                        bLengthChange: false,
                        bInfo: false,
                        async: false,
                        lengthMenu: [[10], [10]],
                        pageLength: 10,
                        ajax: {
                            url: UrlContent("DashBoard/DueAMCCMSSearchList/"),
                            type: "POST",
                            data: function (dtParms) {
                                dtParms.search.value = $("#DueAMCCMCWarrantyListTable_filter input").val();
                                dtParms.startDate = startDate;
                                dtParms.endDate = endDate;
                                return dtParms;
                            },
                        },
                        columns: [

                            { data: "doctorName", name: "DoctorName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                            { data: "endDateString", name: "EndDateString", autoWidth: false, className: "text-left" },
                            { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },




                        ],
                        order: [[1, 'desc']],
                    });

            }
            fetchDueAMCCMCData(null, null);
        }

        this.ExpriredAMCCMCWarratyList = function () {
            var dateRangePicker = flatpickr("#dateRange", {
                mode: "range",
                dateFormat: "Y-m-d",
                onClose: function (selectedDates) {
                    var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                    var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                    fetchExpiredAMCCMCData(startDate, endDate);

                }
            });

            
            function fetchExpiredAMCCMCData(startDate, endDate) {
                $.ajax({
                    url: UrlContent("DashBoard/_ExpriredAMCCMSWarratyList"),
                    type: "POST",
                    data: {
                        startDate: startDate,
                        endDate: endDate
                    },
                    success: function (response) {
                        $("#divExpiredAMCCMSWarratyList").html(response);
                        initializeExpiredAMCCMCWarrantyListTable(startDate, endDate);
                    },
                    error: function (xhr, status, error) {
                        console.error(error);
                    }
                });
            }

           
            function initializeExpiredAMCCMCWarrantyListTable(startDate, endDate) {
                Warranty.Dashboard.Option.ExpipredAMCCMCWarrantyListTable = $("#ExpipredAMCCMCWarrantyListTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: true,
                        processing: true,
                        bLengthChange: false,
                        bInfo: false,
                        async: false,
                        lengthMenu: [[10], [10]],
                        pageLength: 10,
                        ajax: {
                            url: UrlContent("DashBoard/ExpriredAMCCMSSearchList/"),
                            type: "POST",
                            data: function (dtParms) {
                                dtParms.search.value = $("#ExpipredAMCCMCWarrantyListTable_filter input").val();
                                dtParms.startDate = startDate;
                                dtParms.endDate = endDate;
                                return dtParms;
                            },
                        },
                        columns: [

                            { data: "doctorName", name: "DoctorName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                            { data: "endDateString", name: "EndDateString", autoWidth: false, className: "text-left" },
                            { data: "contractTypeName", name: "ContractTypeName", autoWidth: true, className: "text-left" },



                        ],
                        order: [[1, 'desc']],
                    });

            }
            fetchExpiredAMCCMCData(null, null);
        }


        /*****************************  Engineerwise State/District ********************************/

          //AMCCMS Due/Expired Report List
        this.SEwiseStateDistrictList = function () {
            var dateRangePicker = flatpickr("#dateRange", {
                mode: "range",
                dateFormat: "Y-m-d",
                onClose: function (selectedDates) {
                    var startDate = selectedDates[0] ? selectedDates[0].toISOString().split('T')[0] : null;
                    var endDate = selectedDates[1] ? selectedDates[1].toISOString().split('T')[0] : null;
                    fetchSEStateDistrictData(startDate, endDate);

                }
            });

            // Function to fetch and display data for Client Product Mapping
            function fetchSEStateDistrictData(startDate, endDate) {
                $.ajax({
                    url: UrlContent("DashBoard/_DistrictStateList"),
                    type: "POST",
                    data: {
                        startDate: startDate,
                        endDate: endDate
                    },
                    success: function (response) {
                        $("#divSEStateDistrictList").html(response);
                        initializeSEStateDistrictListTable(startDate, endDate);
                    },
                    error: function (xhr, status, error) {
                        console.error(error);
                    }
                });
            }

            // Function to initialize DataTable for Client Product Mapping
            function initializeSEStateDistrictListTable(startDate, endDate) {
                Warranty.Dashboard.Option.EngineerListTable = $("#EngineerListTable").DataTable(
                    {
                        searching: false,
                        paging: true,
                        serverSide: true,
                        processing: true,
                        bLengthChange: false,
                        bInfo: false,
                        async: false,
                        lengthMenu: [[10], [10]],
                        pageLength: 10,
                        ajax: {
                            url: UrlContent("DashBoard/DistrictStateSearchList/"),
                            type: "POST",
                            data: function (dtParms) {
                                dtParms.search.value = $("#EngineerListTable_filter input").val();
                                dtParms.startDate = startDate;
                                dtParms.endDate = endDate;
                                return dtParms;
                            },
                        },
                        columns: [
                            { data: "enggId", name: "EnggId", autoWidth: true, visible: false },
                            { data: "enggName", name: "EnggName", autoWidth: false, className: "text-center text-md-left col-12 col-md-8 col-lg-4" },
                            { data: "districtName", name: "DistrictName", autoWidth: false },                          
                            { data: "stateName", name: "StateName", autoWidth: true },                          
                            

                        ],
                        order: [[0, 'desc']],
                    });

            }
            fetchSEStateDistrictData(null, null);
        }
    }
   
}