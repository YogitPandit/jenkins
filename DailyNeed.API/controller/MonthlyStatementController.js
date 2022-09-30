'use strict';
app.controller('MonthlyStatementController', ['$scope', "$filter", "$http", "ngTableParams", '$modal', function ($scope, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

    $(function () {
        $('input[name="daterange"]').daterangepicker({
            timePicker: true,
            timePickerIncrement: 5,
            timePicker12Hour: true,
            format: 'DD/MM/YYYY',
        });
    });

    console.log(" Customer Monthly Statement Controller reached");

    $scope.currentPageStores = {};
    //Getting all customer Month List for Monthaly Satetment 
    $scope.GetCustomerStatementList = function () {
        debugger;
        var url = serviceBase + 'api/MonthlyBill/MonthlyStatement';
        $http.get(url)
        .success(function (data) {
            $scope.getBillStatement = data;
            $scope.p = $scope.getBillStatement;
            console.log("$scope.getAllBillStatement", $scope.getBillStatement);
            $scope.callmethod();

        });
    }
    $scope.GetCustomerStatementList();

    //Get Customer data by Date filters
    $scope.oldorders = [];
    $scope.getCustomersByDate = function () {
        debugger;
        var start = "";
        var end = "";
        var f = $('input[name=daterangepicker_start]');
        var g = $('input[name=daterangepicker_end]');
        if (!$('#dat').val()) {
            end = '';
            start = '';
            alert("Select Start and End Date")
            return;
        }
        else {
            start = f.val();
            end = g.val();
            $scope.dateformatedata = [];
            angular.forEach($scope.getBillStatement, function (value, key) {
                debugger;
                //var d = value.Date;
                value.Date = $filter("date")(value.Date, 'dd-MM-yyyy');
                if (value.Date >= start && value.Date <= end) {
                    $scope.dateformatedata.push(value);
                }
                else {
                }
                console.log($scope.dateformatedata);
            });
            $scope.getBillStatement = $scope.dateformatedata;
            if ($scope.getBillStatement != null) {
                $scope.callmethod();
            }
        }

    }


    //Getting Customer Monthaly Satetment on the bases of customerid 
    $scope.SelectCustomer = function (data) {
        debugger;
        $scope.subcategorydata = [];
        angular.forEach($scope.getBillStatement, function (value, key) {
            if (value.CustomerId == data) {
                $scope.subcategorydata.push(value);
            }
            else {
            }
            console.log($scope.subcategorydata);
        });
        $scope.getBillStatement = $scope.subcategorydata
        if ($scope.getBillStatement.length == 0) {
            debugger;
            angular.forEach($scope.p, function (value, key) {
                if (value.CustomerId == data) {
                    $scope.subcategorydata.push(value);
                }
                else {
                }
                console.log($scope.subcategorydata);
            });
        }
        if (data != null) {
            $scope.callmethod();
        }
    }



    //Get All Customer for Dropdown by companyid
    $scope.GetCustomer = function () {
        var url = serviceBase + 'api/CustomerAdditional/GetBycomnyId';
        $http.get(url)
        .success(function (data) {
            $scope.getCustomer = data;
            console.log("$scope.getCustomer", $scope.getCustomer);
            if ($scope.subcategorydata == null || $scope.dateformatedata == null) {
                $scope.callmethod();
            }
        });
    }
    $scope.GetCustomer();

    //Get Data
    $scope.casess = [];

    //Pagination  
    $scope.callmethod = function () {
        debugger;
        var init;
        return $scope.stores = $scope.getBillStatement,
            $scope.searchKeywords = "",
            $scope.filteredStores = [],
            $scope.row = "",
            $scope.select = function (page) {
                var end, start; console.log("select"); console.log($scope.stores);
                return start = (page - 1) * $scope.numPerPage, end = start + $scope.numPerPage, $scope.currentPageStores = $scope.filteredStores.slice(start, end)
            },
            $scope.onFilterChange = function () {
                console.log("onFilterChange"); console.log($scope.stores);
                return $scope.select(1), $scope.currentPage = 1, $scope.row = ""
            },
            $scope.onNumPerPageChange = function () {
                console.log("onNumPerPageChange"); console.log($scope.stores);
                return $scope.select(1), $scope.currentPage = 1
            },
            $scope.onOrderChange = function () {
                console.log("onOrderChange"); console.log($scope.stores);
                return $scope.select(1), $scope.currentPage = 1
            },
            $scope.search = function () {
                console.log("search");
                console.log($scope.stores);
                console.log($scope.searchKeywords);
                return $scope.filteredStores = $filter("filter")($scope.stores, $scope.searchKeywords), $scope.onFilterChange()
            },
            $scope.order = function (rowName) {
                console.log("order"); console.log($scope.stores);
                return $scope.row !== rowName ? ($scope.row = rowName, $scope.filteredStores = $filter("orderBy")($scope.stores, rowName), $scope.onOrderChange()) : void 0
            },
            $scope.numPerPageOpt = [20, 50, 100, 500, 1000],
            $scope.numPerPage = $scope.numPerPageOpt[0],
            $scope.currentPage = 1,
            $scope.currentPageStores = [],
            (init = function () {
                return $scope.search(), $scope.select($scope.currentPage)
            })
        ()
        window.location.reload();
    }
}]);

app.controller("ModalInstanceCtrl", ["$scope", '$http', 'ngAuthSettings', "MonthLeaveService", "$filter", 'ProjectService', 'peoplesService', "$modalInstance", "cases", 'FileUploader', function ($scope, $http, ngAuthSettings, MonthLeaveService, $filter, ProjectService, peoplesService, $modalInstance, cases, FileUploader) {
    console.log("cases");
    var User = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.CaseDetail = [];
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


    $scope.casess = [];

    //Get Data In Veriable
    if (cases) {
        $scope.CaseData = cases;
        $scope.CaseData.FromDate = $filter("date")($scope.CaseData.FromDate, 'yyyy-MM-dd');
        $scope.CaseData.ToDate = $filter("date")($scope.CaseData.ToDate, 'yyyy-MM-dd');
        console.log("found case");
        console.log(cases);
        console.log($scope.CaseData);
    }



}])