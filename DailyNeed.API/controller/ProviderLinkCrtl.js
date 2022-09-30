'use strict';

app.controller('ProviderLinkCrtl', ['$scope', 'ProviderLinkService', 'CategoryService', 'SubCategoryService', 'WarehouseService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, ProviderLinkService, CategoryService, SubCategoryService, WarehouseService , $filter, $http, ngTableParams, $modal) {
    debugger;
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.load = $scope.UserRole;

    console.log(" Case Controller reached");

    $scope.currentPageStores = {};

    $scope.casess = [];



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

    //get Warehouses by company id
    $scope.warehouses = [];
    WarehouseService.getwarehouse().then(function (results) {
        $scope.warehouses = results.data;
    }, function (error) {
    });

    $scope.exampledata = [];
    if ($scope.warehouses != null) {
        debugger;
        $scope.exampledata = $scope.warehouses;
        $scope.examplesettings = {
            displayProp: 'WarehouseName', idProp: 'WarehouseId',
            scrollableHeight: '300px',
            scrollableWidth: '450px',
            enableSearch: true,
            scrollable: true
        };
    }
    //Showing Provider Warehouse link data in grid
    $scope.selectedwarehouse = [];
    $scope.pwlinkdata = function () {
        var url = serviceBase + 'api/ProviderLink/GetSelectedWarehouse';
        $http.get(url)
        .success(function (data) {
            $scope.selectedwarehouse = data;
            console.log("$scope.BaseCategory", $scope.bcategory);
        });
    }
   // $scope.pwlinkdata();

    //Showing Provider BaseCategory link data in grid
    $scope.selectedBaseCategory = [];
    $scope.pbclinkdata = function () {
        var url = serviceBase + 'api/ProviderLink/GetSelectedBCategory';
        $http.get(url)
        .success(function (data) {
            $scope.selectedBaseCategory = data;
            console.log("$scope.BaseCategory", $scope.selectedBaseCategory);
        });
    }
    //$scope.pbclinkdata();

    //Showing Provider Category link data in grid
    $scope.selectedCategory = [];
    $scope.pclinkdata = function () {
        var url = serviceBase + 'api/ProviderLink/GetSelectedCategory';
        $http.get(url)
        .success(function (data) {
            $scope.selectedCategory = data;
            console.log("$scope.BaseCategory", $scope.selectedCategory);
        });
    }
    //$scope.pclinkdata();

    //Showing Provider SubCategory link data in grid
    $scope.selectedSubCategory = [];
    $scope.psubclinkdata = function () {
        var url = serviceBase + 'api/ProviderLink/GetSelectedSubCategory';
        $http.get(url)
        .success(function (data) {
            $scope.selectedSubCategory = data;
            console.log("$scope.BaseCategory", $scope.selectedSubCategory);
        });
    }
    //$scope.psubclinkdata();

    //Showing Provider Item link data in grid
    $scope.selectedItem = [];
    $scope.pitemlinkdata = function () {
        var url = serviceBase + 'api/ProviderLink/GetSelectedItemCategory';
        $http.get(url)
        .success(function (data) {
            $scope.selectedItem = data;
            console.log("$scope.BaseCategory", $scope.selectedItem);
        });
    }
    //$scope.pitemlinkdata();

    //post warehouse data
    $scope.getwarehouseData = [];
    $scope.GetwarehouseData = function (data) {
        debugger;
        var url = serviceBase + 'api/ProviderLink/addPWarehouse';
        //Data Posting to controller by ui object
        $scope.ctgry = [];
        $scope.ctgry = data;
        $scope.DatatoPostnew = [];
        for (var i = 0; i < $scope.ctgry.length; i++) {
            var warehouseData = $scope.ctgry[i];
            var dataTopost = {
                PWarehouseId: warehouseData.id
            }
            $scope.DatatoPostnew.push(dataTopost);
        }
        debugger;
        $http.post(url, $scope.DatatoPostnew);

    };

    $scope.basecategorys = [];
    //get All Base Category On the Bases of Companyid and warehouseid // Base category drop down
    $http.get(serviceBase + 'api/BaseCategory').then(function (results) {
        debugger;
        $scope.basecategorys = results.data;
    }, function (error) {
    });
    $scope.baseexampledata = [];
    if ($scope.basecategorys != null) {
        debugger;
        $scope.baseexampledata = $scope.basecategorys;
        $scope.Basecateexamplesettings = {
            displayProp: 'BaseCategoryName', idProp: 'BaseCategoryId',
            scrollableHeight: '300px',
            scrollableWidth: '450px',
            enableSearch: true,
            scrollable: true
        };
    }

    // 
    //Fill Item on the bases of Base category id
    $scope.getItemData = [];
    $scope.GetItemsData = function (data) {
        debugger;
        var url = serviceBase + 'api/ProviderLink/GetItemByBaseCateId';
        //Data Posting to controller by ui object
        $scope.BaseCate = [];
        $scope.BaseCate = data;
        $scope.DatatoPostnew = [];
        for (var i = 0; i < $scope.BaseCate.length; i++) {
            $scope.BaseCategoryId = $scope.BaseCate[i];
            debugger;
            $scope.DatatoPostnew.push($scope.BaseCategoryId);
        }
        debugger;
        $http.post(url, $scope.DatatoPostnew)
        .success(function (data) {
            debugger;
            $scope.getItemData = data;
            console.log("$scope.getItemData", $scope.getItemData);;

        });
    };


    //Fillter Data in Item Drop Down
    $scope.itemexamplemodel = [];
    if ($scope.getItemData != null) {
        $scope.itemexamplemodel = $scope.getItemData;
        $scope.itemexamplesettings = {
            displayProp: 'ItemName', idProp: 'ItemId',
            scrollableHeight: '300px',
            scrollableWidth: '450px',
            enableSearch: true,
            scrollable: true
        };
    }

    //Post Item data from drop down list
    $scope.postItemData = [];
    $scope.PostItemsData = function (data) {
        debugger;
        var url = serviceBase + 'api/ProviderLink/addPItems';
        //Data Posting to controller by ui object
        $scope.pitem = [];
        $scope.pitem = data;
        $scope.DatatoPostnew = [];
        for (var i = 0; i < $scope.pitem.length; i++) {
            var pitemData = $scope.pitem[i];
            var dataTopost = {
                id: pitemData.id
            }
            $scope.DatatoPostnew.push(dataTopost);
        }
        debugger;
        $http.post(url, $scope.DatatoPostnew)
        .success(function (data) {
            debugger;
            $scope.postItemData = data;
            console.log("$scope.getItemData", $scope.postItemData);

        });
    };

}]);

