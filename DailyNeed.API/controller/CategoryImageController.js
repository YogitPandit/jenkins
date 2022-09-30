'use strict';
app.controller('CategoryImageController', ['$scope', 'CategoryService', "$filter", "$http", "ngTableParams", '$modal', function ($scope, CategoryService, $filter, $http, ngTableParams, $modal) {
    var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    if (UserRole.role == 'HQ Master login' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Master login' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ HR executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Agents' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH cash manager' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH delivery planner' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Ops associate' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Super visor' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Service lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH Sales lead' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH sales executives' && UserRole.ItemCategory == 'True' || UserRole.role == 'WH sales associates' && UserRole.ItemCategory == 'True') {
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    console.log(" category Controller reached");
    $scope.currentPageStores = {};

    $scope.open = function () {
        
        console.log("Modal opened ");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myCategoryModal.html",
                controller: "ModalInstanceCtrlCategoryAddimages", resolve: { category: function () { return $scope.items } }
            }), modalInstance.result.then(function (selectedItem) {

                $scope.currentPageStores.push(selectedItem);
            },
            function () {
                console.log("Cancel Condintion");              
            })
    };

    $scope.edit = function (item) {
        
        console.log("Edit Dialog called survey");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myCategoryModalPut.html",
                controller: "ModalInstanceCtrlCategoryEditimages", resolve: { category: function () { return item } }
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
                console.log("Cancel Condintion");
            })
    };
    
    $scope.SetActive = function (item) {
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myactivemodal.html",
                controller: "ModalInstanceCtrlCategoryeditimage", resolve: { category: function () { return item } }
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

    $scope.opendelete = function (data,$index) {
        console.log(data);
        console.log("Delete Dialog called for category");
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myModaldeleteCategory.html",
                controller: "ModalInstanceCtrldeleteCategory", resolve: { category: function () { return data } }
            }), modalInstance.result.then(function (selectedItem) {
                $scope.currentPageStores.splice($index, 1);
            },
            function () {
                console.log("Cancel Condintion");           
            })
    };

    $scope.categoryImages = [];
    var url = serviceBase + "api/CategoryImage/GetCategoryImage";
    $http.get(url)
           .success(function (results) {
             
               $scope.categoryImages = results;
               $scope.callmethod();

           });
    $scope.callmethod = function () {
        var init;
        return $scope.stores = $scope.categoryImages,
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
else
{
        window.location = "#/404";
}
}]);

