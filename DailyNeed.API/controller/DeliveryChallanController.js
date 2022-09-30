'use strict';
app.controller('DeliveryChallanController', ['$scope', "$filter", "$http", "ngTableParams", '$modal',
    function ($scope, $filter, $http, ngTableParams, $modal) {
        debugger;
        var UserRole = JSON.parse(localStorage.getItem('RolePerson'));

        $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

        console.log("Delivery Challan Controller reached");

        //Get all customer bases of provider
        $scope.allcusts = false;
        $scope.GetChallanList = [];
        $scope.ChallanList = function () {
            debugger;
            var url = serviceBase + 'api/DeliveryChallan/GetChallanList';
            $http.get(url)
            .success(function (data) {
                debugger;
                $scope.GetChallanList = data;
                $scope.allcusts = true;
            });
        }
        $scope.ChallanList();

        //Open Popup for show customer data
        $scope.ViewCustomerList = function (data) {
            debugger;
            $scope.getCustomer = [];
            var modalInstance;
            var url = serviceBase + "api/DeliveryChallan/GetCustomerList?cNumber=" + data;
            $http.get(url).success(function (results) {
                debugger;
                $scope.getCustomer = results;
                if ($scope.getCustomer != "") {
                    modalInstance = $modal.open(
                        {
                            templateUrl: "myModalCustomerList.html",
                            controller: "ModalInstanceCtrlList", resolve: { cust: function () { return $scope.getCustomer } }
                        })
                }
            })
             .error(function (data) {
                 console.log(data);
             })
        };

        //Getting all customer Month List for Monthaly Satetment 
        $scope.GetCustomerList = function () {
            debugger;
            var url = serviceBase + 'api/MonthlyBill';
            $http.get(url)
            .success(function (data) {
                $scope.getBillByName = data;
                console.log("$scope.getAllBill", $scope.getBillByName);
                if ($scope.BType == null) {
                    $scope.callmethod();
                }

            });
        }
        $scope.GetCustomerList();


        //Post Data For Monthly Satetment for All Table Data
        $scope.test = [];
        $scope.MonthBillList = function (data) {
            debugger;
            var url = serviceBase + 'api/DeliveryChallan/AddCustomerBillList';
            //Data Posting to controller by ui object
            var postdata = {
            }
            $http.post(url, postdata).success(function (data) {
                debugger;
                window.location.reload();
            });
        };

    }]);

app.controller("ModalInstanceCtrlList", ["$scope", '$http', "$modalInstance", 'ngAuthSettings', 'cust', function ($scope, $http, $modalInstance, ngAuthSettings, cust) {
    debugger;
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
   $scope.CustomerData = {};
    if (cust) {
        debugger;
        $scope.CustomerData = cust;
    }

    //Save Challan in PDF formate // save it in SavePDF folder 
    $scope.PDChallan = function (divName, data) {
        debugger;
        $scope.CustomerData1 = data;
        var bulkpdf = [];
        setTimeout(function () {
            debugger;
            var doc = new jsPDF();
            var specialElementHandlers = {
                '#editor': function (element, renderer) {
                    return true;
                }
            };
            var htmlValue = $('#' + divName).html();
            //doc.fromHTML($('#' + divName).html(), 15, 15, {
            //    'width': 170,
            //    'elementHandlers': specialElementHandlers
            //});
            debugger;
            var pdfcontant = doc.output();
            $scope.getsendemail = [];
            var modalInstance;
            var url = serviceBase + "/api/DeliveryChallan/PrintChallan?ChallanNo=" + data.ChallanNumber;
            //for (var i = 0; i < data.length; i++) {
            //    var pdf = data[i];
            //    var htmldata = {
            //        "ChallanId": pdf.ChallanId,
            //        "Name": pdf.CustomerLink.Name,
            //        "FinalQty": pdf.FinalQty,
            //        "ItemName": pdf.ItemName,
            //        "Comment": pdf.Comment,
            //        "Address": pdf.Address,
            //        "MobileNumber": pdf.MobileNumber,
            //        "Date":pdf.Date,
            //        //"html": $('#' + divName).html()
            //        "html": htmlValue
            //    };
            //    bulkpdf.push(htmldata);
            //}
            $http.post(url).success(function (results) {
                $scope.getsendemail = results;
                if ($scope.getsendemail != "") {
                    modalInstance = $modal.open(
                    {
                        templateUrl: "myModalSendEmail.html",
                        controller: "ModalInstanceCtrlStateCase",
                        resolve: {
                            email: function () {
                                return $scope.getsendemail
                            }
                        }
                    })
                }
            }).error(function (data) {
                console.log(data);
                alert('PDF Saved successfully!');
            })
        }, 8000);
    };

    $scope.EditItem = {};
    //Editing an existing record.
    $scope.Edit = function (index) {
        debugger;
        //Setting EditMode to TRUE makes the TextBoxes visible for the row.
        $scope.CustomerData[index].EditMode = true;

        //The original values are saved in the variable to handle Cancel case.
        $scope.EditItem.Qty = $scope.cust[index].Qty;
        $scope.EditItem.Comment = $scope.cust[index].Comment;
    };

    //Updating an existing record to database.
    $scope.Update = function (data) {
        debugger;
        var url = serviceBase + "api/DeliveryChallan/UpdateCustomer";
        debugger;
        //Setting EditMode to FALSE hides the TextBoxes for the row.
        var postdata = {
            Status: data.Status,
            ItemName: data.ItemName,
            ChallanNumber: data.ChallanNumber,
            ChallanId: data.ChallanId,
            Comment: data.Comment,
            FinalQty: data.FinalQty,
            CustomerLinkId:data.CustomerLink.CustomerLinkId
        }
        $http.put(url, postdata)
            .success(function (data) {
                window.location.reload();
            })
             .error(function (data) {
             })
        std.EditMode = false;
    };
}])