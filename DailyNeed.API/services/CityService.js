'use strict';
app.factory('CityService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("in city ");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var CityServiceFactory = {};

    var _getcitys = function () {
        console.log("get city");
        return $http.get(serviceBase + 'api/City').then(function (results) {
            return results;
        });
    };

    CityServiceFactory.getcitys = _getcitys;



    var _putcitys = function () {

        return $http.put(serviceBase + 'api/City').then(function (results) {
            return results;
        });
    };

    CityServiceFactory.putcitys = _putcitys;




    var _deletecitys = function (data) {
        console.log("Delete Calling");
        console.log(data.Cityid);


        return $http.delete(serviceBase + 'api/City/?id=' + data.Cityid).then(function (results) {
            return results;
        });
    };

    CityServiceFactory.deletecitys = _deletecitys;
    CityServiceFactory.getcitys = _getcitys;





    return CityServiceFactory;

}]);