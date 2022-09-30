'use strict';

app.controller('MonthLeaveController', ['$scope', 'MonthLeaveService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, MonthLeaveService, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

    console.log(" Customer Additional Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];

    MonthLeaveService.getMonthLeave().then(function (results) {
        debugger;
        $scope.casess = results.data;

        $scope.callmethod();
    }, function (error) {

    });

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

app.controller("ModalInstanceCtrlAddl", ["$scope", '$http', 'ngAuthSettings', "MonthLeaveService", "$filter", 'ProjectService', 'peoplesService', "$modalInstance", "cases", 'FileUploader', function ($scope, $http, ngAuthSettings, MonthLeaveService, $filter, ProjectService, peoplesService, $modalInstance, cases, FileUploader) {
    console.log("cases");
    debugger;
    var User = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.CaseDetail = [];
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    //Get All Customer By CompanyId from customer Link for Dropdown
    $scope.CustomerGet = function () {
        var url = serviceBase + 'api/CustomerPriceService/GetCustomerById';
        $http.get(url)
        .success(function (data) {
            $scope.getCustomerLink = data;
            console.log("$scope.getCustomerLink", $scope.getCustomerLink);
        });
    }
    $scope.CustomerGet();

    //Get All Provider for Dropdown Provider

    $scope.GetPeople = function () {
        debugger;
        var url = serviceBase + 'api/Peoples';
        $http.get(url)
        .success(function (data) {

            $scope.getProvider = data;
            $scope.ProvidersType = $scope.getProvider;
            console.log("$scope.getProvider", $scope.getProvider);
            console.log("$scope.ProvidersType", $scope.ProvidersType);

        });
    }
    $scope.GetPeople();

    //Add Customer Services
    $scope.AddMonthLeave = function (data) {
        debugger;
        console.log("case");
        console.log(data);

        //Geting textbox id and pass in validation focus
        var customer = document.getElementById('site-name-customer');
        var FromDate = document.getElementById('site-name-FromDate');
        var ToDate = document.getElementById('site-name-ToDate');

        //validation for item dropdown / Requaired
        if (data == null || data == "") {
            alert("Nothing To Save!!Plz Fill Required Information");
            item.focus();
            return;
        }

        //validation for cutomer dropdown / Requaired
        if (data.CustomerLinkId == null || data.CustomerLinkId == "") {
            alert("Please Select Customer!!");
            customer.focus();
            return;
        }

        //validation for Date text / Requaired
        if (data.FromDate == null || data.FromDate == "") {
            alert("Please Select Enter From Date!!");
            date.focus();
            return;
        }

        //validation for Date text / Requaired
        if (data.ToDate == null || data.ToDate == "") {
            alert("Please Select Enter To Date!!");
            date.focus();
            return;
        }

        //API URL for Cs Controller hit
        var url = serviceBase + "api/MonthLeaves/AddMonthLeave";

        //Data Posting to controller by ui object
        var dataToPost = {
            Description: data.Description,
            FromDate: data.FromDate,
            ToDate: data.ToDate,
            CustomerLinkId: data.CustomerLinkId,
        };
        console.log(dataToPost);
        debugger;
        $http.post(url, dataToPost)
        .success(function (data) {
            debugger;
            console.log("Error Got Here");
            console.log(data);
            if (data.id == 0) {

                $scope.gotErrors = true;
                if (data[0].exception == "Already") {
                    console.log("Got This User Already Exist");
                    $scope.AlreadyExist = true;
                }
            }
                //Passing Alert message by cs 
            else if (data.Message == "Error") {
                alert("Month Leave Not Save!Some Error Occure!")
                $modalInstance.close(data);
            }

                //Passing Alert message by cs 
            else if (data.Message == "Sucessfully") {
                debugger;
                alert("Month Leave Add Sucessfully!")
                $modalInstance.close(data);
                window.location.reload();
            }

            else {
                console.log(data);
                console.log(data);
                $modalInstance.close(data);
            }

        })
        .error(function (data) {
            console.log("Error Got Here is ");
            console.log(data);
            // return $scope.showInfoOnSubmit = !0, $scope.revert()
        })
    };

    //Put PutAddMonthLeave
    $scope.PutAddMonthLeave = function (putdata) {
        debugger;
        $scope.CasesData = {
        };
        $scope.loogourl = cases.LogoUrl;

        var customer = document.getElementById('site-name-customer');
        var FromDate = document.getElementById('site-name-FromDate');
        var ToDate = document.getElementById('site-name-ToDate');

        //Geting All Data in A Variable 
        if (cases) {
            $scope.CaseData = cases;
            console.log("found Puttt case");
            console.log(cases);
            console.log($scope.CaseData);
        }

        //validation for item dropdown / Required
        if (putdata == null || putdata == "") {
            alert("Nothing To Save!!Plz Fill Required Information");
            item.focus();
            return;
        }

        //validation for cutomer dropdown / Required
        if (putdata.CustomerLinkId == null || putdata.CustomerLinkId == "") {
            alert("Please Select Customer!!");
            customer.focus();
            return;
        }

        //validation for Date text / Required
        if (putdata.FromDate == null || putdata.FromDate == "") {
            alert("Please Select Enter From Date!!");
            date.focus();
            return;
        }

        //validation for Date text / Requaired
        if (putdata.ToDate == null || putdata.ToDate == "") {
            alert("Please Select Enter To Date!!");
            date.focus();
            return;
        }

        //API URL for Cs Controller hit
        var url = serviceBase + "api/MonthLeaves";

        //Data Posting to controller by ui object
        var dataToPost = {
            Id: putdata.Id,
            Description: putdata.Description,
            FromDate: putdata.FromDate,
            ToDate: putdata.ToDate,
            CustomerLinkId: putdata.CustomerLinkId
        };
        console.log(dataToPost);

        $http.put(url, dataToPost)
        .success(function (data) {
            console.log("Error Got Here");
            console.log(data);
            if (data.id == 0) {
                $scope.gotErrors = true;
                if (data[0].exception == "Already") {
                    console.log("Got This User Already Exist");
                    $scope.AlreadyExist = true;
                }
            }
                //Passing Alert message by cs 
            else if (data.Message == "Error") {
                alert("Month Leaves Not Updated!Some Error Occurs!")
                $modalInstance.close(data);
            }

                //Passing Alert message by cs 
            else if (data.Message == "Sucessfully") {
                debugger;
                alert("Month Leave Update Sucessfully!")
                $modalInstance.close(data);
                window.location.reload();
            }

            else {
                $modalInstance.close(data);
            }
        })

        .error(function (data) {
            console.log("Error Got Here is ");
            console.log(data);
        })
    }

    console.log("delete modal opened");
    debugger;
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
    //Delete
    $scope.deleteMonthLeave = function (dataToPost, $index) {
        debugger;
        console.log("Delete case controller");

        MonthLeaveService.deleteMonthLeave(dataToPost).then(function (results) {
            console.log("Del");

            console.log("index of item " + $index);
            console.log($scope.casess.length);
            console.log($scope.casess.length);

            $modalInstance.close(dataToPost);

        }, function (error) {
            alert(error.data.message);
        });
    }


}])