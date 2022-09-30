'use strict';
app.factory('MessageService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var MessageServiceFactory = {};

    var _getMessages = function () {
        return $http.get(serviceBase + 'api/MessageApi').then(function (results) {
            return results;
        });
    };
    MessageServiceFactory.getMessages = _getMessages;

    var _deleteMessage = function (data) {
        return $http.delete(serviceBase + 'api/MessageApi/?id=' + data.MessageId).then(function (results) {
            return results;
        });
    };

    MessageServiceFactory.deleteMessage = _deleteMessage;
    return MessageServiceFactory;

}]);