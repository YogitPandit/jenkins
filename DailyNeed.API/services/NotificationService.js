'use strict';
app.factory('NotificationService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var NotificationServiceFactory = {};
    
    var _getNotification = function () {
        
        return $http.get(serviceBase + 'api/Notification').then(function (results) {
            return results;
        });
    };
    NotificationServiceFactory.getNotification = _getNotification;

    var _deleteNotification = function (data) {
        return $http.delete(serviceBase + 'api/Notification/?id=' + data.Id).then(function (results) {
            return results;
        });
    };

    NotificationServiceFactory.deleteNotification = _deleteNotification;
    return NotificationServiceFactory;

}]);