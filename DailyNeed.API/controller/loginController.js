'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', 'ngAuthSettings', function ($scope, $location, authService, ngAuthSettings) {

    console.log("Called login controller");
    $scope.pageClass = 'page-contact';
    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false
    };

    $scope.message = "";
    
    $scope.login = function () {
        debugger;
        authService.login($scope.loginData).then(function (response) {
            
            var RolePerson = { "token": response.access_token, "Email": response.email, "userName": response.userName, "role": response.role, "userid": response.userid, "Warehouseid": response.Warehouseid }

            localStorage.setItem('RolePerson', JSON.stringify(response));

            if (response.Active == false) {
                localStorage.removeItem('RolePerson');
                $location.path('/pages/signin');          

            }
            else {
                localStorage.setItem('RolePerson', JSON.stringify(response));
                $scope.UserRole = JSON.parse(localStorage.getItem('RolePerson'));
                if ($scope.UserRole.role == "Supplier") {
                    $location.path('/Promo');
                    location.reload();
                }
                else {
                    if ($scope.UserRole.role == "Presta") {
                        localStorage.removeItem('RolePerson');
                        $location.path('/pages/signin');
                        location.reload();
                    }
                    else {
                        $location.path('/DashboardReport');
                        location.reload();
                    }
                }
            }
            //else {
            //    localStorage.setItem('RolePerson', JSON.stringify(response));
            //    $location.path('/DashboardReport');
            //    location.reload();
            //}
        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/Account/ExternalLogin?provider=" + provider
                                                                    + "&response_type=token&client_id=" + ngAuthSettings.clientId
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/associate');

            }
            else {
                //Obtain access token and redirect to orders
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/orders');

                },
             function (err) {
                 $scope.message = err.error_description;
             });
            }
        });
    }
}]);
