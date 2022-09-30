'use strict';
app.factory('SearchPOService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("poservive");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    
    var SearchPOServiceFactory = {};
    var dataTosave = [];
    var dataTosave1 = [];

    var _getPorders = function () {

        return $http.get(serviceBase + 'api/PurchaseOrderMaster').then(function (results) {
            console.log("poservive");
            console.log(results);
            return results;
        });
    };
    SearchPOServiceFactory.getPorders = _getPorders;

    var _save = function (data) {
        console.log("brought");
        console.log(data);
        dataTosave = data;
        console.log(dataTosave);
        window.location = "#/PurchaseOrderdetails";
      
    };
    SearchPOServiceFactory.save = _save;


    var _getWarehousebyid = function (id) {
        
        return $http.get(serviceBase + 'api/PurchaseOrderMaster?id=' + id).then(function (results) {
            return results;
        });
    };

    SearchPOServiceFactory.getWarehousebyid = _getWarehousebyid;
    var _view = function (data) {
        console.log("view section");
        console.log(data);        
        dataTosave = data;
        console.log(dataTosave);
        window.location = "#/PurchaseInvoice";
        console.log("dataTosave view section");
    };

    SearchPOServiceFactory.view = _view;

    var _goodsrecived = function (data) {
        
        console.log("view section");
        dataTosave = data;
        console.log(dataTosave);
        window.location = "#/goodsrecived";
        console.log("dataTosave view section");
    };
    SearchPOServiceFactory.goodsrecived = _goodsrecived;

    var _IRrecived = function (data) {
        console.log("view section");
        console.log(data);
        dataTosave = data;
        console.log(dataTosave);
        window.location = "#/IR";
        console.log("dataTosave view section");
    };

    SearchPOServiceFactory.IRrecived = _IRrecived;

    var _getDeatil = function () {
        
        //alert("in getting data");
        return dataTosave;

    };
    SearchPOServiceFactory.getDeatil = _getDeatil;
    //OrderMasterServiceFactory.putinvoice = _putinvoice;
    //var _deletetasks = function (data) {
    //    console.log("Delete Calling");
    //    console.log(data.Id);
    //    return $http.delete(serviceBase + 'api/OrderMaster/?id=' + data.Id).then(function (results) {
    //        return results;
    //    });
    //};
    //OrderMasterServiceFactory.deletetasks = _deletetasks;
    //OrderMasterServiceFactory.gettasks = _gettasks;

    return SearchPOServiceFactory;

}]);