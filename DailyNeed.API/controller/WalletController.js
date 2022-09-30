'use strict'
app.controller('WalletController', ['$scope', "$filter", "$http", "ngTableParams", '$modal', function ($scope, $filter, $http, ngTableParams, $modal) {
    var UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    if (UserRole.role == 'HQ Master login' && UserRole.Offer == 'True' || UserRole.role == 'WH Master login' && UserRole.Offer == 'True' || UserRole.role == 'HQ HR lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ Accounts Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Accounts Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Accounts Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ CS Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ CS EXECUTIVE' && UserRole.Offer == 'True' || UserRole.role == 'HQ CS LEAD' && UserRole.Offer == 'True' || UserRole.role == 'HQ Growth Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Growth Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Growth Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ HR associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ HR executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ IC Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ IC Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ IC Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ Marketing Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Marketing Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Marketing Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ MIS Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ MIS EXECUTIVE' && UserRole.Offer == 'True' || UserRole.role == 'HQ MIS LEAD' && UserRole.Offer == 'True' || UserRole.role == 'HQ Ops Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Ops Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Ops Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ Purchase Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Purchase Executive ' && UserRole.Offer == 'True' || UserRole.role == 'HQ Purchase Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sales Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sales Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sales Lead' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sourcing Associate' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sourcing Executive' && UserRole.Offer == 'True' || UserRole.role == 'HQ Sourcing Lead' && UserRole.Offer == 'True' || UserRole.role == 'WH Agents' && UserRole.Offer == 'True' || UserRole.role == 'WH cash manager' && UserRole.Offer == 'True' || UserRole.role == 'WH delivery planner' && UserRole.Offer == 'True' || UserRole.role == 'WH Ops associate' && UserRole.Offer == 'True' || UserRole.role == 'WH Purchase executive' && UserRole.Offer == 'True' || UserRole.role == 'WH Super visor' && UserRole.Offer == 'True' || UserRole.role == 'WH Service lead' && UserRole.Offer == 'True' || UserRole.role == 'WH Sales lead' && UserRole.Offer == 'True' || UserRole.role == 'WH sales executives' && UserRole.Offer == 'True' || UserRole.role == 'WH sales associates' && UserRole.Offer == 'True') {
    $scope.wallets = [];
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
    $scope.getWalletData = function () { 
        var url = serviceBase + "api/wallet";
        $http.get(url).success(function (response) {
            
            $scope.wallets = response;
        });
    };

    // For export data
    alasql.fn.myfmt = function (n) {
        return Number(n).toFixed(2);
    }
    $scope.exportData = function () {
        alasql('SELECT CustomerId,Skcode,ShopName,TotalAmount,CreatedDate INTO XLSX("WalletPointCustomer.xlsx",{headers:true}) FROM ?', [$scope.wallets]);
    };

    $scope.getWalletData();
   
    $scope.open = function () {
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myADDModal.html",
                controller: "WalletControllerAddController", resolve: { object: function () { return $scope.data } }
            }), modalInstance.result.then(function (selectedItem) {
            },
            function () {
            })
    };
    $scope.edit = function (wallets) {
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "myEDITModal.html",
                controller: "WalletControllerAddController", resolve: { object: function () { return wallets } }
            }), modalInstance.result.then(function (wallets) {
            },
            function () {
            })
    };
    $scope.openCashConvesion = function () {
        var modalInstance;
        modalInstance = $modal.open(
            {
                templateUrl: "cashConversionModal.html",
                controller: "WalletConversionController", resolve: { object: function () { return $scope.data } }
            }), modalInstance.result.then(function (selectedItem) {
            },
            function () {
            })
    };



    //-----for popupdial-----
    // new pagination 
    $scope.pageno = 1; //initialize page no to 1
    $scope.total_count = 0;

    $scope.itemsPerPage = 30;  //this could be a dynamic value from a drop down

    $scope.numPerPageOpt = [30, 60, 90, 100];  //dropdown options for no. of Items per page

    $scope.onNumPerPageChange = function () {
        $scope.itemsPerPage = $scope.selected;

    }
    $scope.selected = $scope.numPerPageOpt[0];  //for Html page dropdown

    $scope.$on('$viewContentLoaded', function () {
        // $scope.WalletHistory($scope.pageno);
    });

    $scope.CustomerId = 0;
    $scope.WalletHistorys = function (data) {
       
        $scope.CustomerId = data.CustomerId;
        $scope.WalletHistory($scope.pageno);
    }

    $scope.WalletHistory = function (pageno) {
        
        $scope.OldStockData = [];
        var url = serviceBase + "api/wallet" + "?CustomerId=" + $scope.CustomerId + "&list=" + $scope.itemsPerPage + "&page=" + pageno;
        $http.get(url).success(function (response) {
            
            $scope.OldStockData = response.ordermaster;
            $scope.total_count = response.total_count;
        })
      .error(function (data) {
      })
    }
}
else
{
        window.location = "#/404";
}
}]);
app.controller("WalletControllerAddController", ["$scope", '$http', 'ngAuthSettings', "$modalInstance", "object", '$modal', 'customerService', function ($scope, $http, ngAuthSettings, $modalInstance, object, $modal, customerService) {
    $scope.saveData = {};
    if (object) { $scope.saveData = object; }
    
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },
 
    $scope.customer = {};   
    $scope.GetCustomer = function (skcode) {
        $scope.customer = {};
        customerService.getcustomerBySkcode(skcode).then(function (results) {
            if (results.data != null) { 
                $scope.customer = results.data;
                alert("SHOP NAME:" + $scope.customer.ShopName + " & MOBILE:" + $scope.customer.Mobile);
            }
            else {
                $scope.customer = null;
                alert("Enter Correct Skcode:");
            }
        }, function (error) {
        });
    };
    $scope.AddWallet = function () {
        
        if (object) {
            $scope.customer.CustomerId = $scope.saveData.CustomerId
        }
        var dataToPost = {
            CustomerId: $scope.customer.CustomerId,
            CreditAmount: $scope.saveData.CreditAmount
        }
        console.log(dataToPost);
        var url = serviceBase + "api/wallet";
        $http.post(url, dataToPost).success(function (data) {
            if (data != null && data != "null") {
                alert("Wallet Amount Added Successfully... :-)");
                $modalInstance.close();
                location.reload();
            }
            else {
                alert("got some error... :-)");
                $modalInstance.close();
            }
        })
     .error(function (data) {
     })
    };    
}]);
app.controller("WalletConversionController", ["$scope", '$http', 'ngAuthSettings', "$modalInstance", '$modal', function ($scope, $http, ngAuthSettings, $modalInstance, $modal) {
    $scope.ok = function () { $modalInstance.close(); },
    $scope.cancel = function () { $modalInstance.dismiss('canceled'); },

    $scope.pointData = [];

    $http.get(serviceBase + "api/wallet/cash").success(function (data) {
        if (data != null && data != "null") {
            $scope.pointData = data;
        }
    })
    $scope.AddcashData = function () {
        var dataToPost = {
            Id: $scope.pointData.Id,
            point: $scope.pointData.point,
            rupee: $scope.pointData.rupee
        }
        console.log(dataToPost);
        var url = serviceBase + "api/wallet/cash";
        $http.post(url, dataToPost).success(function (data) {
            if (data != null && data != "null") {
                alert("margin point Added Successfully... :-)");
                $modalInstance.close();
            }
            else {
                alert("got some error... :-)");
                $modalInstance.close();
            }
        })
     .error(function (data) {
     })
    };
}]);
