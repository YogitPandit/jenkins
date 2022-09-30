'use strict';
app.controller('PendingApprovalController', ['$scope', 'peoplesService', 'CityService', 'StateService', "$filter", "$http", "ngTableParams", '$modal', 
    function ($scope, peoplesService, CityService, StateService, $filter, $http, ngTableParams, $modal) {
        var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        if (UserRole.role == 'HQ Master login' && UserRole.Admin == 'True') {
            $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

            console.log("People Controller reached");

            $scope.currentPageStores = {};
            $scope.peoples = [];
            $http.get(serviceBase + 'api/PendingApproval').then(function (results) {
                debugger;
                $scope.peoples = results.data;

                $scope.callmethod();
            }, function (error) {
            });
            
            $scope.callmethod = function () {

                var init;
                return $scope.stores = $scope.peoples,

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

                    $scope.numPerPageOpt = [30, 50, 100, 200],
                    $scope.numPerPage = $scope.numPerPageOpt[1],
                    $scope.currentPage = 1,
                    $scope.currentPageStores = [],
                    (init = function () {
                        return $scope.search(), $scope.select($scope.currentPage)
                    })

                ()
            }

        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
            $scope.pendingapproval = function (data, $index) {
                console.log(data);
                console.log("Appoval Dialog called for people");
                var modalInstance;
                modalInstance = $modal.open(
                    {
                        templateUrl: "myModalpendingapproval.html",
                        controller: "ModalInstanceCtrlapproveCompany", resolve: { people: function () { return data } }
                    }), modalInstance.result.then(function (selectedItem) {
                        $scope.currentPageStores.splice($index, 1);
                    },
                    function () {
                        console.log("Cancel Condintion");
                    })
            };
        }
        else {
            window.location = "#/404";
        }
       
    }]);

// Permission approve by super asmin
app.controller("ModalInstanceCtrlapproveCompany", ["$scope", '$http', '$modal', 'ngAuthSettings', "peoplesService", 'CityService', 'StateService', "$modalInstance", "people", 'WarehouseService', 'authService',
    function ($scope, $http, $modal, ngAuthSettings, peoplesService, CityService, StateService, $modalInstance, people, WarehouseService, authService) {

        
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        console.log("People");

        $scope.trade = {}; var alrt = {};
        if (people) {
            $scope.trade = people;
        }
 
        $scope.approvalpeoples = function (data) {
            $scope.trade = {};
            if (people) {
                debugger;
                $scope.trade = people;
                console.log("found Puttt People");
                console.log(people);
                console.log($scope.trade);
                console.log("Update People");
            }

            var url = serviceBase + "/api/PendingApproval/ApprovePendingRequest";
            var dataToPost = {
                userId: data.userId
            };
            console.log(dataToPost);
            $http.put(url, dataToPost)
            .success(function (data) {
                window.location.reload();
                console.log("Error Gor Here");
                console.log(data);
                if (data.id == 0) {

                    $scope.gotErrors = true;
                    if (data[0].exception == "Already") {
                        console.log("Got This User Already Exist");
                        $scope.AlreadyExist = true;
                    }
                }
            })
             .error(function (data) {
                 console.log("Error Got Heere is ");
                 console.log(data);
             })
        };
    }
    ]);