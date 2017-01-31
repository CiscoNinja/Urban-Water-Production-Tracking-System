(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope','$location', 'membershipService','$rootScope'];
    function rootCtrl($scope, $location, membershipService, $rootScope) {

        $scope.userData = {};
        $scope.userSystems = { ID: 1 };
        $scope.userstation = {};
        
        $scope.userData.displayUserInfo = displayUserInfo;
        $scope.logout = logout;

        function wsystemsLoadCompleted(response) {
            $scope.userSystems = response.data;
        }

        function stationLoadCompleted(response) {
            $scope.userstation = response.data;
        }


        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();

            if($scope.userData.isUserLoggedIn)
            {
                membershipService.loadUserStation($rootScope.repository.loggedUser, stationLoadCompleted)
                membershipService.loadWsystems(wsystemsLoadCompleted);
                $scope.username = $rootScope.repository.loggedUser.username;
                // save all user parameters here
            }
        }

        function logout() {
            membershipService.removeCredentials();
            $location.path('#/');
            $scope.userData.displayUserInfo();
        }

        $scope.userData.displayUserInfo();
    }

})(angular.module('gwcltdApp'));