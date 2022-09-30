'use strict';
app.factory('PurchaseODetailsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("details");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var PurchaseODetailsServiceFactory = {};

  
    var _getdetails = function () {
        console.log("serviceeee");
        alert("hiiiiii");
        return $http.get(serviceBase + 'api/PurchaseOrderDetail?recordtype=details').then(function (results) {
            return results;
        });
    };

    PurchaseODetailsServiceFactory.getdetails = _getdetails;

    var _getPODetalis = function (i) {
        return $http.get(serviceBase + 'api/PurchaseOrderDetail/?id=' + i).then(function (results) {
            console.log("getting PO Details");
            console.log(results);
            return results;
        });
    };

    PurchaseODetailsServiceFactory.getPODetalis = _getPODetalis;

    var _GetItemMaster = function (data) {
        console.log("Get ItemMaster.. calling");
        if (data.SupplierId) {
            
            return $http.get(serviceBase + 'api/itemMaster?type=Supplier&id=' + data.SupplierId + '&Wid=' + data.WarehouseId).then(function (results) {
                console.log("result ... ");               
                console.log(results.data);
                return results;
            });
        }
    };
    PurchaseODetailsServiceFactory.GetItemMaster = _GetItemMaster;

    return PurchaseODetailsServiceFactory;

}]);