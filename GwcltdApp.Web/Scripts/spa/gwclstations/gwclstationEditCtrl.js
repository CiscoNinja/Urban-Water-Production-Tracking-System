(function (app) {
    'use strict';

    app.controller('gwclstationEditCtrl', gwclstationEditCtrl);

    gwclstationEditCtrl.$inject = ['$scope', '$modalInstance', 'apiService', 'notificationService'];

    function gwclstationEditCtrl($scope, $modalInstance, apiService, notificationService) {

        $scope.cancelEdit = cancelEdit;
        $scope.updateStation = updateStation;
        $scope.regions = [];

        function loadRegions() {
            apiService.get('./api/gwclregions/loadregions', null,
            regionsLoadCompleted,
            regionsLoadFailed);
        }

        function regionsLoadCompleted(response) {
            $scope.regions = response.data;
        }

        function regionsLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function updateStation()
        {
            console.log($scope.EditedStation);
            apiService.post('./api/gwclstations/update/', $scope.EditedStation,
            updateRegionCompleted,
            updateRegionLoadFailed);
        }

        function updateRegionCompleted(response)
        {
            notificationService.displaySuccess($scope.EditedStation.Name + ' with code ' + $scope.EditedStation.Code + ' has been updated');
            $scope.EditedStation = {};
            $modalInstance.dismiss();
        }

        function updateRegionLoadFailed(response)
        {
            notificationService.displayError(response.data);
        }

        function cancelEdit() {
            $scope.isEnabled = false;
            $modalInstance.dismiss();
        }
        loadRegions();
    }

})(angular.module('gwcltdApp'));