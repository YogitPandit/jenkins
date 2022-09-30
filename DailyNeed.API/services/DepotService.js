'use strict';
app.factory('DepotService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("Entered Services");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var DepotServiceFactory = {};

    //Get Category Method
    var _getdepots = function () {

        return $http.get(serviceBase + 'api/Depot').then(function (results) {
            return results;
        });
    };

    DepotServiceFactory.getdepots = _getdepots;

    //Get Category By warehouse Method
    var _getWarhouseCategory = function (data) {
        console.log("Service");
        console.log("get Filter Warhouse category function in warehouse service");
        console.log("ID");
        console.log(data.Warehouseid);

        return $http.get(serviceBase + 'api/Depot', {
            params: {
                recordtype: "warehouse",
                whid: data.Warehouseid
            }
        }).success(function (data, status) {
            console.log(data);
            return data;
        });
    };

    DepotServiceFactory.getWarhouseCategory = _getWarhouseCategory;

    var _getWarhouseCategorybyid = function (data) {
        console.log("Service");
        console.log("get Filter Warhouse categor by id function in warehouse service");
        console.log("ID");
        console.log(data.Warehouseid);
        console.log("WHID");
        console.log(data.WhCategoryid);

        return $http.get(serviceBase + 'api/Depot', {
            params: {
                recordtype: "warehouse",
                whid: data.Warehouseid,
                whcatid: data.WhCategoryid
            }
        }).success(function (data, status) {
            console.log(data);
            return data;
        });
    };

    //CategoryServiceFactory.getWarhouseCategorybyid = _getWarhouseCategorybyid;

    //Update Category Method/PUT Method
    var _putcategorys = function () {

        return $http.put(serviceBase + 'api/Depot').then(function (results) {
            return results;

        });
    };

    DepotServiceFactory.putcategorys = _putcategorys;

    //Delete Category Method/Delete Method
    var _deleteDepots = function (data) {
        console.log("Delete Calling");
        console.log(data.Depotid);

        return $http.delete(serviceBase + 'api/Depot/?id=' + data.Depotid).then(function (results) {
            console.log(results);
            return results;
        });
    };

    DepotServiceFactory.deleteDepots = _deleteDepots;
    DepotServiceFactory.getdepots = _getdepots;

    return DepotServiceFactory;

}]);