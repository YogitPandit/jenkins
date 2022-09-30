'use strict';
app.controller('warehouseController', ['$scope', 'WarehouseService', 'CityService', 'StateService', "$filter", "$http", "ngTableParams", '$modal', 'FileUploader',
    function ($scope, WarehouseService, CityService, StateService, $filter, $http, ngTableParams, $modal, FileUploader) {
        console.log(" Warehouse  Controller reached");
        $scope.currentPageStores = {};

        $scope.update = function () {
            console.log($scope.selectedItem);
            if ($scope.selectedItem == "TextBox") {
            }
            else if ($scope.selectedItem == "RadioButton") {
            }
            else if ($scope.selectedItem == "MultiSelect") {
            }
        }

        $scope.uploadshow = true;
        $scope.toggle = function () {
            $scope.uploadshow = !$scope.uploadshow;
        };

        function sendFileToServer(formData, status) {
            var uploadURL = "/api/quesansupload/post"; //Upload URL
            var extraData = {}; //Extra Data.
            var jqXHR = $.ajax({
                xhr: function () {
                    var xhrobj = $.ajaxSettings.xhr();
                    if (xhrobj.upload) {
                        xhrobj.upload.addEventListener('progress', function (event) {
                            var percent = 0;
                            var position = event.loaded || event.position;
                            var total = event.total;
                            if (event.lengthComputable) {
                                percent = Math.ceil(position / total * 100);
                            }
                            //Set progress
                            status.setProgress(percent);
                        }, false);
                    }
                    return xhrobj;
                },
                url: uploadURL,
                type: "POST",
                contentType: false,
                processData: false,
                cache: false,
                data: formData,
                success: function (data) {
                    status.setProgress(100);
                    $("#status1").append("Data from Server:" + data + "<br>");
                }
            });
            status.setAbort(jqXHR);
        };

        var rowCount = 0;
        function createStatusbar(obj) {
            rowCount++;
            var row = "odd";
            if (rowCount % 2 == 0) row = "even";
            this.statusbar = $("<div class='statusbar " + row + "'></div>");
            this.filename = $("<div class='filename'></div>").appendTo(this.statusbar);
            this.size = $("<div class='filesize'></div>").appendTo(this.statusbar);
            this.progressBar = $("<div class='progressBar'><div></div></div>").appendTo(this.statusbar);
            this.abort = $("<div class='abort'>Abort</div>").appendTo(this.statusbar);
            obj.after(this.statusbar);

            this.setFileNameSize = function (name, size) {
                var sizeStr = "";
                var sizeKB = size / 1024;
                if (parseInt(sizeKB) > 1024) {
                    var sizeMB = sizeKB / 1024;
                    sizeStr = sizeMB.toFixed(2) + " MB";
                }
                else {
                    sizeStr = sizeKB.toFixed(2) + " KB";
                }

                this.filename.html(name);
                this.size.html(sizeStr);
            }
            this.setProgress = function (progress) {
                var progressBarWidth = progress * this.progressBar.width() / 100;
                this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "%&nbsp;");
                if (parseInt(progress) >= 100) {
                    this.abort.hide();
                }
            }
            this.setAbort = function (jqxhr) {
                var sb = this.statusbar;
                this.abort.click(function () {
                    jqxhr.abort();
                    sb.hide();
                });
            }
        };

        function handleFileUpload(files, obj) {
            for (var i = 0; i < files.length; i++) {
                var fd = new FormData();
                fd.append('file', files[i]);
                var status = new createStatusbar(obj); //Using this we can set progress.
                status.setFileNameSize(files[i].name, files[i].size);
                sendFileToServer(fd, status);
            }
        };

        $(document).ready(function () {
            var obj = $("#dragandrophandler");
            obj.on('dragenter', function (e) {
                e.stopPropagation();
                e.preventDefault();
                $(this).css('border', '2px solid #0B85A1');
            });
            obj.on('dragover', function (e) {
                e.stopPropagation();
                e.preventDefault();
            });
            obj.on('drop', function (e) {

                $(this).css('border', '2px dotted #0B85A1');
                e.preventDefault();
                var files = e.originalEvent.dataTransfer.files;

                //We need to send dropped files to Server
                handleFileUpload(files, obj);
            });
            $(document).on('dragenter', function (e) {
                e.stopPropagation();
                e.preventDefault();
            });
            $(document).on('dragover', function (e) {
                e.stopPropagation();
                e.preventDefault();
                obj.css('border', '2px dotted #0B85A1');
            });
            $(document).on('drop', function (e) {
                e.stopPropagation();
                e.preventDefault();
            });
        });

        $scope.open = function () {
            console.log("Modal opened  ");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myWarehouseModal.html",
                    controller: "ModalInstanceCtrlWarehouse", resolve: { warehouse: function () { return $scope.items } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.currentPageStores.push(selectedItem);

                },
                function () {
                    console.log("Cancel Condintion");
                })
        };

        $scope.edit = function (item) {
            console.log("Edit Dialog called warehouse ");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myWarehousePut.html",
                    controller: "ModalInstanceCtrlWarehouse", resolve: { warehouse: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {
                    console.log("sELECT wAREHOUSE..")
                    $scope.warehouse.push(selectedItem);
                    _.find($scope.warehouse, function (warehouse) {
                        if (warehouse.id == selectedItem.id) {
                            warehouse = selectedItem;
                        }
                    });

                    $scope.warehouse = _.sortBy($scope.warehouse, 'Id').reverse();
                    $scope.selected = selectedItem;

                },
                function () {
                    console.log("Cancel Condintion");

                })

        };

        $scope.opendelete = function (data, $index) {
            debugger;
            console.log(data);
            console.log("Delete Dialog called for warehouse ");



            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModaldeleteWarehouse.html",
                    controller: "ModalInstanceCtrldeleteWarehouse", resolve: { warehouse: function () { return data } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.currentPageStores.splice($index, 1);
                },
                function () {
                    console.log("Cancel Condintion");

                })
        };

        $scope.warehouses = [];
        WarehouseService.getwarehouse().then(function (results) {
            $scope.warehouses = results.data;
            $scope.callmethod();
        }, function (error) {
        });

        $scope.callmethod = function () {
            var init;
            return $scope.stores = $scope.warehouses,
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

app.controller("ModalInstanceCtrlWarehouse", ["$scope", '$http', 'ngAuthSettings', 'TaxGroupService', "WarehouseService", 'CityService', 'StateService', "$modalInstance", 'FileUploader', "warehouse",
     function ($scope, $http, ngAuthSettings, TaxGroupService, WarehouseService, CityService, StateService, $modalInstance, FileUploader, warehouse) {
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

         console.log("Warehouse");
         $scope.WarehouseData = {};

         $scope.taxgroups = [];
         TaxGroupService.getTaxGroup().then(function (results) {
             $scope.taxgroups = results.data;
             console.log("tax group");
             console.log($scope.taxgroups);
         }, function (error) {
         });

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

         if (warehouse) {
             debugger;
             console.log("warehouse if conditon");
             var stateId = warehouse.State.Stateid;
             warehouse.Stateid = stateId;
             var cityId = warehouse.City.Cityid;
             warehouse.Cityid = cityId
             $scope.WarehouseData = warehouse;
             console.log($scope.WarehouseData.CityName);
         }

         $scope.ok = function () { $modalInstance.close(); },
         $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

         $scope.AddWarehouse = function (data) {
             debugger;
             if (data.WarehouseName == null || data.WarehouseName == "") {
                 alert('Please Enter Warehouse Name ');
             }
             else if (data.Stateid == null || data.Stateid == 0) {
                 alert('Please Select State ');
             }
             else if (data.Cityid == null || data.Cityid == 0) {
                 alert('Please Select City ');
             }
             else {
                 console.log("warehouse");
                 console.log(data);
                 var url = serviceBase + "api/Warehouse";
                 //var dataToPost = { Warehouseid: $scope.WarehouseData.Warehouseid, Cityid: $scope.QuesAnsData.Cityid, CityName: $scope.QuesAnsData.CityName, StateName: $scope.QuesAnsData.StateName, Stateid: $scope.QuesAnsData.Stateid, CreatedDate: $scope.QuesAnsData.CreatedDate, UpdatedDate: $scope.QuesAnsData.UpdatedDate, CreatedBy: $scope.QuesAnsData.CreatedBy, UpdateBy: $scope.QuesAnsData.UpdateBy, Item_name: $scope.QuesAnsData.Item_name, Item_brand: $scope.QuesAnsData.Item_brand, warehouseid: $scope.QuesAnsData.warehouseid, Item_mrp: $scope.QuesAnsData.Item_mrp, Item_offer: $scope.QuesAnsData.Item_offer, Item_size: $scope.QuesAnsData.Item_size, Item_each: $scope.QuesAnsData.Item_each, FileUrl: $scope.QuesAnsData.FileUrl };
                 var dataToPost = {
                     WarehouseName: $scope.WarehouseData.WarehouseName,
                     Address: $scope.WarehouseData.Address,
                     //Email: $scope.WarehouseData.Email,
                     //Phone: $scope.WarehouseData.Phone,
                     CityId: $scope.WarehouseData.Cityid,
                     StateId: $scope.WarehouseData.Stateid,
                     CreatedDate: $scope.WarehouseData.CreatedDate,
                     UpdatedDate: $scope.WarehouseData.UpdatedDate,
                     CreatedBy: $scope.WarehouseData.CreatedBy,
                     UpdateBy: $scope.WarehouseData.UpdateBy,
                     Active: $scope.WarehouseData.Active
                 };
                 console.log("kkkkkk");
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
                         alert("Warehouse Not Save!Some Error Occure!")
                         $modalInstance.close(data);
                     }

                         //Passing Alert message by cs 
                     else if (data.Message == "Sucessfully") {
                         alert("Warehouse Add Sucessfully!")
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
                      console.log("Error Got Heere is ");
                      console.log(data);
                  })
             }
         };

         $scope.PutWarehouse = function (data) {
             if (data.WarehouseName == null || data.WarehouseName == "") {
                 alert('Please Enter Warehouse Name ');
             }
             else if (data.Stateid == null || data.Stateid == 0) {
                 alert('Please Select State ');
             }
             else if (data.Cityid == null || data.Cityid == 0) {
                 alert('Please Select City ');
             }
             else {
                 $scope.WarehouseData = {};
                 if (warehouse) {
                     $scope.WarehouseData = warehouse;
                     console.log("found Puttt Warehouse ");
                     console.log(warehouse);
                     console.log($scope.WarehouseData);
                 }
                 $scope.ok = function () { $modalInstance.close(); },
                 $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

                 console.log("Update Warehouse ");
                 var url = serviceBase + "api/Warehouse";
                 var dataToPost = {
                     WarehouseId: $scope.WarehouseData.WarehouseId,
                     WarehouseName: $scope.WarehouseData.WarehouseName,
                     Address: $scope.WarehouseData.Address,
                     //Email: $scope.WarehouseData.Email,
                     //Phone: $scope.WarehouseData.Phone,
                     CityId: $scope.WarehouseData.Cityid,
                     StateId: $scope.WarehouseData.Stateid,
                     CreatedDate: $scope.WarehouseData.CreatedDate,
                     UpdatedDate: $scope.WarehouseData.UpdatedDate,
                     CreatedBy: $scope.WarehouseData.CreatedBy,
                     UpdateBy: $scope.WarehouseData.UpdateBy,
                     Active: $scope.WarehouseData.Active
                 };
                 console.log(dataToPost);
                 $http.put(url, dataToPost)
                 .success(function (data) {
                     console.log("Error Got Here");
                     console.log(data);
                         //Passing Alert message by cs 
                     if (data.Message == "Error") {
                         alert("Warehouse Not Update!Some Error Occure!")
                         $modalInstance.close(data);
                     }

                         //Passing Alert message by cs 
                     else if (data.Message == "Sucessfully") {
                         alert("Warehouse Updated Sucessfully!")
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
                      console.log("Error Got Heere is ");
                      console.log(data);
                  })
             };

             /////////////////////////////////////////////////////// angular upload code
             var uploader = $scope.uploader = new FileUploader({
                 url: serviceBase + 'api/upload'
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

app.controller("ModalInstanceCtrldeleteWarehouse", ["$scope", '$http', "$modalInstance", "WarehouseService", 'ngAuthSettings', "warehouse",
    function ($scope, $http, $modalInstance, WarehouseService, ngAuthSettings, warehouse) {
        debugger;
        console.log("delete modal opened");
        function ReloadPage() {
            location.reload();
        };
        $scope.warehouse = [];
        if (warehouse) {
            $scope.WarehouseData = warehouse;
            console.log("found warehouse ");
            //console.log(QuesAnsData);
            console.log($scope.WarehouseData);
        }
        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
        $scope.deleteWarehouse = function (dataToPost, $index) {
            debugger;
            console.log(dataToPost);
            console.log("Delete  warehouse  controller");

            //return $http.delete(serviceBase + 'api/Warehouse/?id=' + WarehouseData.WarehouseId).then(function (results) {
            //    return results;
            //});
            WarehouseService.deletewarehouse(dataToPost).then(function (results) {
                console.log("Del");
                console.log("index of item " + $index);
                console.log($scope.warehouse.length);
                $modalInstance.close(dataToPost);
            }, function (error) {

                alert(error.data.message);
            });
        }
    }])