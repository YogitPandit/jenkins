'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', '$http', function ($scope, $location, $timeout, authService, $http) {
    $scope.pageClass = 'page-about';


    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        userName: "",
        email: "",
        password: "",
        confirmPassword: ""
    };
    $scope.validateFreeEmail = function (email) {
        debugger;
        var reg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/
        var regNumber = /^\d{10}$/
        if (reg.test(email) || (regNumber.test(email))) {
            return true;
        }
        else {
            return false;
        }
        //var val = number.value
        //if (/^\d{10}$/.test(val)) {
        //    // value is ok, use it
        //} else {
        //    alert("Invalid number; must be ten digits")
        //    number.focus()
        //    return false
        //}
    }
    $scope.signUp = function () {

        var errors = [];
        if (!$scope.validateFreeEmail($scope.registration.email)) {
            errors.push("Not a valid company email address");
            $scope.message = "Failed to register user due to:" + errors.join(' ');
        } else {
            debugger;
            authService.saveRegistration($scope.registration).then(function (response) {
                debugger;
                console.log(response);
                $scope.savedSuccessfully = true;
                $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                startTimer();

            },
            function (response) {
                debugger;
                console.log(response);
                console.log(response.data.ModelState);
                for (var key in response.data.ModelState) {
                    console.log(key);
                    for (var i = 0; i < response.data.ModelState[key].length; i++) {
                        console.log(response.data.ModelState[key]);
                        errors.push(response.data.ModelState[key][i]);
                    }
                }
                $scope.message = "Failed to register user due to:" + errors.join(' ');
            });
        }
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/pages/signin');
        }, 2000);
    }

}]);