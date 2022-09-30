'use strict';
app.factory('SalesService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("in role service");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var SalesServiceFactory = {};
    var dataTosave = [];
    var _save = function (data) {
        console.log("cheque");
        console.log(data);
        dataTosave = data;
        
        console.log(dataTosave);
        window.location = "#/SaleCheqBounce";
    };
    var _getDeatil = function () {
      // alert("in getting data");
        return dataTosave;
    };
    SalesServiceFactory.getDeatil = _getDeatil;
    SalesServiceFactory.save = _save;

    return SalesServiceFactory;

}]);