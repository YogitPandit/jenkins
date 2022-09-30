'use strict';
app.factory('ClientProjectService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ClientProjectServiceFactory = {};

    var _getClientprojects = function () {

        return $http.get(serviceBase + 'api/ClientProject').then(function (results) {
            return results;
        });
    };

    ClientProjectServiceFactory.getClientprojects = _getClientprojects;


    return ClientProjectServiceFactory;

}]);