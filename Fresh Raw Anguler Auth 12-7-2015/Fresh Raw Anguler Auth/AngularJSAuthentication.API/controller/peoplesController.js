'use strict';
app.controller('peoplesController', ['$scope', 'peoplesService', 'CityService', 'StateService', "$filter", "$http", "ngTableParams", '$modal',
    function ($scope, peoplesService,CityService, StateService, $filter, $http, ngTableParams, $modal) {

    console.log("People Controller reached");

    $scope.currentPageStores = {};

    $scope.open = function () {
        console.log("Modal opened people");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myPeopleModal.html",
                controller: "ModalInstanceCtrlPeople", resolve: { people: function () { return $scope.items } }
            }), modalInstance.result.then(function (selectedItem) {


                $scope.currentPageStores.push(selectedItem);

                //$scope.peoples = _.sortBy($scope.peoples, 'Id').reverse();
                //$scope.selected = selectedItem;


            },
            function () {
                console.log("Cancel Condintion");
                // $scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                //$log.info("Modal dismissed at: " + new Date)
            })
    };


    $scope.edit = function (item) {
        console.log("Edit Dialog called people");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myPeopleModalPut.html",
                controller: "ModalInstanceCtrlPeople", resolve: { people: function () { return item } }
            }), modalInstance.result.then(function (selectedItem) {

                $scope.peoples.push(selectedItem);
                _.find($scope.peoples, function (people) {
                    if (people.id == selectedItem.id) {
                        people = selectedItem;
                    }
                });

                $scope.peoples = _.sortBy($scope.peoples, 'Id').reverse();
                $scope.selected = selectedItem;
                //$scope.customers.push(selectedItem);

                //$scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                //$scope.selected = selectedItem;


            },
            function () {
                console.log("Cancel Condintion");
                // $scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                //  $log.info("Modal dismissed at: " + new Date)
            })
    };

    $scope.opendelete = function (data) {
        console.log(data);
        console.log("Delete Dialog called for people");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myModaldeletepeople.html",
                controller: "ModalInstanceCtrldeletepeople", resolve: { people: function () { return data } }
            }), modalInstance.result.then(function (selectedItem) {


                //$scope.tasktypes.push(selectedItem);

                //_.filter($scope.tasktypes, function (a) {

                //    if (a.id == selectedItem.id) {

                //        a.Name = selectedItem.Name;
                //        a.Desc = selectedItem.Desc;
                //    }

                //});

                //$scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                //$scope.selected = selectedItem;


            },
            function () {
                console.log("Cancel Condintion");
                // $scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                //  $log.info("Modal dismissed at: " + new Date)
            })
    };

    $scope.peoples = [];

    peoplesService.getpeoples().then(function (results) {

        $scope.peoples = results.data;
        console.log("Got people collection");
        console.log($scope.peoples);
        $scope.callmethod();
    }, function (error) {
        //alert(error.data.message);
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

app.controller("ModalInstanceCtrlPeople", ["$scope", '$http', 'ngAuthSettings', "peoplesService", 'CityService', 'StateService', "$modalInstance", "people",
    function ($scope, $http, ngAuthSettings, peoplesService,CityService, StateService, $modalInstance, people) {
    console.log("People");

    $scope.PeopleData = {

    };
    $scope.citys = [];
    CityService.getcitys().then(function (results) {
        $scope.citys = results.data;
    }, function (error) {
    });


    $scope.states = [];
    StateService.getstates().then(function (results) {
        $scope.states = results.data;
    }, function (error) {
    });



    if (people) {
        console.log("People if conditon");

        $scope.PeopleData = people;

        console.log($scope.PeopleData.PeopleName);
        //console.log($scope.ProjectData.Description);
    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

     $scope.AddPeople = function (data) {


         console.log("People");
         var url = serviceBase + "/api/Peoples";
         var dataToPost = {
             PeopleFirstName: $scope.PeopleData.PeopleFirstName, PeopleLastName: $scope.PeopleData.PeopleLastName,
             Stateid: $scope.PeopleData.Stateid, Cityid: $scope.PeopleData.Cityid, Mobile: $scope.PeopleData.Mobile,
             CreatedDate: $scope.PeopleData.CreatedDate, UpdatedDate: $scope.PeopleData.UpdatedDate, CreatedBy: $scope.PeopleData.CreatedBy, UpdateBy: $scope.PeopleData.UpdateBy,
             Email: $scope.PeopleData.Email, Department: $scope.PeopleData.Department, BillableRate: $scope.PeopleData.BillableRate, CostRate: $scope.PeopleData.CostRate, Permissions: $scope.PeopleData.Permissions, Type: $scope.PeopleData.Type
         };
         console.log(dataToPost);

         $http.post(url, dataToPost)
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
             else {
                 //console.log(data);
                 //  console.log(data);
                 $modalInstance.close(data);
             }

         })
          .error(function (data) {
              console.log("Error Got Heere is ");
              console.log(data);
              // return $scope.showInfoOnSubmit = !0, $scope.revert()
          })
     };

    $scope.PutPeople = function (data) {
        $scope.PeopleData = {

        };
        if (people) {
            $scope.PeopleData = people;
            console.log("found Puttt People");
            console.log(people);
            console.log($scope.PeopleData);
            //console.log($scope.Customer.name);
            //console.log($scope.Customer.description);
        }
        $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

        console.log("Update People");
        var url = serviceBase + "/api/Peoples";
        var dataToPost = { PeopleID: $scope.PeopleData.PeopleID, PeopleFirstName: $scope.PeopleData.PeopleFirstName, PeopleLastName: $scope.PeopleData.PeopleLastName, CreatedDate: $scope.PeopleData.CreatedDate, UpdatedDate: $scope.PeopleData.UpdatedDate, CreatedBy: $scope.PeopleData.CreatedBy, UpdateBy: $scope.PeopleData.UpdateBy, Email: $scope.PeopleData.Email, Department: $scope.PeopleData.Department, BillableRate: $scope.PeopleData.BillableRate, CostRate: $scope.PeopleData.CostRate, Permissions: $scope.PeopleData.Permissions, Type: $scope.PeopleData.Type, ImageUrl: data.ImageUrl };
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
            else {
                $modalInstance.close(data);
            }

        })
         .error(function (data) {
             console.log("Error Got Heere is ");
             console.log(data);

             // return $scope.showInfoOnSubmit = !0, $scope.revert()
         })

    };

}])

app.controller("ModalInstanceCtrldeletepeople", ["$scope", '$http', "$modalInstance", "peoplesService", 'ngAuthSettings', "people", function ($scope, $http, $modalInstance, peoplesService, ngAuthSettings, people) {
    console.log("delete modal opened");

    //$scope.currentPageStores = [];
    //$scope.PeopleData = {

    //};
    if (people) {
        $scope.PeopleData = people;
        console.log("found people");
        console.log(people);
        console.log($scope.PeopleData);
        //console.log($scope.Customer.name);
        //console.log($scope.Customer.description);
    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


    $scope.deletepeoples = function (dataToPost) {

        console.log("Delete people controller");
        //alert(Id);
        //Id = window.encodeURIComponent(Id);

        peoplesService.deletepeoples(dataToPost).then(function (results) {
            console.log("Del");

            $modalInstance.close(dataToPost);
        }, function (error) {
            alert(error.data.message);
        });
    }

}])