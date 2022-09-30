'use strict';
app.controller('headerController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {
    console.log("user name in header");
    console.log(authService.authentication.userName);
    $scope.userName = authService.authentication.userName;

  

}]);