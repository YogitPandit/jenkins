'use strict';
app.factory('CurrentStockService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("in Stock Service  ");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var StockServiceFactory = {};

    var _getstock = function () {
        console.log("get StockService ");

        return $http.get(serviceBase + 'api/CurrentStock').then(function (results) {
            return results;
        });
    };

    StockServiceFactory.getstock = _getstock;

    var _getEmptystock = function () {
        console.log("get StockService ");
        return $http.get(serviceBase + 'api/CurrentStock/GetEmptyStockItemForWeb').then(function (results) {
            return results;
        });
    };

    StockServiceFactory.getEmptystock = _getEmptystock;

    //var _putcitys = function () {

    //    return $http.put(serviceBase + 'api/City').then(function (results) {
    //        return results;
    //    });
    //};

    //CityServiceFactory.putcitys = _putcitys;




    //var _deletecitys = function (data) {
    //    console.log("Delete Calling");
    //    console.log(data.Cityid);


    //    return $http.delete(serviceBase + 'api/City/?id=' + data.Cityid).then(function (results) {
    //        return results;
    //    });
    //};

    //CityServiceFactory.deletecitys = _deletecitys;
   





    return StockServiceFactory;

}]);