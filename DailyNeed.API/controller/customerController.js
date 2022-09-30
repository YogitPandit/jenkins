'use strict';
app.controller('customerController', ['$scope', 'customerService', 'CityService', 'WarehouseService', "$filter", "$http", "ngTableParams", 'FileUploader', '$modal', '$log', function ($scope, customerService, CityService, WarehouseService, $filter, $http, ngTableParams, FileUploader, $modal, $log) {
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    //.................File Uploader method start..................
    $(function () {
        $('input[name="daterange"]').daterangepicker({
            timePicker: true,
            timePickerIncrement: 5,
            timePicker12Hour: true,
            format: 'MM/DD/YYYY h:mm A'
        });
    });

    // get all Provider list
    $scope.SelectProvider = [];
    $scope.Provider = function (pname) {
        debugger;
        var url = serviceBase + 'api/Customers/getProviders?pname=' + pname;
        $http.get(url)
        .success(function (data) {
            debugger;
            $scope.SelectProvider = data;
        });
    }


    $scope.getProviderfill = function (providername) {
        debugger;
        $scope.ProviderName = providername;
        $('#providerlst').val($scope.ProviderName);
        $scope.hidethis = true;

    }
    ///////////////////
    // get all Provider list
    //$scope.SelectProvider = [];
    //$scope.Provider = function () {
    //    debugger;
    //    var url = serviceBase + 'api/Customers/getProviders';
    //    $http.get(url)
    //    .success(function (data) {
    //        debugger;
    //        $scope.SelectProvider = data;

    //    });
    //}
    //$scope.Provider();
    //$scope.Providers = function (providername) {
    //    debugger;
    //    var output = [];
    //    angular.foreach($scope.SelectProvider,function(providerlst){
    //    if(providerlst.toLowerCase().indexOf(providername.toLowerCase())>=0){
    //        output.push(providerlst);
    //    }
    //    });
    //    $scope.SelectProviderlist = output;

    //}
    //Open Popup for show child customer data
    $scope.childCustomerList = function (data, $index) {
        $scope.getChildCustomer = [];
        var modalInstance;
        var url = serviceBase + "api/customers/GetChildCustomer?parentid=" + data.CustomerLinkId;
        $http.get(url).success(function (results) {
            $scope.getChildCustomer = results;
            if ($scope.getChildCustomer != "") {
                modalInstance = $modal.open(
                    {
                        templateUrl: "myModalChildCustomerData.html",
                        controller: "ModalInstanceCtrlChildCase", resolve: { child: function () { return $scope.getChildCustomer } }
                    })
            }
        })
         .error(function (data) {
             console.log(data);
         })
    };

    debugger;
    var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    if (UserRole.role == 'HQ Master login' && UserRole.Customer == 'True' || UserRole.role == 'WH Master login' && UserRole.Supplier == 'True' || UserRole.role == 'HQ HR lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.Customer == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.Customer == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ HR associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ HR executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.Customer == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.Customer == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.Customer == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.Supplier == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.Customer == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.Customer == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.Customer == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.Supplier == 'True' || UserRole.role == 'WH Agents' && UserRole.Customer == 'True' || UserRole.role == 'WH cash manager' && UserRole.Customer == 'True' || UserRole.role == 'WH delivery planner' && UserRole.Customer == 'True' || UserRole.role == 'WH Ops associate' && UserRole.Customer == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.Customer == 'True' || UserRole.role == 'WH Super visor' && UserRole.Customer == 'True' || UserRole.role == 'WH Service lead' && UserRole.Customer == 'True' || UserRole.role == 'WH Sales lead' && UserRole.Customer == 'True' || UserRole.role == 'WH sales executives' && UserRole.Customer == 'True' || UserRole.role == 'WH sales associates' && UserRole.Customer == 'True') {
        $scope.citys = [];
        CityService.getcitys().then(function (results) {
            $scope.citys = results.data;
        }, function (error) { });

        //$scope.warehouse = [];
        //WarehouseService.getwarehouse().then(function (results) {
        //    $scope.warehouse = results.data;
        //}, function (error) { });

        //Get Warehouse from PWarehouse table


        $scope.uploadshow = true;
        $scope.toggle = function () {
            $scope.uploadshow = !$scope.uploadshow; 
        }
        function sendFileToServer(formData, status) {
            var comp = $scope.UserRole.compid;
            var user = $scope.UserRole.userid;
            debugger;
            var uploadURL = "/api/customerupload/post?compid=" + comp + "&userid=" + user; //Upload URL
            var extraData = {}; //Extra Data.
            var jqXHR = $.ajax({
                xhr: function () {
                    debugger;
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
                    if (data.msg == "Your Exel data is succesfully saved") {
                        alert('Your Exel data is succesfully saved');
                    }
                    else {
                        alert('Error occurred');
                    }
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

        //............................File Uploader Method End.....................//

        //............................Exel export Method.....................//

        alasql.fn.myfmt = function (n) {
            return Number(n).toFixed(2);
        }
        $scope.exportData1 = function () {
            debugger;
            var url = serviceBase + "api/Customers/export";
            $http.get(url).then(function (results) {

                $scope.storesitem = results.data;
                //alasql('SELECT RetailerId,RetailersCode,ShopName,RetailerName,Mobile,Address,Area,Warehouse,ExecutiveName,Emailid,ClusterId,Day,latitute,longitute,BeatNumber,ExecutiveId,ClusterName,[Active],[Deleted] INTO XLSX("Customer.xlsx",{headers:true}) FROM ?', [$scope.storesitem]);

                alasql('SELECT RetailerId,RetailersCode,ShopName,RetailerName,Mobile,Address,Area,Emailid,Day,latitute,longitute,[Active],[Deleted] INTO XLSX("Customer.xlsx",{headers:true}) FROM ?', [$scope.storesitem]);
            }, function (error) {
            });
        };
        $scope.exportData = function () {
            debugger;
            alasql('SELECT * INTO XLSX("Customer.xlsx",{headers:true}) \ FROM HTML("#tblNeedsScrolling",{headers:true})');
        };

        //............................Exel export Method.....................//

        $scope.currentPageStores = {};
        $scope.open = function () {
            debugger;
            console.log("Modal opened customer");
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModalContent.html",
                    controller: "CustomerInstancecl", resolve: { customer: function () { return $scope.items } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.currentPageStores.push(selectedItem);
                },
                function () {
                    console.log("Cancel Condintion");
                    $log.info("Modal dismissed at: " + new Date)
                })
        };

        $scope.customercategorys = {};
        $scope.opendelete = function (data, $index) {
            debugger;
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myCustomerModaldelete.html",
                    controller: "ModalInstanceCtrldeleteCustomer", resolve: { customer: function () { return data } }
                }), modalInstance.result.then(function (selectedItem) {

                    $scope.data.splice($index, 1);
                    $scope.tableParams.reload();
                },
                function () {
                })
        };

        $scope.edit = function (item) {
            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModalContentPut.html",
                    controller: "CustomerInstancecl", resolve: { customer: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.customers.push(selectedItem);
                    _.find($scope.customers, function (customer) {
                        if (customer.id == selectedItem.id) {
                            customer = selectedItem;
                        }
                    });
                    $scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                    $scope.selected = selectedItem;
                },
                function () {
                })
        };

        $scope.childCustomer = function (item) {

            var modalInstance;
            modalInstance = $modal.open(
                {
                    templateUrl: "myModalContentChildCustomer.html",
                    controller: "CustomerInstancecl", resolve: { customer: function () { return item } }
                }), modalInstance.result.then(function (selectedItem) {
                    $scope.customers.push(selectedItem);
                    _.find($scope.customers, function (customer) {
                        if (customer.id == selectedItem.id) {
                            customer = selectedItem;
                        }
                    });
                    $scope.customers = _.sortBy($scope.customers, 'CustomerId').reverse();
                    $scope.selected = selectedItem;
                },
                function () {
                })
        };


        $scope.customers = [];
        $scope.getallcustomers = function () {
            customerService.getcustomers().then(function (results) {
                $scope.customers = results.data;
                $scope.callmethod();
            }, function (error) {
            });

        }
        $scope.getallcustomers();

        //Pagination and 
        $scope.callmethod = function () {
            debugger;
            var init;
            return $scope.stores = $scope.customers,

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
        //search fn
        $scope.dataforsearch = { Cityid: "", mobile: "", datefrom: "", dateto: "", skcode: "" };
        $scope.Search = function (data) {

            var f = $('input[name=daterangepicker_start]');
            var g = $('input[name=daterangepicker_end]');

            if (data == undefined) {
                alert("Please select one parameter");
                return;
            }


            $scope.dataforsearch.Cityid = data.Cityid;
            $scope.dataforsearch.mobile = data.mobile;
            $scope.dataforsearch.Cityid = data.Cityid;
            $scope.dataforsearch.skcode = data.skcode;
            if (!$('#dat').val()) {
                $scope.dataforsearch.datefrom = '';
                $scope.dataforsearch.dateto = '';
            }
            else {
                $scope.dataforsearch.datefrom = f.val();
                $scope.dataforsearch.dateto = g.val();
            }



            customerService.getfiltereddetails($scope.dataforsearch).then(function (results) {
                $scope.customers = results.data;
                if ($scope.customers.length > 0) {

                    $scope.data = $scope.customers;

                    $scope.allcusts = true;
                    $scope.tableParams = new ngTableParams({
                        page: 1,
                        count: 100,
                        ngTableParams: $scope.customers
                    }, {
                        total: $scope.data.length,
                        getData: function ($defer, params) {
                            var orderedData = params.sorting() ? $filter('orderBy')($scope.data, params.orderBy()) : $scope.data;
                            orderedData = params.filter() ?
                                    $filter('filter')(orderedData, params.filter()) :
                                    orderedData;
                            $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                        }
                    });
                }
                else { alert("No customers found for search"); }
            });
        }
    }
    else {
        window.location = "#/404";
    }
}]);


app.controller("CustomerInstancecl", ["$scope", "$filter", '$http', "$modalInstance", 'customer', 'ngAuthSettings', 'WarehouseService', function
    ($scope, $filter, $http, $modalInstance, customer, ngAuthSettings, WarehouseService) {

    ////for image
    $scope.uploadedfileName = '';
    $scope.upload = function (files) {
        debugger;
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


    debugger;
    $scope.CustWarehouse = [];
    $scope.CustWarehouse = function (CustomerId) {
        var url = serviceBase + 'api/Customers/CustWarehouseCustomer?CustomerId=' + CustomerId;
        $http.get(url)
        .success(function (data) {
            debugger;
            $scope.CustWarehouse = data;

        });
    }

    //Get PWarehouse from Pwarehouse table to fill dropdown
    $scope.warehouse = [];
    $scope.GetWarehouses = function () {
        debugger;
        var url = serviceBase + 'api/Customers/GetPWarehouse';
        $http.get(url)
        .success(function (data) {
            $scope.warehouse = data;
            console.log("$scope.getWarehouse", $scope.warehouse);
        });
    }
    $scope.GetWarehouses();


    if (customer) {
        debugger;
        var warehouseId = customer.PWarehouse.PWarehouseId;
        customer.PWarehouseId = warehouseId;
        $scope.CustomerData = customer;
    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    $scope.CheckCustomerMobile = function (Mobile) {
        var url = serviceBase + "api/Customers/Mobile?Mobile=" + Mobile;
        $http.get(url)
        .success(function (data) {
            if (data == 'null') {
            }
            else {
                alert("Mobile Number Is already Exist");
                //$scope.CustomerData.Mobile = '';
            }
            console.log(data);
        });
    }
    $scope.CheckCustomerEmail = function (Email) {
        var url = serviceBase + "api/Customers/Email?Email=" + Email;
        $http.get(url)
        .success(function (data) {
            if (data == 'null') {
            }
            else {
                alert("Email Id Is already Exist");
                //$scope.CustomerData.Emailid = '';
            }
            console.log(data);
        });
    }


    //Add Cusotmer 
    $scope.AddCustomer = function (data) {
        debugger;
        var abs = $('#Customer-mobile').val().length;
        if (data.Name == null || data.Name == "") {
            alert('Please Enter Name ');
        }
        else if (data.Mobile == null) {
            alert('Please Enter Mobile Number ');
        }
        else if (abs < 10 || abs > 10) {

            alert('Please Enter Mobile Number in 10 digit ');
        }
        else if (data.Address == null || data.Address == "") {
            alert('Please Enter Address');
        }
        else if (data.PWarehouseId == null || data.PWarehouseId == 0) {
            alert('Please Select Warehouse');
        }
        else {
            debugger;
            $scope.CustomerData = data;
            var PhotoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
            $scope.CustomerData.PhotoUrl = PhotoUrl;
            console.log($scope.CustomerData.PhotoUrl);
            var url = serviceBase + "api/customers/AddCustomer";
            var dataToPost = {
                PhotoUrl: $scope.CustomerData.PhotoUrl,
                Name: $scope.CustomerData.Name,
                Mobile: $scope.CustomerData.Mobile,
                PWarehouseId: $scope.CustomerData.PWarehouseId,
                EmailId: $scope.CustomerData.EmailId,
                Active: $scope.CustomerData.Active,
                Address: $scope.CustomerData.Address,
                Mobile1: $scope.CustomerData.Mobile1,
                Mobile2: $scope.CustomerData.Mobile2,
                Mobile3: $scope.CustomerData.Mobile3,
                Comment: $scope.CustomerData.Comment,
                GPSLocation: $scope.CustomerData.GPSLocation,
                LandMard: $scope.CustomerData.LandMard,
                BillingName: $scope.CustomerData.BillingName
            };
            debugger;
            $http.post(url, dataToPost)
            .success(function (data) {
                debugger;
                //Passing Alert message by cs 
                if (data.Message == "Error") {
                    alert("Customer Not Added!Some Error Occure!")
                    $modalInstance.close(data);
                }
                    //Passing Alert message by cs 
                else if (data.Message == "Sucessfully") {
                    debugger;
                    alert("Customer Added Sucessfully!")
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

    //Update Cusotmer
    $scope.PutCustomer = function (data) {
        debugger;
        if (data.Name == null || data.Name == "") {
            alert('Please Enter Name ');
        }
        else if (data.Address == null || data.Address == "") {
            alert('Please Enter Address');
        }
        else if (data.PWarehouseId == null || data.PWarehouseId == 0) {
            alert('Please Select Warehouse');
        }
        else {
            var PhotoUrl = serviceBase + "../../UploadedLogos/" + $scope.uploadedfileName;
            $scope.CustomerData.PhotoUrl = PhotoUrl;
            console.log($scope.CustomerData.PhotoUrl);
            var url = serviceBase + "api/customers";
            var dataToPost = {
                CustomerLinkId: $scope.CustomerData.CustomerLinkId,
                PhotoUrl: $scope.CustomerData.PhotoUrl,
                Name: $scope.CustomerData.Name,
                PWarehouseId: $scope.CustomerData.PWarehouseId,
                EmailId: $scope.CustomerData.EmailId,
                Active: $scope.CustomerData.Active,
                Address: $scope.CustomerData.Address,
                Mobile1: $scope.CustomerData.Mobile1,
                Mobile2: $scope.CustomerData.Mobile2,
                Mobile3: $scope.CustomerData.Mobile3,
                Comment: $scope.CustomerData.Comment,
                GPSLocation: $scope.CustomerData.GPSLocation,
                LandMard: $scope.CustomerData.LandMard,
                BillingName: $scope.CustomerData.BillingName
            };
            $http.put(url, dataToPost)
            .success(function (data) {
                console.log("Error Got Here");
                console.log(data);
                //Passing Alert message by cs 
                if (data.Message == "Error") {
                    alert("Customer Not Updated!Some Error Occure!")
                    $modalInstance.close(data);
                }

                    //Passing Alert message by cs 
                else if (data.Message == "Sucessfully") {
                    alert("Customer Updated Sucessfully!")
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
             })
        }
    };

    //Add ChildCustomer
    $scope.AddChildCustomer = function (data) {
        debugger;
        $scope.CustomerData = {};
        if (data) {
            debugger;
            var customerId = data.Customer.CustomerId;
            data.CustomerId = customerId;
            $scope.CustomerData = data;
        }
        if (customer) {
            debugger;
            $scope.CustomerData = customer;
        }
        var abs = $('#Customer-mobile').val().length;
        if (data.Address == null) {
            alert('Please Enter Address ');
        }
        else if (data.Mobile == null || data.Mobile == "") {

            alert('Please Enter Mobile Number ');
        }
        else if (abs < 10 || abs > 10) {

            alert('Please Enter Mobile Number in 10 digit ');
        }
        else {
            var url = serviceBase + "api/customers/ChildCustomers";
            var dataToPost = {
                ParentCustomer: { CustomerLinkId: $scope.CustomerData.CustomerLinkId },
                Mobile: $scope.CustomerData.Mobile,
                WarehouseId: $scope.CustomerData.WarehouseId,
                EmailId: $scope.CustomerData.EmailId,
                CustomerLinkId: $scope.CustomerData.CustomerLinkId,
                Name: $scope.CustomerData.Name,
                Address: $scope.CustomerData.Address,
                CustomerId: data.CustomerId
            };
            $http.post(url, dataToPost)
            .success(function (data) {
                alert("Added ChildCustomer");
                if (data.id == 0) {
                    $scope.gotErrors = true;
                    if (data[0].exception == "Already") {
                        $scope.AlreadyExist = true;
                    }
                }
                else {
                    $modalInstance.close(data);
                }
                window.location.reload();
            })
             .error(function (data) {
             })
        }
    };

}])
app.controller("ModalInstanceCtrldeleteCustomer", ["$scope", '$http', "$modalInstance", "customerService", 'ngAuthSettings', "customer", function ($scope, $http, $modalInstance, customerService, ngAuthSettings, customer) {
    debugger;
    $scope.CustomerData = {};
    if (customer) {
        $scope.CustomerData = customer;
    }
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
    $scope.deletecustomers = function (dataToPost, index) {
        customerService.deletecustomers(dataToPost).then(function (results) {
            $modalInstance.close(dataToPost);
        }, function (error) {
            alert(error.data.message);
        });
    }
}])

app.controller("ModalInstanceCtrlChildCase", ["$scope", '$http', "$modalInstance", "customerService", 'ngAuthSettings', 'child', function ($scope, $http, $modalInstance, customerService, ngAuthSettings, child) {
    debugger;
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
   $scope.ChildCustomerData = {};
    if (child) {
        $scope.ChildCustomerData = child;
    }
}])