


'use strict';
app.controller('ConfigureController', ['$scope', function ($scope) {
    $scope.tabs = [
      { title: ' Default Values', content: 'Dynamic content 1' },
      { title: 'Appearance', content: 'Dynamic content 2' },
      { title: 'Messages', content: 'Dynamic content 3'},
      { title: 'Translatons', content: 'Dynamic content 4' },
      { title: 'Categories', content: 'Dynamic content 4' },
      { title: 'Online Payment', content: 'Dynamic content 4' }
    ];





    //$scope.alertMe = function () {
    //    setTimeout(function () {
    //        $window.alert('You\'ve selected the alert tab!');
    //    });
    //};

}]);