'use strict';
app.controller('peoplesController', ['$scope', 'peoplesService', 'CityService', 'StateService', "$filter", "$http", "ngTableParams", '$modal', 'WarehouseService',
    function ($scope, peoplesService, CityService, StateService, $filter, $http, ngTableParams, $modal, WarehouseService) {
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

        console.log("People Controller reached");
        var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        if (UserRole.role == 'HQ Master login' && UserRole.Admin == 'True' || UserRole.role == 'WH Master login' && UserRole.Admin == 'True' || UserRole.role == 'HQ HR lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.Admin == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.Admin == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ HR associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ HR executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.Admin == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.Admin == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.Admin == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.Admin == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.Admin == 'True' || UserRole.role == 'WH Agents' && UserRole.Admin == 'True' || UserRole.role == 'WH cash manager' && UserRole.Admin == 'True' || UserRole.role == 'WH delivery planner' && UserRole.Admin == 'True' || UserRole.role == 'WH Ops associate' && UserRole.Admin == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.Admin == 'True' || UserRole.role == 'WH Super visor' && UserRole.Admin == 'True' || UserRole.role == 'WH Service lead' && UserRole.Admin == 'True' || UserRole.role == 'WH Sales lead' && UserRole.Admin == 'True' || UserRole.role == 'WH sales executives' && UserRole.Admin == 'True' || UserRole.role == 'WH sales associates' && UserRole.Admin == 'True') {
            //.................File Uploader method start..................    
            $(function () {
                $('input[name="daterange"]').daterangepicker({
                    timePicker: true,
                    timePickerIncrement: 5,
                    timePicker12Hour: true,
                    //locale: {
                    format: 'MM/DD/YYYY h:mm A'
                    //}
                });
            });

            $scope.citys = [];
            CityService.getcitys().then(function (results) {

                $scope.citys = results.data;
            }, function (error) {
            });

            $scope.warehouse = [];
            WarehouseService.getwarehouse().then(function (results) {
                $scope.warehouse = results.data;

            }, function (error) {
            });

            $scope.uploadshow = true;
            $scope.toggle = function () {
                $scope.uploadshow = !$scope.uploadshow;
            }

            function sendFileToServer(formData, status) {
                var uploadURL = "/api/peopleupload/post"; //Upload URL
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
            }

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
            }
            function handleFileUpload(files, obj) {
                for (var i = 0; i < files.length; i++) {
                    var fd = new FormData();
                    fd.append('file', files[i]);
                    var status = new createStatusbar(obj); //Using this we can set progress.
                    status.setFileNameSize(files[i].name, files[i].size);
                    sendFileToServer(fd, status);

                }
            }
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
            $scope.currentPageStores = {};
            $scope.open = function () {

                console.log("Modal opened people");
                var modalInstance;
                modalInstance = $modal.open(
                    {
                        templateUrl: "myPeopleModal.html",
                        controller: "ModalInstanceCtrlPeople", resolve: { people: function () { return $scope.items } }
                    }), modalInstance.result.then(function (selectedItem) {
                        window.location.reload();
                    },
                    function () {
                        console.log("Cancel Condintion");
                    })
            };
            $scope.edit = function (item) {
                debugger;
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
                    },
                    function () {
                        console.log("Cancel Condintion");
                    })
            };
            $scope.opendelete = function (data, $index) {
                console.log(data);
                console.log("Delete Dialog called for people");
                var modalInstance;
                modalInstance = $modal.open(
                    {
                        templateUrl: "myModaldeletepeople.html",
                        controller: "ModalInstanceCtrldeletepeople", resolve: { people: function () { return data } }
                    }), modalInstance.result.then(function (selectedItem) {
                        $scope.currentPageStores.splice($index, 1);
                    },
                    function () {
                        console.log("Cancel Condintion");
                    })
            };

            $scope.peoples = [];
            peoplesService.getpeoples().then(function (results) {
                debugger;
                $scope.peoples = results.data;
                console.log("Got people collection");
                console.log($scope.peoples);
                $scope.callmethod();

                $scope.AddData = function () {

                    var url = serviceBase + "api/trackuser?action=View&item=People";
                    $http.post(url).success(function (results) {

                    });

                }
                $scope.AddData();
            }, function (error) {
            });

            $scope.warehouse = [];
            WarehouseService.getwarehouse().then(function (results) {

                $scope.warehouse = results.data;
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

                    $scope.numPerPageOpt = [3, 5, 10, 20],
                    $scope.numPerPage = $scope.numPerPageOpt[1],
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

app.controller("ModalInstanceCtrlPeople", ["$scope", '$http', '$modal', 'ngAuthSettings', "peoplesService", 'CityService', 'StateService', "$modalInstance", "people", 'WarehouseService', 'authService',
    function ($scope, $http, $modal, ngAuthSettings, peoplesService, CityService, StateService, $modalInstance, people, WarehouseService, authService) {
        debugger;
        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
        console.log("People");

        $scope.PeopleData = {};
        var alrt = {};
        if (people) {

            var warehouseId = people.Warehouse.WarehouseId;
            people.WarehouseId = warehouseId;
            var cityId = people.City.Cityid;
            people.Cityid = cityId;
            var stateId = people.State.Stateid;
            people.Stateid = stateId;
            $scope.PeopleData = people;
        }
        $scope.citys = [];
        $scope.getRole = function () {
            debugger;
            var url = serviceBase + "api/usersroles/GetAllRoles";
            $http.get(url)
            .success(function (data) {
                $scope.roles = data;
                console.log(data);
            }, function (error) { });
        }
        $scope.getRole();
        CityService.getcitys().then(function (results) {
            debugger;
            $scope.citys = results.data;
        }, function (error) {
        });
        $scope.states = [];
        StateService.getstates().then(function (results) {

            $scope.states = results.data;
        }, function (error) {
        });
        $scope.warehouse = [];
        WarehouseService.getwarehouse().then(function (results) {
            debugger;
            $scope.warehouse = results.data;
        }, function (error) {
        });
        // For Validation 
        $scope.CheckEmail = function (Email) {

            var url = serviceBase + "api/Peoples/Email?Email=" + Email;
            $http.get(url)
            .success(function (data) {
                if (data == 'null') {
                }
                else {
                    alert("Email Is already Exist");
                    //$scope.PeopleData.Email = '';
                }
                console.log(data);
            });
        }
        $scope.CheckMobile = function (Mobile) {
            var url = serviceBase + "api/Peoples/Mobile?Mobile=" + Mobile;
            $http.get(url)
            .success(function (data) {
                if (data == 'null') {
                }
                else {
                    alert("Mobile Number Is already Exist");
                    //$scope.PeopleData.Mobile = '';
                }
                console.log(data);
            });
        }
        $scope.ok = function () { $modalInstance.close(); },
        $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

        //Add People
        $scope.AddPeople = function (PeopleData) {
            debugger;
            if (PeopleData.Email != null) {
                PeopleData.Email = PeopleData.Email;
            }
            if (PeopleData.PeopleFirstName == null || PeopleData.PeopleFirstName == "") {
                alert('Please Enter First Name');
                $modalInstance.open();
            }
            else if (PeopleData.PeopleLastName == null || PeopleData.PeopleLastName == "") {
                alert('Please Enter Last Name');
                $modalInstance.open();
            }
            else if (PeopleData.Email == null || PeopleData.Email == "") {
                alert('Please Enter Email');
                $modalInstance.open();
            }
            else if (PeopleData.Password == null || PeopleData.Password == "") {
                alert('Please Enter Password');
                $modalInstance.open();
            }
            else if (PeopleData.Mobile == null || PeopleData.Mobile == "") {
                alert('Please Enter Mobile');
                $modalInstance.open();
            }
            else if (PeopleData.Stateid == "" || PeopleData.Stateid == 0) {
                alert('Please Select State');
                $modalInstance.open();
            }
            else if (PeopleData.Cityid == "" || PeopleData.Cityid == 0) {
                alert('Please Select City');
                $modalInstance.open();
            }
            else if (PeopleData.warehouseId == "" || PeopleData.warehouseId == 0) {
                alert('Please Select warehouse');
                $modalInstance.open();
            }
            else {
                authService.saveRegistrationpeople($scope.PeopleData).then(function (response) {
                    console.log(response);
                    $modalInstance.close(response);
                    if (response.status == "200") {
                        $scope.AddTrack = function (Atype, page, Detail) {

                            console.log("Tracking Code");
                            var url = serviceBase + "api/trackuser?action=" + Atype + "&item=" + page + " " + Detail;
                            $http.post(url).success(function (results) { });
                        }
                        //End User Tracking

                        $scope.AddTrack("Add(people)", "PeopleName:", $scope.PeopleData.PeopleFirstName);

                        alrt.msg = "Record has been Save successfully";
                        $modal.open(
                                        {
                                            templateUrl: "PopUpModel.html",
                                            controller: "PopUpController", resolve: { message: function () { return alrt } }
                                        }), modalInstance.result.then(function (selectedItem) {

                                        },
                                    function () {
                                        console.log("Cancel Condintion");
                                    })
                        window.location.reload();
                    }
                });
            }
        };

        //Edit People
        $scope.PutPeople = function (data) {
            debugger;
            if (data.PeopleFirstName == null || data.PeopleFirstName == "") {
                alert('Please Enter First Name');
                $modalInstance.open();
            }
            else if (data.PeopleLastName == null || data.PeopleLastName == "") {
                alert('Please Enter Last Name');
                $modalInstance.open();
            }
            else if (data.Email == null || data.Email == "") {
                alert('Please Enter Email');
                $modalInstance.open();
            }
            else if (data.Password == null || data.Password == "") {
                alert('Please Enter Password');
                $modalInstance.open();
            }
            else if (data.Mobile == null || data.Mobile == "") {
                alert('Please Enter Mobile');
                $modalInstance.open();
            }
            else if (data.Stateid == "" || data.Stateid == 0) {
                alert('Please Select State');
                $modalInstance.open();
            }
            else if (data.Cityid == "" || data.Cityid == 0) {
                alert('Please Select City');
                $modalInstance.open();
            }
            else if (data.warehouseId == "" || data.warehouseId == 0) {
                alert('Please Select warehouse');
                $modalInstance.open();
            }
            $scope.PeopleData = {};
            if (people) {
                debugger;
                $scope.PeopleData = people;
                console.log("found Puttt People");
                console.log(people);
                console.log($scope.PeopleData);
                $scope.ok = function () { $modalInstance.close(); },
                $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

                console.log("Update People");


                $scope.AddData = function () {

                    var url = serviceBase + "api/trackuser?action=Edit&item=PeopleName:" + $scope.PeopleData.PeopleFirstName;
                    $http.post(url).success(function (results) {

                    });

                }
                $scope.AddData();
                var url = serviceBase + "/api/Peoples";
                var dataToPost = {
                    PeopleID: $scope.PeopleData.PeopleID,
                    Password: $scope.PeopleData.Password,
                    Active: $scope.PeopleData.Active,
                    StateId: $scope.PeopleData.Stateid,
                    CityId: $scope.PeopleData.Cityid,
                    WarehouseId: $scope.PeopleData.WarehouseId,
                    Mobile: $scope.PeopleData.Mobile,
                    PeopleFirstName: $scope.PeopleData.PeopleFirstName,
                    PeopleLastName: $scope.PeopleData.PeopleLastName,
                    CreatedDate: $scope.PeopleData.CreatedDate,
                    UpdatedDate: $scope.PeopleData.UpdatedDate,
                    CreatedBy: $scope.PeopleData.CreatedBy,
                    UpdateBy: $scope.PeopleData.UpdateBy,
                    Email: $scope.PeopleData.Email,
                    Department: $scope.PeopleData.Department,
                    Permissions: $scope.PeopleData.Permissions,
                    ImageUrl: data.ImageUrl,
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
                        alrt.msg = "Record has been Update successfully";
                        $modal.open(
                                        {
                                            templateUrl: "PopUpModel.html",
                                            controller: "PopUpController", resolve: { message: function () { return alrt } }
                                        }), modalInstance.result.then(function (selectedItem) {
                                        },
                                    function () {
                                        console.log("Cancel Condintion");
                                    })
                    }
                })
                 .error(function (data) {
                     console.log("Error Got Heere is ");
                     console.log(data);
                 })
            };
        }
    }]);

app.controller("ModalInstanceCtrldeletepeople", ["$scope", '$http', '$modal', "$modalInstance", "peoplesService", 'ngAuthSettings', "people", function ($scope, $http, $modal, $modalInstance, peoplesService, ngAuthSettings, people) {
    console.log("delete modal opened");
    var alrt = {};
    if (people) {
        $scope.PeopleData = people;
        console.log("found people");
        console.log(people);
        console.log($scope.PeopleData);

    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },


    $scope.deletepeoples = function (dataToPost) {

        console.log("Delete people controller");


        peoplesService.deletepeoples(dataToPost).then(function (results) {
            console.log("Del");
            $modalInstance.close(dataToPost);
            alrt.msg = "Entry Deleted";

            $scope.AddData = function () {

                var url = serviceBase + "api/trackuser?action=Delete&item=PeopleName:" + $scope.PeopleData.PeopleFirstName;
                $http.post(url).success(function (results) {

                });

            }
            $scope.AddData();
            $modal.open(
                            {
                                templateUrl: "PopUpModel.html",
                                controller: "PopUpController", resolve: { message: function () { return alrt } }
                            }), modalInstance.result.then(function (selectedItem) {
                            },
                        function () {
                            console.log("Cancel Condintion");
                        })
        }, function (error) {
            alert(error.data.message);
        });
    }
}]);