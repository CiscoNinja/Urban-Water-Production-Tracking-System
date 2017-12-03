(function (app) {
    'use strict';

    app.controller('rootCtrl', rootCtrl);

    rootCtrl.$inject = ['$scope', '$location', 'membershipService', '$rootScope', 'Idle', 'Keepalive', '$modal', '$modalStack'];
    function rootCtrl($scope, $location, membershipService, $rootScope, Idle, Keepalive, $modal, $modalStack) {

        $scope.userData = {};
        $scope.userSystems = {};
        $scope.userstation = {};
        $scope.userInfo = {selectedSys: 1}
        
        $scope.userData.displayUserInfo = displayUserInfo;
        $scope.logout = logout;

        function wsystemsLoadCompleted(response) {
            $scope.userSystems = response.data;
        }

        //function stationLoadCompleted(response) {
        //    $scope.userstation = response.data;
        //}

        $scope.started = false;

        function closeModals() {
            if ($scope.warning) {
                $scope.warning.close();
                $scope.warning = null;
            }

            if ($scope.timedout) {
                $scope.timedout.close();
                $scope.timedout = null;
            }
        }

        $scope.$on('IdleStart', function () {
            closeModals();

            $scope.warning = $modal.open({
                templateUrl: './Scripts/spa/home/warning-dialog.html',
                windowClass: 'modal-danger'
            });
        });

        $scope.$on('IdleEnd', function () {
            closeModals();
        });

        $scope.$on('IdleTimeout', function () {
            closeModals();
            $scope.timedout = $modal.open({
                templateUrl: './Scripts/spa/home/timedout-dialog.html',
                windowClass: 'modal-danger'
            });
            logout();
        });

        $(document).on('click', '.closebtn', function (event) {
            event.preventDefault();
            $modalStack.dismissAll(event);
        });

        function displayUserInfo() {
            $scope.userData.isUserLoggedIn = membershipService.isUserLoggedIn();
            
            if($scope.userData.isUserLoggedIn)
            {
                //membershipService.loadUserStation($rootScope.repository.loggedUser, stationLoadCompleted)
                membershipService.loadWsystems(wsystemsLoadCompleted);
                $scope.username = $rootScope.repository.loggedUser.username;
                $scope.ustationname = $rootScope.repository.loggedUser.stationname;
                $scope.uregionname = $rootScope.repository.loggedUser.regionname;
                $scope.ustationid = $rootScope.repository.loggedUser.stationid;
                closeModals();
                Idle.watch();
                $scope.started = true;
                // save all user parameters here
            }
        }

        function logout() {
            membershipService.removeCredentials();
            $location.path('/login');
            closeModals();
            Idle.unwatch();
            $scope.started = false;
            $scope.userData.displayUserInfo();
        }

        $scope.userData.displayUserInfo();
    }

})(angular.module('gwcltdApp').config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    
}]));