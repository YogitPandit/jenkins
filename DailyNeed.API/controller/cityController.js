'use strict';
app.controller('cityController', ['$scope', "CityService", 'StateService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, CityService, StateService, $filter, $http, ngTableParams, $modal) {
    var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    if (UserRole.role == 'HQ Master login' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Master login' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ HR lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ HR associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ HR executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Agents' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH cash manager' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH delivery planner' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Ops associate' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Super visor' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Service lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH Sales lead' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH sales executives' && UserRole.StatisticalRep == 'True' || UserRole.role == 'WH sales associates' && UserRole.StatisticalRep == 'True') {
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        console.log(" city Controller reached");
        //User Tracking
        $scope.AddTrack = function (Atype, page, Detail) {

            console.log("Tracking Code");
            var url = serviceBase + "api/trackuser?action=" + Atype + "&item=" + page + " " + Detail;
            $http.post(url).success(function (results) { });
        }
        //End User Tracking
        $scope.currentPageStores = {};

        $scope.open = function () {
            console.log("Modal opened city");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myCityModal.html",
                    controller: "ModalInstanceCtrlCity", resolve: { city: function () { return $scope.items } }
                }), modalInstance.result.then(function (selectedItem) {


                    $scope.currentPageStores.push(selectedItem);

                },
                function () {
                    console.log("Cancel Condintion");

                })
        };


        $scope.edit = function (item) {
            console.log("Edit Dialog called city");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myCityModalPut.html",
                    controller: "ModalInstanceCtrlCity", resolve: { city: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {

                    $scope.city.push(selectedItem);
                    _.find($scope.city, function (city) {
                        if (city.id == selectedItem.id) {
                            city = selectedItem;
                        }
                    });

                    $scope.city = _.sortBy($scope.city, 'Id').reverse();
                    $scope.selected = selectedItem;

                },
                function () {
                    console.log("Cancel Condintion");

                })
        };

        $scope.opendelete = function (data, $index) {
            console.log(data);
            console.log($index);
            console.log("Delete Dialog called for city");

            var myData = { all: $scope.currentPageStores, city1: data };


            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModaldeleteCity.html",
                    controller: "ModalInstanceCtrldeleteCity", resolve: { city: function () { return myData } }
                }), modalInstance.result.then(function (selectedItem) {

                    $scope.currentPageStores.splice($index, 1);
                },
                function () {
                    console.log("Cancel Condintion");

                })
            //$scope.city.splice($scope.city.indexOf($scope.city), 1)
            // $scope.city.splice($index, 1);
        };

        $scope.city = [];

        CityService.getcitys().then(function (results) {
            console.log("ingetfn");
            console.log(results.data);
            $scope.city = results.data;

            $scope.callmethod();
            $scope.AddTrack("View", "CityPage", "");
        }, function (error) {

        });

        $scope.callmethod = function () {

            var init;
            return $scope.stores = $scope.city,

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
    }
    else {
        window.location = "#/404";
    }
}]);

