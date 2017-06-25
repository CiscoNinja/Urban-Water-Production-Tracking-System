(function (app) {
    'use strict';

    app.controller('gwclsystemEditCtrl', gwclsystemEditCtrl);

    gwclsystemEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwclsystemEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateSystem = updateSystem;

        function loadStations() {
            apiService.get('./api/gwclstations/loadstations', null,
            stationsLoadCompleted,
            stationsLoadFailed);
        }

        function stationsLoadCompleted(response) {
            $scope.stations = response.data;
        }

        function stationsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function updateSystem()
        {
            console.log($scope.EditedSystem);
            apiService.post('./api/gwclsystems/update/', $scope.EditedSystem,
            updateSystemCompleted,
            updateSystemLoadFailed);
        }

        function updateSystemCompleted(response)
        {
            notificationService.displaySuccess($scope.EditedSystem.Name + ' with code ' + $scope.EditedSystem.Code + ' has been updated');
            $scope.EditedSystem = {};
            $modalInstance.dismiss();
        }

        function updateSystemLoadFailed(response)
        {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
        loadStations();
    }

})(angular.module('gwcltdApp'));