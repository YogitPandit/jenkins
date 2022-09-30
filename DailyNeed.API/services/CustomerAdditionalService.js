'use strict';
app.factory('CustomerAdditionalService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var CustomerAdditionalFactory = {};

    var _getcustomersAdditional = function () {
        debugger;
        return $http.get(serviceBase + 'api/CustomerAdditional/GetCustomer').then(function (results) {
            return results;
        });
    };
    CustomerAdditionalFactory.getcustomersAdditional = _getcustomersAdditional;

    var _putcustomersAdditional = function () {

        return $http.put(serviceBase + 'api/CustomerAdditional').then(function (results) {
            return results;
        });
    };
    CustomerAdditionalFactory.putcustomersAdditional = _putcustomersAdditional;


    var _deletecustomersAdditional = function (data) {
        debugger;
        return $http.delete(serviceBase + 'api/CustomerAdditional/Delete?id=' + data.ID).then(function (results) {
            return results;
        });
    };
    CustomerAdditionalFactory.deletecustomersAdditional = _deletecustomersAdditional;

    CustomerAdditionalFactory.getcustomersAdditional = _getcustomersAdditional;
    return CustomerAdditionalFactory;

}]);