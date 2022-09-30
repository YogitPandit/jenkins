'use strict';
app.factory('InvoiceProductService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("prd sevice called");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var InvoiceProductServiceFactory = {};

    //var _getClientprojects = function () {

    //    return $http.get(serviceBase + 'api/ClientProject').then(function (results) {
    //        return results;
    //    });
    //};

    //InvoiceProductServiceFactory.getClientprojects = _getClientprojects;


    return InvoiceProductServiceFactory;

}]);