app.controller("ModalInstanceCtrlCategoryAddimages", ["$scope", '$http', 'ngAuthSettings', "CategoryService", "$modalInstance", "category", 'FileUploader', function ($scope, $http, ngAuthSettings, CategoryService, $modalInstance, category, FileUploader) {
    
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

    $scope.Categories = [];
    var caturl = serviceBase + "api/CategoryImage";
    $http.get(caturl)
     .success(function (data) {
         console.log("Got Here");
         console.log(data);
         $scope.Categories = data;
     })
      .error(function (data) {
          console.log("Error Got Heere is ");
          console.log(data);
      })

    $scope.CategoryData = {  };
    if (category) {
        console.log("category if conditon");
        $scope.CategoryData = category;
        console.log($scope.CategoryData.CategoryName);
    }

    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


  $scope.AddCategory = function (data) {    
      console.log("Add category");
      var LogoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
      console.log(LogoUrl);    
      console.log("Image name in Insert function :" + $scope.uploadedfileName);
      $scope.CategoryData.LogoUrl = LogoUrl;
      console.log($scope.CategoryData.LogoUrl);
      var url = serviceBase + "api/CategoryImage";
      var dataToPost = {
          CategoryImg: $scope.CategoryData.LogoUrl,
          Categoryid: data.Categoryid,
          IsActive: true,
      };
      console.log(dataToPost);
      $http.post(url, dataToPost)
      .success(function (data) {
       
          console.log("Error Gor Here");
          console.log(data);
          if (data == "1") {
              alert("Category Image Added");
              $modalInstance.close(data);
            
              window.location.reload("true");
          }
          else {
              alert("Not Added Some Error");
          }
      })
       .error(function (data) {
           console.log("Error Got Heere is ");
           console.log(data);
       })
  };
    
    $scope.PutCategory = function (data) {       
        $scope.CategoryData = {};
        $scope.loogourl = data.CategoryImg;

        if (category) {
            $scope.CategoryData = category;
            console.log("found Puttt category");
            console.log(category);
            console.log($scope.CategoryData);
        }
        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

        console.log("Update category");
        if ($scope.uploadedfileName == null || $scope.uploadedfileName == '') {
          
            console.log("if looppppppppppp");
            var url = serviceBase + "api/CategoryImage";
            var dataToPost = {
                CategoryImageId: $scope.CategoryData.CategoryImageId,
                CategoryImg: $scope.loogourl,
                CategoryId: $scope.CategoryData.CategoryId,
                IsActive: $scope.CategoryData.IsActive,
                CreatedDate: $scope.CategoryData.CreatedDate
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
            $scope.CategoryData.LogoUrl = LogoUrl;
            console.log($scope.CategoryData.LogoUrl);
            var url = serviceBase + "api/CategoryImage";
            var dataToPost = {
                CategoryImageId: $scope.CategoryData.CategoryImageId,
                CategoryImg: $scope.CategoryData.LogoUrl,
                CategoryId: $scope.CategoryData.CategoryId,
                IsActive: $scope.CategoryData.IsActive,
                CreatedDate: $scope.CategoryData.CreatedDate
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
}])

app.controller("ModalInstanceCtrlCategoryEditimages", ["$scope", '$http', 'ngAuthSettings', "CategoryService", "$modalInstance", "category", 'FileUploader', function ($scope, $http, ngAuthSettings, CategoryService, $modalInstance, category, FileUploader) {
    
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

    $scope.Categories = [];
    var caturl = serviceBase + "api/CategoryImage";
    $http.get(caturl)
     .success(function (data) {
         console.log("Got Here");
         console.log(data);
         $scope.Categories = data;
     })
      .error(function (data) {
          console.log("Error Got Heere is ");
          console.log(data);
      })

    $scope.CategoryData = {};
    if (category) {
        console.log("category if conditon");
        $scope.CategoryData = category;
        console.log($scope.CategoryData.CategoryName);
    }

    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
    $scope.PutCategory = function (data) {
        
        $scope.CategoryData = {};
        $scope.loogourl = data.CategoryImg;

        if (category) {
            $scope.CategoryData = category;
            console.log("found Puttt category");
            console.log(category);
            console.log($scope.CategoryData);
        }
        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

        console.log("Update category");
        if ($scope.uploadedfileName == null || $scope.uploadedfileName == '') {

            console.log("if looppppppppppp");
            var url = serviceBase + "api/CategoryImage";
            var dataToPost = {
                CategoryImageId: $scope.CategoryData.CategoryImageId,
                CategoryImg: $scope.loogourl,
                CategoryId: $scope.CategoryData.CategoryId,
                IsActive: $scope.CategoryData.IsActive,
                CreatedDate: $scope.CategoryData.CreatedDate
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
            $scope.CategoryData.LogoUrl = LogoUrl;
            console.log($scope.CategoryData.LogoUrl);
            var url = serviceBase + "api/CategoryImage";
            var dataToPost = {
                CategoryImageId: $scope.CategoryData.CategoryImageId,
                CategoryImg: $scope.CategoryData.LogoUrl,
                CategoryId: $scope.CategoryData.CategoryId,
                IsActive: $scope.CategoryData.IsActive,
                CreatedDate: $scope.CategoryData.CreatedDate
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
}])

app.controller("ModalInstanceCtrldeleteCategory", ["$scope", '$http', "$modalInstance", "CategoryService", 'ngAuthSettings', "category", function ($scope, $http, $modalInstance, CategoryService, ngAuthSettings, category) {
    console.log("delete modal opened");
    function ReloadPage() {
        location.reload();
    };
    $scope.categorys = [];
    if (category) {
        $scope.CategoryData = category;
        console.log("found category");
        console.log(category.Categoryid);
        console.log($scope.CategoryData);      
    }
    $scope.ok = function () {
        
        $modalInstance.close();
    },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
    $scope.deleteCategory = function (dataToPost, $index) {
        console.log("Delete  category controller");       
        $http.delete(serviceBase + 'api/CategoryImage/?id=' + $scope.CategoryData.CategoryImageId).then(function (results) {
            console.log(results);
            return results;
            console.log("Del");
            console.log(results);
            console.log("index of item " + $index);
            console.log($scope.categorys);
            $modalInstance.close(dataToPost);
        }, function (error) {
            alert(error.data.message);
        });
        $modalInstance.close();
    }
}])