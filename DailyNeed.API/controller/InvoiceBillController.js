'use strict';
app.controller('InvoiceBillController', ['$scope', "$filter", "$http", "ngTableParams", '$modal', 'FileUploader',
    function ($scope, $filter, $http, ngTableParams, $modal, FileUploader) {
        var UserRole = JSON.parse(localStorage.getItem('RolePerson'));

        $scope.currentPageStores = {};


        //Get Paid Bill List in Table
        $scope.GetBill = function () {
            var url = serviceBase + 'api/MonthInvoiceBill/GetAllPaidBillList';
            $http.get(url)
            .success(function (data) {
                $scope.AllBills = data;
                $scope.callmethod();
                console.log("$scope.AllBills", $scope.AllBills)
            });
        }
        $scope.GetBill();

        //Add ItemMaster popup
        //$scope.open = function () {
        //    debugger;
        //    var modalInstance;
        //    modalInstance = $modal.open(
        //        {
        //            templateUrl: "InvoiceBillModal.html",
        //            controller: "ModalInstanceAddItem", resolve: { itemMaster: function () { return $scope.items } }
        //        }), modalInstance.result.then(function (selectedItem) {
        //            $scope.currentPageStores.push(selectedItem);
        //        },
        //        function () {
        //        })
        //};


        //Update ItemMaster Popup
        $scope.edit = function (billpay) {
            debugger;
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "InvoiceBillModal.html",
                    controller: "ModalInstance", resolve: { billcase: function () { return billpay } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.itemMaster.push(selectedItem);
                    _.find($scope.billcase, function (billcase) {
                        if (billcase.id == selectedItem.id) {
                            billcase = selectedItem;
                        }
                    });
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
                templateUrl: "MyItemDelete.html",
                controller: "ModalInstanceCtrldeleteitemtest", resolve: { cases: function () { return data } }
            }), modalInstance.result.then(function (selectedItem) {
                $scope.currentPageStores.splice($index, 1);
            },
            function () {
                console.log("Cancel Condintion");

            })
        };
        //Pagging Method
        $scope.callmethod = function () {

            var init;
            return $scope.stores = $scope.AllBills,

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

//Add and Update Instance
app.controller("ModalInstance", ["$scope", '$http', 'ngAuthSettings', "$modalInstance", "billcase",
    function ($scope, $http, ngAuthSettings, $modalInstance, billcase) {
        debugger;

        $scope.billcaseData = {};
        if (billcase) {
            $scope.billcaseData = billcase;
        }


        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


        $scope.AdditemMaster = function (data) {
            var url = serviceBase + "api/itemMasterNew/addNewItem";
            var dataToPost = {
                ItemCode: data.ItemCode,
                BasePrice: data.BasePrice,
                Quantity: data.Quantity,
            };
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
                   alert("Item Not Save!Some Error Occure!")
                   $modalInstance.close(data);
               }

                   //Passing Alert message by cs 
               else if (data.Message == "Sucessfully") {
                   alert("Item Add Sucessfully!")
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
             })

        }
        $scope.PutitemMaster = function (data) {
            if (data.ItemName == null || data.ItemName == "") {
                alert('Please Enter Item Name ');
            }
            else if (data.Cityid == null || data.Cityid == 0) {
                alert('Please Select City ');
            }
            else if (data.BaseCategoryId == null || data.BaseCategoryId == 0) {
                alert('Please Select BaseCategory ');
            }
            else if (data.Categoryid == null || data.Categoryid == 0) {
                alert('Please Select Category ');
            }
            else if (data.SubCategoryId == null || data.SubCategoryId == 0) {
                alert('Please Select SubCategory ');
            }
            else if (data.WarehouseId == null || data.WarehouseId == 0) {
                alert('Please Select Warehouse ');
            }
            else {
                var url = serviceBase + "api/itemMasterNew/UpdateItem";
                var dataToPost = {
                    ItemId: $scope.itemMasterData.ItemId,
                    ItemName: $scope.itemMasterData.ItemName,
                    BaseCategoryId: $scope.itemMasterData.BaseCategoryId,
                    CategoryId: $scope.itemMasterData.Categoryid,
                    SubCategoryId: $scope.itemMasterData.SubCategoryId,
                    CityId: $scope.itemMasterData.Cityid,
                    WarehouseId: $scope.itemMasterData.WarehouseId,
                    //ItemCode: $scope.itemMasterData.ItemCode,
                    BasePrice: $scope.itemMasterData.BasePrice,
                    Quantity: $scope.itemMasterData.Quantity,
                };
                $http.put(url, dataToPost)
               .success(function (data) {
                   console.log("Error Got Here");
                   console.log(data);
                   //Passing Alert message by cs 
                   if (data.Message == "Error") {
                       alert("Item Not Updated!Some Error Occure!")
                       $modalInstance.close(data);
                   }

                       //Passing Alert message by cs 
                   else if (data.Message == "Sucessfully") {
                       alert("Item Updated Sucessfully!")
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
            }
        }
    }])

//Delete Instance 
app.controller("ModalInstanceCtrldeleteitemtest", ["$scope", '$http', 'ItemMasterNewService', "$modalInstance", "cases", 'ngAuthSettings', function ($scope, $http, ItemMasterNewService, $modalInstance, cases, ngAuthSettings) {
    debugger;
    if (cases) {
        $scope.CaseData = cases;
        console.log("found case");
        console.log(cases);
        console.log($scope.CaseData);

    }
    $scope.ok = function () { $modalInstance.close(); },
 $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
    //Delete item from grid
 $scope.deleteitemMaster = function (data, $index) {
     ItemMasterNewService.deleteitemMaster(data).then(function (results) {
         $modalInstance.close(data);
         //ReloadPage();

     }, function (error) {
         alert(error.data.message);
     });
 }


}])

