'use strict';
app.controller('MonthlyBillController', ['$scope', 'MonthLeaveService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, MonthLeaveService, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

    console.log(" Customer Additional Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];

    //Binding Billing DropDown Hard Code
    $scope.BillingType = [{
        "id": 1,
        "name": "Monthly"
    },
        {
            "id": 2,
            "name": "Weekly"
        },
        {
            "id": 3,
            "name": "Half-Monthly"
        }];

    //Filling Sub Sub Category Data On Basis of Selected Category
    $scope.GetGridData = function (data) {
        $scope.GetCustomerList();
        debugger;
        $scope.BType = [];
        angular.forEach($scope.getBillByName, function (value, key) {
            if (value.BillingType == data) {
                $scope.BType.push(value);
            }
            if (value == null) {
                alert ('Data Not Exists')
            }
            console.log($scope.BType);
        });
        $scope.getBillByName = $scope.BType;
        if(data!=null)
        {
            $scope.callmethod();
        }
    }


    //Getting all customer Month List for Monthaly Satetment 
    $scope.GetCustomerList = function () {
        debugger;
        var url = serviceBase + 'api/MonthlyBill';
        $http.get(url)
        .success(function (data) {
            $scope.getBillByName = data;
            console.log("$scope.getAllBill", $scope.getBillByName);
            if($scope.BType == null)
            {
                $scope.callmethod();
            }
            
        });
    }
    $scope.GetCustomerList();

    // Getting Search
    $scope.GetCustomerListByName = function () {
        $scope.getBillByName = [];
        var url = serviceBase + "api/MonthlyBill/GetBillByName?name=" + $scope.Name;
        $http.get(url).success(function (results) {
            $scope.getBillByName = results;

            $scope.callmethod();
        })
         .error(function (data) {
             console.log(data);
         })
    };

    //Post Data For Monthly Satetment for All Table Data
    $scope.test = [];
    $scope.MonthBillList = function (data) {
        debugger;
        var url = serviceBase + 'api/MonthlyBill/AddCustomerBillList';
        //Data Posting to controller by ui object
        $scope.p = [];
        $scope.p = data;
        $scope.DatatoPostnew = [];
        for (var i = 0; i < $scope.p.length; i++) {
            var d = $scope.p[i];
            debugger;
            var dataTopost = {
                FinalQty: d.Quantity,
                Price: d.Price,
                Date: d.LastBillingDate,
                CustomerLinkId:d.CustomerLinkId,
                PeopleID: d.PeopleID,
                BillingType: d.BillingType,
            }
            $scope.DatatoPostnew.push(dataTopost);
        }
        $http.post(url, $scope.DatatoPostnew);
    };

    //Post One Custmer Data For Monthly Satetment
    $scope.singleTest = [];
    $scope.monthBillListSingle = function (data) {
        debugger;
        var url = serviceBase + 'api/MonthlyBill/AddCustomerBillList';
        //Data Posting to controller by ui object
        $scope.DatatoPostnewsingle = [];
        var dataToPostSingle = {
            FinalQty: data.Quantity,
            Price: data.Price,
            Date: data.LastBillingDate,
            Customer: {
                CustomerId: data.CustomerId
            },
            Provider: {
                PeopleID: data.PeopleID
            },
            BillingType: data.BillingType,
        };
        $scope.DatatoPostnewsingle.push(dataToPostSingle);

        $http.post(url, $scope.DatatoPostnewsingle).success(function (results) {
            debugger;
            window.location.reload();
        })
    };

    //Get Data
    $scope.casess = [];

    //Pagination  
    $scope.callmethod = function () {
        debugger;
        var init;
        return $scope.stores = $scope.getBillByName,
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
            $scope.numPerPageOpt = [50, 100, 500, 100],
            $scope.numPerPage = $scope.numPerPageOpt[2],
            $scope.currentPage = 1,
            $scope.currentPageStores = [],
            (init = function () {
                return $scope.search(), $scope.select($scope.currentPage)
            })
        ()
        window.location.reload();
    }
}]);

app.controller("ModalInstanceCtrlAdd", ["$scope", '$http', 'ngAuthSettings', "MonthLeaveService", "$filter", 'ProjectService', 'peoplesService', "$modalInstance", "cases", 'FileUploader', function ($scope, $http, ngAuthSettings, MonthLeaveService, $filter, ProjectService, peoplesService, $modalInstance, cases, FileUploader) {
    console.log("cases");
    var User = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.CaseDetail = [];
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },



    //Get All Customer List By Name

    //$scope.GetCustomerListByName = function () {
    //    debugger;
    //    var url = serviceBase + 'api/MonthlyBill/GetBillByName?name=' + Name;
    //    $http.get(url)
    //    .success(function (data) {

    //        $scope.getBillByName = data;
    //        console.log("$scope.getBillByName", $scope.getBillByName);
    //        $scope.callmethod();
    //    });
    //}
    //  $scope.GetCustomerListByName();

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