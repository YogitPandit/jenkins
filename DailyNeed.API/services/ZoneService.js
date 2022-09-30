'use strict';
app.factory('ZoneService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("insubcat service");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ZoneServiceFactory = {};

    var _getzones = function () {

        return $http.get(serviceBase + 'api/Zone').then(function (results) {
            return results;
        });
    };

    ZoneServiceFactory.getzones = _getzones;
    //============================================================================================================
    //var _getWarhouseSubCategory = function (data) {
    //    console.log("Service");
    //    console.log("get Filter Warhouse sub category function in warehouse service");
    //    console.log("ID");
    //    console.log(data.Warehouseid);

    //    return $http.get(serviceBase + 'api/Zone', {
    //        params: {
    //            recordtype: "warehouse",
    //            whid: data.Warehouseid
    //        }
    //    }).success(function (data, status) {
    //        console.log(data);
    //        return data;
    //    });
    //};

    //ZoneServiceFactory.getWarhouseSubCategory = _getWarhouseSubCategory;

    ////===================================================================================================
    //var _putsubcategorys = function () {

    //    return $http.put(serviceBase + 'api/Zone').then(function (results) {
    //        return results;
    //    });
    //};

    //ZoneServiceFactory.putsubcategorys = _putsubcategorys;

    //var _deletesubcategorys = function (data) {
    var _deletezones = function (data) {
        console.log("Delete Calling");
        console.log(data.Zoneid);

        return $http.delete(serviceBase + 'api/Zone/?id=' + data.Zoneid).then(function (results) {
            return results;
        });
    };

    //ZoneServiceFactory.deletesubcategorys = _deletesubcategorys;
    ZoneServiceFactory.deletezones = _deletezones;
    ZoneServiceFactory.getzones = _getzones;

    return ZoneServiceFactory;

}]);