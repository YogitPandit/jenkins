'use strict';
app.controller('CustomerInvoiceController', ['$scope', "$filter", "$http", "ngTableParams", '$modal', function ($scope, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

    console.log(" Customer Additional Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];

    //Get all Invoice by Company Id
    $http.get(serviceBase + 'api/CustomerInvoice/GetAllInvoice').then(function (results) {
        $scope.casess = results.data;
        $scope.callmethod();
    }, function (error) {
    });

    //Open Popup for show child customer data
    $scope.customerStatement = function (data) {
        debugger;
        var fDate = $filter("date")(data.FromDate, 'yyyy-MM-dd');
        var tDate = $filter("date")(data.ToDate, 'yyyy-MM-dd');
        $scope.getCustStatement = [];
        var modalInstance;
        var url = serviceBase + "api/MonthlyBill/MonthlyDateStatement?custId=" + data.CustomerId + "&fDate=" + fDate + "&tDate=" + tDate;
        $http.get(url).success(function (results) {
            $scope.getCustStatement = results;
            if ($scope.getCustStatement != "") {
                modalInstance = $modal.open(
                    {
                        templateUrl: "myModalCustomerStatement.html",
                        controller: "ModalInstanceCtrlStateCase", resolve: { state: function () { return $scope.getCustStatement } }
                    })
            }
        })
         .error(function (data) {
             console.log(data);
         })
    };

    //Open Popup
    $scope.open = function () {
        console.log("Modal opened Customer Additional");

        var modalInstance;

        modalInstance = $modal.open(
        {
            templateUrl: "myModalContentAdd.html",
            controller: "ModalInstanceCtrlAddl", resolve: { cases: function () { return $scope.items } }
        }), modalInstance.result.then(function (selectedItem) {
            $scope.currentPageStores.push(selectedItem);
        },
        function () {
            console.log("Cancel Condintion");

        })
    };

    //Open Edit Popup
    $scope.edit = function (customer) {
        debugger;
        var modalInstance;
        modalInstance = $modal.open(
        {
            templateUrl: "myModalContentPut.html",
            controller: "ModalInstanceCtrlAddl", resolve: { cases: function () { return customer } }
        }), modalInstance.result.then(function (selectedItem) {
            $scope.casess.push(selectedItem);
            _.find($scope.casess, function (cases) {
                if (cases.id == selectedItem.id) {
                    cases = selectedItem;
                }
            });
            $scope.casess = _.sortBy($scope.cases, 'Id').reverse();
            $scope.selected = selectedItem;
        },
        function () {
            console.log("Cancel Condintion");
        })
    };
    //Open Delete Popup
    $scope.opendelete = function (data, $index) {
        debugger;
        console.log(data);
        console.log("Delete Dialog called for Case");


        var modalInstance;
        modalInstance = $modal.open(
        {
            templateUrl: "myModaldeleteMonthLeave.html",
            controller: "ModalInstanceCtrlAddl", resolve: { cases: function () { return data } }
        }), modalInstance.result.then(function (selectedItem) {
            $scope.currentPageStores.splice($index, 1);
        },
        function () {
            console.log("Cancel Condintion");

        })
    };

    //Get Data
    $scope.casess = [];

    //Pagination and 
    $scope.callmethod = function () {

        var init;
        return $scope.stores = $scope.casess,

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

        $scope.numPerPageOpt = [3, 5, 10, 20],
        $scope.numPerPage = $scope.numPerPageOpt[2],
        $scope.currentPage = 1,
        $scope.currentPageStores = [],
        (init = function () {
            return $scope.search(), $scope.select($scope.currentPage)
        })

        ()
    }
}]);

app.controller("ModalInstanceCtrlStateCase", ["$scope", '$http', 'state', "$modalInstance", 'ngAuthSettings', function ($scope, $http, state, $modalInstance, ngAuthSettingsstate) {
    debugger;
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
   $scope.CustomerData = {};
    if (state) {
        $scope.CustomerData = state;
    }
}])