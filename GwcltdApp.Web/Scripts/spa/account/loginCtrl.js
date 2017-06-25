(function (app) {
    'use strict';

    app.controller('loginCtrl', loginCtrl);

    loginCtrl.$inject = ['apiService', '$scope', 'membershipService', 'notificationService', '$rootScope', '$location'];

    function loginCtrl(apiService, $scope, membershipService, notificationService, $rootScope, $location) {
        $scope.pageClass = 'page-login';
        $scope.login = login;
        $scope.user = {};
        $scope.userstation = {};

        function loadUserStation(user) {
            apiService.get('./api/account/userstation/' + user.username, user,
            stationLoadCompleted,
            stationLoadFailed);
        }

        function stationLoadCompleted(response) {
            $scope.userstation = response.data;
            membershipService.saveCredentials($scope.user, $scope.userstation);
            notificationService.displaySuccess('Hello ' + $scope.user.username);
            $scope.userData.displayUserInfo();
            if ($rootScope.previousState)
                $location.path($rootScope.previousState);
            else
                $location.path('/');
        }

        function stationLoadFailed(response) {
            notificationService.displayError(response.data);
        }


        function login() {
            membershipService.login($scope.user, loginCompleted)
        }
        
        function loginCompleted(result) {
            if (result.data.success) {
                loadUserStation($scope.user);
            }
            else {
                notificationService.displayError('Login failed. Try again.');
            }
        }
    }

})(angular.module('common.core'));