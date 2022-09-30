'use strict';
app.factory('OfferService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var OfferServiceFactory = {};

    var _getoffer = function () {

        return $http.get(serviceBase + 'api/offer').then(function (results) {
            return results;
        });
    };

    OfferServiceFactory.getoffer = _getoffer;



    var _putoffer = function () {

        return $http.put(serviceBase + 'api/offer').then(function (results) {
            return results;
        });
    };

    OfferServiceFactory.putoffer = _putoffer;




    var _deleteoffer = function (data) {

        return $http.delete(serviceBase + 'api/offer/?id=' + data.OfferId).then(function (results) {
            return results;
        });
    };

    OfferServiceFactory.deleteoffer = _deleteoffer;

    return OfferServiceFactory;

}]);