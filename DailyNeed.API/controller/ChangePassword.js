'use strict';
app.controller('ChangePassword', ['$scope',  '$http', '$location', 'ngAuthSettings',  'logger', function ($scope,  $http, $location, ngAuthSettings, logger) {
    console.log(" ChangePassword controller start loading");
   
    $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));

    $scope.UpdatePassword = function (password, confirmpassword) {
           

            if (password != confirmpassword || password == undefined && confirmpassword == undefined || password == "" && confirmpassword == "") {
                alert("Passwords do not match.");
                return false;
            }
            if (password.length < 6 &&password.length <6) {
                alert("Passwords length atleast 6 digit.");
                return false;
            }

            var url = serviceBase + "api/Account/ChangePassword";
            var dataToPost =
                {
                    Password: password,
                    UserName: $scope.UserRole.userName
                }
           
            console.log(dataToPost);
            $http.post(url, dataToPost)
            .success(function (data) {
                
                alert('Password changed Successfuly');
                localStorage.clear();

                $location.path('/pages/signin');
               
            })
             .error(function (data) {
                 console.log("Error Got Heere is ");
                 console.log(data);

             })


        };
  

}]);


