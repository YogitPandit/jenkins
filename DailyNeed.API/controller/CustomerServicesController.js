'use strict';

app.controller('customerservices', ['$scope', 'CustomerPriceServices', "$filter", "$http", "ngTableParams", '$modal', function ($scope, CustomerPriceServices, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.load = $scope.UserRole;
    $scope.compid = $scope.UserRole;
    console.log(" Case Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];

    //Get CustomerServices By Services
    CustomerPriceServices.getcustomersPriceServices().then(function (results) {
        $scope.casess = results.data;
        $scope.callmethod();
    }, function (error) {

    });

    //Open Popup
    $scope.open = function () {
        console.log("Modal opened Case");
        var modalInstance;

        modalInstance = $modal.open(
            {
                templateUrl: "myModalContentAdd.html",
                controller: "ModalInstanceCtrlCase", resolve: { cases: function () { return $scope.items } }
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
                templateUrl: "myModalContentPut.html",
                controller: "ModalInstanceCtrlCase", resolve: { cases: function () { return customer } }
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
                templateUrl: "myCustomerModaldelete.html",
                controller: "ModalInstanceCtrldeleteCase", resolve: { cases: function () { return data } }
            }), modalInstance.result.then(function (selectedItem) {
                $scope.currentPageStores.splice($index, 1);
            },
            function () {
                console.log("Cancel Condintion");
            })
    };

    //Get Data
    $scope.casess = [];

    //Pagination Method
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


//Model Instance Controller
app.controller("ModalInstanceCtrlCase", ["$scope", '$http', 'ngAuthSettings', "customerService", 'ProjectService', 'peoplesService', "$modalInstance", "cases", 'FileUploader', "$filter", function ($scope, $http, ngAuthSettings, customerService, ProjectService, peoplesService, $modalInstance, cases, FileUploader, $filter) {
    console.log("cases");
    var User = JSON.parse(localStorage.getItem('RolePerson'));
    debugger;
    $scope.CustomerService = {
    };

    $scope.peoples = [];

    //People Services
    peoplesService.getpeoples().then(function (results) {
        $scope.peoples = results.data;
        console.log("peoples");
        console.log($scope.peoples);
    }, function (error) {
    });

    //variable
    if (cases) {
        debugger;
        console.log("case if conditon");
        $scope.CustomerService = cases;
        $scope.CustomerService.StartDate = $filter("date")($scope.CustomerService.StartDate, 'yyyy-MM-dd');
    }

    //Get All Service for Dropdown Service
    $scope.GetService = function () {
        var url = serviceBase + 'api/CustomerPriceService/GetSerices';
        $http.get(url)
        .success(function (data) {
            $scope.bcategory = data;
            $scope.service = $scope.bcategory;
            console.log("$scope.BaseCategory", $scope.bcategory);
            console.log("$scope.Basecategory", $scope.service);
        });
    }
    $scope.GetService();

    //Get All Category for Dropdown Category
    //$scope.GetCategory = function () {
    //    var url = serviceBase + 'api/CustomerPriceService/GetCategory';
    //    $http.get(url)
    //    .success(function (data) {
    //        $scope.category = data;
    //        $scope.scategory = $scope.category;
    //        console.log("$scope.Category", $scope.category);
    //        console.log("$scope.Category", $scope.scategory);
    //    });
    //}
    //$scope.GetCategory();

    //Filling Category Data On Basis of Selected Base Category
    $scope.GetCategoryda = function (data) {
        $scope.GetShift(data.Id);
        $scope.categorydata = [];
        angular.forEach($scope.scategory, function (value, key) {
            if (value.BaseCategoryId == data.BaseCategoryId) {
                $scope.categorydata.push(value);
            }
            else {
            }
            console.log($scope.categorydata);
        });
    }


    //Get All Shift On Base Category besis 
    $scope.GetShift = function (data) {
        debugger;
            var url = serviceBase + 'api/CustomerPriceService/GetShift';
            $http.get(url)
            .success(function (data) {
                debugger;
                $scope.shift = data;
                console.log("$scope.Shift", $scope.shift);
                //if ($scope.shift.length > 0) {
                //    document.getElementById('divShift').style.display = 'block';
                //}
                //if ($scope.shift == 0) {
                //    document.getElementById('divShift').style.display = 'none';
                //}
            });
    }
    $scope.GetShift();

    //Get All Sub Category for Dropdown Sub Category
    //$scope.GetSubCategory = function () {
    //    var url = serviceBase + 'api/CustomerPriceService/GetSubCategory';
    //    $http.get(url)
    //    .success(function (data) {
    //        $scope.subcategory = data;
    //        console.log("$scope.SubCategory", $scope.subcategory);
    //    });
    //}
    //$scope.GetSubCategory();

    //Filling Sub Category Data On Basis of Selected Category
    //$scope.GetSubCategoryda = function (data) {
    //    $scope.subcategorydata = [];
    //    angular.forEach($scope.subcategory, function (value, key) {
    //        if (value.Categoryid == data.Categoryid) {
    //            $scope.subcategorydata.push(value);
    //        }
    //        else {
    //        }
    //        console.log($scope.subcategorydata);
    //    });
    //}

    //Get All Sub Sub Category for Dropdown Sub Sub Category
    //$scope.GetSubSubCategory = function () {
    //    var url = serviceBase + 'api/CustomerPriceService/GetSubSubCategory';
    //    $http.get(url)
    //    .success(function (data) {
    //        $scope.subsubcategory = data;
    //        console.log("$scope.SubSubCategory", $scope.subsubcategory);
    //    });
    //}
    //$scope.GetSubSubCategory();

    //Filling Sub Sub Category Data On Basis of Selected Category
    //$scope.GetSubSubCategoryda = function (data) {
    //    $scope.subssubcategorydata = [];
    //    angular.forEach($scope.item, function (value, key) {
    //        if (value.SubCategoryId == data.SubCategoryId) {
    //            $scope.subssubcategorydata.push(value);
    //        }
    //        else {
    //        }
    //        console.log($scope.subssubcategorydata);
    //    });
    //}

    //Get All Provider for Dropdown Provider
    $scope.GetPeople = function () {
        debugger;
        var url = serviceBase + 'api/CustomerPriceService/GetProvider';
        $http.get(url)
        .success(function (data) {
            $scope.getProvider = data;
            console.log("$scope.getProvider", $scope.getProvider);
        });
    }
    $scope.GetPeople();


    //Get All Items from PItemLink by CompanyId for Dropdown items
    $scope.GetItem = function () {
        var url = serviceBase + 'api/CustomerPriceService/GetItemById';
        $http.get(url)
        .success(function (data) {
            debugger;
            $scope.getItem = data;
            console.log("$scope.getItem Link", $scope.getItem);

        });
    }
    $scope.GetItem();


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


    //Get All WareHouse By ProviderId from PWarehouse Link for Dropdown 
    $scope.GetWarehouse = function () {
        var url = serviceBase + 'api/CustomerPriceService/GetWareHouse';
        $http.get(url)
        .success(function (data) {
            $scope.getWarehouse = data;
            console.log("$scope.getWarehouseLink", $scope.getWarehouse);
        });
    }
    $scope.GetWarehouse();

    //Onclick Everyday radio button checked all checkboxs
    $scope.Everyday = function () {
        $('input[type=checkbox]').prop('checked', true);
        var x = document.getElementByName("day");

    }

    //Onclick Alternate radio button checked only alternate days
    $scope.Alternate = function () {
        $('input[type=checkbox]').prop('checked', false);
        $('input[type=checkbox]' && 'input[id=site-Mon]').prop('checked', true);
        $('input[type=checkbox]' && 'input[id=site-Wed]').prop('checked', true);
        $('input[type=checkbox]' && 'input[id=site-Fri]').prop('checked', true);
        $('input[type=checkbox]' && 'input[id=site-Sun]').prop('checked', true);
    }

    //Onclick Custom radio button checked any chekbox
    $scope.Custom = function () {
        $('input[type=checkbox]').prop('checked', false);
        $('input[type=checkbox]' && 'input[id=site-Mon]').prop('checked', true);
    }


    //Get All WareHouse for Dropdown Warehouse
    //$scope.GetUnitMaster = function () {
    //    var url = serviceBase + 'api/CustomerPriceService/GetUnitMaster';
    //    $http.get(url)
    //    .success(function (data) {
    //        $scope.unitmaster = data;
    //        $scope.unit = $scope.unitmaster;
    //        console.log("$scope.unitmaster", $scope.unitmaster);
    //        console.log("$scope.unit", $scope.unit);

    //    });
    //}
    //$scope.GetUnitMaster();

    //Binding Billing DropDown Hard Code
    //$scope.BillingType = [{
    //    "id": 1,
    //    "name": "Monthly"
    //},
    //    {
    //        "id": 2,
    //        "name": "Weekly"
    //    },
    //    {
    //        "id": 3,
    //        "name": "Half-Monthly"
    //    }];

    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    //Add Customer Services
     $scope.AddCustomerServices = function (data) {
         debugger;
         console.log("case");
         console.log(data);
         //Geting textbox id and pass in validation focus
         //var item = document.getElementById('site-name-project');
         //var basecategory = document.getElementById('site-name-service');
         //var category = document.getElementById('site-name-category');
         //var subcategory = document.getElementById('site-name-subcategory');
         //var billingtype = document.getElementById('site-name-btype');
         var customer = document.getElementById('site-name-issue');
         var provider = document.getElementById('site-name-Provider');
         var price = document.getElementById('site-name');
         var qty = document.getElementById('site-name-qty');
         var ServiceSDate = document.getElementById('site-name-sdate');
         var ShiftId = document.getElementById('divShift');

         //validation for shift if visible true then only
         if ($('#divShift').css('display') == 'block') {
             if (data.ShiftID == null || data.ShiftID == "") {
                 alert("Please Select Shift");
                 ShiftId.focus();
                 return;
             }
         }
         //validation for item dropdown / Requaired
         if (data == null || data == "") {
             alert("Nothing To Save!!Plz Fill Required Information");
             item.focus();
             return;
         }

         if (data.PWarehouseId == null || data.PWarehouseId == 0) {
             alert("Please Select Warehouse!!");
         }
         //validation for item dropdown / Requaired
         if (data.PItemId == null || data.PItemId == "") {
             alert("Please Select item!!");
             item.focus();
             return;
         }

         //validation for cutomer dropdown / Requaired
         if (data.CustomerLinkId == null || data.CustomerLinkId == 0) {
             alert("Please Select Customer!!");
             customer.focus();
             return;
         }

         //Get days which have selected from ui
         var days = [];
         $.each($("input[name='day']:checked"), function () {
             debugger;
             days.push($(this).val());
         });
         var daylist = days.join(",");


         //validation for price text/ Requaired
         if (data.Price == null || data.Price == "") {
             alert("Please Enter Price!!");
             price.focus();
             return;
         }

         //validation for Qty text / Requaired
         if (data.Qty == null || data.Qty == "") {
             alert("Please Enter Qty!!");
             qty.focus();
             return;
         }

         //validation for Service Start Date text / Requaired
         if (data.StartDate == null || data.StartDate == "") {
             alert("Please Enter Service Start Date!!");
             ServiceSDate.focus();
             return;
         }
         //If Provider is not Supper admin then by default setting login companyid
         //if (data.PeopleID == null) {
         //    $scope.Provider = User.compid;
         //}
         //else {
         //    $scope.Provider = data.PeopleID;
         //}


         //API URL for Cs Controller hit
         var url = serviceBase + "api/CustomerPriceService/AddCustomerServices";

         //Data Posting to controller by ui object
         var dataToPost = {
             ProviderId: $scope.Provider,
             PWarehouseId: data.PWarehouseId,
             CustomerLinkId: data.CustomerLinkId,
             PItemId: data.PItemId,
             StartDate: data.StartDate,
             Price: data.Price,
             DeliveryDays: daylist,
             DeliveryTime: data.DeliveryTime,
             Quantity: data.Qty,
             ShiftId: data.ShiftID
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
                 alert("CustomerServices Not Save!Some Error Occure!")
                 $modalInstance.close(data);
             }

                 //Passing Alert message by cs 
             else if (data.Message == "Sucessfully") {
                 alert("CustomerServices Add Sucessfully!")
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
    $scope.PutCustomerServices = function (data) {
        debugger;
        $scope.CaseData = {
        };
        $scope.loogourl = cases.LogoUrl;
      
        //validation for item dropdown / Requaired
        var customer = document.getElementById('site-name-issue');
        var provider = document.getElementById('site-name-Provider');
        var price = document.getElementById('site-name');
        var qty = document.getElementById('site-name-qty');
        var ServiceSDate = document.getElementById('site-name-sdate');
        var ShiftId = document.getElementById('divShift');

        //validation for shift if visible true then only
        if ($('#divShift').css('display') == 'block') {
            if (data.ShiftID == null || data.ShiftID == "") {
                alert("Please Select Shift");
                ShiftId.focus();
                return;
            }
        }
        //validation for item dropdown / Requaired
        if (data == null || data == "") {
            alert("Nothing To Save!!Plz Fill Required Information");
            item.focus();
            return;
        }

        if (data.PWarehouseId == null || data.PWarehouseId == 0) {
            alert("Please Select Warehouse!!");
        }
        //validation for item dropdown / Requaired
        if (data.PItemId == null || data.PItemId == "") {
            alert("Please Select item!!");
            item.focus();
            return;
        }

        //validation for cutomer dropdown / Requaired
        if (data.CustomerLinkId == null || data.CustomerLinkId == 0) {
            alert("Please Select Customer!!");
            customer.focus();
            return;
        }

        //Get days which have selected from ui
        var days = [];
        $.each($("input[name='day']:checked"), function () {
            debugger;
            days.push($(this).val());
        });
        var daylist = days.join(",");


        //validation for price text/ Requaired
        if (data.Price == null || data.Price == "") {
            alert("Please Enter Price!!");
            price.focus();
            return;
        }

        //validation for Qty text / Requaired
        if (data.Quantity == null || data.Quantity == "") {
            alert("Please Enter Qty!!");
            qty.focus();
            return;
        }

        //validation for Service Start Date text / Requaired
        if (data.StartDate == null || data.StartDate == "") {
            alert("Please Enter Service Start Date!!");
            ServiceSDate.focus();
            return;
        }


        //If Provider is not Supper admin then by default setting login companyid
        //if (data.PeopleID == null) {
        //    $scope.Provider = User.compid;
        //}
        //else {
        //    $scope.Provider = data.PeopleID;
        //}

        console.log("Update");

        //API URL for Cs Controller hit
        var url = serviceBase + "api/CustomerPriceService/UpdateCustomerServices";

        //Posting Data To Cs Controller
        var dataToPost = {
            ID: data.ID,
            PWarehouseId: data.PWarehouseId,
            CustomerLinkId: data.CustomerLinkId,
            PItemId: data.PItemId,
            StartDate: data.StartDate,
            Price: data.Price,
            DeliveryDays: daylist,
            DeliveryTime: data.DeliveryTime,
            Quantity: data.Quantity,
            ShiftId: data.ShiftID
        };
        console.log(dataToPost);
        $http.put(url, dataToPost)
        .success(function (data) {
            debugger;
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
                alert("CustomerServices Not Update!Some Error Occure!")
                $modalInstance.close(data);
            }

                //Passing Alert message by cs 
            else if (data.Message == "Sucessfully") {
                alert("CustomerServices Update Sucessfully!")
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
}])

app.controller("ModalInstanceCtrldeleteCase", ["$scope", '$http', "$modalInstance", "CustomerPriceServices", 'ngAuthSettings', "cases", function ($scope, $http, $modalInstance, CustomerPriceServices, ngAuthSettings, cases) {
    console.log("delete modal opened");
    $scope.casess = [];
    //Get Data In Veriable
    if (cases) {
        $scope.CaseData = cases;
        console.log("found case");
        console.log(cases);
        console.log($scope.CaseData);
    }

    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    //Delete
    $scope.deletecustomers = function (dataToPost, $index) {
        debugger;
        console.log("Delete  case controller");
        CustomerPriceServices.deletecustomersPriceServices(dataToPost).then(function (results) {
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