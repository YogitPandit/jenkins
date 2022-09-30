'use strict';
app.controller('ZoneController', ['$scope', 'ZoneService', "CategoryService", "$filter", "$http", "ngTableParams", '$modal', function ($scope, ZoneService, CategoryService, $filter, $http, ngTableParams, $modal) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    console.log("subcategoryController reached");

    $scope.currentPageStores = {};

    //............................Exel export Method.....................//

    alasql.fn.myfmt = function (n) {
        return Number(n).toFixed(2);
    }
    $scope.exportData = function () {

        var url = serviceBase + "api/SubCategory/export";
        $http.get(url).then(function (results) {

            $scope.storesitem = results.data;

            alasql('SELECT SubCategoryId,Categoryid,CategoryName,SubcategoryName,Discription,SortOrder,IsPramotional,CreatedDate,UpdatedDate,CreatedBy,UpdateBy,Code,LogoUrl,[Deleted],[IsActive] INTO XLSX("BaseCategory.xlsx",{headers:true}) FROM ?', [$scope.storesitem]);
        }, function (error) {
        });
    };
    //............................Exel export Method.....................//


    //open popup
    $scope.open = function () {
        console.log("Modal opened sub category");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "mySubCategoryModal.html",
                controller: "ModalInstanceCtr", resolve: { subcategory: function () { return $scope.items } }
            }), modalInstance.result.then(function (selectedItem) {


                $scope.currentPageStores.push(selectedItem);

            },
            function () {
                console.log("Cancel Condintion");

            })
    };

    //edit popup
    $scope.edit = function (item) {
        console.log("Edit Dialog called subcategory");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "mySubCategoryModalPut.html",
                controller: "ModalInstanceCtr", resolve: { subcategory: function () { return item } }
            }), modalInstance.result.then(function (selectedItem) {

                $scope.subcategorys.push(selectedItem);
                _.find($scope.subcategorys, function (subcategory) {
                    if (subcategory.id == selectedItem.id) {
                        subcategory = selectedItem;
                    }
                });

                $scope.subcategorys = _.sortBy($scope.subcategorys, 'Id').reverse();
                $scope.selected = selectedItem;

            },
            function () {
                console.log("Cancel Condintion");

            })
    };

    //Set active or deactive popup
    $scope.SetActive = function (item) {
        var modalInstance;

        modalInstance = $modal.open(
            {
                templateUrl: "myactivemodal.html",
                controller: "ModalInstanceCtr", resolve: { subcategory: function () { return item } }
            }), modalInstance.result.then(function (selectedItem) {

                $scope.subcategorys.push(selectedItem);
                _.find($scope.subcategorys, function (subcategory) {
                    if (subcategory.id == selectedItem.id) {
                        subcategory = selectedItem;
                    }
                });

                $scope.subcategorys = _.sortBy($scope.subcategorys, 'Id').reverse();
                $scope.selected = selectedItem;

            },
            function () {

            })
    };

    //delete popup
    $scope.opendelete = function (data, $index) {
        debugger;
        console.log(data);
        console.log("Delete Dialog called for subcategory");

        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myModaldeleteZones.html",
                controller: "ModalInstanceCtrldeleteZone",
                resolve: {
                    subcategory: function () {
                        return data
                    }
                }
            }), modalInstance.result.then(function (selectedItem) {

                $scope.currentPageStores.splice($index, 1);
            },
            function () {
                console.log("Cancel Condintion");

            })
    };

    $scope.subcategorys = [];

    //Get All Zones
    ZoneService.getzones().then(function (results) {
        debugger;
        console.log("ingetfn");
        console.log(results.data);
        $scope.subcategorys = results.data;

        $scope.callmethod();
    }, function (error) {

    });

    $scope.callmethod = function () {

        var init;
        return $scope.stores = $scope.subcategorys,

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

app.controller("ModalInstanceCtr", ["$scope", '$http', 'ngAuthSettings', "ZoneService", "DepotService", "$modalInstance", "subcategory", 'FileUploader', function ($scope, $http, ngAuthSettings, ZoneService, DepotService, $modalInstance, subcategory, FileUploader) {
    console.log("subcategory");
    debugger;
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

    //$scope.ZoneData = {

    //};
    //Get Data In Veriable
    if (subcategory) {
        debugger;
        console.log("SubCategory if conditon");

        $scope.ZoneData = subcategory;
        $scope.ZoneData.WarehouseId = $scope.ZoneData.Warehouse.WarehouseId;
        $scope.ZoneData.Depotid = $scope.ZoneData.Depot.Depotid;
        console.log("kkkkkk");
    }

    //$scope.categorys = [];
    //CategoryService.getcategorys().then(function (results) {
    //    $scope.categorys = results.data;
    //}, function (error) {
    //});

    $scope.Warehouses = [];
    var caturl = serviceBase + "api/Warehouse";
    $http.get(caturl)
     .success(function (data) {
         console.log("Got Here");
         console.log(data);

         $scope.Warehouses = data;
     })
      .error(function (data) {
          console.log("Error Got Heere is ");
          console.log(data);
      })

    $scope.depots = [];
    DepotService.getdepots().then(function (results) {
        debugger;
        $scope.depots = results.data;
    }, function (error) {
    });


    $scope.AddDepot = function (data) {
        debugger;
        var url = serviceBase + 'api/Zone/GetWarehouseById?warid=' + data.WarehouseId;;
        //+data.WarehouseId;
        $http.get(url)
        .success(function (data) {
            debugger;
            $scope.depots = data;
        });
    }

    //$scope.AddDepot();


    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


     $scope.AddZones = function (data) {
         debugger;
         if (data.Depotid == null || data.Depotid == 0) {
             alert('Please Select Depot ');
         }
         else if (data.ZoneName == null || data.ZoneName == "") {
             alert('Please Enter ZoneName ');
         }
         else {

             console.log("Zones");
             var url = serviceBase + "api/Zone";
             var dataToPost = {
                 Zoneid: data.Zoneid,
                 ZoneName: data.ZoneName,
                 ZoneAddress: data.ZoneAddress,
                 Description: data.Description,
                 CreatedDate: data.CreatedDate,
                 UpdatedDate: data.UpdatedDate,
                 CreatedBy: data.CreatedBy,
                 UpdateBy: data.UpdateBy,
                 Depotid: data.Depotid,
                 WarehouseId: data.WarehouseId,
                 IsActive: true,
             };
             console.log(dataToPost);

             $http.post(url, dataToPost)
             .success(function (data) {
                 console.log("Error Got Here");
                 console.log(data);
                 debugger;
                 //Passing Alert message by cs 
                 if (data.Message == "Sucessfully") {
                     debugger;
                     alert("Zone Added Sucessfully!")
                     $modalInstance.close(data);
                     window.location.reload();
                 }

                     //Passing Alert message by cs 
                 else if (data.Message == "Error") {
                     alert("Zone Not Added!Some Error Occure!")
                     $modalInstance.close(data);
                 }
                 if (data.id == 0) {


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

     };

    $scope.PutZones = function (data) {
        debugger;
        if (data.Depotid == null || data.Depotid == 0) {
            alert('Please Select Depot ');
        }
        else if (data.ZoneName == null || data.ZoneName == "") {
            alert('Please Enter ZoneName ');
        }
        else {
            $scope.ZoneData = {

            };

            $scope.loogourl = subcategory.LogoUrl;

            if (subcategory) {
                $scope.ZoneData = subcategory;
                console.log("found Puttt SubCategory");
                console.log(subcategory);
                console.log($scope.ZoneData);

            }
            $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

            console.log("Update sub SubCategory");
            if ($scope.uploadedfileName == null || $scope.uploadedfileName == '') {
                var url = serviceBase + "api/Zone";
                var dataToPost = {
                    //LogoUrl: $scope.loogourl,
                    Zoneid: $scope.ZoneData.Zoneid,
                    ZoneName: $scope.ZoneData.ZoneName,
                    ZoneAddress: $scope.ZoneData.ZoneAddress,
                    Description: $scope.ZoneData.Description,
                    CreatedDate: $scope.ZoneData.CreatedDate,
                    UpdatedDate: $scope.ZoneData.UpdatedDate,
                    CreatedBy: $scope.ZoneData.CreatedBy,
                    UpdateBy: $scope.ZoneData.UpdateBy,
                    Depotid: $scope.ZoneData.Depotid,
                    IsActive: $scope.ZoneData.IsActive
                };

                console.log(dataToPost);
                $http.put(url, dataToPost)
                .success(function (data) {
                    debugger;
                    console.log("Error Gor Here");
                    console.log(data);
                    //Passing Alert message by cs 
                    if (data.Message == "Sucessfully") {
                        debugger;
                        alert("Sub Category Update Sucessfully!")
                        $modalInstance.close(data);
                        window.location.reload();
                    }

                        //Passing Alert message by cs 
                    else if (data.Message == "Error") {
                        alert("Sub Category Not Add!Some Error Occure!")
                        $modalInstance.close(data);
                    }
                    if (data.id == 0) {


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
            else {
                var LogoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
                console.log(LogoUrl);
                console.log("Image name in Insert function :" + $scope.uploadedfileName);
                $scope.ZoneData.LogoUrl = LogoUrl;
                console.log($scope.ZoneData.LogoUrl);
                var url = serviceBase + "api/SubCategory";
                var dataToPost = {
                    //LogoUrl: $scope.ZoneData.LogoUrl,
                    Zoneid: $scope.ZoneData.Zoneid,
                    ZoneName: $scope.ZoneData.ZoneName,
                    ZoneAddress: $scope.ZoneData.ZoneAddress,
                    Description: $scope.ZoneData.Description,
                    CreatedDate: $scope.ZoneData.CreatedDate,
                    UpdatedDate: $scope.ZoneData.UpdatedDate,
                    CreatedBy: $scope.ZoneData.CreatedBy,
                    UpdateBy: $scope.ZoneData.UpdateBy,
                    Depotid: $scope.ZoneData.Depotid,
                    //Code: $scope.ZoneData.Code,
                    IsActive: $scope.ZoneData.IsActive
                };


                console.log(dataToPost);


                $http.put(url, dataToPost)
                .success(function (data) {

                    console.log("Error Gor Here");
                    console.log(data);
                    if (data.id == 0) {


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
        };


        /////////////////////////////////////////////////////// angular upload code


        var uploader = $scope.uploader = new FileUploader({
            url: serviceBase + 'api/logoUpload'
        });

        //FILTERS

        uploader.filters.push({
            name: 'customFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                return this.queue.length < 10;
            }
        });

        //CALLBACKS

        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader.onAfterAddingFile = function (fileItem) {
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            console.info('onSuccessItem', fileItem, response, status, headers);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
            console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
            console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
            console.log("File Name :" + fileItem._file.name);
            $scope.uploadedfileName = fileItem._file.name;
        };
        uploader.onCompleteAll = function () {
            console.info('onCompleteAll');
        };

        console.info('uploader', uploader);
    }

}])


app.controller("ModalInstanceCtrldeleteZone", ["$scope", '$http', "$modalInstance", "ZoneService", 'ngAuthSettings', "subcategory", function ($scope, $http, $modalInstance, ZoneService, ngAuthSettings, subcategory) {
    console.log("delete modal opened");
    function ReloadPage() {
        location.reload();
    };



    $scope.subcategorys = [];

    if (subcategory) {
        $scope.ZoneData = subcategory;
        console.log("found  subcategory");
        console.log(subcategory);
        console.log($scope.ZoneData);

    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


    //$scope.deletesubCategory = function (dataToPost, $index) {
     $scope.deleteZones = function (dataToPost, $index) {
         debugger;
         console.log("Delete zones");

         ZoneService.deletezones(dataToPost).then(function (results) {
             debugger;
             console.log("Del");

             console.log("index of item " + $index);
             console.log($scope.subcategorys.length);
             console.log($scope.subcategorys.length);

             $modalInstance.close(dataToPost);
             //ReloadPage();

         }, function (error) {
             alert(error.data.message);
         });
     }

}])