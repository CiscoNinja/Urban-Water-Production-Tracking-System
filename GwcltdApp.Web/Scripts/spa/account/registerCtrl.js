(function (app) {
    'use strict';

    app.controller('registerCtrl', registerCtrl);

    registerCtrl.$inject = ['$scope', 'membershipService', 'apiService', 'notificationService', '$rootScope', '$location'];

    function registerCtrl($scope, membershipService, apiService, notificationService, $rootScope, $location) {
        $scope.pageClass = 'page-login';
        $scope.register = register;
        $scope.user = {};
        $scope.roles = [];
        $scope.gwclregions = [];

        function loadRoles() {
            apiService.get('/api/roles/', null,
            rolesLoadCompleted,
            rolesLoadFailed);
        }

        function rolesLoadCompleted(response) {
            $scope.roles = response.data;
        }

        function rolesLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function loadGwclRegions() {
            apiService.get('/api/gwclregions/loadregions', null,
            gwclregionsLoadCompleted,
            gwclregionsLoadFailed);
        }

        function gwclregionsLoadCompleted(response) {
            $scope.gwclregions = response.data;
        }

        function gwclregionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function register() {
            membershipService.register($scope.user, registerCompleted);
        }

        function registerCompleted(result) {
            if (result.data.success) {
                //membershipService.saveCredentials($scope.user);
                notificationService.displaySuccess('you have successully registered ' + $scope.user.username);
                //$scope.userData.displayUserInfo();
                $location.path('#/register');
            }
            else {
                notificationService.displayError('Registration failed. Try again.');
            }
        }
        loadRoles();
        loadGwclRegions();
    }

})(angular.module('common.core'));