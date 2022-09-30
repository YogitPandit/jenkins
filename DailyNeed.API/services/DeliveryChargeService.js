'use strict';
app.factory('DeliveryChargeService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("Entered Services");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var DeliveryChargeServiceFactory = {};

    var _getDeliveryData = function () {
        return $http.get(serviceBase + 'api/deliverycharge').success(function (data, status) {
            return data;
        });
    };
    DeliveryChargeServiceFactory.getDeliveryData = _getDeliveryData;

    var _getWarhouse = function (data) {
        return $http.get(serviceBase + 'api/Warehouse').success(function (data, status) {
            return data;
        });
    };
    DeliveryChargeServiceFactory.getWarhouse = _getWarhouse;

    var _getCluster = function (data) {
        return $http.get(serviceBase + 'api/cluster/all').success(function (data, status) {
            return data;
        });
    };
    DeliveryChargeServiceFactory.getCluster = _getCluster;
    DeliveryChargeServiceFactory.getcategorys = _getDeliveryData;

    return DeliveryChargeServiceFactory;

}]);