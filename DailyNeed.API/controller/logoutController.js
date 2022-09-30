'use strict';
app.controller('logoutController', ['$scope', '$location', 'authService', 'ngAuthSettings', function ($scope, $location, $window, authService, ngAuthSettings, $localStorage) {


         localStorage.clear();
       
        $location.path('/pages/signin');
        
}]);
