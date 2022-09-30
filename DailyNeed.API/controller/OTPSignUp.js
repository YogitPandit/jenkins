'use strict';
app.controller('OTPsignupController', ['$scope', '$location', '$timeout', 'authService', '$http', function ($scope, $location, $timeout, authService, $http) {
    debugger;

    $scope.pageClass = 'page-about';

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.Data = {
        MobileNumber: ""
    };

    $scope.OTPData = {
        VerifyOTP: ""
    };

    $scope.otp = function () {
        debugger;
        $scope.OTP = JSON.parse(localStorage.getItem('OTPStorage'));
    }

    //Verify Valid Mobile Number
    $scope.validateFreeEmail = function (mobile) {
        debugger;
        //var reg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/
        var regNumber = /^\d{10}$/
        if ((regNumber.test(mobile))) {
            return true;
        }
        else {
            return false;
        }
    }

    //Generate OTP
    $scope.OTPlogin = function () {
        debugger;
        var errors = [];
        if (!$scope.validateFreeEmail($scope.Data.MobileNumber)) {
            errors.push("Not a valid Mobile Number");
            $scope.message = "Failed to register user due to:" + errors.join(' ');
        } else {
            debugger;
            var m = $scope.Data.MobileNumber;
            debugger;
            var url = serviceBase + "api/OTP/Genotp?MobileNumber=" + m;
            //$http.post(serviceBase + "api/OTP/Genotp?MobileNumber=" + m).success(function (response) {
            $http.post(url)
                .success(function (data) {
                    debugger;
                    console.log(data);
                    if (data.OtpNo != null || data.OtpNo != "") {
                        debugger;
                        $scope.queryString = data.OtpNo;
                        localStorage.setItem('OTPStorage', JSON.stringify(data));
                        //$scope.otp();
                        $location.path('/pages/OTPVerify');
                    }
                    return data;
                }).error(function (data, status, headers, config) {
                    console.log("saved comment", data);
                    return data;
                });

            //authService.saveRegistration($scope.Data).then(function (response) {
            //    debugger;
            //    console.log(response);
            //    $scope.savedSuccessfully = true;
            //    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
            //    startTimer();

            //},
            //function (response) {
            //    debugger;
            //    console.log(response);
            //    console.log(response.data.ModelState);
            //    for (var key in response.data.ModelState) {
            //        console.log(key);
            //        for (var i = 0; i < response.data.ModelState[key].length; i++) {
            //            console.log(response.data.ModelState[key]);
            //            errors.push(response.data.ModelState[key][i]);
            //        }
            //    }
            //    $scope.message = "Failed to register user due to:" + errors.join(' ');
            //});
        }
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/pages/OTPSignUp');
        }, 2000);
    }


    //Generate OTP
    $scope.OTPVerify = function () {
        debugger;
        debugger;
        var verifyOtp = $scope.OTPData.VerifyOTP;
        if (verifyOtp == $scope.OTP.OtpNo) {
            var url = serviceBase + "api/Account/RegisterByMobile";
            var dataToPost = {
                MobileNumber: $scope.OTP.MobileNumber,
            };
            debugger;
            $http.post(url, dataToPost).success(function (data) {
                debugger;
                console.log(data);
                if (data.Message == "Successfully") {
                    debugger;
                    $location.path('/pages/signin');
                }
                else
                {
                    return alert('Something Went Wrong!Please Check')
                }
                return data;
            }).error(function (data) {
                console.log("Error Occured", data);
                return alert('Something Went Wrong!Please Check')
            });
        }
        else
        {
            return alert("OTP does'nt macth!")
        }
    }
}]);