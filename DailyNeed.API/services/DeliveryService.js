'use strict';
app.factory('DeliveryService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("in city ");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var DBoyDeliveryService = {};

    var _getdboys = function () {
           return $http.get(serviceBase + 'api/DeliveryOrder').then(function (results) {
            return results;
        });

    };

    var _getordersbyId = function (mob) {
        return $http.get(serviceBase + 'api/DeliveryOrder?mob=' + mob).then(function (results) {
            return results;
        })
    }

    DBoyDeliveryService.getordersbyId = _getordersbyId;

    DBoyDeliveryService.getdboys = _getdboys;
    var _getDBoyCurrencyID = function (PeopleID) {
        return $http.get(serviceBase + 'api/CurrencySettle?PeopleID=' + PeopleID).then(function (results) {
            return results;
        })
    }

    DBoyDeliveryService.getDBoyCurrencyID = _getDBoyCurrencyID;




    return DBoyDeliveryService;

}]);