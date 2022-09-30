'use strict';
app.factory('ProviderLinkService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var CustomerPriceServicesFactory = {};

    var _getcustomersPriceServices = function () {

        return $http.get(serviceBase + 'api/CustomerPriceService/GetCustomer').then(function (results) {
            return results;
        });
    };
    CustomerPriceServicesFactory.getcustomersPriceServices = _getcustomersPriceServices;

    var _putcustomersPriceServices = function () {

        return $http.put(serviceBase + 'api/CustomerPriceService').then(function (results) {
            return results;
        });
    };
    CustomerPriceServicesFactory.putcustomersPriceServices = _putcustomersPriceServices;


    var _deletecustomersPriceServices = function (data) {

        return $http.delete(serviceBase + 'api/CustomerPriceService/?id=' + data.CustomerId).then(function (results) {
            return results;
        });
    };
    CustomerPriceServicesFactory.deletecustomersPriceServices = _deletecustomersPriceServices;

    CustomerPriceServicesFactory.getcustomersPriceServices = _getcustomersPriceServices;
    return CustomerPriceServicesFactory;

}]);