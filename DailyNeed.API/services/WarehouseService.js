'use strict';
app.factory('WarehouseService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var WarehouseServiceFactory = {};

    var _getwarehouse = function () {
        debugger;
        console.log("in warehouse service")

        return $http.get(serviceBase + 'api/Warehouse').then(function (results) {
            
            return results;
        });
    };

    WarehouseServiceFactory.getwarehouse = _getwarehouse;

    var _getwarehousedistinctstates = function () {
        console.log("get distinct states function in warehouse service");
       return $http.get(serviceBase + 'api/Warehouse', {
        params: {
            recordtype: "states"
        }
    }).success(function (data, status) {
         return data
     });
    };

    WarehouseServiceFactory.getwarehousedistinctstates = _getwarehousedistinctstates;

    

    var _putwarehouse = function () {

        return $http.put(serviceBase + 'api/Warehouse').then(function (results) {
            return results;
        });
    };

    WarehouseServiceFactory.putwarehouse = _putwarehouse;




    var _deletewarehouse = function (data) {
        console.log("Delete Calling");
        console.log(data.Warehouseid);
        return $http.delete(serviceBase + 'api/Warehouse/?id=' + data.WarehouseId).then(function (results) {
            return results;
        });
    };

    WarehouseServiceFactory.deletewarehouse = _deletewarehouse;
    WarehouseServiceFactory.getwarehouse = _getwarehouse;





    return WarehouseServiceFactory;

}]);