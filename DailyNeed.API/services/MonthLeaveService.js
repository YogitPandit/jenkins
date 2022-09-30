'use strict';
app.factory('MonthLeaveService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
    console.log("in city ");
    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var MonthLeaveServiceFactory = {};

    var _getMonthLeave = function () {
        console.log("get MonthLeave");
        return $http.get(serviceBase + 'api/MonthLeaves/GetAllMonthLeaves').then(function (results) {
            return results;
        });
    };

    MonthLeaveServiceFactory.getMonthLeave = _getMonthLeave;



    var _putcitys = function () {

        return $http.put(serviceBase + 'api/City').then(function (results) {
            return results;
        });
    };

    MonthLeaveServiceFactory.putcitys = _putcitys;




    var _deleteMonthLeave = function (data) {
        debugger;
        console.log("Delete Calling");
        console.log(data.Id);

        return $http.delete(serviceBase + 'api/MonthLeaves/?Id=' + data.Id).then(function (results) {
            return results;
        });
    };

    MonthLeaveServiceFactory.deleteMonthLeave = _deleteMonthLeave;
    MonthLeaveServiceFactory.getMonthLeave = _getMonthLeave;





    return MonthLeaveServiceFactory;

}]);