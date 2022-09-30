'use strict';
app.controller('DepotController', ['$scope', 'DepotService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, DepotService, $filter, $http, ngTableParams, $modal) {

    //Getting User Role Data in Veriable 
    var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    if (UserRole.role == 'HQ Master login' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Master login' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Agents' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH cash manager' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH delivery planner' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Ops associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Super visor' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Service lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Sales lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH sales executives' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH sales associates' && UserRole.ItemCategory == 'True') {
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        console.log(" category Controller reached");
        $scope.currentPageStores = {};

        //............................Exel export Method.....................//

        alasql.fn.myfmt = function (n) {
            return Number(n).toFixed(2);
        }
        $scope.exportData = function () {

            var url = serviceBase + "api/Category/export";
            $http.get(url).then(function (results) {

                $scope.storesitem = results.data;

                alasql('SELECT Categoryid,CategoryName,CategoryHindiName,Discription,CreatedDate,UpdatedDate,CreatedBy,UpdateBy,LogoUrl,Code,Level,BaseCategoryId,[Deleted],[IsActive] INTO XLSX("Category.xlsx",{headers:true}) FROM ?', [$scope.storesitem]);
            }, function (error) {
            });
        };
        //............................Exel export Method.....................//

        //Open Add popup
        $scope.open = function () {
            console.log("Modal opened ");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myCategoryModal.html",
                    controller: "ModalInstanceCtrlCategoryedit", resolve: { category: function () { return $scope.items } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.currentPageStores.push(selectedItem);
                },
                function () {
                    console.log("Cancel Condintion");
                })
        };

        //Open Add popup
        $scope.edit = function (item) {
            debugger;
            console.log("Edit Dialog called survey");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myCategoryModalPut.html",
                    controller: "ModalInstanceCtrlCategoryedit", resolve: { category: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {
                    debugger;
                    $scope.categorys.push(selectedItem);
                    _.find($scope.categorys, function (category) {
                        if (category.id == selectedItem.id) {
                            category = selectedItem;
                        }
                    });
                    $scope.categorys = _.sortBy($scope.categorys, 'Id').reverse();
                    $scope.selected = selectedItem;
                },
                function () {
                    console.log("Cancel Condintion");
                })
        };

        //Open Set Active Popup
        $scope.SetActive = function (item) {
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myactivemodal.html",
                    controller: "ModalInstanceCtrlCategoryedit", resolve: { category: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.categorys.push(selectedItem);
                    _.find($scope.categorys, function (category) {
                        if (category.id == selectedItem.id) {
                            category = selectedItem;
                        }
                    });
                    $scope.categorys = _.sortBy($scope.categorys, 'Id').reverse();
                    $scope.selected = selectedItem;
                },
                function () {
                })
        };

        //Open Set Delete Popup
        $scope.opendelete = function (data, $index) {
            debugger;
            console.log(data);
            console.log("Delete Dialog called for category");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModaldeleteCategory.html",
                    controller: "ModalInstanceCtrldeleteDepot", resolve: { category: function () { return data } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.currentPageStores.splice($index, 1);
                },
                function () {
                    console.log("Cancel Condintion");
                })
        };

        $scope.categorys = [];
        DepotService.getdepots().then(function (results) {
            debugger;
            $scope.categorys = results.data;
            $scope.callmethod();
        }, function (error) {
        });

        //Paging Method
        $scope.callmethod = function () {
            var init;
            return $scope.stores = $scope.categorys,
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


//Instance Controller
app.controller("ModalInstanceCtrlCategoryedit", ["$scope", '$http', 'ngAuthSettings', "DepotService", "$modalInstance", "category", 'FileUploader', function ($scope, $http, ngAuthSettings, DepotService, $modalInstance, category, FileUploader) {
    console.log("category");
    var input = document.getElementById("file");
    var today = new Date();
    $scope.today = today.toISOString();
    $scope.$watch('files', function () {
        $scope.upload($scope.files);
    });

    ////for image
    $scope.uploadedfileName = '';
    $scope.upload = function (files) {

        console.log(files);
        if (files && files.length) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                console.log(config.file.name);

                console.log("File Name is " + $scope.uploadedfileName);
                var fileuploadurl = '/api/logoUpload/post', files;

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

    debugger;
    //$scope.BaseCategories = [];
    //var caturl = serviceBase + "api/BaseCategory";
    //$http.get(caturl)
    // .success(function (data) {
    //     console.log("Got Here");
    //     console.log(data);

    //     $scope.BaseCategories = data;
    // })
    //  .error(function (data) {
    //      console.log("Error Got Heere is ");
    //      console.log(data);
    //  })

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

    $scope.DepotData = {};
    if (category) {
        console.log("category if conditon");
        var basecateId = category.Warehouse.WarehouseId;
        category.WarehouseId = basecateId;
        $scope.DepotData = category;
        console.log($scope.DepotData.CategoryName);
    }

    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    //Add Depot
    $scope.AddDepot = function (data) {
        debugger;
        if (data.WarehouseId == null || data.WarehouseId == 0) {
            alert('Please Select Warehouse ');
        }
        else if (data.DepotName == null) {
            alert('Please Enter DepotName ');
        }
        else {
            console.log("Add DepotName");
            var LogoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
            $scope.DepotData.LogoUrl = LogoUrl;
            console.log($scope.DepotData.LogoUrl);
            var url = serviceBase + "api/Depot";
            var dataToPost = {
                LogoUrl: $scope.DepotData.LogoUrl,
                WarehouseId: data.WarehouseId,
                Depotid: data.Depotid,
                DepotName: data.DepotName,
                DepotAddress: data.DepotAddress,
                Description: data.Description,
                IsActive: true,
            };

            console.log(dataToPost);
            $http.post(url, dataToPost)
            .success(function (data) {
                debugger;
                if (data.Depotid == 0) {

                    //Passing Alert message by cs 
                    if (data.Message == "Error") {
                        alert("Depot Not Added!Some Error Occurs!")
                        $modalInstance.close(data);
                    }

                }
                    //Passing Alert message by cs 
                else if (data.Message == "Sucessfully") {
                    debugger;
                    alert("Depot Added Sucessfully!")
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
    };

    //Update Depot / PUT Method
    $scope.PutDepot = function (data) {
        debugger;
        if (data.WarehouseId == null || data.WarehouseId == 0) {
            alert('Please Select Warehouse ');
        }
        else if (data.DepotName == null) {
            alert('Please Enter DepotName ');
        }
        else {
            $scope.DepotData = {};
            $scope.loogourl = category.LogoUrl;

            $scope.ok = function () { $modalInstance.close(); },
            $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

            console.log("Update Depot");
            if ($scope.uploadedfileName == null || $scope.uploadedfileName == '') {
                console.log("if looppppppppppp");
                var url = serviceBase + "api/Depot";
                var dataToPost = {
                    //LogoUrl: $scope.loogourl,
                    Depotid: data.Depotid,
                    WarehouseId: data.WarehouseId,
                    DepotName: data.DepotName,
                    DepotAddress: data.DepotAddress,
                    //CategoryHindiName: data.CategoryHindiName,
                    Description: data.Description,
                    IsActive: data.IsActive,
                    //Code: data.Code,
                };
                console.log(dataToPost);
                $http.put(url, dataToPost)
                .success(function (data) {
                    debugger;
                    //Passing Alert message by cs 
                    if (data.Message == "Error") {
                        alert("Depot Not Updated!Some Error Occure!")
                        $modalInstance.close(data);
                    }
                    else if (data.Message == "Sucessfully") {
                        debugger;
                        alert("Depot Updated Sucessfully!")
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
            else {

                console.log("Else loppppppppp ");
                var LogoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
                console.log(LogoUrl);
                console.log("Image name in Insert function :" + $scope.uploadedfileName);
                $scope.DepotData.LogoUrl = LogoUrl;
                console.log($scope.DepotData.LogoUrl);
                var url = serviceBase + "api/Depot";
                var dataToPost = {
                    //LogoUrl: $scope.DepotData.LogoUrl,
                    Depotid: data.Depotid,
                    Warehouse: {
                        WarehouseId: data.WarehouseId
                    },
                    DepotName: data.DepotName,
                    DepotAddress: data.DepotAddress,
                    Description: data.Description,
                    IsActive: data.IsActive
                    //Code: data.Code
                };
                console.log(dataToPost);
                $http.put(url, dataToPost)
                .success(function (data) {
                    debugger;
                    console.log("Error Gor Here");
                    console.log(data);
                    if (data.Categoryid == 0) {
                        $scope.gotErrors = true;
                        if (data[0].exception == "Already") {
                            console.log("Got This User Already Exist");
                            $scope.AlreadyExist = true;
                        }
                            //Passing Alert message by cs 
                        else if (data.Message == "Error") {
                            alert("Depot Not Updated!Some Error Occure!")
                            $modalInstance.close(data);
                        }

                    }
                        //Passing Alert message by cs 
                    else if (data.Message == "Sucessfully") {
                        debugger;
                        alert("Depot Updated Sucessfully!")
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

//ModalInstance Delete
app.controller("ModalInstanceCtrldeleteDepot", ["$scope", '$http', "$modalInstance", "DepotService", 'ngAuthSettings', "category", function ($scope, $http, $modalInstance, DepotService, ngAuthSettings, category) {
    debugger;
    console.log("delete modal opened");
    function ReloadPage() {
        location.reload();
    };
    $scope.categorys = [];
    if (category) {
        $scope.DepotData = category;
        console.log("found category");
        console.log(category.Depotid);
        console.log($scope.DepotData);
    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    $scope.deleteDepot = function (dataToPost, $index) {
        debugger;
        console.log("Delete  category controller");
        DepotService.deleteDepots(dataToPost).then(function (results) {
            console.log("Del");
            console.log(results);
            console.log("index of item " + $index);
            console.log($scope.categorys);
            $modalInstance.close(dataToPost);
        }, function (error) {
            alert(error.data.message);
        });
    }
}])