app.controller("ModalInstanceCtrlCity", ["$scope", '$http', 'ngAuthSettings', "CityService", "StateService", "$modalInstance", "city", 'FileUploader', function ($scope, $http, ngAuthSettings, CityService, StateService, $modalInstance, city, FileUploader) {
    console.log("city");

    //User Tracking
    $scope.AddTrack = function (Atype, page, Detail) {

        console.log("Tracking Code");
        var url = serviceBase + "api/trackuser?action=" + Atype + "&item=" + page + " " + Detail;
        $http.post(url).success(function (results) { });
    }
    //End User Tracking

    var input = document.getElementById("file");
    console.log(input);

    var today = new Date();
    $scope.today = today.toISOString();

    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

    $scope.uploadedfileName = '';
    $scope.upload = function (files) {
        console.log(files);
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                console.log(config.file.name);

                console.log("File Name is " + $scope.uploadedfileName);
                var fileuploadurl = '/api/upload/post', files;
                $upload.upload({
                    url: fileuploadurl,
                    method: "POST",
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: file
                }).progress(function (evt) {
                    var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                    console.log('progress: ' + progressPercentage + '% ' +
                                evt.config.file.name);
                }).success(function (data, status, headers, config) {


                    console.log('file ' + config.file.name + 'uploaded. Response: ' +
                                JSON.stringify(data));
                    cosole.log("uploaded");
                });
            }
        }
    };

    $scope.states = [];
    StateService.getstates().then(function (results) {
        console.log("sumit");
        console.log(results.data);
        $scope.states = results.data;
    }, function (error) {
    });

    $scope.CityData = {};
    if (city) {
        debugger;
        var stateId = city.State.Stateid;
        city.Stateid = stateId;
        $scope.CityData = city;
        console.log($scope.CityData.Stateid);

    }


    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
    //Add City Mehtod
     $scope.AddCity = function (data) {
         if (data.Stateid == null || data.Stateid == 0) {
             alert('Please Select State ');
         }
         else if (data.CityName == null || data.CityName == "") {
             alert('Please Enter CityName ');
         }
         else {

             console.log("AddCity");

             var url = serviceBase + "api/City";
             var dataToPost = {
                 CityName: data.CityName,
                 Code: data.Code,
                 aliasName: data.aliasName,
                 CreatedDate: data.CreatedDate,
                 UpdatedDate: data.UpdatedDate,
                 CreatedBy: data.CreatedBy,
                 UpdateBy: data.UpdateBy,
                 State: {
                     Stateid: data.Stateid
                 }
             };
             console.log(dataToPost);

             $http.post(url, dataToPost)
             .success(function (data) {
                 debugger;
                 //Passing Alert message by cs 
                 if (data.Message == "Error") {
                     alert("City Not Added!Some Error Occure!")
                     $modalInstance.close(data);
                 }
                  //Passing Alert message by cs 
                 else if (data.Message == "Sucessfully") {
                     debugger;
                     alert("City Added Sucessfully!")
                     $modalInstance.close(data);
                     window.location.reload();
                 }
                 else {
                     $modalInstance.close(data);
                 }
             })
              .error(function (data) {
              })
         }
     };
    //update city method
    $scope.PutCity = function (data) {
        if (data.Stateid == null || data.Stateid == 0) {
            alert('Please Select State ');
        }
        else if (data.CityName == null || data.CityName == "") {
            alert('Please Enter CityName ');
        }
        else {

            $scope.CityData = {

            };
            if (city) {
                $scope.CityData = city;
                console.log("found Puttt City");
                console.log(city);
                console.log($scope.CityData);
                console.log("selected City");

            }
            $scope.ok = function () { $modalInstance.close(); },
            $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

            console.log("PutCity");
            var url = serviceBase + "api/City";
            var dataToPost = {
                Cityid: data.Cityid,
                CityName: data.CityName,
                aliasName: data.aliasName,
                CreatedDate: data.CreatedDate,
                UpdatedDate: data.UpdatedDate,
                CreatedBy: data.CreatedBy,
                UpdateBy: data.UpdateBy,
                State: {
                    Stateid: data.Stateid
                },
                Code: data.Code
            };

            console.log(dataToPost);
            $http.put(url, dataToPost)
            .success(function (data) {
                debugger;
                //Passing Alert message by cs 
                if (data.Message == "Error") {
                    alert("City Not Updated!Some Error Occure!")
                    $modalInstance.close(data);
                }
                    //Passing Alert message by cs 
                else if (data.Message == "Sucessfully") {
                    debugger;
                    alert("City Updated Sucessfully!")
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

        };
    }
}])

app.controller("ModalInstanceCtrldeleteCity", ["$scope", '$http', "$modalInstance", "CityService", 'ngAuthSettings', "city", function ($scope, $http, $modalInstance, CityService, ngAuthSettings, myData) {
    console.log("delete modal opened");

    //User Tracking
    $scope.AddTrack = function (Atype, page, Detail) {

        console.log("Tracking Code");
        var url = serviceBase + "api/trackuser?action=" + Atype + "&item=" + page + " " + Detail;
        $http.post(url).success(function (results) { });
    }
    //End User Tracking
    $scope.city = [];


    if (myData) {
        $scope.CityData = myData.city1;

    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


    $scope.deleteCity = function (dataToPost, $index) {
        console.log("Delete  controller");

        CityService.deletecitys(dataToPost).then(function (results) {
            console.log("Del");
            //myData.all.splice($index, 1);

            $modalInstance.close(dataToPost);
            // ReloadPage();
            $scope.AddTrack("Delete(City)", "CityName:", dataToPost.CityName);
        }, function (error) {
            alert(error.data.message);
        });
    }

}])