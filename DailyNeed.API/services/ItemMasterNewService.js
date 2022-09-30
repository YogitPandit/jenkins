'use strict';
app.factory('ItemMasterNewService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    debugger;
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var itemMasterFactory = {};

  
    var _deleteitemMaster = function (data) {
        debugger;
        console.log("Delete Calling");
        console.log(data.ItemId);

        return $http.delete(serviceBase + 'api/itemMasterNew/?id=' + data.ItemId).then(function (results) {
            return results;
        });
    };

    itemMasterFactory.deleteitemMaster = _deleteitemMaster;

    return itemMasterFactory;

}]);