'use strict';
app.factory('itemMasterService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var itemMasterServiceFactory = {};

    //......................................................................//
    var _getorders = function () {

        return $http.get(serviceBase + 'api/itemMaster').then(function (results) {
            return results;
        });
    };
    itemMasterServiceFactory.getorders = _getorders;

    var _GetitemMaster = function () {
        console.log("in item Master Service Factory")
        return $http.get(serviceBase + 'api/itemMaster').then(function (results) {
            return results;
        });
    };
    itemMasterServiceFactory.GetitemMaster = _GetitemMaster;

    var _getfiltereditemmaster = function (data) {
        
        console.log("in demand detail service item master");
        console.log(data);
        return $http.get(serviceBase + 'api/itemMaster?Cityid=' + data.Cityid + '&&' + 'Categoryid=' + data.Categoryid + '&&' + 'SubCategoryId=' + data.SubCategoryId + '&&' + 'SubsubCategoryid=' + data.SubsubCategoryid).then(function (results) {
            return results;
        });
    };
    itemMasterServiceFactory.getfiltereditemmaster = _getfiltereditemmaster;

    

    var _PutitemMaster = function () {

        return $http.put(serviceBase + 'api/itemMaster').then(function (results) {
            return results;
        });
    };

    itemMasterServiceFactory.PutitemMaster = _PutitemMaster;

    var _deleteitemMaster = function (data) {
        console.log("Delete Calling");
        console.log(data.ItemId);
        
        return $http.delete(serviceBase + 'api/itemMaster/?id=' + data.Id).then(function (results) {
            return results;
        });
    };

    itemMasterServiceFactory.deleteitemMaster = _deleteitemMaster;
    itemMasterServiceFactory.GetitemMaster = _GetitemMaster;

    return itemMasterServiceFactory;

}]);