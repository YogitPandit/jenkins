'use strict';

app.controller('CustomerAdditionalCtrl', ['$scope', 'CustomerAdditionalService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, CustomerAdditionalService, $filter, $http, ngTableParams, $modal) {
    debugger;
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.Provider = $scope.UserRole;

    console.log(" Customer Additional Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];

    CustomerAdditionalService.getcustomersAdditional().then(function (results) {
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
                templateUrl: "myModalContentAddService.html",
                controller: "ModalInstanceCtrlAddServ", resolve: { cases: function () { return $scope.items } }
            }), modalInstance.result.then(function (selectedItem) {


                //$scope.currentPageStores.push(selectedItem);

            },
            function () {
                console.log("Cancel Condintion");

            })
    };

    //Open Edit Popup
    $scope.edit = function (customer) {
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myModalPut.html",
                controller: "ModalInstanceCtrlAddServ", resolve: { cases: function () { return customer } }
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
        console.log(data);
        console.log("Delete Dialog called for Case");


        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myModaldelete.html",
                controller: "ModalInstanceCtrlAddServ", resolve: { cases: function () { return data } }
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

app.controller("ModalInstanceCtrlAddServ", ["$scope", '$http', 'ngAuthSettings', "CustomerAdditionalService", "$filter", 'ProjectService', 'peoplesService', "$modalInstance", "cases", 'FileUploader', function ($scope, $http, ngAuthSettings, CustomerAdditionalService, $filter, ProjectService, peoplesService, $modalInstance, cases, FileUploader) {
    console.log("cases");
    $scope.User = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.CaseDetail = [];
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    $scope.casess = [];

    //Get Data In Veriable
    if (cases) {
        $scope.CaseData = cases;
        $scope.CaseData.Date = $filter("date")($scope.CaseData.Date, 'yyyy-MM-dd');
        console.log("found case");
        console.log(cases);
        console.log($scope.CaseData);
    }

    //Get All Customer Service for Dropdown Service
    $scope.custServices = function (data) {
        debugger;
        if (data != null) {
            debugger;
            var url = serviceBase + 'api/CustomerAdditional/GetCustServicesById?custid=' + data.CustomerLinkId;
            $http.get(url)
            .success(function (data) {
                $scope.getCustService = data;
                console.log("$scope.custServices", $scope.getCustService);
            });
        }
        else if ($scope.CaseData != null) {
            var url = serviceBase + 'api/CustomerAdditional/GetCustServicesById?custid=' + $scope.CaseData.CustomerlinkId;
            $http.get(url)
            .success(function (data) {
                debugger;
                $scope.getCustService = data;
                console.log("$scope.custServices", $scope.getCustService);
            });
        }
    }
    $scope.custServices();


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

    //Get PWarehouse from Pwarehouse table to fill dropdown // api in cusotmer controller
    $scope.warehouse = [];
    $scope.GetWarehouses = function () {
        debugger;
        var url = serviceBase + 'api/Customers/GetPWarehouse';
        $http.get(url)
        .success(function (data) {
            $scope.warehouse = data;
            console.log("$scope.getWarehouse", $scope.warehouse);
        });
    }
    $scope.GetWarehouses();

    //Filling Category Data On Basis of Selected Base Category
    $scope.GetServices = function (data) {
        $scope.custServices(data);
    }


    //Add Customer Services
    $scope.AddCustomerAddServices = function (data) {
        console.log("case");
        console.log(data);
        var itemid = data.Itemid;

        //Geting textbox id and pass in validation focus
        var item = document.getElementById('site-name-item');
        var customer = document.getElementById('site-name-customer');
        var Pwarehouse = document.getElementById('site-name-warehouse');
        var qty = document.getElementById('site-name-qty');
        var date = document.getElementById('site - name - Date');

        //validation for item dropdown / Requaired
        if (data == null || data == "") {
            alert("Nothing To Save!!Plz Fill Required Information");
            item.focus();
            return;
        }
        //validation for item dropdown / Requaired
        if (data.ID == null || data.ID == "") {
            alert("Please Select Service!!");
            item.focus();
            return;
        }

        //validation for cutomer dropdown / Requaired
        if (data.CustomerLinkId == null || data.CustomerLinkId == "") {
            alert("Please Select Customer!!");
            customer.focus();
            return;
        }

        //validation for Warehouse dropdown / Requaired
        if (data.PWarehouseId == null || data.PWarehouseId == "") {
            alert("Please Select Warehouse!!");
            customer.focus();
            return;
        }

        //validation for Qty text / Requaired
        if (data.Qty == null || data.Qty == "") {
            alert("Please Enter Qty!!");
            qty.focus();
            return;
        }

        //validation for Date text / Requaired
        if (data.Date == null || data.Date == "") {
            alert("Please Enter Date!!");
            date.focus();
            return;
        }

        //API URL for Cs Controller hit
        var url = serviceBase + "api/CustomerAdditional/CustomerAdditionalService";
        debugger;
        //Data Posting to controller by ui object
        var dataToPost = {
            Quantity: data.Qty,
            Date: data.Date,
            CustomerLinkId: data.CustomerLinkId,
            CustomerServiceId: data.ID,
            PWarehouseId: data.PWarehouseId,
            IsLess: data.IsLess
        };
        console.log(dataToPost);

        $http.post(url, dataToPost)
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
                alert("Customer Additional Services Not Save!Some Error Occure!")
                $modalInstance.close(data);
            }

                //Passing Alert message by cs 
            else if (data.Message == "Sucessfully") {
                alert("Customer Additional Services Add Sucessfully!")
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

    //Put
    $scope.PutCustomerAdditionalServices = function (data) {
        $scope.CaseData = {
        };
        $scope.loogourl = cases.LogoUrl;
        //Geting All Data in A Variable 
        if (cases) {
            $scope.CaseData = cases;
            console.log("found Puttt case");
            console.log(cases);
            console.log($scope.CaseData);
        }

        //validation for item dropdown / Requaired
        if (data == null || data == "") {
            alert("Nothing To Save!!Plz Fill Required Information");
            item.focus();
            return;
        }
        //validation for item dropdown / Requaired
        if (data.ID == null || data.ID == "") {
            alert("Please Select Service!!");
            item.focus();
            return;
        }

        //validation for cutomer dropdown / Requaired
        if (data.CustomerLinkId == null || data.CustomerLinkId == "") {
            alert("Please Select Customer!!");
            customer.focus();
            return;
        }

        //validation for Warehouse dropdown / Requaired
        if (data.PWarehouseId == null || data.PWarehouseId == "") {
            alert("Please Select Warehouse!!");
            customer.focus();
            return;
        }

        //validation for Qty text / Requaired
        if (data.Quantity == null || data.Quantity == "") {
            alert("Please Enter Qty!!");
            qty.focus();
            return;
        }

        //validation for Date text / Requaired
        if (data.Date == null || data.Date == "") {
            alert("Please Enter Date!!");
            date.focus();
            return;
        }

        console.log("Update");

        //API URL for Cs Controller hit
        var url = serviceBase + "api/CustomerAdditional/UpdateCustomerAddServices";

        //Data Posting to controller by ui object
        var dataToPost = {
            custAddServID: data.custAddServID,
            Quantity: data.Quantity,
            Date: data.Date,
            CustomerLinkId: data.CustomerLinkId,
            CustomerServiceId: data.ID,
            PWarehouseId: data.PWarehouseId,
            IsLess: data.IsLess
        };
        console.log(dataToPost);

        $http.put(url, dataToPost)
        .success(function (data) {
            console.log("Error Gor Here");
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
                alert("Customer Additional Services Not Update!Some Error Occure!")
                $modalInstance.close(data);
            }

                //Passing Alert message by cs 
            else if (data.Message == "Sucessfully") {
                alert("Customer Additional Services Update Sucessfully!")
                $modalInstance.close(data);
                window.location.reload();
            }

            else {
                $modalInstance.close(data);
            }
        })

         .error(function (data) {
             console.log("Error Got Heere is ");
             console.log(data);
         })
    }

    console.log("delete modal opened");

    //Delete
    $scope.deletecustomers = function (dataToPost, $index) {
        console.log("Delete  case controller");

        CustomerAdditionalService.deletecustomersAdditional(dataToPost).then(function (results) {
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