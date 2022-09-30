'use strict';
app.factory('AssetsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var AssetsServiceFactory = {};

    var _getassetss = function () {

        return $http.get(serviceBase + 'api/Assetss').then(function (results) {
            return results;
        });
    };

    AssetsServiceFactory.getassetss = _getassetss;



    var _putassetss = function () {

        return $http.put(serviceBase + 'api/Assetss').then(function (results) {
            return results;
        });
    };

    AssetsServiceFactory.putassetss = _putassetss;




    var _deleteassetss = function (data) {
        console.log("Delete Calling");
        console.log(data.AssetId);


        return $http.delete(serviceBase + 'api/Assetss/?id=' + data.AssetId).then(function (results) {
            return results;
        });
    };

    AssetsServiceFactory.deleteassetss = _deleteassetss;
    AssetsServiceFactory.getassetss = _getassetss;





    return AssetsServiceFactory;

}]);